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
    public class AddDeleteView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        DataRepository dataRepository;

        public AddDeleteView(IRepository repository, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            container = new UnityContainer();

            EmloyeeStatus = new List<string>();
            EmloyeeStatus.Add("Руководитель");
            EmloyeeStatus.Add("Сотрудник");
            RaisePropertyChanged(nameof(EmloyeeStatus));

            AddEnabled = true;
            RaisePropertyChanged(nameof(AddEnabled));

            GetLeadersCommand = new DelegateCommand(() => GetLeaders());
            AddEmployeeCommand = new DelegateCommand(() => AddEmployee());
            GetAddEnabledCommand = new DelegateCommand(() => GetAddEnabled());
            GetDeletedCommand = new DelegateCommand(() => GetDeleted());
            GetEmployeesToDeleteCommand = new DelegateCommand(() => GetEmployeesToDelete());
            DeleteEmployeeCommand = new DelegateCommand(() => DeleteEmployee());
        }

        public DelegateCommand GetLeadersCommand { get; }
        public DelegateCommand AddEmployeeCommand { get; }
        public DelegateCommand GetAddEnabledCommand { get; }
        public DelegateCommand GetDeletedCommand { get; }
        public DelegateCommand GetEmployeesToDeleteCommand { get; }
        public DelegateCommand DeleteEmployeeCommand { get; }

        public List<string> EmloyeeStatus { get; set; }
        public List<string> Employees { get; set; }
        public List<string> LeadersNames { get; set; }
        public bool DeleteEnabled { get; set; }
        public bool AddEnabled { get; set; }

        string status;
        public string Status
        {
            get => status;
            set { status = value; RaisePropertyChanged(nameof(Status)); }
        }
        string empName;
        public string EmpName
        {
            get => empName;
            set { empName = value; RaisePropertyChanged(nameof(EmpName)); }
        }
        string password;
        public string Password
        {
            get => password;
            set { password = value; RaisePropertyChanged(nameof(Password)); }
        }
        int leaderIndex;
        public int LeaderIndex
        {
            get => leaderIndex;
            set { leaderIndex = value; RaisePropertyChanged(nameof(LeaderIndex)); }
        }
        string leaderName;
        public string LeaderName
        {
            get => leaderName;
            set { leaderName = value; RaisePropertyChanged(nameof(LeaderName)); }
        }

        private void GetLeaders()
        {
            try
            {
                LeadersNames = new List<string>();
                if (LeaderIndex == 1)
                {
                    foreach (var t in repository.Employees.Where(x => x.Role == 1))
                        LeadersNames.Add(t.Name);
                    RaisePropertyChanged(nameof(LeadersNames));
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void GetEmployees()
        {
            try
            {
                LeadersNames = new List<string>();
                if (LeaderIndex == 1)
                {
                    foreach (var t in repository.Employees.Where(x => x.Role == 1))
                        LeadersNames.Add(t.Name);
                    RaisePropertyChanged(nameof(LeadersNames));
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void GetEmployeesToDelete()
        {
            try
            {
                Employees = new List<string>();
                if (LeaderIndex == 0)
                {
                    foreach (var t in repository.Employees.Where(x => x.Role == 1))
                        Employees.Add(t.Name);
                    RaisePropertyChanged(nameof(Employees));
                }
                if (LeaderIndex == 1)
                {
                    foreach (var t in repository.Employees.Where(x => x.Role == 2))
                        Employees.Add(t.Name);
                    RaisePropertyChanged(nameof(Employees));
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void DeleteEmployee()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    var emp = context.Employees
                        .Where(x => x.Name == EmpName && x.IsRetired == false)
                        .FirstOrDefault();
                    emp.IsRetired = false;
                    context.SaveChanges();
                    ShowMessageWindow("Success", "Удален", MessageEnums.Success);
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void AddEmployee()
        {
            try
            {
                using(var context = new TaskContext())
                {
                    if(LeaderIndex == 0)
                    {
                        var teamLeaderId = repository.Employees.Where(x => x.Role == 0).FirstOrDefault().Id;
                        context.Employees.Add(new Employee()
                        {
                            IsRetired = false,
                            LeaderId = teamLeaderId,
                            Name = EmpName,
                            Password = Password,
                            Role = 1
                        });
                        context.SaveChanges();
                    }
                    if (LeaderIndex == 1)
                    {
                        var leaderId = repository.Employees
                            .Where(x => x.Name == LeaderName && x.Role == 1)
                            .FirstOrDefault().Id;
                        context.Employees.Add(new Employee()
                        {
                            IsRetired = false,
                            LeaderId = leaderId,
                            Name = EmpName,
                            Password = Password,
                            Role = 2
                        });
                        context.SaveChanges();
                    }
                    ShowMessageWindow("Success", "Сохранено", MessageEnums.Success);
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void GetAddEnabled()
        {
            AddEnabled = true;
            RaisePropertyChanged(nameof(AddEnabled));
            DeleteEnabled = false;
            RaisePropertyChanged(nameof(DeleteEnabled));
        }

        private void GetDeleted()
        {
            AddEnabled = false;
            RaisePropertyChanged(nameof(AddEnabled));
            DeleteEnabled = true;
            RaisePropertyChanged(nameof(DeleteEnabled));
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
