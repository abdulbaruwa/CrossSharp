using System;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CrossPuzzleClient.Controls
{
    public class CustomStackPanal : StackPanel
    {
        internal String ContentText { get; set; }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new CustomStackPanelAutomationPeer(this);
        }
        protected void OnKeyDown(KeyRoutedEventArgs e)
        {
            ContentText = "A key was pressed @ " + DateTime.Now.ToString();
        }
    }
}