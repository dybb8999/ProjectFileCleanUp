using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectFileCleanUp.ValueConvert
{
    class EnableStatusValueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bRet = false;
            string strParameter = (string)parameter;
            if(strParameter.Equals("runningdisable", StringComparison.OrdinalIgnoreCase) == true)
            {
                bRet = !(bool)value;
            }
            else
            {
                bRet = (bool)value;
            }
            return bRet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
