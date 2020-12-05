using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFileCleanUp.ModeView
{
    enum ScanType
    {
        Unknow,
        Suffix,
        Folder,
    }

    class ScanTypeModeView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool m_bIsUse = false;
        string m_strData = "";
        ScanType m_Type = ScanType.Unknow;

        public ScanTypeModeView()
        {

        }

        public ScanTypeModeView(bool IsUse, string data, ScanType type, PropertyChangedEventHandler propertyChangedEventHandler)
        {
            m_bIsUse = IsUse;
            m_strData = data;
            m_Type = type;
            PropertyChanged += propertyChangedEventHandler;
        }

        public bool IsUse
        {
            get => m_bIsUse;
            set
            {
                m_bIsUse = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsUse"));
            }
        }

        public string Data
        {
            get => m_strData;
            set
            {
                m_strData = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
            }
        }

        public ScanType Type
        {
            get => m_Type;
            set
            {
                m_Type = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScanType)));
            }
        }
    }
}
