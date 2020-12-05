using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectFileCleanUp.ModeView
{
    class MainWindowModeView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool m_bIsScanning;
        string m_strScanPath = "";
        ObservableCollection<string> m_historyScanPath = new ObservableCollection<string>();
        bool? m_IsFullSelect = false;

        //扫描信息添加框
        string m_strInput = "";
        string m_strScanTypeSelect = "";
        ObservableCollection<ScanTypeModeView> m_scanTypeList = new ObservableCollection<ScanTypeModeView>();

        ObservableCollection<FileItemModeView> m_scanFileList = new ObservableCollection<FileItemModeView>();
        public MainWindowModeView()
        {

        }

        public bool IsScanning
        {
            get => m_bIsScanning;
            set
            {
                m_bIsScanning = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsScanning)));
            }
        }

        public string ScanPath
        {
            get => m_strScanPath;
            set
            {
                m_strScanPath = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScanPath)));
            }
        }

        public ObservableCollection<string> HistoryScanPath
        {
            get => m_historyScanPath;
            set
            {
                m_historyScanPath = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HistoryScanPath)));
            }
        }

        public string Input
        {
            get => m_strInput;
            set
            {
                m_strInput = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input)));
            }
        }

        public string ScanTypeSelect
        {
            get => m_strScanTypeSelect;
            set
            {
                m_strScanTypeSelect = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScanTypeSelect)));
            }
        }

        public ObservableCollection<ScanTypeModeView> ScanTypeList
        {
            get => m_scanTypeList;
            set
            {
                m_scanTypeList = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScanTypeList)));
            }
        }

        public bool? IsFullSelect
        {
            get => m_IsFullSelect;
            set
            {
                m_IsFullSelect = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFullSelect)));
            }
        }

        public ObservableCollection<FileItemModeView> ScanFileList
        {
            get => m_scanFileList;
            set
            {
                m_scanFileList = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScanFileList)));
            }
        }
    }
}
