using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;
using WpfWorkManager.ViewModels;

namespace WpfWorkManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserManageWindow.xaml
    /// </summary>
    public partial class UserManageWindow : MahApps.Metro.Controls.MetroWindow
    {
        [Dependency]
        public UserManageView UserManageView
        {
            set => DataContext = value;
        }
        public UserManageWindow()
        {
            InitializeComponent();
        }
    }
}
