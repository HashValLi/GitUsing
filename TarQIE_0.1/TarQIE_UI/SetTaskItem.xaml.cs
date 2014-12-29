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
    /// SetTaskItem.xaml 的交互逻辑
    /// </summary>
    public partial class SetTaskItem : Window
    {
        public string TaskID { get; set; }
        public string TaskContent { get; set; }
        public string TaskType { get; set; }
        public string TaskFather { get; set; }
        public string TaskEP { get; set; }
        public string TaskSTime { get; set; }
        public bool TaskChanged;
        public SetTaskItem(object sender,string setType)
        {
            InitializeComponent();
            TaskChanged = false;
            switch (setType)
            {
                case "MO":
                    TaskID = (sender as monthTask).taskID;
                    TaskContent = (sender as monthTask).taskContent;
                    TaskType = "Month Task";
                    TaskFather = (sender as monthTask).fatherTaskID;
                    TaskEP = (sender as monthTask).enterprisePoint.ToString();
                    TaskSTime = (sender as monthTask).startTime.ToShortDateString();
                    break;
                case "WE":
                    TaskID = (sender as weekTask).taskID;
                    TaskContent = (sender as weekTask).taskContent;
                    TaskType = "Week Task";
                    TaskFather = (sender as weekTask).fatherTaskID;
                    TaskEP = (sender as weekTask).enterprisePoint.ToString();
                    TaskSTime = (sender as weekTask).startTime.ToShortDateString();
                    break;
                case "BA":
                    TaskID = (sender as baseTask).taskID;
                    TaskContent = (sender as baseTask).taskContent;
                    TaskType = "Base Task";
                    TaskFather = (sender as baseTask).fatherTaskID;
                    TaskEP = (sender as baseTask).enterprisePoint.ToString();
                    TaskSTime = (sender as baseTask).startTime.ToShortDateString();
                    break;
                default:
                    break;
            }
            TType.Content = TaskType;
            BtoTask.Content = TaskFather;
            EPoint.Text = TaskEP;
            STime.Text = TaskSTime;
            TContent.Text = TaskContent;
            TID.Content = TaskID;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // no return but close
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                TaskChanged = true;
                TaskContent = TContent.Text;
                TaskSTime = STime.Text;
                TaskEP = EPoint.Text;
                this.Close();
            }                
            // do things return and close
        }

        public bool CheckData()
        {
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
                    + DateTime.Now.ToShortDateString() + ".\nOr using Today Date Button.");
                return false;
            }
            DateTime dateValue;
            if (!DateTime.TryParse(STime.Text, out dateValue) && UNowtime.IsChecked == false)
            {
                MessageBox.Show("Invalid StartTime,please input StartTime like "
                    + DateTime.Now.ToShortDateString() + ".\nOr using Today Date Button.");
                return false;
            }
            if (DateTime.Compare(dateValue,Convert.ToDateTime(TaskSTime)) < 0 && 
                UNowtime.IsChecked == false)
            {
                MessageBox.Show("Invalid StartTime,please input StartTime after Task's startTime");
                return false;
            }
            return true;
        }
    }
}
