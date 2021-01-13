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
    /// Логика взаимодействия для NewSprintWindow.xaml
    /// </summary>
    public partial class NewSprintWindow : MahApps.Metro.Controls.MetroWindow
    {
        [Dependency]
        public NewSprintView NewSprintView
        {
            set => DataContext = value;
        }
        public NewSprintWindow()
        {
            InitializeComponent();
        }
    }
}
