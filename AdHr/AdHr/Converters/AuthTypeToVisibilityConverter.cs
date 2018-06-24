using AdHr.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AdHr.Converters
{
    public class AuthTypeToVisibilityConverter : IValueConverter
    {

        private object GetVisibility(object value)
        {
            if (!(value is AuthTypes))
                return DependencyProperty.UnsetValue;
            var objValue = (AuthTypes)value;
            if (objValue==AuthTypes.WindowsAuthentication)
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
