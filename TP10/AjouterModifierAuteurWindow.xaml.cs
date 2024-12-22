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

namespace TP10
{
    /// <summary>
    /// Interaction logic for AjouterModifierAuteurWindow.xaml
    /// </summary>
    public partial class AjouterModifierAuteurWindow : Window
    {
        public string AuthorName { get; private set; }
        public AjouterModifierAuteurWindow(string name = "")
        {
            InitializeComponent();
            NomAuteurTxb.Text = name;
        }

        private void EnregistrerBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthorName = NomAuteurTxb.Text;
            DialogResult = true;
        }
    }
}
