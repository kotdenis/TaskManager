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
    public class ReportChangeView : BindableBase
    {
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        UnityContainer container;
        DataRepository dataRepository;
        ViewMain viewMain;

        public ReportChangeView(IRepository repository, IMessageSender messageSender, ViewMain viewMain)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            this.viewMain = viewMain;
            container = new UnityContainer();
            dataRepository = new DataRepository(repository);

            GetIssueReport();
            SaveChangedReportCommand = new DelegateCommand(() => SaveChangedReport());
        }

        public DelegateCommand SaveChangedReportCommand { get; }

        string report;
        public string Report
        {
            get => report;
            set { report = value; RaisePropertyChanged(nameof(Report)); }
        }

        Issue issue;
        private void GetIssueReport()
        {
            try
            {
                issue = repository.Issues
                    .Where(x => x.EmployeeId == viewMain.UserId)
                    .FirstOrDefault();
                //Report = repository.IssueConditions
                    //.Where(x => x.IssueId == issue.Id)
                    //.FirstOrDefault().IssueReport;
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void SaveChangedReport()
        {
            try
            {
                using(var context = new TaskContext())
                {
                    var temp = context.IssueConditions.Where(x => x.IssueId == viewMain.IssueConditionProp.IssueId).FirstOrDefault();
                    temp.IssueReport += "\r\n Внесено изменение " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() +
                        "\r\n" + Report;
                    context.SaveChanges();
                    ShowMessageWindow("Сообщение", "Изменения сохранены", MessageEnums.Success);
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
