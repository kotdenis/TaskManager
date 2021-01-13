using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Prism.Commands;
using Prism.Mvvm;
using WpfWorkManager.Controls;
using WpfWorkManager.Infrastrucure;
using WpfWorkManager.Models;
using WpfWorkManager.Enums;

namespace WpfWorkManager.ViewModels
{
    public class UserManageView : BindableBase
    {
        UnityContainer container;
        IRepository repository;
        IMessageSender messageSender;
        MessageWindow messageWindow;
        DataRepository dataRepository;

        public UserManageView(IRepository repository, IMessageSender messageSender)
        {
            this.repository = repository;
            this.messageSender = messageSender;
            container = new UnityContainer();
            dataRepository = new DataRepository(repository);
        }
    }
}
