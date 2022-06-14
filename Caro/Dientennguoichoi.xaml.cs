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

namespace Caro
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FormGetName : Window
    {
        public FormGetName()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainPlay = new MainWindow(player1.Text, player2.Text);
            mainPlay.Show();
            mainPlay.SetTurn();
            this.Close();
        }
    }
}
