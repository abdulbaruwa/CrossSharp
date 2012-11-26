using CrossPuzzleClient.Common;
using CrossPuzzleClient.ViewModels;

namespace CrossPuzzleClient.Views
{
    public class ViewBase : LayoutAwarePage
    {
        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            var vm = DataContext as ViewModelBase;
            if (vm != null) 
                vm.LoadState(navigationParameter,pageState);
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            var vm = DataContext as ViewModelBase;
            if(vm!= null)vm.SaveState(pageState);
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}