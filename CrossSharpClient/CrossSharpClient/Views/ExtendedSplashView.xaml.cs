﻿using System;
using CrossSharpClient.DataModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CrossSharpClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashView
    {
        private Rect splashImageCoordinates; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        private bool dismissed = false; // Variable to track splash screen dismissal status.


                public ExtendedSplashView(SplashScreen splashScreen, bool dismissed)
        {
            InitializeComponent();
            this.splashImageCoordinates = splashScreen.ImageLocation;
            this.splash = splashScreen;
            this.dismissed = dismissed;

            // Position the extended splash screen image in the same location as the splash screen image.
            this.extendedSplashImage.SetValue(Canvas.LeftProperty, this.splashImageCoordinates.X);
            this.extendedSplashImage.SetValue(Canvas.TopProperty, this.splashImageCoordinates.Y);
            this.extendedSplashImage.Height = this.splashImageCoordinates.Height;
            this.extendedSplashImage.Width = this.splashImageCoordinates.Width;

            LoadPuzzle.Click += new RoutedEventHandler(StartButton_Click);
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
        }


        void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = new MainViewModel();
            var rootFrame = new Frame();
            //rootFrame.Navigate(typeof(GroupedItemsPage), sampleData.ItemGroups);
            rootFrame.Navigate(typeof(GroupedItemsPage), mainViewModel.Board);

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;

        }

        internal void dismissedEventHandler(SplashScreen sender, object e)
        {
            this.dismissed = true;
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        void ExtendedSplash_OnResize(Object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // Safely update the splash screen image coordinates
            if (this.splash != null)
            {
                this.splashImageCoordinates = this.splash.ImageLocation;

                // Re-position the extended splash screen image due to window resize event.
                this.extendedSplashImage.SetValue(Canvas.LeftProperty, this.splashImageCoordinates.X);
                this.extendedSplashImage.SetValue(Canvas.TopProperty, this.splashImageCoordinates.Y);
                this.extendedSplashImage.Height = this.splashImageCoordinates.Height;
                this.extendedSplashImage.Width = this.splashImageCoordinates.Width;
            }
        }
    }
}
