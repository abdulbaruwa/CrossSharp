using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace CrossPuzzleClient.Behaviors
{
    /// <summary>
    /// Attached dependency property storing 'behaviors'
    /// </summary>
    public static class Interaction
    {
        /// <summary>
        /// Dictionary containing the hash code of a collection and a weak reference to the owner of the collection. This way, the 
        /// <see cref="OnBehaviorsCollectionChanged"/> can be introcuded without causing memory leaks.
        /// </summary>
        private static readonly Dictionary<int, WeakReference<FrameworkElement>> _collectionOwners = new Dictionary<int, WeakReference<FrameworkElement>>(); 

        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof (ObservableCollection<CrossPuzzleClient.Behaviors.Behavior>),
            typeof (Interaction), new PropertyMetadata(DesignMode.DesignModeEnabled ? new ObservableCollection<CrossPuzzleClient.Behaviors.Behavior>() : null, BehaviorsChanged));


        /// <summary>
        /// Called when Property is retrieved
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Always)]
        public static ObservableCollection<CrossPuzzleClient.Behaviors.Behavior> GetBehaviors(DependencyObject obj)
        {
            var behaviors = obj.GetValue(BehaviorsProperty) as ObservableCollection<CrossPuzzleClient.Behaviors.Behavior>;
            if (behaviors == null)
            {
                behaviors = new ObservableCollection<CrossPuzzleClient.Behaviors.Behavior>();
                SetBehaviors(obj, behaviors);
            }

            return behaviors;
        }

        /// <summary>
        /// Called when Property is retrieved
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetBehaviors(DependencyObject obj, ObservableCollection<CrossPuzzleClient.Behaviors.Behavior> value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        /// <summary>
        /// Called when the property changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void BehaviorsChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var associatedObject = sender as FrameworkElement;
            if (associatedObject != null)
            {
                var oldList = args.OldValue as ObservableCollection<CrossPuzzleClient.Behaviors.Behavior>;
                if (oldList != null)
                {
                    foreach (var behavior in oldList)
                    {
                        behavior.AssociatedObject = null;
                    }

                    _collectionOwners.Remove(oldList.GetHashCode());
                    oldList.CollectionChanged -= OnBehaviorsCollectionChanged;
                }

                var newList = args.NewValue as ObservableCollection<CrossPuzzleClient.Behaviors.Behavior>;
                if (newList != null)
                {
                    foreach (var behavior in newList)
                    {
                        behavior.AssociatedObject = sender as FrameworkElement;
                    }

                    _collectionOwners.Add(newList.GetHashCode(), new WeakReference<FrameworkElement>(associatedObject));
                    newList.CollectionChanged += OnBehaviorsCollectionChanged;
                }
            }
        }

        private static void OnBehaviorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FrameworkElement associatedObject;
            if (!_collectionOwners[sender.GetHashCode()].TryGetTarget(out associatedObject))
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (CrossPuzzleClient.Behaviors.Behavior behavior in e.NewItems)
                        {
                            behavior.AssociatedObject = associatedObject;
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (CrossPuzzleClient.Behaviors.Behavior behavior in e.OldItems)
                        {
                            behavior.AssociatedObject = null;
                        }
                        break;
                    }
            }
        }
    }
}
