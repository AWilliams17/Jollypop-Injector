using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jollypop_Injector.Classes
{
    // Taken from https://www.codeproject.com/Questions/1101746/Need-to-enable-the-button-when-all-textbox-has-val
    public class HasAllTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool res = true;

            foreach (object val in values)
            {
                if (string.IsNullOrEmpty(val as string))
                {
                    res = false;
                }
            }

            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
