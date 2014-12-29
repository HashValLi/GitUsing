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
using System.Windows.Shapes;
using TarQIE_Task;

namespace TarQIE_UI
{
    /// <summary>
    /// ToDoList.xaml 的交互逻辑
    /// </summary>
    public partial class ToDoList : Window
    {
        public List<weekTask> wTList;
        public List<baseTask> bTList;
        public List<baseTask> TDList;
        public ToDoList(List<weekTask> wTaskList,List<baseTask> bTaskList)
        {
            InitializeComponent();
            foreach (weekTask awT in wTaskList)
            {
                var newItem = new ComboBoxItem { Content = awT.taskID, FontSize = 20 };
                WTask.Items.Add(newItem);
            }
            wTList = wTaskList;
            bTList = bTaskList;
            TDList = new List<baseTask>();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // do things return and close
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // no return but close
        }

        private void WTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SubTaskList.Items.Clear();
            string taskID = (WTask.SelectedItem as ComboBoxItem).Content.ToString();
            int index = -1;
            foreach (weekTask aWT in wTList)
            {
                if (aWT.taskID == taskID)
                {
                    index = wTList.IndexOf(aWT);
                    break;
                }
            }
            foreach (baseTask aBT in wTList[index].subTaskList)
            {
                var newItem = new ListViewItem { Content = aBT.taskID, FontSize = 20 };
                SubTaskList.Items.Add(newItem);
            }
        }

        private void AddP_Click(object sender, RoutedEventArgs e)
        {
            if (SubTaskList.SelectedIndex == -1)
                MessageBox.Show("Please select a task.");
            else if (PTask.Items.Count < 3)
            {
                var tempItem = SubTaskList.SelectedItem as ListViewItem;
                TDList.Add(bTList.Find(x => x.taskID == tempItem.Content.ToString()));
                var newItem = new ListBoxItem { Content = tempItem.Content, FontSize = 20 };
                PTask.Items.Add(newItem as ListBoxItem);
            }
            else MessageBox.Show("Full");
        }

        private void AddS_Click(object sender, RoutedEventArgs e)
        {
            if (SubTaskList.SelectedIndex == -1)
                MessageBox.Show("Please select a task.");
            else if (STask.Items.Count < 3)
            {
                var tempItem = SubTaskList.SelectedItem as ListViewItem;
                TDList.Add(bTList.Find(x => x.taskID == tempItem.Content.ToString()));
                var newItem = new ListBoxItem { Content = tempItem.Content, FontSize = 20 };
                STask.Items.Add(newItem as ListBoxItem);
            }
            else MessageBox.Show("Full");
        }

        private void AddT_Click(object sender, RoutedEventArgs e)
        {
            if (SubTaskList.SelectedIndex == -1)
                MessageBox.Show("Please select a task.");
            else if (TTask.Items.Count < 3)
            {
                var tempItem = SubTaskList.SelectedItem as ListViewItem;
                TDList.Add(bTList.Find(x => x.taskID == tempItem.Content.ToString()));
                var newItem = new ListBoxItem { Content = tempItem.Content, FontSize = 20 };
                TTask.Items.Add(newItem as ListBoxItem);
            }
            else MessageBox.Show("Full");
        }
    }
}
