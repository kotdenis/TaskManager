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
    /// Логика взаимодействия для LeaderTaskWindow.xaml
    /// </summary>
    public partial class LeaderTaskWindow : MahApps.Metro.Controls.MetroWindow
    {
        [Dependency]
        public LeaderTaskView LeaderTaskView
        {
            set => DataContext = value;
        }
        public LeaderTaskWindow()
        {
            InitializeComponent();
        }
    }
}
