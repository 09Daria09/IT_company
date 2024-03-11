using IT_company.ViewModel;
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

namespace IT_company.View
{
    /// <summary>
    /// Interaction logic for EditEmploeeWin.xaml
    /// </summary>
    public partial class EditEmploeeWin : Window
    {
        public EditEmploeeWin()
        {
            InitializeComponent();
            this.DataContext = new EditEmploeeViewModel();
        }
    }
}
