using CrossPuzzleClient.Observables;
using CrossPuzzleClient.ViewModels;
using CrossPuzzleClient.Views;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CrossPuzzleClient.Infrastructure
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            SimpleIoc.Default.Register<PuzzlesViewModel>();
            SimpleIoc.Default.Register<IPuzzleRepository, FakePuzzleRepository>();
            SimpleIoc.Default.Register<IPuzzlesService, PuzzlesService>();
        }

        public PuzzlesViewModel PuzzlesViewModel
        {
            get
            {
                return new PuzzlesViewModel(App.NavigationService); 
            }
        }

        public PuzzleBoardViewModel PuzzleBoardViewModel
        {
            get{return new PuzzleBoardViewModel(SimpleIoc.Default.GetInstance<IPuzzlesService>(), new SchedulerProvider());}
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}