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
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace AnalysisJunYuDict
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string GetDataList1(string key)
        {
            if (key == "")
            {
                return "";
            }
            try
            {
                var list = SearchIndex.Search("term", key, out var recCount, out var eventTime);
                string str = "";
                if (list == null)
                {
                    str = "";
                }
                else
                {
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 >= list.Count)
                        {
                            str = str ?? "";
                            break;
                        }
                        string str3 = str;
                        var strArray = new string[] { str3, list[num3].English + "\r\n" + list[num3].Chinese + "\r\n" + list[num3].Explain + "\r\n" };
                        str = string.Concat(strArray);
                        num3++;
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string GetDataList2(string key)
        {
            if (key == "")
            {
                return "";
            }
            try
            {
                var list = SearchIndex.Search("acronym", key, out var recCount, out var eventTime);
                string str = "";
                if (list == null)
                {
                    str = "";
                }
                else
                {
                    int num3 = 0;
                    while (true)
                    {
                        if (num3 >= list.Count)
                        {
                            str = str ?? "";
                            break;
                        }
                        string str3 = str;
                        var strArray = new string[] { str3, "\r\n" + list[num3].Acronym + "\r\n" + list[num3].English + "\r\n" + list[num3].Chinese + "\r\n" };
                        str = string.Concat(strArray);
                        num3++;
                    }
                }
                return str;
            }
            catch
            {
                return "";
            }
        }

        private void GetDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var dataList1 = GetDataList1("c");
            //var dataList2 = GetDataList2("cw");
            var items = SearchIndex.GetAllItems();
            ResultListBox.ItemsSource = items;
        }
        private void CopyData0Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.ItemsSource is List<SearchItem> items)
            {
                var list = items.Select(i => i.TableName).ToList();
                var text = string.Join("\r\n", list);
                Clipboard.SetText(text);
            }
        }
        private void CopyData1Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.ItemsSource is List<SearchItem> items)
            {
                var list = items.Select(i => i.Acronym).ToList();
                var text = string.Join("\r\n", list);
                Clipboard.SetText(text);
            }
        }
        private void CopyData2Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.ItemsSource is List<SearchItem> items)
            {
                var list = items.Select(i => i.English).ToList();
                var text = string.Join("\r\n", list);
                Clipboard.SetText(text);
            }
        }
        private void CopyData3Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.ItemsSource is List<SearchItem> items)
            {
                var list = items.Select(i => i.Chinese).ToList();
                var text = string.Join("\r\n", list);
                Clipboard.SetText(text);
            }
        }
        private void CopyData4Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.ItemsSource is List<SearchItem> items)
            {
                var list = items.Select(i => i.Explain).ToList();
                var text = string.Join("\r\n", list);
                Clipboard.SetText(text);
            }
        }


    }
}
