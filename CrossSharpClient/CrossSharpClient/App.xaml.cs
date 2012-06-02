using CrossSharpClient.DataModel;
using CrossSharpClient.Views;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Grid Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace CrossSharpClient
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// An overview of the Grid Application design will be linked to in future revisions of
    /// this template.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // TODO: Create a data model appropriate for your problem domain to replace the sample data
            var mainViewModel = new MainViewModel();
            
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Create a Frame to act navigation context and navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            //var rootFrame = new Frame();
            ////rootFrame.Navigate(typeof(GroupedItemsPage), sampleData.ItemGroups);
            //rootFrame.Navigate(typeof(GroupedItemsPage), mainViewModel.Board);

            //// Place the frame in the current Window and ensure that it is active
            //Window.Current.Content = rootFrame;

            SplashScreen splashscreen = args.SplashScreen;
            var extendedSplashScreen = new ExtendedSplashView(splashscreen, false);
            splashscreen.Dismissed += extendedSplashScreen.dismissedEventHandler;
            Window.Current.Content = extendedSplashScreen;
            Window.Current.Activate();
        }

        void splashscreen_Dismissed(SplashScreen sender, object args)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity
        }
    }
}
