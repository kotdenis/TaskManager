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
    /// Логика взаимодействия для TeamLeaderTaskWindow.xaml
    /// </summary>
    public partial class TeamLeaderTaskWindow : MahApps.Metro.Controls.MetroWindow
    {
        [Dependency]
        public TeamLeaderTaskView TeamLeaderTask
        {
            set => DataContext = value;
        }
        public TeamLeaderTaskWindow()
        {
            InitializeComponent();
        }
    }
}
