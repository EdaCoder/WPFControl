﻿using CandyControls.ControlsModel.Thicks;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace CandyControls
{
    public class CandyImage : Control
    {
        static CandyImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CandyImage), new FrameworkPropertyMetadata(typeof(CandyImage)));
        }

        private Path PART_LOAD;
        private Button PART_BTN;
        private Path PART_INFO;
        private Grid PART_RECT;
        private Grid PART_RECT_INFO;
        private Trigger TRIGGER;

        public override void OnApplyTemplate()
        {
            PART_LOAD = (Path)this.Template.FindName("PART_LOAD", this);
            PART_BTN = (Button)this.Template.FindName("PART_BTN", this);
            PART_RECT = (Grid)this.Template.FindName("PART_RECT", this);
            PART_RECT_INFO = (Grid)this.Template.FindName("PART_RECT_INFO", this);
            PART_BTN.Click += ClickEvent;
            PART_LOAD.Height = LoadingThickness.Height;
            PART_LOAD.Width = LoadingThickness.Width;

            TRIGGER = (Trigger)this.Template.Triggers.Where(t => t is Trigger).First();
            if (!EnableMask)
                TRIGGER.EnterActions.Clear();
            else
            {
                if (EnablePopupBtn == Visibility.Collapsed)
                {
                    PART_RECT_INFO.RowDefinitions.Last().Height = new GridLength(0, GridUnitType.Pixel);
                    CloseAnime();
                    TRIGGER.ExitActions.Add(new BeginStoryboard
                    {
                        Storyboard = CloseStory
                    });
                }
            }
            LoadAnime();
        }

        #region Property
        /// <summary>
        /// 是否启用遮罩层默认启用
        /// </summary>
        public bool EnableMask
        {
            get { return (bool)GetValue(EnableMaskProperty); }
            set { SetValue(EnableMaskProperty, value); }
        }
        public static readonly DependencyProperty EnableMaskProperty =
            DependencyProperty.Register("EnableMask", typeof(bool), typeof(CandyImage), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 遮罩层颜色默认透明
        /// </summary>
        public Brush MaskFill
        {
            get { return (Brush)GetValue(MaskFillProperty); }
            set { SetValue(MaskFillProperty, value); }
        }
        public static readonly DependencyProperty MaskFillProperty =
            DependencyProperty.Register("MaskFill", typeof(Brush), typeof(CandyImage), new FrameworkPropertyMetadata(Brushes.Transparent));
        /// <summary>
        /// 边框圆角
        /// </summary>
        public CornerRadius BorderRadius
        {
            get { return (CornerRadius)GetValue(BorderRadiusProperty); }
            set { SetValue(BorderRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.Register("BorderRadius", typeof(CornerRadius), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 边框阴影
        /// </summary>
        public Effect BorderEffect
        {
            get { return (Effect)GetValue(BorderEffectProperty); }
            set { SetValue(BorderEffectProperty, value); }
        }
        public static readonly DependencyProperty BorderEffectProperty =
          DependencyProperty.Register("BorderEffect", typeof(Effect), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 图片长宽大小
        /// </summary>
        public WidthHeightStruct ImageThickness
        {
            get { return (WidthHeightStruct)GetValue(ImageThicknessProperty); }
            set { SetValue(ImageThicknessProperty, value); }
        }
        public static readonly DependencyProperty ImageThicknessProperty =
          DependencyProperty.Register("ImageThickness", typeof(WidthHeightStruct), typeof(CandyImage), new FrameworkPropertyMetadata(new WidthHeightStruct(160, 240)));
        /// <summary>
        /// 等待图像的大小
        /// </summary>
        public WidthHeightStruct LoadingThickness
        {
            get { return (WidthHeightStruct)GetValue(LoadingThicknessProperty); }
            set { SetValue(LoadingThicknessProperty, value); }
        }
        public static readonly DependencyProperty LoadingThicknessProperty =
        DependencyProperty.Register("LoadingThickness", typeof(WidthHeightStruct), typeof(CandyImage), new FrameworkPropertyMetadata(new WidthHeightStruct(25, 25)));
        /// <summary>
        /// 是否启用加载等待默认启用
        /// </summary>
        public bool EnableLoading
        {
            get { return (bool)GetValue(EnableLoadingProperty); }
            set { SetValue(EnableLoadingProperty, value); }
        }
        public static readonly DependencyProperty EnableLoadingProperty =
          DependencyProperty.Register("EnableLoading", typeof(bool), typeof(CandyImage), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 遮罩层模板
        /// </summary>
        public DataTemplate MaskTemplate
        {
            get { return (DataTemplate)GetValue(MaskTemplateProperty); }
            set { SetValue(MaskTemplateProperty, value); }
        }
        public static readonly DependencyProperty MaskTemplateProperty =
            DependencyProperty.Register("MaskTemplate", typeof(DataTemplate), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 弹出层模板
        /// </summary>
        public DataTemplate PopupTemplate
        {
            get { return (DataTemplate)GetValue(PopupTemplateProperty); }
            set { SetValue(PopupTemplateProperty, value); }
        }
        public static readonly DependencyProperty PopupTemplateProperty =
            DependencyProperty.Register("PopupTemplate", typeof(DataTemplate), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 图片链接
        /// </summary>
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(CandyImage), new FrameworkPropertyMetadata(default));

        /// <summary>
        /// bytes数据
        /// </summary>
        public object Base64Source
        {
            get { return (object)GetValue(Base64SourceProperty); }
            set { SetValue(Base64SourceProperty, value); }
        }
        public static readonly DependencyProperty Base64SourceProperty =
            DependencyProperty.Register("Base64Source", typeof(object), typeof(CandyImage), new FrameworkPropertyMetadata(default));

        /// <summary>
        /// 命令参数
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 命令
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 弹出层的长宽
        /// </summary>
        public WidthHeightStruct PopupThickness
        {
            get { return (WidthHeightStruct)GetValue(PopupThicknessProperty); }
            set { SetValue(PopupThicknessProperty, value); }
        }
        public static readonly DependencyProperty PopupThicknessProperty =
         DependencyProperty.Register("PopupThickness", typeof(WidthHeightStruct), typeof(CandyImage), new FrameworkPropertyMetadata(new WidthHeightStruct(0, 0)));
        /// <summary>
        /// 是否显示弹出层按钮默认不显示
        /// </summary>
        public Visibility EnablePopupBtn
        {
            get { return (Visibility)GetValue(EnablePopupBtnProperty); }
            set { SetValue(EnablePopupBtnProperty, value); }
        }
        public static readonly DependencyProperty EnablePopupBtnProperty =
            DependencyProperty.Register("EnablePopupBtn", typeof(Visibility), typeof(CandyImage), new FrameworkPropertyMetadata(Visibility.Collapsed));
        /// <summary>
        /// 是否启用图片缓存默认启用
        /// </summary>
        public bool EnableCache
        {
            get { return (bool)GetValue(EnableCacheProperty); }
            set { SetValue(EnableCacheProperty, value); }
        }
        public static readonly DependencyProperty EnableCacheProperty =
            DependencyProperty.Register("EnableCache", typeof(bool), typeof(CandyImage), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 缓存时常默认5分钟
        /// </summary>
        public int CacheSpan
        {
            get { return (int)GetValue(CacheSpanProperty); }
            set { SetValue(CacheSpanProperty, value); }
        }
        public static readonly DependencyProperty CacheSpanProperty =
            DependencyProperty.Register("CacheSpan", typeof(int), typeof(CandyImage), new FrameworkPropertyMetadata(5));
        /// <summary>
        /// 绑定参数
        /// </summary>
        public object Entity
        {
            get { return (object)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }
        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(object), typeof(CandyImage), new FrameworkPropertyMetadata(default));
        /// <summary>
        /// 完成后停用动画
        /// </summary>
        internal bool Complete
        {
            get { return (bool)GetValue(CompleteProperty); }
            set { SetValue(CompleteProperty, value); }
        }
        internal static readonly DependencyProperty CompleteProperty =
            DependencyProperty.Register("Complete", typeof(bool), typeof(CandyImage), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// 是否启用异步下载默认启用
        /// </summary>
        public bool PushDown
        {
            get { return (bool)GetValue(EnableAsyncLoadProperty); }
            set { SetValue(EnableAsyncLoadProperty, value); }
        }
        public static readonly DependencyProperty EnableAsyncLoadProperty =
            DependencyProperty.Register("EnableAsyncLoad", typeof(bool), typeof(CandyImage), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable ItemSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(CandyImage), new FrameworkPropertyMetadata(default));

        /// <summary>
        /// ListItemStyle
        /// </summary>
        public Style ListItemStyle
        {
            get { return (Style)GetValue(ListItemStyleProperty); }
            set { SetValue(ListItemStyleProperty, value); }
        }
        public static readonly DependencyProperty ListItemStyleProperty =
            DependencyProperty.Register("ListItemStyle", typeof(Style), typeof(CandyImage), new FrameworkPropertyMetadata(default));

        /// <summary>
        /// 弹出位置
        /// </summary>
        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(CandyImage), new FrameworkPropertyMetadata(PlacementMode.Bottom));
        #endregion

        #region Anime
        public Storyboard LoadAnimeStory;
        public Storyboard CollapsedStory;
        public Storyboard ExpendStory;
        public Storyboard CloseStory;
        private void LoadAnime()
        {
            LoadAnimeStory = new Storyboard();
            DoubleAnimationUsingKeyFrames KF = new DoubleAnimationUsingKeyFrames
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(KF, PART_LOAD);
            Storyboard.SetTargetProperty(KF, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
            KF.KeyFrames.Add(new EasingDoubleKeyFrame(0, TimeSpan.FromSeconds(0)));
            KF.KeyFrames.Add(new EasingDoubleKeyFrame(180, TimeSpan.FromSeconds(1)));
            KF.KeyFrames.Add(new EasingDoubleKeyFrame(360, TimeSpan.FromSeconds(2)));
            LoadAnimeStory.Children.Add(KF);
            LoadAnimeStory.Begin();
        }
        private void ExpendAnime()
        {
            ExpendStory = new Storyboard();
            DoubleAnimationUsingKeyFrames Revolve = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(Revolve, PART_INFO);
            Storyboard.SetTargetProperty(Revolve, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
            Revolve.KeyFrames.Add(new EasingDoubleKeyFrame(-90, TimeSpan.FromSeconds(0)));
            Revolve.KeyFrames.Add(new EasingDoubleKeyFrame(0, TimeSpan.FromSeconds(1)));
            ExpendStory.Children.Add(Revolve);
            ExpendStory.Begin();
        }
        private void CollapsedAnime()
        {
            CollapsedStory = new Storyboard();
            DoubleAnimationUsingKeyFrames Revolve = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(Revolve, PART_INFO);
            Storyboard.SetTargetProperty(Revolve, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
            Revolve.KeyFrames.Add(new EasingDoubleKeyFrame(0, TimeSpan.FromSeconds(0)));
            Revolve.KeyFrames.Add(new EasingDoubleKeyFrame(-90, TimeSpan.FromSeconds(1)));
            CollapsedStory.Children.Add(Revolve);
            CollapsedStory.Begin();
        }
        private void CloseAnime()
        {
            CloseStory = new Storyboard();
            DoubleAnimationUsingKeyFrames Close = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(Close, PART_RECT);
            Storyboard.SetTargetProperty(Close, new PropertyPath("Height"));
            Close.KeyFrames.Add(new EasingDoubleKeyFrame(100, TimeSpan.FromSeconds(0)));
            Close.KeyFrames.Add(new EasingDoubleKeyFrame(0, TimeSpan.FromSeconds(1)));
            CloseStory.Children.Add(Close);
        }
        #endregion

        #region Method
        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            PART_INFO = (Path)((Button)sender).Template.FindName("PART_INFO", PART_BTN);
            ExpendAnime();
            Command?.Execute(CommandParameter);
            Grid panal = new Grid();
            panal.Children.Add(new Rectangle
            {
                Height = PopupThickness.Height == 0 ? this.Height : PopupThickness.Height,
                Width = PopupThickness.Width == 0 ? Application.Current.MainWindow.ActualWidth : PopupThickness.Width,
                Fill = MaskFill,
            });
            if (ItemSource == null)
                panal.Children.Add(new ContentPresenter
                {
                    ContentTemplate = PopupTemplate,
                    Content = Entity
                });
            else
                panal.Children.Add(new ListBox
                {
                    ItemsSource = ItemSource,
                    Style = ListItemStyle,
                    ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel))),
                    ItemTemplate = PopupTemplate
                });

            Popup popup = new Popup
            {
                Placement = Placement,
                PlacementTarget = this,
                AllowsTransparency = true,
                StaysOpen = false,
                Height = PopupThickness.Height == 0 ? this.Height : PopupThickness.Height,
                Width = PopupThickness.Width == 0 ? Application.Current.MainWindow.ActualWidth : PopupThickness.Width,
                Child = panal
            };
            popup.IsOpen = true;
            popup.Closed += delegate
            {
                CollapsedAnime();
            };
        }
        #endregion
    }
}
