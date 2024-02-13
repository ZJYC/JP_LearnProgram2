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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Gui.Items.Save();
        }
        public MainWindow()
        {
            //TEXT tEXT = new TEXT();
            //tEXT.Sim("きょうだい", "きょうだい");
            InitializeComponent();
            FilterMode.SelectedIndex = 0;
            FilterMode.ItemsSource = Gui.Filter.SupportedMode;
            SorterMode.SelectedIndex = 0;
            SorterMode.ItemsSource = Gui.Sorter.SupportedMode;
            SelectMode.SelectedIndex = 0;
            SelectMode.ItemsSource = Gui.Select.SupportedMode;
            ItemsKeys.SelectedIndex = 0;
            ItemsKeys.ItemsSource = Gui.Items.GetAllKey();
            MutilSeclectParam.ItemsSource = new List<string>() {
                "DicTyp","JPkana","JPProp","JPEmp1",
                "JPWord","JPUsed","CHWord","CHEmp1",
            };
            MutilSeclectParam.SelectedIndex = 0;

            MutilSeclectMode.ItemsSource = Gui.Mulits.SupportedMode;
            MutilSeclectMode.SelectedIndex = 0;

            NexusMode.ItemsSource = Gui.Nexusx.SupportedMode;
            NexusMode.SelectedIndex = 0;

            NexusParam.ItemsSource = Gui.Items.GetAllKey();
            NexusParam.SelectedIndex = 0;

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
            Gui.Items.Writ();
            Gui.Items.Read();
            UpdateListView();
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
                if (Gui.Items.JsonFileName != "")
                {
                    Gui.Items.Save();
                }
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

        private void MainTableControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MainTableControl.SelectedIndex != -1)
            {
                if(MainTableControl.SelectedIndex == 1)
                {
                    Gui.Mulits.ImportItems(ref Gui.Internal,(string)MutilSeclectMode.SelectedValue);
                }
            }
            UpdateListView();
        }

        #region 选择题

        private void MutilSeclectParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MutilSeclectParamShow.Text += (string)MutilSeclectParam.SelectedValue + "\r\n";
        }

        private void MutilSeclectClearShow_Click(object sender, RoutedEventArgs e)
        {
            MutilSeclectParamShow.Text = "";
        }

        private void MutilSeclectAdd2QSButton_Click(object sender, RoutedEventArgs e)
        {
            MutilSeclectQSParam.Text = MutilSeclectParamShow.Text;
            MutilSeclectParamShow.Text = "";
        }

        private void MutilSeclectAdd2ASButton_Click(object sender, RoutedEventArgs e)
        {
            MutilSeclectASParam.Text = MutilSeclectParamShow.Text;
            MutilSeclectParamShow.Text = "";
        }

        private void MutilSeclectAdd2KQButton_Click(object sender, RoutedEventArgs e)
        {
            MutilSeclectKQParam.Text = MutilSeclectParamShow.Text;
            MutilSeclectParamShow.Text = "";
        }


        private void MutilSeclectBegin_Click(object sender, RoutedEventArgs e)
        {
            MutilSeclectRes.Text = "";

            MutilSeclectAnsTxt1.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x04, 0xe0, 0xae));
            MutilSeclectAnsTxt2.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x04, 0xe0, 0xae));
            MutilSeclectAnsTxt3.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x04, 0xe0, 0xae));
            MutilSeclectAnsTxt4.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x04, 0xe0, 0xae));

            List<string> KeyForQuestion = MutilSeclectKQParam.Text.Replace("\r\n", "").Split().ToList();
            List<string> ShowForQuestion = MutilSeclectQSParam.Text.Replace("\r\n"," ").Split().ToList();
            List<string> ShowForAnswer = MutilSeclectASParam.Text.Replace("\r\n", " ").Split().ToList();
            List<string> ShowForRes = MutilSeclectResShow.Text.Replace("\r\n", " ").Split().ToList();

            for (int i = 0; i < KeyForQuestion.Count;)
            {
                if (KeyForQuestion[i] == "")
                {
                    KeyForQuestion.Remove(KeyForQuestion[i]);
                }
                else
                {
                    i++;
                }
            }
            for (int i = 0; i < ShowForQuestion.Count;)
            {
                if (ShowForQuestion[i] == "")
                {
                    ShowForQuestion.Remove(ShowForQuestion[i]);
                }
                else
                {
                    i++;
                }
            }
            for (int i = 0; i < ShowForAnswer.Count;)
            {
                if (ShowForAnswer[i] == "")
                {
                    ShowForAnswer.Remove(ShowForAnswer[i]);
                }
                else
                {
                    i++;
                }
            }
            for (int i = 0; i < ShowForRes.Count;)
            {
                if (ShowForRes[i] == "")
                {
                    ShowForRes.Remove(ShowForRes[i]);
                }
                else
                {
                    i++;
                }
            }

            Gui.Mulits.GenOneQuestion(ShowForAnswer, ShowForQuestion, ShowForAnswer, ShowForRes);
            MutilSeclectQuestion.Text = Gui.Mulits.Out.Question;
            MutilSeclectAnsTxt1.Text = Gui.Mulits.Out.Options[0];
            MutilSeclectAnsTxt2.Text = Gui.Mulits.Out.Options[1];
            MutilSeclectAnsTxt3.Text = Gui.Mulits.Out.Options[2];
            MutilSeclectAnsTxt4.Text = Gui.Mulits.Out.Options[3];

            MutilSeclectRemainCount.Text = Gui.Mulits.GetRemain().ToString();

        }

        public bool JudgeUsersSelection(ref TextBlock Text)
        {
            Gui.Mulits.Out.UserSelected = Text.Text;
            MutilSeclectRes.Text = Gui.Mulits.Out.Result;
            if (Gui.Mulits.JudgeTheAnswer() == true)
            {
                Text.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x79, 0xFF, 0x00));
                return true;
            }
            else
            {
                Text.Background = new SolidColorBrush(Colors.OrangeRed);
                return false;
            }
        }

        private void MutilSeclectAnsButton1_Click(object sender, RoutedEventArgs e)
        {
            JudgeUsersSelection(ref MutilSeclectAnsTxt1);
        }

        private void MutilSeclectAnsButton2_Click(object sender, RoutedEventArgs e)
        {
            JudgeUsersSelection(ref MutilSeclectAnsTxt2);
        }

        private void MutilSeclectAnsButton3_Click(object sender, RoutedEventArgs e)
        {
            JudgeUsersSelection(ref MutilSeclectAnsTxt3);
        }

        private void MutilSeclectAnsButton4_Click(object sender, RoutedEventArgs e)
        {
            JudgeUsersSelection(ref MutilSeclectAnsTxt4);
        }


        #endregion

        private void FilterMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gui.Filter.Mode = (string)FilterMode.SelectedValue;
        }

        private void SorterMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gui.Sorter.Mode = (string)SorterMode.SelectedValue;
        }

        private void SelectMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gui.Select.Mode = (string)SelectMode.SelectedValue;
        }

        private void FilterParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            Gui.Filter.Param = FilterParam.Text;
        }

        private void SelectParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            Gui.Select.Param = SelectParam.Text;
        }

        private void MutilSeclectAdd2RSButton_Click(object sender, RoutedEventArgs e)
        {
            MutilSeclectResShow.Text = MutilSeclectParamShow.Text;
            MutilSeclectParamShow.Text = "";
        }

        private void NexusMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gui.Nexusx.ImportItems(ref Gui.Internal, (string)NexusMode.SelectedValue);
        }

        private void MutilSeclectMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Gui.Mulits.ImportItems(ref Gui.Internal, (string)MutilSeclectMode.SelectedValue);
        }

        private void NexusParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NexusParamShow.Text += (string)NexusParam.SelectedValue + "\r\n";
        }

        private void NexusAdd2MatchButton_Click(object sender, RoutedEventArgs e)
        {
            NexusMatchParam.Text = NexusParamShow.Text;
            NexusParamShow.Text = "";
        }

        private void NexusAdd2ResButton_Click(object sender, RoutedEventArgs e)
        {
            NexusResParam.Text = NexusParamShow.Text;
            NexusParamShow.Text = "";
        }

        private void NexusClearButton_Click(object sender, RoutedEventArgs e)
        {
            NexusParamShow.Text = "";
        }

        private void NexusBeginOrNextButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> KeyForMatch = NexusMatchParam.Text.Replace("\r\n", " ").Split().ToList();
            List<string> KeyForResut = NexusResParam.Text.Replace("\r\n", " ").Split().ToList();

            for (int i = 0; i < KeyForMatch.Count;)
            {
                if (KeyForMatch[i] == "")
                {
                    KeyForMatch.Remove(KeyForMatch[i]);
                }
                else
                {
                    i++;
                }
            }
            for (int i = 0; i < KeyForResut.Count;)
            {
                if (KeyForResut[i] == "")
                {
                    KeyForResut.Remove(KeyForResut[i]);
                }
                else
                {
                    i++;
                }
            }

            Gui.Nexusx.GetOneToShow(KeyForMatch, KeyForResut);
            MutilSeclectQuestion.Text = Gui.Mulits.Out.Question;
            NexusTitle.Text = Gui.Nexusx.Out.Title;
            NexusOptions1.Text = Gui.Nexusx.Out.Related[0];
            NexusOptions2.Text = Gui.Nexusx.Out.Related[1];
            NexusOptions3.Text = Gui.Nexusx.Out.Related[2];
            NexusOptions4.Text = Gui.Nexusx.Out.Related[3];
            NexusRemainCount.Text = Gui.Nexusx.GetRemain().ToString();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(MainTableControl.SelectedIndex == 1)
            {
                if (e.Key == Key.Enter || e.Key == Key.Space)
                {
                    MutilSeclectBegin_Click(null, null);
                }
                if(e.Key == Key.D1 || e.Key == Key.A) 
                {
                    MutilSeclectAnsButton1_Click(null,null);
                }
                if (e.Key == Key.D2 || e.Key == Key.S)
                {
                    MutilSeclectAnsButton2_Click(null, null);
                }
                if (e.Key == Key.D3 || e.Key == Key.D)
                {
                    MutilSeclectAnsButton3_Click(null, null);
                }
                if (e.Key == Key.D4 || e.Key == Key.F)
                {
                    MutilSeclectAnsButton4_Click(null, null);
                }
            }
            
        }

        private void MutilSeclectAdd2KAButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ItemsKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
