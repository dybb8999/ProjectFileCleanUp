using ProjectFileCleanUp.ModeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectFileCleanUp.ValueConvert
{
    class ScanTypeIsFullSelectValueConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<ScanTypeModeView> scanTypes = value as ObservableCollection<ScanTypeModeView>;
            bool? bRet = false;
            do
            {
                if(scanTypes == null)
                {
                    break;
                }

                var collect = from scanType in scanTypes where scanType.IsUse == true select scanType;
                if(collect.Count() == scanTypes.Count)
                {
                    bRet = true;
                }
                else if(collect.Count() == 0)
                {
                    bRet = false;
                }
                
            } while (false);

            return bRet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
