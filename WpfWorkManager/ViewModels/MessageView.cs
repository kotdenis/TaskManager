using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfWorkManager.Infrastrucure;

namespace WpfWorkManager.ViewModels
{
    public class MessageView : BindableBase
    {
        IMessageSender messageSender;
        public MessageView(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
            Label = messageSender.Label; RaisePropertyChanged(nameof(Label));
            Message = messageSender.Message; RaisePropertyChanged(nameof(Message));
            EnumCount = messageSender.EnumCount; RaisePropertyChanged(nameof(EnumCount));

            CloseCommand = new DelegateCommand(() => CloseMessageWindow());
        }

        public string Label { get; set; }
        public string Message { get; set; }
        public int EnumCount { get; set; }

        public DelegateCommand CloseCommand { get; }

        private void CloseMessageWindow()
        {
            messageSender.CloseWindow.Invoke();
        }
    }
}
