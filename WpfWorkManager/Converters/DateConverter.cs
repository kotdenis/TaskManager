using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkManager.Converters
{
    public class DateConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime temp = (DateTime)value;
                string date = temp.ToString("dd.MM.yyyy");
                return (date);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return DateTime.Parse(value.ToString());
            }
            catch (Exception ex) { return null; }
        }
    }
}
