using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ListeLivreWindow.xaml
    /// </summary>
    public partial class ListeLivreWindow : Window
    {
        private string connectionString = "server=localhost;uid=root;pwd=;database=gestion_bibliotheque";
        public ListeLivreWindow()
        {
            InitializeComponent();
            AfficherLivres();
        }

        private void AfficherLivres()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT livres.id AS Id, titre as Titre, auteurs.nom AS Auteur, nb_page AS NbPages, prix AS Prix FROM livres INNER JOIN auteurs ON livres.auteur_id = auteurs.id", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                LivresDataGrid.ItemsSource = table.DefaultView;
            }
        }

        private void AjouterBtn_Click(object sender, RoutedEventArgs e)
        {
            AjouterModifierLivreWindow addWindow = new AjouterModifierLivreWindow(null);
            if (addWindow.ShowDialog() == true)
            {
                AfficherLivres();
            }
        }

        private void ModifierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LivresDataGrid.SelectedItem is DataRowView selectedRow)
            {
                AjouterModifierLivreWindow editWindow = new AjouterModifierLivreWindow(selectedRow);
                if (editWindow.ShowDialog() == true)
                {
                    AfficherLivres();
                }
            }
        }

        private void SupprimerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LivresDataGrid.SelectedItem is DataRowView selectedRow)
            {
                int id = Convert.ToInt32(selectedRow["id"]);

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM livres WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                AfficherLivres();
            }
        }

        private void OuvrirMenuPrincipale_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
