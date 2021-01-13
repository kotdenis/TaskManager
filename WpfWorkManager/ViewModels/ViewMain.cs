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
    public class ViewMain : BindableBase
    {
        UnityContainer container;
        LoginWindow loginWindow;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        ReportChangeWindow changeWindow;
        CurrentSprintWindow currentSprintWindow;
        TeamLeaderTaskWindow teamLeaderTaskWindow;
        TaskFindWindow taskFindWindow;
        AddDeleteWindow addDeleteWindow;
        EmployeeResultWindow resultWindow;
        LeaderTaskWindow leaderTaskWindow;
        DataRepository dataRepository;
        NewSprintWindow sprintWindow;

        public ViewMain(IRepository repository, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            container = new UnityContainer();
            dataRepository = new DataRepository(repository);

            GetStartValues();

            LoginAction += GetUserControl;
            UserRoleNumber = 5;

            ShowLoginWindowCommand = new DelegateCommand(() => ShowLoginWindow());
            ShowIssueChangeReportWindowCommand = new DelegateCommand(() => ShowIssueChangeReportWindow());
            AddDataToSimpleReportCommand = new DelegateCommand(() => AddDataToSimpleReport());
            GetConcreteEmployeeCommand = new DelegateCommand(async() => await GetConcreteEmployeeAsync());
            ShowNewSprintWindowCommand = new DelegateCommand(() => ShowNewSprintWindow());
            ShowCurrentSprintWindowCommand = new DelegateCommand(() => ShowCurrentSprintWindow());
            ShowTeamLeaderTaskWindowCommand = new DelegateCommand(async () => await ShowTeamLeaderTaskWindow());
            ShowTaskFindWindowCommand = new DelegateCommand(() => ShowTaskFindWindow());
            ShowAddDeleteWindowCommand = new DelegateCommand(() => ShowAddDeleteWindow());
            GetEmployeeEnabledCommand = new DelegateCommand(() => GetEmployeeEnabled());
            GetTaskEnabledCommand = new DelegateCommand(() => GetTaskEnabled());
            ShowLeaderTaskWindowCommand = new DelegateCommand(() => ShowLeaderTaskWindow());
            SelectSimpleTaskCommand = new DelegateCommand(async() => await SelectSimpleTaskAsync());
            FinishSimpleTaskCommand = new DelegateCommand(async() => await FinishSimpleTaskAsync());
        }
        

        public DelegateCommand ShowLoginWindowCommand { get; }
        public DelegateCommand GetUserControlCommand { get; }
        public DelegateCommand ShowIssueChangeReportWindowCommand { get; }
        public DelegateCommand AddDataToSimpleReportCommand { get; }
        public DelegateCommand GetConcreteEmployeeCommand { get; }
        public DelegateCommand ShowNewSprintWindowCommand { get; }
        public DelegateCommand ShowCurrentSprintWindowCommand { get; }
        public DelegateCommand ShowTeamLeaderTaskWindowCommand { get; }
        public DelegateCommand ShowTaskFindWindowCommand { get; }
        public DelegateCommand ShowAddDeleteWindowCommand { get; }
        public DelegateCommand GetEmployeeEnabledCommand { get; }
        public DelegateCommand GetTaskEnabledCommand { get; }
        public DelegateCommand ShowLeaderTaskWindowCommand { get; }
        public DelegateCommand SelectSimpleTaskCommand { get; }
        public DelegateCommand FinishSimpleTaskCommand { get; }


        public List<SimpleUser> SimpleUsersTask { get; set; }
        public List<AllEmployee> AllEmployees { get; set; }
        public List<Leaders> Leaders { get; set; }

        string sprintName;
        public string SprintName
        {
            get => sprintName;
            set { sprintName = value; RaisePropertyChanged(nameof(SprintName)); }
        }
        DateTime sprintStart;
        public DateTime SprintStart
        {
            get => sprintStart;
            set { sprintStart = value; RaisePropertyChanged(nameof(SprintStart)); }
        }
        DateTime sprintEnd;
        public DateTime SprintEnd
        {
            get => sprintEnd;
            set { sprintEnd = value; RaisePropertyChanged(nameof(SprintEnd)); }
        }
        string userName;
        public string UserName
        {
            get => userName;
            set { userName = value; RaisePropertyChanged(nameof(UserName)); }
        }
        int userId;
        public int UserId
        {
            get => userId;
            set { userId = value; RaisePropertyChanged(nameof(UserId)); }
        }
        int userRoleNumber;
        public int UserRoleNumber
        {
            get => userRoleNumber;
            set { userRoleNumber = value; RaisePropertyChanged(nameof(UserRoleNumber)); }
        }
        string reportSimpleAdding;
        public string ReportSimpleAdding
        {
            get => reportSimpleAdding;
            set { reportSimpleAdding = value; RaisePropertyChanged(nameof(ReportSimpleAdding)); }
        }
        string issueReport;
        public string IssueReport
        {
            get => issueReport;
            set { issueReport = value; RaisePropertyChanged(nameof(IssueReport)); }
        }
        public bool IsEntered { get; set; }

        private void GetStartValues()
        {
            try
            {
                var temp = repository.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault();
                SprintName = temp.SprintName;
                SprintEnd = temp.SprintEndDate;
                SprintStart = temp.SprintStartDate;
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        public Action<string, string> LoginAction { get; set; }
        public async void GetUserControl(string user, string password)
        {
            try
            {
                var role = repository.Employees
                    .Where(x => x.Name == user && x.Password == password)
                    .FirstOrDefault();
                loginWindow.Close();
                switch (role.Role)
                {
                    case ((int)EnumEmployee.Simple):
                        UserRoleNumber = (int)EnumEmployee.Simple;
                        UserId = role.Id;
                        UserName = role.Name;
                        SimpleUsersTask = await dataRepository.GetSimpleUserDatasListAsync(role.Id);
                        RaisePropertyChanged(nameof(SimpleUsersTask));
                        LoadStartDatas();
                        break;
                    case ((int)EnumEmployee.Leader):
                        UserRoleNumber = (int)EnumEmployee.Leader;
                        UserId = role.Id;
                        UserName = role.Name;
                        SimpleUsersTask = await dataRepository.GetSimpleUserDatasListAsync(role.Id);
                        AllEmployees = await dataRepository.GetAllEmployeiesAsync();
                        AllEmployees = AllEmployees.Where(x => x.LeadeID == role.Id).ToList();
                        RaisePropertyChanged(nameof(SimpleUsersTask));
                        RaisePropertyChanged(nameof(AllEmployees));
                        LoadStartDatas();
                        break;
                    case ((int)EnumEmployee.TeamLeader):
                        UserRoleNumber = (int)EnumEmployee.TeamLeader;
                        UserId = role.Id;
                        UserName = role.Name;
                        AllEmployees = await dataRepository.GetAllEmployeiesAsync();
                        UserId = role.Id;
                        RaisePropertyChanged(nameof(AllEmployees));
                        break;
                    default:
                        break;
                }
                IsEntered = true;
                RaisePropertyChanged(nameof(IsEntered));
            }
            catch(Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        public IssueCondition IssueConditionProp { get; set; }
        private void LoadStartDatas()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    var temp = context.Issues.Where(x => x.EmployeeId == UserId).ToList();
                    IssueConditionProp = null;
                    foreach (var t in temp)
                        foreach (var d in context.IssueConditions.Where(x => x.IssueId == t.Id && x.IssueState == 2))
                            IssueConditionProp = d;
                    IssueReport = IssueConditionProp.IssueReport;
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void AddDataToSimpleReport()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    var c = UserId;
                    var cond = context.IssueConditions.Where(x => x.IssueId == IssueConditionProp.IssueId).FirstOrDefault();
                    var rep = IssueConditionProp.IssueReport;
                    if (IssueConditionProp.IssueState == 2)
                    {
                        var text = rep;
                        cond.IssueReport = text + "\r\n" +
                            DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "\r\n" +
                            ReportSimpleAdding + "\r\n";
                        context.SaveChanges();
                        IssueReport = cond.IssueReport;
                        ShowMessageWindow("Сообщение", "Сохранено", MessageEnums.Success);
                    }
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        
        public bool IsEmployee { get; set; }
        public bool IsTask { get; set; }
        private void GetEmployeeEnabled()
        {
            IsEmployee = true;
            RaisePropertyChanged(nameof(IsEmployee));
            IsTask = false;
            RaisePropertyChanged(nameof(IsTask));
        }

        private void GetTaskEnabled()
        {
            IsEmployee = false;
            RaisePropertyChanged(nameof(IsEmployee));
            IsTask = true;
            RaisePropertyChanged(nameof(IsTask));
        }

        public int SimpleTaskIndex { get; set; }
        private async Task SelectSimpleTaskAsync()
        {
            try
            {
                RaisePropertyChanged(nameof(SimpleTaskIndex));
                var temp = SimpleUsersTask[SimpleTaskIndex].UserIssue;
                var tempId = SimpleUsersTask[SimpleTaskIndex].UserId;

                using(var context = new TaskContext())
                {
                    var iss = context.Issues.Where(x => x.TaskName == temp && x.EmployeeId == tempId).FirstOrDefault();
                    var cond = context.IssueConditions.Where(x => x.IssueId == iss.Id).FirstOrDefault();
                    if (cond.IssueState == 0 && !SimpleUsersTask.Any(x => x.IssueState == 2))
                    {
                        cond.IssueState = 2;
                        context.SaveChanges();
                    }
                    else
                        ShowMessageWindow("Warning", "Задача не может быть принята в работу", MessageEnums.Warning);
                }
                SimpleUsersTask = await dataRepository.GetSimpleUserDatasListAsync(UserId);
                RaisePropertyChanged(nameof(SimpleUsersTask));
            }
            catch (Exception ex) { /*ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error);*/ }
        }

        private async Task FinishSimpleTaskAsync()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    //var temp = context.Issues.Where(x => x.EmployeeId == UserId).FirstOrDefault();
                    var cond = context.IssueConditions.Where(x => x.IssueId == IssueConditionProp.IssueId).FirstOrDefault();
                    cond.IssueState = 1;
                    context.SaveChanges();
                    ShowMessageWindow("Success", "Задача завершена", MessageEnums.Success);
                }
                SimpleUsersTask = await dataRepository.GetSimpleUserDatasListAsync(UserId);
                RaisePropertyChanged(nameof(SimpleUsersTask));
            }
            catch (Exception ex) { /*ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error);*/ }
        }

        public int CurrentEmployeeIndex { get; set; }
        public List<SimpleUser> TempSimpleUserTasks { get; set; }
        private async Task GetConcreteEmployeeAsync()
        {
            RaisePropertyChanged(nameof(CurrentEmployeeIndex));
            var tempId = AllEmployees[CurrentEmployeeIndex].Id;
            TempSimpleUserTasks = await dataRepository.GetSimpleUserDatasListAsync(tempId);
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            container.RegisterInstance<ViewMain>(this);
            resultWindow = container.Resolve<EmployeeResultWindow>();
            resultWindow.ShowDialog();
        }


        private void ShowLoginWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            container.RegisterInstance<ViewMain>(this);
            loginWindow = container.Resolve<LoginWindow>();
            loginWindow.ShowDialog();
        }

        private void ShowIssueChangeReportWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            container.RegisterInstance<ViewMain>(this);
            changeWindow = container.Resolve<ReportChangeWindow>();
            changeWindow.ShowDialog();
        }

        private void ShowNewSprintWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            sprintWindow = container.Resolve<NewSprintWindow>();
            sprintWindow.ShowDialog();
        }

        private void ShowCurrentSprintWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            currentSprintWindow = container.Resolve<CurrentSprintWindow>();
            currentSprintWindow.ShowDialog();
        }

        private async Task ShowTeamLeaderTaskWindow()
        {
            Leaders = await dataRepository.GetLeadersAsync();
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            container.RegisterInstance<ViewMain>(this);
            teamLeaderTaskWindow = container.Resolve<TeamLeaderTaskWindow>();
            teamLeaderTaskWindow.ShowDialog();
        }

        private void ShowLeaderTaskWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            container.RegisterInstance<ViewMain>(this);
            leaderTaskWindow = container.Resolve<LeaderTaskWindow>();
            leaderTaskWindow.ShowDialog();
        }

        private void ShowTaskFindWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            taskFindWindow = container.Resolve<TaskFindWindow>();
            taskFindWindow.ShowDialog();
        }

        private void ShowAddDeleteWindow()
        {
            container.RegisterInstance<IRepository>(repository);
            container.RegisterInstance<IMessageSender>(messageSender);
            addDeleteWindow = container.Resolve<AddDeleteWindow>();
            addDeleteWindow.ShowDialog();
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
