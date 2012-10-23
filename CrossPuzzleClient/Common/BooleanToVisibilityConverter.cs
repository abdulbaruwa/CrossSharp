using System;
using CrossPuzzleClient.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CrossPuzzleClient.Common
{
    /// <summary>
    /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
    /// <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }


    public sealed  class BooleanToCellShadeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var enabledcolor = new SolidColorBrush(Colors.WhiteSmoke);
            var disabledcolor = new SolidColorBrush(Colors.AntiqueWhite);
            var activecolor = new SolidColorBrush(Colors.LightGray);
            var errorcolor = new SolidColorBrush(Colors.LightSalmon);

            if (value is CellState )
            {
                if((CellState)value == CellState.IsEmpty) return disabledcolor;
                if((CellState)value == CellState.IsUsed) return enabledcolor;
                if((CellState)value == CellState.IsActive) return activecolor;
                if((CellState)value == CellState.IsError) return errorcolor;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
