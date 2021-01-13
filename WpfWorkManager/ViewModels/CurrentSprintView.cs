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
    public class CurrentSprintView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;

        public CurrentSprintView(IRepository repository, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            container = new UnityContainer();

            GetReportCommand = new DelegateCommand(() => GetReport());
            GetReportDraftCommand = new DelegateCommand(() => GetReportDraft());
            SaveReportChangesCommand = new DelegateCommand(() => SaveReportChanges());
            EndSprintCommand = new DelegateCommand(() => EndSprint());
        }

        public DelegateCommand GetReportCommand { get; }
        public DelegateCommand GetReportDraftCommand { get; }
        public DelegateCommand SaveReportChangesCommand { get; }
        public DelegateCommand EndSprintCommand { get; }

        string report;
        public string Report
        {
            get => report;
            set { report = value; RaisePropertyChanged(nameof(Report)); }
        }
        string reportDraft;
        public string ReportDraft
        {
            get => reportDraft;
            set { reportDraft = value; RaisePropertyChanged(nameof(ReportDraft)); }
        }
        string reportAdding;
        public string ReportAdding
        {
            get => reportAdding;
            set { reportAdding = value; RaisePropertyChanged(nameof(ReportAdding)); }
        }
        bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set { isVisible = value; RaisePropertyChanged(nameof(IsVisible)); }
        }

        private void GetReport()
        {
            try
            {
                IsVisible = false;
                using(var context = new TaskContext())
                {
                    Report = context.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault().SprintTotalReport;
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void GetReportDraft()
        {
            try
            {
                IsVisible = true;
                using (var context = new TaskContext())
                {
                    ReportDraft = context.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault().SprintDraft;
                }
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void SaveReportChanges()
        {
            try
            {
                if (IsVisible)
                {
                    using (var context = new TaskContext())
                    {
                        var temp = context.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault();
                        temp.SprintDraft = ReportDraft;
                        context.SaveChanges();
                    }
                }
                else
                {
                    using (var context = new TaskContext())
                    {
                        var temp = context.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault();
                        temp.SprintTotalReport += "\r\n" +
                            DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "\r\n" +
                            ReportAdding;
                        context.SaveChanges();
                    }
                }
                ShowMessageWindow("Success", "Добавлено.", MessageEnums.Success);
            }
            catch (Exception ex) { ShowMessageWindow("Ошибка", ex.Message, MessageEnums.Error); }
        }

        private void EndSprint()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    var temp = context.Sprints.Where(x => x.SprintSate == 0).FirstOrDefault();
                    temp.SprintSate = 1;
                    context.SaveChanges();
                    ShowMessageWindow("Success", "Завершен.", MessageEnums.Success);
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
