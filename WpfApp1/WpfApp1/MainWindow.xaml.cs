using Microsoft.Win32;
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
using ZJYC;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        GUI Gui = new GUI();

        public void UpdateListView()
        {
            this.Dispatcher.BeginInvoke((Action)delegate () { GUI_Listview.ItemsSource = null; });
            ShowCount.Text = Gui.ImportItems().ToString();
            this.Dispatcher.BeginInvoke((Action)delegate () { GUI_Listview.ItemsSource = Gui.Show; });
        }

        public MainWindow()
        {
            InitializeComponent();
            FilterMode.SelectedIndex = 0;
            FilterMode.ItemsSource = Gui.Filter.SupportedMode;
            SorterMode.SelectedIndex = 0;
            SorterMode.ItemsSource = Gui.Sorter.SupportedMode;
            SelectMode.SelectedIndex = 0;
            SelectMode.ItemsSource = Gui.Select.SupportedMode;
            ItemsKeys.SelectedIndex = 0;
            ItemsKeys.ItemsSource = Gui.Items.GetAllKey();
            UpdateListView();
        }

        private void ItemsAdd_Click(object sender, RoutedEventArgs e)
        {
            ITEM NewItem = new ITEM();
            NewItem.MainBody[(string)ItemsKeys.SelectedValue] = TheValOfKey.Text;
            Gui.Items.Add(NewItem);
            UpdateListView();
            Gui.Items.Writ();
            Gui.Items.Read();
        }

        private void ItemsMod_Click(object sender, RoutedEventArgs e)
        {
            if (Gui.CurSelectedIndex < 0) return;
            string Key = (string)ItemsKeys.SelectedValue;
            string Val = TheValOfKey.Text;
            Gui.Items.Mod(Gui.Internal[Gui.CurSelectedIndex].UniqueID, new List<string>() { Key }, new List<string>() { Val });
            UpdateListView();
            Gui.Items.Writ();
            Gui.Items.Read();
        }

        private void GUI_Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(GUI_Listview.SelectedValue != null)
            {
                Gui.CurSelectedIndex = GUI_Listview.SelectedIndex;

                string Temp = "";
                if (Gui.Internal[Gui.CurSelectedIndex].MainBody.TryGetValue((string)ItemsKeys.SelectedValue, out Temp))
                {
                    TheValOfKey.Text = Temp;
                }

            }
        }

        private void ItemsDel_Click(object sender, RoutedEventArgs e)
        {
            if (Gui.CurSelectedIndex < 0) return;
            Gui.Items.Del(Gui.Internal[Gui.CurSelectedIndex].UniqueID);
        }

        private void OpenDicButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = "请打开Json文件";
            fileDialog.Filter = "Json|*.json;";
            if (fileDialog.ShowDialog() == true)
            {
                DicName.Text = fileDialog.FileName;
                Gui.Items.JsonFileName = DicName.Text;
                Gui.Items.Read();
                UpdateListView();
            }
        }

        private void CreateNewDicButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Json files (*.json)|*.json";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.FileName.Length > 0)
            {
                DicName.Text = saveFileDialog1.FileName;
                Gui.Items.Writ();
                Gui.Items.CreatJsonWithSomeContent(DicName.Text);
                UpdateListView();
            }
        }

        private void ImportFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = "请打开Txt文件";
            fileDialog.Filter = "txt|*.txt;";
            if (fileDialog.ShowDialog() == true)
            {
                ImExportFileName.Text = fileDialog.FileName;
                Gui.Import.WORK(ImExportFileName.Text, DicName.Text);
                UpdateListView();
            }
        }

        private void ExportFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.FileName.Length > 0)
            {
                ImExportFileName.Text = saveFileDialog1.FileName;
                Gui.Export.WORK(ref Gui.Internal, ImExportFileName.Text, DicName.Text);
            }
        }

        private void Keyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            Gui.Search.Keyword = Keyword.Text;
        }

        private void RefreshAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateListView();
        }
    }
}
