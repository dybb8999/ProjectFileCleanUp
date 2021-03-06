﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectFileCleanUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ModeView.MainWindowModeView m_pModeView = new ModeView.MainWindowModeView();
        public MainWindow()
        {
            InitializeComponent();

            DataGrid.DataContext = m_pModeView;

            LoadScanPath();
            LoadScanType();
            ScanTypeCheckChanged(null, null);
        }


        private void IsFullCheckbox_Click(object sender, RoutedEventArgs e)
        {
            var scanTypes = m_pModeView.ScanTypeList;

            bool? bRet = false;
            do
            {
                if (scanTypes == null)
                {
                    break;
                }

                var collect = from scanType in scanTypes where scanType.IsUse == true select scanType;
                if (collect.Count() == scanTypes.Count)
                {
                    bRet = true;
                }
                else if (collect.Count() == 0)
                {
                    bRet = false;
                }
                else
                {
                    bRet = null;
                }
            } while (false);

            m_pModeView.IsFullSelect = bRet;
        }

#nullable enable
        private void ScanTypeCheckChanged(object? sender, PropertyChangedEventArgs e)
        {
            IsFullCheckbox_Click(null, null);
        }
#nullable disable

        private void OnStartScan(object sender, RoutedEventArgs e)
        {
            m_pModeView.IsScanning = true;
            m_pModeView.ScanFileList.Clear();

            string strScanBasePath = m_pModeView.ScanPath;
            
            List<ModeView.ScanTypeModeView> scanTypes = new List<ModeView.ScanTypeModeView>();

            var scanTypeCollect = from scanType in m_pModeView.ScanTypeList where scanType.IsUse == true select scanType;
            scanTypes.AddRange(scanTypeCollect);

            Task.Factory.StartNew(()=> {
                ObservableCollection<ModeView.FileItemModeView> collect = new ObservableCollection<ModeView.FileItemModeView>();
                try
                {
                    collect = ScanFile(strScanBasePath, scanTypes);
                }
                catch (Exception)
                {

                }
                
                this.Dispatcher.Invoke(()=> {
                    foreach (var item in collect)
                    {
                        m_pModeView.ScanFileList.Add(item);
                    }
                    m_pModeView.IsScanning = false;
                });
            });

            SaveScanType();
            SaveScanPath();
            LoadScanPath();
            if(m_pModeView.HistoryScanPath.Count > 0)
            {
                m_pModeView.ScanPath = m_pModeView.HistoryScanPath[0];
            }
            
        }

        private ObservableCollection<ModeView.FileItemModeView> ScanFile(string strBaseDir, List<ModeView.ScanTypeModeView> scanType)
        {
            ObservableCollection<ModeView.FileItemModeView> scanFiles = new ObservableCollection<ModeView.FileItemModeView>();

            //检查路径结尾是不是\符号
            strBaseDir = strBaseDir.Replace('\\', '/');
            var pathSplit = strBaseDir.Split("/");
            if(pathSplit[pathSplit.Length - 1].Length == 0)
            {
                strBaseDir = strBaseDir[0..^1];
            }

            DirectoryInfo pDirInfo = new DirectoryInfo(strBaseDir);
            foreach (var dirItem in pDirInfo.GetDirectories())
            {
                ModeView.FileItemModeView fileItem = new ModeView.FileItemModeView();
                fileItem.Name = dirItem.FullName;
                fileItem.FullName = dirItem.FullName;
                fileItem.Attributes = FileAttributes.Directory;

                if (CheckNameEqule(dirItem.Name, FileAttributes.Directory, scanType) == false)
                {
                    string strNewBasePath = dirItem.FullName;
                    fileItem.FileItems = ScanFile(strNewBasePath, scanType);
                    if(fileItem.FileItems.Count > 0)
                    {
                        scanFiles.Add(fileItem);
                    }
                }
                else
                {
                    fileItem.IsSelect = true;
                    scanFiles.Add(fileItem);
                }
            }

            foreach (var fItem in pDirInfo.GetFiles())
            {
                if(CheckNameEqule(fItem.Name, FileAttributes.Normal, scanType) == false)
                {
                    continue;
                }

                ModeView.FileItemModeView fileItem = new ModeView.FileItemModeView();
                fileItem.Name = fItem.Name;
                fileItem.FullName = fItem.FullName;
                fileItem.Attributes = FileAttributes.Normal;
                fileItem.IsSelect = true;
                scanFiles.Add(fileItem);
            }

            return scanFiles;
        }

        private bool CheckNameEqule(string name, FileAttributes fileAttributes, List<ModeView.ScanTypeModeView> scanType)
        {
            bool bRet = false;
            if(fileAttributes == FileAttributes.Directory)
            {
                //全部符合
                var folderTypeScan = from s in scanType where s.Type == ModeView.ScanType.Folder select s;
                foreach (var scanItem in folderTypeScan)
                {
                    if(name.Equals(scanItem.Data, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        bRet = true;
                        break;
                    }
                }
            }
            else
            {
                //以.xxxx结尾
                var fileTypeScan = from s in scanType where s.Type == ModeView.ScanType.Suffix select s;
                foreach (var scanItem in fileTypeScan)
                {
                    if (name.EndsWith(scanItem.Data, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        bRet = true;
                        break;
                    }
                }
            }

            return bRet;
        }

        private void OnDeleteFile(object sender, RoutedEventArgs e)
        {
            DeleteAllItem(m_pModeView.ScanFileList);
            m_pModeView.ScanFileList.Clear();
        }

        private void DeleteAllItem(ObservableCollection<ModeView.FileItemModeView> files)
        {
            foreach (var item in files)
            {
                if(item.Attributes == FileAttributes.Normal)
                {
                    FileInfo fileInfo = new FileInfo(item.FullName);
                    fileInfo.Delete();
                }
                else if(item.Attributes == FileAttributes.Directory)
                {
                    if(item.IsSelect == true)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(item.FullName);
                        DeleteAllFileInFolder(item.FullName);
                        directoryInfo.Delete();
                    }
                    else
                    {
                        DeleteAllItem(item.FileItems);
                    }
                }
            }
        }

        private void DeleteAllFileInFolder(string strBaseDir)
        {
            DirectoryInfo pDirInfo = new DirectoryInfo(strBaseDir);
            foreach(var fileItem in pDirInfo.GetFiles())
            {
                fileItem.Delete();
            }

            foreach (var dirItem in pDirInfo.GetDirectories())
            {
                string strNewBaseFolder = dirItem.FullName;
                DeleteAllFileInFolder(strNewBaseFolder);
                dirItem.Delete();
            }
        }

        private void OnAddScanType(object sender, RoutedEventArgs e)
        {
            ModeView.ScanType type = ModeView.ScanType.Unknow;
            switch (m_pModeView.ScanTypeSelect)
            {
                case "后缀":
                    type = ModeView.ScanType.Suffix;
                    break;

                case "文件夹":
                    type = ModeView.ScanType.Folder;
                    break;
            }

            //检测是否重复
            var repeatCheck = from scanType in m_pModeView.ScanTypeList where (scanType.Type == type) && scanType.Data.Equals(m_pModeView.Input, StringComparison.OrdinalIgnoreCase) == true select scanType;
            if(repeatCheck.Count() == 0)
            {
                m_pModeView.ScanTypeList.Add(new ModeView.ScanTypeModeView(false, m_pModeView.Input, type, ScanTypeCheckChanged));
                //IsFullCheckbox_Click(null, null);
            }
        }

        private void OnScanTypeListStatusChange(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            do
            {
                if(checkBox == null)
                {
                    break;
                }

                if(checkBox.IsChecked == true)
                {
                    foreach (var scanTypeItem in m_pModeView.ScanTypeList)
                    {
                        scanTypeItem.IsUse = true;
                    }
                }
                else
                {
                    foreach (var scanTypeItem in m_pModeView.ScanTypeList)
                    {
                        scanTypeItem.IsUse = false;
                    }
                    checkBox.IsChecked = false;
                }
            } while (false);
        }

        private void SaveScanPath()
        {
            Config.SaveScanPath(m_pModeView.ScanPath);
        }

        private void LoadScanPath()
        {
            var scanHistory = Config.LoadScanPath();

            m_pModeView.HistoryScanPath.Clear();
            foreach (var item in scanHistory)
            {
                m_pModeView.HistoryScanPath.Add(item);
            }
        }

        private void SaveScanType()
        {
            DateTimeOffset dto = new DateTimeOffset(DateTime.Now);
            var unixTime = dto.ToUnixTimeSeconds();

            ScanTypeJson scanTypeJson = new ScanTypeJson();
            scanTypeJson.Version = unixTime;

            scanTypeJson.ScanTypes.AddRange(m_pModeView.ScanTypeList);

            string strJson = JsonConvert.SerializeObject(scanTypeJson);
            Config.SaveScanType(strJson);
        }

        private void LoadScanType()
        {
            var strJson = Config.LoadScanType();
            SetScanTypeList(strJson);

        }

        private void OnUpdateScanTypes(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp("https://raw.githubusercontent.com/dybb8999/ProjectFileCleanUp/master/Config/ScanTypes.json");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader readStream = new StreamReader(response.GetResponseStream()))
                {
                    string strJson = readStream.ReadToEnd();
                    SetScanTypeList(strJson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetScanTypeList(string strJson)
        {
            try
            {
                ScanTypeJson scanTypeJson = JsonConvert.DeserializeObject<ScanTypeJson>(strJson);
                m_pModeView.ScanTypeList.Clear();
                if (scanTypeJson != null)
                {
                    foreach (var item in scanTypeJson.ScanTypes)
                    {
                        item.PropertyChanged += ScanTypeCheckChanged;
                        m_pModeView.ScanTypeList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnRemoteScanType(object sender, RoutedEventArgs e)
        {
            do
            {
                var pDeleteButton = sender as Button;
                if(pDeleteButton == null)
                {
                    break;
                }

                var pScanType = pDeleteButton.DataContext as ModeView.ScanTypeModeView;
                if(pScanType == null)
                {
                    break;
                }

                m_pModeView.ScanTypeList.Remove(pScanType);
            } while (false);
        }
    }
}
