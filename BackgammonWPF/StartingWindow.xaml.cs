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
using System.Windows.Ink;

namespace BackgammonWPF
{
    /// <summary>
    /// Interaction logic for StartingWindow.xaml
    /// </summary>
    public partial class StartingWindow : Window
    {
        private Button startButton = new Button();
        public StartingWindow()
        {
            InitializeComponent();
            this.Height -= 230;
            this.Width -= 200;
            Panel.SetZIndex(grid, 1);
            Panel.SetZIndex(startButton, 3);
            Panel.SetZIndex(startingWindowHeader, 2);
            Panel.SetZIndex(startingWindowText, 2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
