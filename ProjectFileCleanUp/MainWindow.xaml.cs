using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
