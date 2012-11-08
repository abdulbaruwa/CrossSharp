﻿using System.IO;
using CrossPuzzleClient.Common;
using CrossPuzzleClient.GameDataService;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.Views;
using GalaSoft.MvvmLight.Ioc;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace CrossPuzzleClient
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private const string PuzzleDb = "Puzzle.db";

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static NavigationService NavigationService;


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            //Create Database if it does not exists

            //Do not repeat app initialization when already running, just ensure that window is active
            if(args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key

            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                await SuspensionManager.RestoreAsync();
            }

            SplashScreen splashScreen = args.SplashScreen;
            var extendedSplashScreen = new ExtendedSplashView(splashScreen, false);
            splashScreen.Dismissed += splashScreen_Dismissed;

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = extendedSplashScreen;
            Window.Current.Activate();
            var gameDataService = SimpleIoc.Default.GetInstance<IGameDataService>();
            await gameDataService.GetGameDataAndStoreInLocalDb(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
            
        }


        void splashScreen_Dismissed(SplashScreen sender, object args)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
