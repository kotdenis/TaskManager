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
    public class NewSprintView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;

        public NewSprintView(IRepository repository, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            container = new UnityContainer();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            SaveNewSprintCommand = new DelegateCommand(() => SaveNewSprint());
        }

        public DelegateCommand SaveNewSprintCommand { get; }

        string sprintName;
        public string SprintName
        {
            get => sprintName;
            set { sprintName = value; RaisePropertyChanged(nameof(SprintName)); }
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


        private void SaveNewSprint()
        {
            try
            {
                var b = StartDate.ToShortDateString();
                using(var context = new TaskContext())
                {
                    if(!context.Sprints.Any(x => x.SprintSate == 0))
                    {
                        context.Sprints.Add(new Sprint()
                        {
                            SprintDraft = "",
                            SprintSate = 0,
                            SprintEndDate = EndDate,
                            SprintStartDate = StartDate,
                            SprintName = SprintName,
                            SprintTotalReport = ""
                        });
                        context.SaveChanges();
                        ShowMessageWindow("Success", "Сохранено.", MessageEnums.Success);
                    }
                    else ShowMessageWindow("Warning", "Текущий спринт не закрыт", MessageEnums.Warning);
                }
            }
            catch (Exception ex) { ShowMessageWindow("Error", ex.Message, MessageEnums.Error); }
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
