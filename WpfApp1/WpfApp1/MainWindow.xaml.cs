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
    }
}
