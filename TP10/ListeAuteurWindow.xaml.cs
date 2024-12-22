using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
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
    /// Interaction logic for ListeAuteurWindow.xaml
    /// </summary>
    public partial class ListeAuteurWindow : Window
    {
        private string connectionString = "server=localhost;uid=root;pwd=;database=gestion_bibliotheque";
        public ListeAuteurWindow()
        {
            InitializeComponent();
            AfficherAuteur();
        }

        private void AfficherAuteur()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT id AS Id, nom AS Nom FROM auteurs", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                AuteursDataGrid.ItemsSource = table.DefaultView;
            }
        }

        private void AjouterBtn_Click(object sender, RoutedEventArgs e)
        {
            AjouterModifierAuteurWindow popup = new AjouterModifierAuteurWindow();
            if (popup.ShowDialog() == true)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO auteurs (nom) VALUES (@nom)", conn);
                    cmd.Parameters.AddWithValue("@nom", popup.NomAuteurTxb.Text);
                    cmd.ExecuteNonQuery();
                }
                AfficherAuteur();
            }
            popup.Close();
        }

        private void ModifierBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AuteursDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)AuteursDataGrid.SelectedItem;
                AjouterModifierAuteurWindow popup = new AjouterModifierAuteurWindow(row["nom"].ToString());
                if (popup.ShowDialog() == true)
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("UPDATE auteurs SET nom = @nom WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@nom", popup.NomAuteurTxb.Text);
                        cmd.Parameters.AddWithValue("@id", row["id"]);
                        cmd.ExecuteNonQuery();
                    }
                    AfficherAuteur();
                }
            }
        }

        private void SupprimerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AuteursDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)AuteursDataGrid.SelectedItem;
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM auteurs WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", row["id"]);
                    cmd.ExecuteNonQuery();
                }
                AfficherAuteur();
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
