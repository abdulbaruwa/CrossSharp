using Windows.UI.Xaml;

namespace CrossPuzzleClient.Behaviors
{
  /// <summary>
  /// Base class for behaviors
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Behavior<T> : CrossPuzzleClient.Behaviors.Behavior where T : FrameworkElement
  {
    protected Behavior()
    {
    }

    public T AssociatedObject
    {
      get
      {
        return (T)base.AssociatedObject;
      }
      set
      {
        base.AssociatedObject = value;
      }
    }
  }
}
