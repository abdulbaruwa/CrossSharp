using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CrossPuzzleClient.Behaviors
{
  /// <summary>
  /// Base class for behaviors. Do no use. Derive from Behavior&lt;T&gt; in stead "/>
  /// </summary>
  public abstract class Behavior : FrameworkElement
  {
    private FrameworkElement associatedObject;

    /// <summary>
    /// The associated object
    /// </summary>
    public FrameworkElement AssociatedObject
    {
      get
      {
        return associatedObject;
      }
      set
      {
        if (associatedObject != null)
        {
          OnDetaching();
        }
        DataContext = null;

        associatedObject = value;
        if (associatedObject != null)
        {
          // FIX LocalJoost 17-08-2012 moved ConfigureDataContext to OnLoaded
          // to prevent the app hanging on a behavior attached to an element#
          // that's not directly loaded (like a FlipViewItem)
          OnAttached();
        }
      }
    }

    protected virtual void OnAttached()
    {
      AssociatedObject.Unloaded += AssociatedObjectUnloaded;
      AssociatedObject.Loaded += AssociatedObjectLoaded;
    }

    protected virtual void OnDetaching()
    {
      AssociatedObject.Unloaded -= AssociatedObjectUnloaded;
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
    }

    private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      ConfigureDataContext();
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
    {
      OnDetaching();
    }

    /// <summary>
    /// Configures data context. 
    /// Courtesy of Filip Skakun
    /// http://twitter.com/xyzzer
    /// </summary>
    private async void ConfigureDataContext()
    {
      while (associatedObject != null)
      {
        if (AssociatedObjectIsInVisualTree)
        {
          Debug.WriteLine(associatedObject.Name + " found in visual tree");
          SetBinding(
              DataContextProperty,
              new Binding
              {
                Path = new PropertyPath("DataContext"),
                Source = associatedObject
              });

          return;
        }
        Debug.WriteLine(associatedObject.Name + " Not in visual tree");
        await WaitForLayoutUpdateAsync();
      }
    }

    /// <summary>
    /// Checks if object is in visual tree
    /// Courtesy of Filip Skakun
    /// http://twitter.com/xyzzer
    /// </summary>
    private bool AssociatedObjectIsInVisualTree
    {
      get
      {
        if (associatedObject != null)
        {
          return Window.Current.Content != null && Ancestors.Contains(Window.Current.Content);
        }
        return false;
      }
    }

    /// <summary>
    /// Finds the object's associatedobject's ancestors
    /// Courtesy of Filip Skakun
    /// http://twitter.com/xyzzer
    /// </summary>
    private IEnumerable<DependencyObject> Ancestors
    {
      get
      {
        if (associatedObject != null)
        {
          var parent = VisualTreeHelper.GetParent(associatedObject);

          while (parent != null)
          {
            yield return parent;
            parent = VisualTreeHelper.GetParent(parent);
          }
        }
      }
    }

    /// <summary>
    /// Creates a task that waits for a layout update to complete
    /// Courtesy of Filip Skakun
    /// http://twitter.com/xyzzer
    /// </summary>
    /// <returns></returns>
    private async Task WaitForLayoutUpdateAsync()
    {
      await EventAsync.FromEvent<object>(
          eh => associatedObject.LayoutUpdated += eh,
          eh => associatedObject.LayoutUpdated -= eh);
    }
  }
}
