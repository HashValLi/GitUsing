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
using TarQIE_Task;
using System.Xml;
using System.IO;

namespace TarQIE_UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public taskCollection taskDataBlock;
        public MainWindow()
        {
            taskDataBlock = new taskCollection();
            taskDataLoad();
            taskDataBlock.refreshTaskList();
            /*taskDataBlock.addSubTask("1411MO01", "default", "1411MO01", "-1");
            taskDataBlock.addSubTask("1411MO02", "default", "1411MO02", "-1");
            taskDataBlock.addSubTask("1411MO03", "default", "1411MO03", "-1");
            int newMonthIndex = taskDataBlock.findMonthTask("1411MO01");
            taskDataBlock.monthTaskList[newMonthIndex].addSubTask("1411WE01", "default", "default1", "-1");
            taskDataBlock.monthTaskList[newMonthIndex].addSubTask("1411WE02", "default", "default2", "-1");
            taskDataBlock.refreshTaskList();*/
            /*experiment*/
            /*ex end*/

            InitializeComponent();
            refreshTreeList();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            taskDataSave();
        }

        private void NTIButton_Click(object sender, RoutedEventArgs e)
        {
            NewTaskItem newTI = new NewTaskItem(taskDataBlock);
            newTI.ShowDialog();
            string newTaskType = newTI.selectType;
            if (newTI.newTaskID.Length != 0)
            {
                string newTaskID = newTI.newTaskID;
                string newTaskContent = newTI.newTaskContent;
                string newTaskEP = newTI.newTaskEP;
                string newTaskSTime = newTI.newTaskSTime;
                string newFatherTask = newTI.fatherTaskID;
                DateTime newTaskDT = newTI.newTaskDT;
                //do create new task
                switch (newTaskType)
                {
                    case " Base":
                        int newFatherTaskIndex = taskDataBlock.findWeekTask(
                            newFatherTask);
                        taskDataBlock.weekTaskList[newFatherTaskIndex].addSubTask(newTaskID, newTaskSTime,
                            newTaskContent, newTaskEP);
                        break;
                    case " Week":
                        newFatherTaskIndex = taskDataBlock.findMonthTask(
                            newFatherTask);
                        taskDataBlock.monthTaskList[newFatherTaskIndex].addSubTask(newTaskID, newTaskSTime,
                            newTaskContent, newTaskEP);
                        break;
                    case " Month":
                        taskDataBlock.addSubTask(newTaskID, newTaskSTime,
                            newTaskContent, newTaskEP);
                        break;
                    default:
                        MessageBox.Show("Nothing added.");
                        break;
                }
                taskDataBlock.refreshTaskList();
                refreshTreeList();
            }

        }

        private void STIButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataBlock.SelectedItem == null)
                MessageBox.Show("Please select a task first.");
            else
            {
                string header = (MainDataBlock.SelectedItem as TreeViewItem).Header.ToString();
                string setID = "";
                for (int i = 0; i < 8; i++)
                {
                    setID += header[i];
                }
                string setType = Convert.ToString(header[4]);
                setType += header[5];
                object setTask;
                switch (setType)
                {
                    case "MO":
                        setTask = taskDataBlock.monthTaskList[taskDataBlock.findMonthTask(setID)];
                        break;
                    case "WE":
                        setTask = taskDataBlock.weekTaskList[taskDataBlock.findWeekTask(setID)];
                        break;
                    case "BA":
                        setTask = taskDataBlock.baseTaskList[taskDataBlock.findBaseTask(setID)];
                        break;
                    default:
                        setTask = null;
                        break;
                }
                SetTaskItem setTI = new SetTaskItem(setTask, setType);
                setTI.ShowDialog();
                //do set task
                if (setTI.TaskChanged)
                {
                    int taskIndex;
                    switch (setType)
                    {
                        case "MO":
                            taskIndex = taskDataBlock.findMonthTask(setID);
                            taskDataBlock.monthTaskList[taskIndex].setContent(setTI.TaskContent);
                            taskDataBlock.monthTaskList[taskIndex].setEnterprisePoint(setTI.TaskEP);
                            taskDataBlock.monthTaskList[taskIndex].setStartTime(setTI.TaskSTime);
                            break;
                        case "WE":
                            taskIndex = taskDataBlock.findWeekTask(setID);
                            taskDataBlock.weekTaskList[taskIndex].setContent(setTI.TaskContent);
                            taskDataBlock.weekTaskList[taskIndex].setEnterprisePoint(setTI.TaskEP);
                            taskDataBlock.weekTaskList[taskIndex].setStartTime(setTI.TaskSTime);
                            break;
                        case "BA":
                            taskIndex = taskDataBlock.findBaseTask(setID);
                            taskDataBlock.baseTaskList[taskIndex].setContent(setTI.TaskContent);
                            taskDataBlock.baseTaskList[taskIndex].setEnterprisePoint(setTI.TaskEP);
                            taskDataBlock.baseTaskList[taskIndex].setStartTime(setTI.TaskSTime);
                            break;
                        default:
                            break;
                    }
                    refreshTreeList();
                }
            }

        }

        private void DTIButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete the item you selected?");
            //do delete task
            string taskhead = (MainDataBlock.SelectedItem as TreeViewItem).Header.ToString();
            string taskID = "";
            for (int i = 0; i < 8; i++)
            {
                taskID += taskhead[i];
            }
            deleteTask(taskID);
            refreshTreeList();
        }

        private void FTIButton_Click(object sender, RoutedEventArgs e)//only can finish base task
        {
            MessageBox.Show("Finish the item you selected?");
            string taskhead = (MainDataBlock.SelectedItem as TreeViewItem).Header.ToString();
            string taskID = "";
            for (int i = 0; i < 8; i++)
            {
                taskID += taskhead[i];
            }
            int index = taskDataBlock.findBaseTask(taskID);
            //do finish task
            taskDataBlock.baseTaskList[index].finishTask(DateTime.Now.ToShortDateString());
            MessageBox.Show(taskDataBlock.baseTaskList[index].isFinished.ToString());
            taskDataBlock.refreshTaskList();
            MessageBox.Show(taskDataBlock.baseTaskList[index].isFinished.ToString());
            refreshTreeList();
            MessageBox.Show(taskDataBlock.baseTaskList[index].isFinished.ToString());
        }

        private void TDLButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoList toDL = new ToDoList(taskDataBlock.weekTaskList, taskDataBlock.baseTaskList);
            toDL.ShowDialog();
            int listCounts = toDL.TDList.Count;
            int divide = listCounts / 3 + 1;
            int dev = 1;
            for (int i = 0; i < listCounts + divide; i++)
            {
                if (i == 0)
                {
                    var newItem = new ListViewItem { Content = "Primary", FontSize = 20 };
                    TDL.Items.Add(newItem);
                    dev = 1;
                }
                else if (i == 4)
                {
                    var newItem = new ListViewItem { Content = "Secondary", FontSize = 20 };
                    TDL.Items.Add(newItem);
                    dev = 2;
                }
                else if (i == 8)
                {
                    var newItem = new ListViewItem { Content = "Thirdary", FontSize = 20 };
                    TDL.Items.Add(newItem);
                    dev = 3;
                }
                else
                {
                    var newItem = new ListViewItem
                    {
                        Content = toDL.TDList[i - dev].taskID,
                        FontSize = 20
                    };
                    TDL.Items.Add(newItem);
                }
            }
            TDL.Items.Refresh();
            //do edit new TDL
        }

        private void CALButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAnalysisList checkAL = new CheckAnalysisList(taskDataBlock.weekTaskList);
            checkAL.ShowDialog();
            //do edit new CAList

        }

        public void refreshTreeList()
        {
            MainDataBlock.Items.Clear();
            foreach (monthTask oneMTask in taskDataBlock.monthTaskList)
            {
                var newItem = new TreeViewItem()
                {
                    Header = oneMTask.taskID + ": " + oneMTask.taskContent + " finished: " + oneMTask.isFinished,
                    FontSize = 20
                };
                MainDataBlock.Items.Add(newItem);
                foreach (weekTask oneWTask in oneMTask.subTaskList)
                {
                    var newWeekItem = new TreeViewItem()
                    {
                        Header = oneWTask.taskID + ": " + oneWTask.taskContent + " finished: " + oneWTask.isFinished,
                        FontSize = 20
                    };
                    (newItem as TreeViewItem).Items.Add(newWeekItem);
                    foreach (baseTask oneBTask in oneWTask.subTaskList)
                    {
                        var newBaseItem = new TreeViewItem()
                        {
                            Header = oneBTask.taskID + ": " + oneBTask.taskContent + " finished: " + oneBTask.isFinished,
                            FontSize = 20
                        };
                        (newWeekItem as TreeViewItem).Items.Add(newBaseItem);
                    }
                }

            }
        }

        public void deleteTask(string inputTaskID)
        {
            int[] temp = taskDataBlock.findTask(inputTaskID);
            if (temp[0] == -1)
                MessageBox.Show("No task found!");
            else
            {
                if (temp[1] == 0)
                    taskDataBlock.monthTaskList.RemoveAt(temp[0]);
                else if (temp[1] == 1)
                {
                    string fatherID = taskDataBlock.weekTaskList[temp[0]].fatherTaskID;
                    int indexOfFather = taskDataBlock.findMonthTask(fatherID);
                    taskDataBlock.monthTaskList[indexOfFather].removeSubTask(inputTaskID);
                    taskDataBlock.weekTaskList.RemoveAt(temp[0]);
                }

                else if (temp[1] == 2)
                    taskDataBlock.baseTaskList.RemoveAt(temp[0]);
            }
            taskDataBlock.refreshTaskList();
        }

        public void taskDataSave()
        {
            XmlDocument document = new XmlDocument();
            document.Load(@"...\TaskData.xml");

            XmlElement root = document.DocumentElement;

            if (root.HasChildNodes)
            {
                root.RemoveAll();
            }

            foreach (monthTask aMT in taskDataBlock.monthTaskList)
            {
                XmlElement newMonthTask = document.CreateElement("MonthTask");

                XmlElement newTaskID = document.CreateElement("ID");
                XmlElement newContent = document.CreateElement("Content");
                XmlElement newStartTime = document.CreateElement("StartTime");
                XmlElement newEPoint = document.CreateElement("EnterPrisePoint");
                /*
                XmlElement newFinishTime = document.CreateElement("FinishTime");
                XmlElement newEndTime = document.CreateElement("EndTime");
                */
                // not use cause we had calculated these in task lib
                XmlText ID = document.CreateTextNode(aMT.taskID);
                XmlText Content = document.CreateTextNode(aMT.taskContent);
                XmlText StartTime = document.CreateTextNode(aMT.startTime.ToShortDateString());
                XmlText EPoint = document.CreateTextNode(aMT.enterprisePoint.ToString());
                /*
                XmlText FinishTime = document.CreateTextNode(aMT.finishTime.ToShortDateString());
                XmlText EndTime = document.CreateTextNode(aMT.endTime.ToShortDateString());
                */
                //same as above
                newMonthTask.AppendChild(newTaskID);
                newMonthTask.AppendChild(newContent);
                newMonthTask.AppendChild(newStartTime);
                newMonthTask.AppendChild(newEPoint);
                newTaskID.AppendChild(ID);
                newContent.AppendChild(Content);
                newStartTime.AppendChild(StartTime);
                newEPoint.AppendChild(EPoint);
                if (aMT.subTaskList.Count != 0)
                {
                    foreach (weekTask aWT in aMT.subTaskList)
                    {
                        XmlElement newWeekTask = document.CreateElement("WeekTask");
                        newMonthTask.AppendChild(newWeekTask);

                        XmlElement newTaskIDW = document.CreateElement("ID");
                        XmlElement newContentW = document.CreateElement("Content");
                        XmlElement newStartTimeW = document.CreateElement("StartTime");
                        XmlElement newEPointW = document.CreateElement("EnterPrisePoint");

                        XmlText IDW = document.CreateTextNode(aWT.taskID);
                        XmlText ContentW = document.CreateTextNode(aWT.taskContent);
                        XmlText StartTimeW = document.CreateTextNode(aWT.startTime.ToShortDateString());
                        XmlText EPointW = document.CreateTextNode(aWT.enterprisePoint.ToString());

                        newWeekTask.AppendChild(newTaskIDW);
                        newWeekTask.AppendChild(newContentW);
                        newWeekTask.AppendChild(newStartTimeW);
                        newWeekTask.AppendChild(newEPointW);
                        newTaskIDW.AppendChild(IDW);
                        newContentW.AppendChild(ContentW);
                        newStartTimeW.AppendChild(StartTimeW);
                        newEPointW.AppendChild(EPointW);
                        if (aWT.subTaskList.Count != 0)
                        {
                            foreach (baseTask aBT in aWT.subTaskList)
                            {
                                XmlElement newBaseTask = document.CreateElement("BaseTask");
                                newWeekTask.AppendChild(newBaseTask);

                                XmlElement newTaskIDB = document.CreateElement("ID");
                                XmlElement newContentB = document.CreateElement("Content");
                                XmlElement newStartTimeB = document.CreateElement("StartTime");
                                XmlElement newEPointB = document.CreateElement("EnterPrisePoint");

                                XmlText IDB = document.CreateTextNode(aBT.taskID);
                                XmlText ContentB = document.CreateTextNode(aBT.taskContent);
                                XmlText StartTimeB = document.CreateTextNode(aBT.startTime.ToShortDateString());
                                XmlText EPointB = document.CreateTextNode(aBT.enterprisePoint.ToString());

                                newBaseTask.AppendChild(newTaskIDB);
                                newBaseTask.AppendChild(newContentB);
                                newBaseTask.AppendChild(newStartTimeB);
                                newBaseTask.AppendChild(newEPointB);
                                newTaskIDB.AppendChild(IDB);
                                newContentB.AppendChild(ContentB);
                                newStartTimeB.AppendChild(StartTimeB);
                                newEPointB.AppendChild(EPointB);
                            }
                        }

                    }
                }
                root.InsertAfter(newMonthTask, root.FirstChild);
            }
            document.Save(@"...\TaskData.xml");
        }

        public void taskDataLoad()
        {
            XmlDocument document = new XmlDocument();
            document.Load(@"...\TaskData.xml");
            XmlElement root = document.DocumentElement;
            if (root.HasChildNodes) // data have
            {
                foreach (XmlNode nodeMonth in root.ChildNodes)
                {
                    taskDataBlock.addSubTask(nodeMonth["ID"].InnerText, nodeMonth["StartTime"].InnerText,
                        nodeMonth["Content"].InnerText, nodeMonth["EnterPrisePoint"].InnerText);
                    int mTaskI = taskDataBlock.findMonthTask(nodeMonth["ID"].InnerText);
                    if (nodeMonth["EnterPrisePoint"] != nodeMonth.LastChild)
                    {
                        XmlNode nodeWeekS = nodeMonth["EnterPrisePoint"].NextSibling;
                        taskDataBlock.monthTaskList[mTaskI].addSubTask(
                            nodeWeekS["ID"].InnerText, nodeWeekS["StartTime"].InnerText,
                            nodeWeekS["Content"].InnerText, nodeWeekS["EnterPrisePoint"].InnerText
                            );
                        taskDataBlock.refreshTaskList();
                        int wTaskI = taskDataBlock.findWeekTask(nodeWeekS["ID"].InnerText);

                        if (nodeWeekS["EnterPrisePoint"] != nodeWeekS.LastChild)
                        {
                            XmlNode nodeBaseS = nodeWeekS["EnterPrisePoint"].NextSibling;
                            taskDataBlock.weekTaskList[wTaskI].addSubTask
                                (
                                nodeBaseS["ID"].InnerText, nodeBaseS["StartTime"].InnerText,
                            nodeBaseS["Content"].InnerText, nodeBaseS["EnterPrisePoint"].InnerText
                                );
                            taskDataBlock.refreshTaskList();
                            while (nodeBaseS != nodeWeekS.LastChild)
                            {
                                nodeBaseS = nodeBaseS.NextSibling;
                                taskDataBlock.weekTaskList[wTaskI].addSubTask
                                (
                                nodeBaseS["ID"].InnerText, nodeBaseS["StartTime"].InnerText,
                            nodeBaseS["Content"].InnerText, nodeBaseS["EnterPrisePoint"].InnerText
                                );
                                taskDataBlock.refreshTaskList();
                            }
                        }
                        while (nodeWeekS != nodeMonth.LastChild)
                        {
                            nodeWeekS = nodeWeekS.NextSibling;
                            taskDataBlock.monthTaskList[mTaskI].addSubTask(
                                nodeWeekS["ID"].InnerText, nodeWeekS["StartTime"].InnerText,
                                nodeWeekS["Content"].InnerText, nodeWeekS["EnterPrisePoint"].InnerText
                                );
                            taskDataBlock.refreshTaskList();
                            wTaskI = taskDataBlock.findWeekTask(nodeWeekS["ID"].InnerText);
                            if (nodeWeekS["EnterPrisePoint"] != nodeWeekS.LastChild)
                            {
                                XmlNode nodeBaseS = nodeWeekS["EnterPrisePoint"].NextSibling;
                                taskDataBlock.weekTaskList[wTaskI].addSubTask
                                    (
                                    nodeBaseS["ID"].InnerText, nodeBaseS["StartTime"].InnerText,
                                nodeBaseS["Content"].InnerText, nodeBaseS["EnterPrisePoint"].InnerText
                                    );
                                taskDataBlock.refreshTaskList();
                                while (nodeBaseS != nodeWeekS.LastChild)
                                {
                                    nodeBaseS = nodeBaseS.NextSibling;
                                    taskDataBlock.weekTaskList[wTaskI].addSubTask
                                    (
                                    nodeBaseS["ID"].InnerText, nodeBaseS["StartTime"].InnerText,
                                nodeBaseS["Content"].InnerText, nodeBaseS["EnterPrisePoint"].InnerText
                                    );
                                    taskDataBlock.refreshTaskList();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
