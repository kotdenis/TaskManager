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
    public class EmployeeResultView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        ViewMain viewMain;

        public EmployeeResultView(IRepository repository, IMessageSender messageSender, ViewMain viewMain)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            this.viewMain = viewMain;
            container = new UnityContainer();


            SimpleUsersTask = viewMain.TempSimpleUserTasks;

            SelectEmployeeCommand = new DelegateCommand(() => SelectEmployee());
            GetTaskEnabledCommand = new DelegateCommand(() => GetTaskEnabled());
        }

        public DelegateCommand SelectEmployeeCommand { get; }
        public DelegateCommand GetTaskEnabledCommand { get; }

        public List<SimpleUser> SimpleUsersTask { get; set; }

        string issueReport;
        public string IssueReport
        {
            get => issueReport;
            set { issueReport = value; RaisePropertyChanged(nameof(IssueReport)); }
        }

        public int Index { get; set; }
        private void SelectEmployee()
        {
            RaisePropertyChanged(nameof(Index));
            IssueReport = SimpleUsersTask[Index].IssueReport;
            GetReportEnabled();
        }

        public bool IsReport { get; set; }
        public bool IsTask { get; set; }
        private void GetTaskEnabled()
        {
            IsReport = true;
            RaisePropertyChanged(nameof(IsReport));
            IsTask = false;
            RaisePropertyChanged(nameof(IsTask));
        }

        private void GetReportEnabled()
        {
            IsReport = false;
            RaisePropertyChanged(nameof(IsReport));
            IsTask = true;
            RaisePropertyChanged(nameof(IsTask));
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
