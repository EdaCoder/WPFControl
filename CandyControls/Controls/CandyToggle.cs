﻿using CandyControls.ControlsModel.Setting;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using XExten.Advance.LinqFramework;

namespace CandyControls
{
    public class CandyToggle : ListBox
    {

        public IEnumerable<string> ModelSource
        {
            get { return (IEnumerable<string>)GetValue(ModelSourceProperty); }
            set { SetValue(ModelSourceProperty, value); }
        }

        public static readonly DependencyProperty ModelSourceProperty =
            DependencyProperty.Register("ModelSource", typeof(IEnumerable<string>), typeof(CandyToggle), new FrameworkPropertyMetadata(OnModelChanged));

        public IEnumerable<CandyToggleItemSetting> SettingSource
        {
            get { return (IEnumerable<CandyToggleItemSetting>)GetValue(SettingSourceProperty); }
            set { SetValue(SettingSourceProperty, value); }
        }
        public static readonly DependencyProperty SettingSourceProperty =
            DependencyProperty.Register("SettingSource", typeof(IEnumerable<CandyToggleItemSetting>), typeof(CandyToggle), new FrameworkPropertyMetadata(OnModelChanged));

        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(CandyToggle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0x70, 0x80, 0xCB, 0xCC))));

        public Brush MouseOverBrush
        {
            get { return (Brush)GetValue(MouseOverBrushProperty); }
            set { SetValue(MouseOverBrushProperty, value); }
        }
        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(CandyToggle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0x70, 0xE0, 0xE0, 0xDB))));

        public Brush BackBrush
        {
            get { return (Brush)GetValue(BackBrushProperty); }
            set { SetValue(BackBrushProperty, value); }
        }
        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(Brush), typeof(CandyToggle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0x40, 0xB3, 0xB3, 0xB3))));

        public Brush ForeBrush
        {
            get { return (Brush)GetValue(ForeBrushProperty); }
            set { SetValue(ForeBrushProperty, value); }
        }
        public static readonly DependencyProperty ForeBrushProperty =
            DependencyProperty.Register("ForeBrush", typeof(Brush), typeof(CandyToggle), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0xFD, 0xF5, 0xE6))));

        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CandyToggle), new FrameworkPropertyMetadata(default));

        public bool FirstSelect
        {
            get { return (bool)GetValue(FirstSelectProperty); }
            set { SetValue(FirstSelectProperty, value); }
        }
        public static readonly DependencyProperty FirstSelectProperty =
            DependencyProperty.Register("FirstSelect", typeof(bool), typeof(CandyToggle), new FrameworkPropertyMetadata(false));

        public int UnderBorderWidth
        {
            get { return (int)GetValue(UnderBorderWidthProperty); }
            set { SetValue(UnderBorderWidthProperty, value); }
        }
        public static readonly DependencyProperty UnderBorderWidthProperty =
            DependencyProperty.Register("UnderBorderWidth", typeof(int), typeof(CandyToggle), new FrameworkPropertyMetadata(80));

        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }
        public static readonly DependencyProperty ContentPaddingProperty =
            DependencyProperty.Register("ContentPadding", typeof(Thickness), typeof(CandyToggle), new FrameworkPropertyMetadata(new Thickness(50, 0, 50, 0)));

        private static void OnModelChanged(DependencyObject dp, DependencyPropertyChangedEventArgs eve)
        {
            var toggle = dp as CandyToggle;
            int Index = 0;
            toggle.Items.Clear();
            if (toggle.ModelSource != null && toggle.ModelSource.Count() > 0)
            {
                toggle.ModelSource.ForEnumerEach(item =>
                {
                    toggle.AddChild(new CandyToggleItem
                    {
                        FontSize = toggle.FontSize,
                        IsSelected = Index == 0 && toggle.FirstSelect,
                        Content = item,
                        ParentElement = toggle,
                        Tag = Index,
                    });
                    Index++;
                });
            }
            else
            {
                toggle.SettingSource.ForEnumerEach(item =>
                {
                    var Model = new CandyToggleItem
                    {
                        IsSelected = Index == 0 && toggle.FirstSelect,
                        Content = item.Content,
                        ParentElement = toggle,
                        Tag = Index,
                        UnderLine = item.UseUnderLine,
                    };
                    if (item.Width != 0)
                        Model.Width = item.Width;
                    toggle.AddChild(Model);
                    Index++;

                });
            }

        }
    }
}
