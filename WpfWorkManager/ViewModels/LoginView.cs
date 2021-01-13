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
    public class LoginView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        ViewMain viewMain;

        public LoginView(IRepository repository, IMessageSender messageSender, ViewMain viewMain)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            this.viewMain = viewMain;
            container = new UnityContainer();

            LoginCommand = new DelegateCommand(() => Login());
        }

        public DelegateCommand LoginCommand { get; }

        string userName;
        public string UserName
        {
            get => userName;
            set { userName = value; RaisePropertyChanged(nameof(UserName)); }
        }
        string password;
        public string Password
        {
            get => password;
            set { password = value; RaisePropertyChanged(nameof(Password)); }
        }

        private void Login()
        {
            viewMain.LoginAction.Invoke(UserName, Password);
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
