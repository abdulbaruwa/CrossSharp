using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using WinRtBehaviors;
using Windows.UI.Xaml;

namespace CrossPuzzleClient.Behaviors
{
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            var evt = AssociatedObject.GetType().GetRuntimeEvent(Event);
            if (evt != null)
            {
                Observable.FromEventPattern<RoutedEventArgs>(AssociatedObject, Event)
                  .Subscribe(se => FireCommand());
            }

            base.OnAttached();
        }

        private void FireCommand()
        {
            var dataContext = AssociatedObject.DataContext;
            var dcType = dataContext.GetType();
            var commandGetter = dcType.GetRuntimeMethod("get_" + Command, new Type[0]);
            if (commandGetter != null)
            {
                var command = commandGetter.Invoke(dataContext, null) as ICommand;
                if (command != null)
                {
                    command.Execute(CommandParameter);
                }
            }
        }

        public const string EventPropertyName = "Event";

        public const string CommandPropertyName = "Command";


        public const string CommandParameterPropertyName = "CommandParameter";

        public static readonly DependencyProperty EventProperty = DependencyProperty.Register(EventPropertyName,
                                                                                              typeof (string),
                                                                                              typeof (EventToCommandBehavior),
                                                                                              new PropertyMetadata(default(string))
                                                                                        );

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(CommandPropertyName,
                                                                                                typeof (string),
                                                                                                typeof (EventToCommandBehavior),
                                                                                                new PropertyMetadata(default(string)));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
                                                                    CommandParameterPropertyName,
                                                                    typeof (object),
                                                                    typeof (EventToCommandBehavior),
                                                                    new PropertyMetadata(default(object)));

        public string Event
        {
            get { return (string) GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }

        public string Command
        {
            get { return (string) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
