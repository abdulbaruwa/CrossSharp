using System;
using System.Collections.Generic;
using CrossPuzzleClient.Common;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CrossPuzzleClient.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PuzzleBoard : LayoutAwarePage
    {
        public PuzzleBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            var keyint = (int) e.Key;
            if (keyint >= 60 && keyint <= 90)
            {
                string keyname = Enum.GetName(typeof (VirtualKey), e.Key);
                Messenger.Default.Send(new KeyReceivedMessage {KeyChar = keyname});
            }
            else if (keyint == 46)
            {
                Messenger.Default.Send(new KeyReceivedMessage {KeyCharType = KeyCharType.Delete});
            }
            else if (keyint == 8)
            {
                Messenger.Default.Send(new KeyReceivedMessage {KeyCharType = KeyCharType.BackSpace});
            }

            base.OnKeyUp(e);
        }
    }
}