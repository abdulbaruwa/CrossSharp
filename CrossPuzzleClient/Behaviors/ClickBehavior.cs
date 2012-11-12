using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace CrossPuzzleClient.Behaviors
{
    public static class ClickBehavior
    {
        public static DependencyProperty PointerClickCommandProperty = DependencyProperty.RegisterAttached("PointerClick", typeof (ICommand),
                                                                                                            typeof (ClickBehavior),
                                                                                                            new PropertyMetadata(null,PointerClickChanged));



        public static void SetPointerClick(DependencyObject target, ICommand value)
        {
            target.SetValue(PointerClickCommandProperty, value);
        }


        public static ICommand GetPointerClick(DependencyObject target)
        {
            return (ICommand) target.GetValue(PointerClickCommandProperty);
        }


        private static void PointerClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as UIElement;

            if (element != null)
            {
                // If we're putting in a new command and there wasn't one already

                // hook the event

                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.PointerReleased += element_MouseRightButtonUp;
                }

                    // If we're clearing the command and it wasn't already null
                    // unhook the event
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.PointerReleased -= element_MouseRightButtonUp;
                }
            }
        }


        private static void element_MouseRightButtonUp(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            var element = (UIElement) sender;

            var command = (ICommand) element.GetValue(PointerClickCommandProperty);

            command.Execute(null);
        }
    }
}