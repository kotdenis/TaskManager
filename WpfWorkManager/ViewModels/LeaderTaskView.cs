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
    public class LeaderTaskView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        DataRepository dataRepository;
        ViewMain viewMain;

        public LeaderTaskView(IRepository repository, IMessageSender messageSender, ViewMain viewMain)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            this.viewMain = viewMain;
            container = new UnityContainer();
            dataRepository = new DataRepository(repository);

            GetEmployees();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            GetCurrentTasksCommand = new DelegateCommand(() => GetCurrentTasks());
            AddNewTaskCommand = new DelegateCommand(() => AddNewTask());
            DeleteTaskCommand = new DelegateCommand(() => DeleteTask());
        }

        public DelegateCommand GetCurrentTasksCommand { get; }
        public DelegateCommand AddNewTaskCommand { get; }
        public DelegateCommand DeleteTaskCommand { get; }

        public List<string> Employees { get; set; }
        public List<string> CurrentTasks { get; set; }

        string allTasks;
        public string AllTasks
        {
            get => allTasks;
            set { allTasks = value; RaisePropertyChanged(nameof(AllTasks)); }
        }

        string empName;
        public string EmpName
        {
            get => empName;
            set { empName = value; RaisePropertyChanged(nameof(EmpName)); }
        }
        string newTask;
        public string NewTask
        {
            get => newTask;
            set { newTask = value; RaisePropertyChanged(nameof(NewTask)); }
        }
        string comment;
        public string Comment
        {
            get => comment;
            set { comment = value; RaisePropertyChanged(nameof(Comment)); }
        }
        string description;
        public string Description
        {
            get => description;
            set { description = value; RaisePropertyChanged(nameof(Description)); }
        }
        string taskName;
        public string TaskName
        {
            get => taskName;
            set { taskName = value; RaisePropertyChanged(nameof(TaskName)); }
        }
        DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; RaisePropertyChanged(nameof(StartDate)); }
        }
        DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set { endDate = value; RaisePropertyChanged(nameof(EndDate)); }
        }

        private void GetEmployees()
        {
            try
            {
                Employees = new List<string>();
                foreach (var t in repository.Employees.Where(x => x.LeaderId == viewMain.UserId))
                    Employees.Add(t.Name);
                RaisePropertyChanged(nameof(Employees));
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void GetCurrentTasks()
        {
            try
            {
                CurrentTasks = new List<string>();
                var tempId = repository.Employees.Where(x => x.Name == EmpName).FirstOrDefault().Id;
                var list = repository.Issues.Where(x => x.EmployeeId == tempId).ToList();
                if(list.Count > 0)
                foreach (var t in list)
                    CurrentTasks.Add(t.TaskName);
                RaisePropertyChanged(nameof(CurrentTasks));
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void AddNewTask()
        {
            try
            {
                var empId = repository.Employees.Where(x => x.Name == EmpName && x.Role == 2).FirstOrDefault().Id;
                var sprintId = repository.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault().Id;
                using (var context = new TaskContext())
                {
                    using (var trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Issues.Add(new Issue()
                            {
                                Description = Description,
                                Comment = Comment,
                                EmployeeId = empId,
                                SprintId = sprintId,
                                TaskName = newTask,
                                IsDeleted = false
                            });
                            context.SaveChanges();
                            var issueId = context.Issues.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                            context.IssueConditions.Add(new IssueCondition()
                            {
                                IssueComment = Comment,
                                IssueId = issueId,
                                IssueEndDate = EndDate,
                                IssueStartDate = StartDate,
                                IssueState = 0
                            });
                            context.SaveChanges();
                            trans.Commit();

                            ShowMessageWindow("Success", "Добавлено.", MessageEnums.Success);
                        }
                        catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); trans.Rollback(); }
                    }
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void DeleteTask()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    var empId = repository.Employees.Where(x => x.Name == EmpName && x.Role == 2).FirstOrDefault().Id;
                    var issue = context.Issues.Where(x => x.EmployeeId == empId && x.TaskName == TaskName).FirstOrDefault();
                    issue.IsDeleted = true;
                    context.SaveChanges();
                    ShowMessageWindow("Success", "Удалено.", MessageEnums.Success);
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
