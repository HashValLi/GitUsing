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
    /// CheckAnalysisList.xaml 的交互逻辑
    /// </summary>
    public partial class CheckAnalysisList : Window
    {
        public List<weekTask> weekTaskList;
        public CheckAnalysisList(List<weekTask> weekTask)
        {
            InitializeComponent();
            weekTaskList = weekTask;
            foreach(weekTask aWeekTask in weekTaskList)
            {
                var newItem = new ListBoxItem
                {
                    Content = aWeekTask.taskID + ": " + aWeekTask.isFinished,
                    FontSize = 18
                };
                WTasks.Items.Add(newItem);
            }
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
    }
}
