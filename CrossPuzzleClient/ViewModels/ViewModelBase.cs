using System.Collections.Generic;
using CrossPuzzleClient.Common;

namespace CrossPuzzleClient.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        public virtual void LoadState(object navParameter, Dictionary<string, object> viewModelState )
        {}
        public virtual void SaveState(Dictionary<string, object>  viewModelState)
        {}
    }
}