using System;
using System.Collections.ObjectModel;
using CrossClient.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CrossClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : LayoutAwarePage
    {
        private int _index = 0;
        public BlankPage()
        {
            this.InitializeComponent();
        }
        private Random _rand = new Random();
        private ObservableCollection<CellViewModel> _cells;

        public ObservableCollection<CellViewModel> Cells
        {
            get { return _cells; }
            set { _cells = value; }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var item = (BoardViewModel) e.Parameter;
            this.DefaultViewModel["Board"] = item;
            this.DefaultViewModel["Cells"] = item.Cells;
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            //itemsTwo.Items.Add(CreateButton(_index));
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (itemsTwo.Items.Count > 0)
                itemsTwo.Items.RemoveAt(0);
        }
    }

}
