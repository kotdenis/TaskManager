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
    /// Логика взаимодействия для AddDeleteWindow.xaml
    /// </summary>
    public partial class AddDeleteWindow : MahApps.Metro.Controls.MetroWindow
    {
        [Dependency]
        public AddDeleteView AddDeleteView
        {
            set => DataContext = value;
        }
        public AddDeleteWindow()
        {
            InitializeComponent();
        }
    }
}
