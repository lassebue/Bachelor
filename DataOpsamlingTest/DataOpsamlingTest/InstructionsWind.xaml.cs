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

namespace DataOpsamlingTest
{
    /// <summary>
    /// Interaction logic for InstructionsWind.xaml
    /// </summary>
    public partial class InstructionsWind : Window
    {
        public InstructionsWind()
        {
            InitializeComponent();
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseBtn(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
