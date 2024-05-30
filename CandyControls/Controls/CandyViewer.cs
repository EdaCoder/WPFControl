﻿using SkiaImageView;
using System;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XExten.Advance.CacheFramework;
using XExten.Advance.LinqFramework;
using XExten.Advance.NetFramework;

namespace CandyControls
{
    public class CandyViewer:UserControl
    {
        static CandyViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CandyViewer), new FrameworkPropertyMetadata(typeof(CandyViewer)));
        }

        private SKImageView PART_IMG;
        private static Action<double> ProcessAction;
        public override void OnApplyTemplate()
        {
            PART_IMG = (SKImageView)this.Template.FindName("PART_IMG", this);
            ProcessAction = (process) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.ProcessValue = CalcProgress(process, 50, 12);
                    if (process >= 100)
                        Show = true;
                });
            };
        }

        public string ViewSoucre
        {
            get { return (string)GetValue(ViewSoucreProperty); }
            set { SetValue(ViewSoucreProperty, value); }
        }

        public static readonly DependencyProperty ViewSoucreProperty =
            DependencyProperty.Register("ViewSoucre", typeof(string), typeof(CandyViewer), new FrameworkPropertyMetadata(Onchanged));

        internal DoubleCollection ProcessValue
        {
            get { return (DoubleCollection)GetValue(ProcessValueProperty); }
            set { SetValue(ProcessValueProperty, value); }
        }

        internal static readonly DependencyProperty ProcessValueProperty =
            DependencyProperty.Register("ProcessValue", typeof(DoubleCollection), typeof(CandyViewer), new FrameworkPropertyMetadata(new DoubleCollection { 0, 0 }));

        internal bool Show
        {
            get { return (bool)GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }

        internal static readonly DependencyProperty ShowProperty =
            DependencyProperty.Register("Show", typeof(bool), typeof(CandyViewer), new FrameworkPropertyMetadata(false));

        private static async void Onchanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = (CandyViewer)d;
            uc.Show = false;
            uc.ProcessValue = [0, 0];
            var key = e.NewValue.ToString().ToMd5();
            var result = Caches.RunTimeCacheGet<byte[]>(key);
            if (result != null)
            {
                var data = BitmapHelper.Bytes2Image(result, (int)uc.Width, (int)uc.Height);
                uc.PART_IMG.Source = data;
            }
            else
            {
                HttpClient Client = new(ProgressHandler());
                Client.DefaultRequestHeaders.Add(ConstDefault.UserAgent, ConstDefault.UserAgentValue);
                var bytes = await Client.GetByteArrayAsync(e.NewValue.ToString());
                var data = BitmapHelper.Bytes2Image(bytes, (int)uc.Width, (int)uc.Height);
                Caches.RunTimeCacheSet(key, bytes, 5);
               uc.PART_IMG.Source = data;
            }
        }
        private static ProgressMessageHandler ProgressHandler()
        {
            var ProgressHandler = new ProgressMessageHandler(new HttpClientHandler());
            ProgressHandler.HttpReceiveProgress += HttpReceiveProgress;
            return ProgressHandler;
        }

        private static void HttpReceiveProgress(object sender, HttpProgressEventArgs e)
        {
            ProcessAction?.Invoke(e.ProgressPercentage);
        }

        private static DoubleCollection CalcProgress(double progress, double radius, double thickness)
        {
            var r = radius - thickness / 2;
            var perimeter = 2 * Math.PI * r / thickness;
            var step = progress / 100 * perimeter;
            return [step, 1000];
        }
    }
}
