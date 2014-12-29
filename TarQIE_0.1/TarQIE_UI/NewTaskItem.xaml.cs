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
    /// NewTaskItem.xaml 的交互逻辑
    /// </summary>
    public partial class NewTaskItem : Window
    {
        public taskCollection taskData;
        public string selectType;
        public string newTaskID{ get; set; }
        public string newTaskEP { get; set; }
        public string newTaskContent { get; set; }
        public string newTaskSTime { get; set; }
        public DateTime newTaskDT { get; set; }
        public string fatherTaskID { get; set; }

        public NewTaskItem(object sender)
        {
            taskData = sender as taskCollection;
            InitializeComponent();
            newTaskID = "";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                buildTask();
                this.Close();
            }
            // do things return and close
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // no return but close
        }

        public void TType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectItem = TType.SelectedItem as ComboBoxItem;
            selectType = selectItem.Content.ToString();
            switch (selectType)
            {
                case " Base":
                    BtoTask.Items.Clear();
                    foreach (weekTask aBTask in taskData.weekTaskList)
                    {
                        var newItem = new ComboBoxItem()
                        {
                            Content = aBTask.taskID,
                            FontSize = 20
                        };
                        BtoTask.Items.Add(newItem);
                    }
                    break;
                case " Week":
                    BtoTask.Items.Clear();
                    foreach (monthTask aWTask in taskData.monthTaskList)
                    {
                        var newItem = new ComboBoxItem()
                        {
                            Content = aWTask.taskID,
                            FontSize = 20
                        };
                        BtoTask.Items.Add(newItem);
                    }
                    break;
                case " Month":
                    BtoTask.Items.Clear();
                    break;
                default:
                    BtoTask.Items.Clear();
                    break;
            }
        }

        public bool CheckData()
        {
            if (TType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Task Type.");
                return false;
            }
            TType.Items.Refresh();
            if (BtoTask.SelectedIndex == -1 && BtoTask.Items.Count != 0)
            {
                MessageBox.Show("Please select Father Task");
                return false;
            }
            if (EPoint.Text.Length == 0)
            {
                MessageBox.Show("Invalid EPoint, please input between 0 and 1.");
                return false;
            }
            if (Convert.ToDouble(EPoint.Text) > 1.01 || Convert.ToDouble(EPoint.Text) < 0)
            {
                MessageBox.Show("Invalid EPoint, please input between 0 and 1.");
                return false;
            }
            if (STime.Text.Length == 0 && UNowtime.IsChecked == false)
            {
                MessageBox.Show("Invalid StartTime,please input StartTime like "
                    + DateTime.Now.ToShortDateString() +".\nOr using Today Date Button.");
                return false;
            }
            DateTime dateValue;
            if (!DateTime.TryParse(STime.Text, out dateValue) && UNowtime.IsChecked == false)
            {
                MessageBox.Show("Invalid StartTime,please input StartTime like "
                    + DateTime.Now.ToShortDateString() + ".\nOr using Today Date Button.");
                return false;
            }
            if (DateTime.Compare(dateValue, taskData.startTime) < 0 && UNowtime.IsChecked == false)
            {
                MessageBox.Show("Invalid StartTime,please input StartTime after Task's startTime");
                return false;
            }            
            return true;
        }
        
        public void buildTask()
        {
            newTaskEP = EPoint.Text;
            newTaskContent = TContent.Text;
            if (UNowtime.IsChecked == true)
            {
                newTaskSTime = DateTime.Now.ToShortDateString();
            }
            else 
            {
                newTaskSTime = STime.Text;
            }
            newTaskDT = Convert.ToDateTime(newTaskSTime);
            ComboBoxItem selectItem = TType.SelectedItem as ComboBoxItem;
            selectType = selectItem.Content.ToString();
            
            switch (selectType)
            {
                case " Base":
                    fatherTaskID = (BtoTask.SelectedItem as ComboBoxItem).Content.ToString();
                    newTaskID = (newTaskDT.Year%2000).ToString();
                    if (newTaskDT.Month < 10) 
                        newTaskID += "0";
                    else newTaskID += newTaskDT.Month.ToString();//"1411" get
                    newTaskID += "BA";
                    if (taskData.baseTaskList.Count < 10)
                        newTaskID += "0";
                    newTaskID += (taskData.baseTaskList.Count+1).ToString();
                    break;
                case " Week":
                    fatherTaskID = (BtoTask.SelectedItem as ComboBoxItem).Content.ToString();
                    newTaskID = (newTaskDT.Year%2000).ToString();
                    if (newTaskDT.Month < 10) 
                        newTaskID += "0";
                    else newTaskID += newTaskDT.Month.ToString();//"1411" get
                    newTaskID += "WE";
                    if (taskData.weekTaskList.Count < 10)
                        newTaskID += "0";
                    newTaskID += (taskData.weekTaskList.Count+1).ToString();
                    break;
                case " Month":
                    newTaskID = (newTaskDT.Year%2000).ToString();
                    if (newTaskDT.Month < 10) 
                        newTaskID += "0";
                    else newTaskID += newTaskDT.Month.ToString();//"1411" get
                    newTaskID += "MO";
                    if (taskData.monthTaskList.Count < 10)
                        newTaskID += "0";
                    newTaskID += (taskData.monthTaskList.Count+1).ToString();
                    break;
                default:
                    MessageBox.Show("Something error here!");
                    break;
            } 
        }
    }
}
