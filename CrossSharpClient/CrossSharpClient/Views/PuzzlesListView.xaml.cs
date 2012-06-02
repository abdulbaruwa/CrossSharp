using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace CrossSharpClient.Views
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class PuzzlesListView
    {
        public PuzzlesListView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the collection of items to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DefaultViewModel["Items"] = e.Parameter;
        }
    }
}
