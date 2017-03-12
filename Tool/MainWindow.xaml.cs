using MahApps.Metro.Controls;
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
using TBS.Core.Common;
using Tools.Common;

namespace Tools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {

        }
        #region Rest Reqest

        public void OpenHyperlink(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (sender != null)
            {
                string url = ObjectHelper.ToString(btn.Tag);
                if (!string.IsNullOrEmpty(url))
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
        }

        private void btnRestTester_Reqest_Click(object sender, RoutedEventArgs e)
        {

            txtRestResponseContent.Document.Blocks.Clear();

            try
            {
                txtRestResponseContent.AppendText(RestConnector.Connect(txtRestUrl.Text,
                    GetRichTextBoxText(txtRestData)));
            }
            catch (Exception ex){
                txtRestResponseContent.AppendText(ex.Message);
            }
        }
        public string GetRichTextBoxText(RichTextBox richTextBox1)
        {
            TextRange textRange = new TextRange(richTextBox1.Document.ContentStart,
                richTextBox1.Document.ContentEnd);
            return textRange.Text;
        }
        #endregion
    }
}
