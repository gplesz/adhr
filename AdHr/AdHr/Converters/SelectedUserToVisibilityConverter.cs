using AdHr.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AdHr.Converters
{
    /// <summary>
    /// Objektum és ObjektumProperty érték konverziója láthatóságra.
    /// Ha nincs kiválasztva semmi, akkor a SelectedItem == null, és ezt is vizsgálni kell
    /// </summary>
    public class SelectedUserToVisibilityConverter : IValueConverter
    {
        private object GetVisibility(object value)
        {
            if (value==null || !(value is AdhrUserViewModel))
            {
                return Visibility.Hidden;
            }

            if (!((AdhrUserViewModel)value).IsDirty)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetVisibility(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
