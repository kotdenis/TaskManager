using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Unity;
using WpfWorkManager.Controls;
using WpfWorkManager.Enums;
using WpfWorkManager.Infrastrucure;
using WpfWorkManager.Models;
using WpfWorkManager.Models.DataViewModels;

namespace WpfWorkManager.ViewModels
{
    public class TaskFindView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;

        public TaskFindView(IRepository repository, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            container = new UnityContainer();

            StartDate = DateTime.Now;

            Roles = new List<string>();
            Roles.Add("Руководитель");
            Roles.Add("Сотрудник");
            RaisePropertyChanged(nameof(Roles));

            GetEmployeeCommand = new DelegateCommand(() => GetEmployee());
            GetInsertedChangesCheckedCommand = new DelegateCommand(() => GetInsertedChangesChecked());
            UseLastChangeCommand = new DelegateCommand(() => UseLastChange());
            FindTaskCommand = new DelegateCommand(() => FindTask());
        }

        public DelegateCommand GetEmployeeCommand { get; }
        public DelegateCommand GetInsertedChangesCheckedCommand { get; }
        public DelegateCommand UseLastChangeCommand { get; }
        public DelegateCommand FindTaskCommand { get; }

        public List<string> Roles { get; set; }
        public List<string> Names { get; set; }
        public int RoleIndex { get; set; }

        string empRole;
        public string EmpRole
        {
            get => empRole;
            set { empRole = value; RaisePropertyChanged(nameof(EmpRole)); }
        }
        DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; RaisePropertyChanged(nameof(StartDate)); }
        }
        string empName;
        public string EmpName
        {
            get => empName;
            set { empName = value; RaisePropertyChanged(nameof(EmpName)); }
        }
        string findingTask;
        public string FindingTask
        {
            get => findingTask;
            set { findingTask = value; RaisePropertyChanged(nameof(FindingTask)); }
        }

        private void GetEmployee()
        {
            try
            {
                RaisePropertyChanged(nameof(RoleIndex));
                Names = new List<string>();
                foreach (var t in repository.Employees.Where(x => x.Role == (RoleIndex + 1)))
                    Names.Add(t.Name);
                RaisePropertyChanged(nameof(Names));
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        public bool IsChangesInserted { get; set; }
        private void GetInsertedChangesChecked()
        {
            RaisePropertyChanged(nameof(IsChangesInserted));
        }

        public bool IsLastChangeUsed { get; set; }
        private void UseLastChange()
        {
            RaisePropertyChanged(nameof(IsLastChangeUsed));  
        }

        private void FindTask()
        {
            try
            {
                var empId = repository.Employees.Where(x => x.Name == EmpName).FirstOrDefault().Id;
                var sprintId = repository.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault().Id;
                var listOne = repository.Issues.Where(x => x.SprintId == sprintId && x.EmployeeId == empId && x.IsDeleted == false).ToList();
                if (IsLastChangeUsed == false && IsChangesInserted == false)
                {
                    foreach (var t in listOne)
                        FindingTask += t.TaskName + "\r\n";
                }
                var listTwo = from temp in listOne
                              join cond in repository.IssueConditions on temp.Id equals cond.IssueId into condTemp
                              from Cond in condTemp.DefaultIfEmpty()
                              where Cond.IssueReportChangeDate == StartDate

                              select Cond.IssueId;

                if (IsLastChangeUsed && IsChangesInserted)
                {
                    FindingTask = repository.Issues.Where(x => x.Id == listTwo.FirstOrDefault()).FirstOrDefault().TaskName;
                }
                var listThree = from temp in listOne
                                join cond in repository.IssueConditions on temp.Id equals cond.IssueId into condTemp
                                from Cond in condTemp.DefaultIfEmpty()
                                where Cond.IssueReportChangeDate != null 
                                select Cond.IssueId;
                if (IsLastChangeUsed == false && IsChangesInserted)
                {
                    foreach (var t in listOne)
                        foreach (var d in listThree)
                        {
                            if(t.Id == d)
                                FindingTask += t.TaskName + "\r\n";
                        }
                }

            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void CloseMessageWindow()
        {
            messageWindow.Close();
        }
        
        public void ShowMessageWindow(string label, string message, MessageEnums enumMessage)
        {
            messageSender.Label = label;
            messageSender.Message = message;
            messageSender.EnumCount = (int)enumMessage;
            messageSender.CloseWindow += CloseMessageWindow;
            container.RegisterInstance<IMessageSender>(messageSender);
            messageWindow = container.Resolve<MessageWindow>();
            messageWindow.ShowDialog();
        }
    }
}
