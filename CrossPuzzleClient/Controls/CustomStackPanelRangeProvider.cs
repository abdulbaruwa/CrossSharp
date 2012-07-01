using System;
using Windows.Foundation;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Automation.Text;

namespace CrossPuzzleClient.Controls
{

    /// <summary>
    /// A minimal implementation of ITextRangeProvider, used by CustomStackPanelAutomationPeer
    /// A real implementation is beyond the scope of this sample
    /// </summary>
    public sealed class CustomStackPanelRangeProvider : ITextRangeProvider
    {
        private String _text;
        private CustomStackPanelAutomationPeer _peer;

        public CustomStackPanelRangeProvider(String text, CustomStackPanelAutomationPeer peer)
        {
            _text = text;
            _peer = peer;
        }

        public void AddToSelection()
        {

        }

        public ITextRangeProvider Clone()
        {
            return new CustomStackPanelRangeProvider(_text, _peer);
        }

        public bool Compare(ITextRangeProvider other)
        {
            return true;
        }

        public int CompareEndpoints(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint)
        {
            return 0;
        }

        public void ExpandToEnclosingUnit(TextUnit unit)
        {

        }

        public ITextRangeProvider FindAttribute(int attribute, Object value, bool backward)
        {
            return this;
        }

        public ITextRangeProvider FindText(String text, bool backward, bool ignoreCase)
        {
            return this;
        }

        public Object GetAttributeValue(int attribute)
        {
            return this;
        }

        public void GetBoundingRectangles(out double[] rectangles)
        {
            rectangles = new double[0];
        }

        public IRawElementProviderSimple[] GetChildren()
        {
            return new IRawElementProviderSimple[0];
        }

        public IRawElementProviderSimple GetEnclosingElement()
        {
            return _peer.GetRawElementProviderSimple();
        }

        public String GetText(int maxLength)
        {
            return (maxLength < 0) ? _text : _text.Substring(0, maxLength);
        }

        public int Move(TextUnit unit, int count)
        {
            return 0;
        }

        public void MoveEndpointByRange(TextPatternRangeEndpoint endpoint, ITextRangeProvider targetRange, TextPatternRangeEndpoint targetEndpoint)
        {

        }

        public int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count)
        {
            return 0;
        }

        public void RemoveFromSelection()
        {

        }

        public void ScrollIntoView(bool alignToTop)
        {

        }

        public void Select()
        {

        }
    }

    /// <summary>
    /// Automation Peer class for CustomStackPanel2.  
    /// 
    /// Note: The difference between this and CustomControl1AutomationPeer is that this one implements
    /// Text Pattern (ITextProvider) and Value Pattern (IValuePattern) interfaces.  So Touch keyboard shows 
    /// automatically when user taps on the control with Touch or Pen.
    /// </summary>
    public class CustomStackPanelAutomationPeer : FrameworkElementAutomationPeer, ITextProvider, IValueProvider
    {
        private CustomStackPanal customStackPanel;
        private string accClass = "CustomStackPanelClass";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public CustomStackPanelAutomationPeer(CustomStackPanal owner)
            : base(owner)
        {
            this.customStackPanel = owner;
        }

        /// <summary>
        /// Override GetPatternCore to return the object that supports the specified pattern.  In this case the Value pattern, Text
        /// patter and any base class patterns.
        /// </summary>
        /// <param name="patternInterface"></param>
        /// <returns>the object that supports the specified pattern</returns>
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Value)
            {
                return this;
            }
            else if (patternInterface == PatternInterface.Text)
            {
                return this;
            }
            return base.GetPatternCore(patternInterface);
        }

        /// <summary>
        /// Override GetClassNameCore and set the name of the class that defines the type associated with this control.
        /// </summary>
        /// <returns>The name of the control class</returns>
        protected override string GetClassNameCore()
        {
            return this.accClass;
        }

        #region Implementation for ITextPattern interface
        // Complete implementation of the ITextPattern is beyond the scope of this sample.  The implementation provided
        // is specific to this sample's custom control, so it is unlikely that they are directly transferable to other 
        // custom control.

        ITextRangeProvider ITextProvider.DocumentRange
        {
            // A real implementation of this method is beyond the scope of this sample.
            // If your custom control has complex text involving both readonly and non-readonly ranges, 
            // it will need a smarter implementation than just returning a fixed range
            get { return new CustomStackPanelRangeProvider(customStackPanel.ContentText, this); ; }
        }

        ITextRangeProvider[] ITextProvider.GetSelection()
        {
            return new ITextRangeProvider[0];
        }

        ITextRangeProvider[] ITextProvider.GetVisibleRanges()
        {
            ITextRangeProvider[] ret = new ITextRangeProvider[1];
            ret[0] = new CustomStackPanelRangeProvider(customStackPanel.ContentText, this);
            return ret;
        }

        ITextRangeProvider ITextProvider.RangeFromChild(IRawElementProviderSimple childElement)
        {
            return new CustomStackPanelRangeProvider(customStackPanel.ContentText, this);
        }

        ITextRangeProvider ITextProvider.RangeFromPoint(Point screenLocation)
        {
            return new CustomStackPanelRangeProvider(customStackPanel.ContentText, this);
        }

        SupportedTextSelection ITextProvider.SupportedTextSelection
        {
            get { return SupportedTextSelection.Single; }
        }

        #endregion

        #region Implementation for IValueProvider interface
        // Complete implementation of the IValueProvider is beyond the scope of this sample.  The implementation provided
        // is specific to this sample's custom control, so it is unlikely that they are directly transferable to other 
        // custom control.

        /// <summary>
        /// The value needs to be false for the Touch keyboard to be launched automatically because Touch keyboard
        /// does not appear when the input focus is in a readonly UI control.
        /// </summary>
        bool IValueProvider.IsReadOnly
        {
            get { return false; }
        }

        void IValueProvider.SetValue(string value)
        {
            customStackPanel.ContentText = value;
        }

        string IValueProvider.Value
        {
            get { return customStackPanel.ContentText; }
        }

        #endregion //Implementation for IValueProvider interface

        public IRawElementProviderSimple GetRawElementProviderSimple()
        {
            return ProviderFromPeer(this);
        }
    }
}