using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProjectFileCleanUp.ValueConvert
{
    class FileTypeToImageValueConvert : IValueConverter
    {
        public System.Windows.Media.ImageSource FileImage { get; set; }
        public System.Windows.Media.ImageSource FolderImage { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.ImageSource imageSource = null;
            var item = (System.IO.FileAttributes)value;
            do
            {
                if((item | System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
                {
                    imageSource = FolderImage;
                }
                else
                {
                    imageSource = FileImage;
                }
            } while (false);
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
