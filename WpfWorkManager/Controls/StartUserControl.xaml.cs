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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;
using WpfWorkManager.ViewModels;

namespace WpfWorkManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для StartUserControl.xaml
    /// </summary>
    public partial class StartUserControl : MahApps.Metro.Controls.MetroContentControl
    {
        [Dependency]
        public ViewMain ViewMain
        {
            set => DataContext = value;
        }
        public StartUserControl()
        {
            InitializeComponent();
        }
    }
}
