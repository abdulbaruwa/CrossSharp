using System;
using Windows.UI.Xaml.Controls;

namespace CrossPuzzleClient.Infrastructure
{
    public interface INavigationService
    {
        void GoBack();
        void GoForward();
        bool Navigate<T>(object parameter = null);
        bool Navigate(Type source, object parameter = null);
    }

    public class NavigationService : INavigationService
    {
        readonly Frame frame;

        public NavigationService(Frame frame)
        {
            this.frame = frame;
        }

        public void GoBack()
        {
            frame.GoBack();
        }

        public void GoForward()
        {
            frame.GoForward();
        }

        public bool Navigate<T>(object parameter = null)
        {
            var type = typeof(T);

            return Navigate(type, parameter);
        }

        public bool Navigate(Type source, object parameter = null)
        {
            return frame.Navigate(source,  parameter != null ? JsonUtility.ToJson(parameter) : null);
        }
    }

}