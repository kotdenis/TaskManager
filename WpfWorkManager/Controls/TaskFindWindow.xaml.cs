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
    /// Логика взаимодействия для TaskFindWindow.xaml
    /// </summary>
    public partial class TaskFindWindow : MahApps.Metro.Controls.MetroWindow
    {
        [Dependency]
        public TaskFindView TaskFindView
        {
            set => DataContext = value;
        }
        public TaskFindWindow()
        {
            InitializeComponent();
        }
    }
}
