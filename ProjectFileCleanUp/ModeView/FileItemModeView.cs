using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFileCleanUp.ModeView
{
    class FileItemModeView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string m_strName = "";
        ObservableCollection<FileItemModeView> m_fileItems = new ObservableCollection<FileItemModeView>();
        FileAttributes m_Attributes;

        public FileItemModeView()
        {

        }

        public FileItemModeView(string name, FileAttributes attributes)
        {
            m_strName = name;
            m_Attributes = attributes;
        }

        public string Name
        {
            get => m_strName;
            set
            {
                m_strName = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public ObservableCollection<FileItemModeView> FileItems
        {
            get => m_fileItems;
            set
            {
                m_fileItems = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileItems)));
            }
        }

        public FileAttributes Attributes
        {
            get => m_Attributes;
            set
            {
                m_Attributes = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Attributes)));
            }
        }
    }
}
