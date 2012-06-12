﻿using CrossPuzzleClient.ViewModels;
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

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            INavigationService x = App.NavigationService;

            
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<PuzzlesViewModel>();
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
            get{return new PuzzleBoardViewModel();}
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}