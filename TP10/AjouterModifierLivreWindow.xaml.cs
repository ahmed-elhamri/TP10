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
    /// Interaction logic for AjouterModifierLivreWindow.xaml
    /// </summary>
    public partial class AjouterModifierLivreWindow : Window
    {
        private DataRowView livreRow;
        private string connectionString = "server=localhost;uid=root;pwd=;database=gestion_bibliotheque";
        public AjouterModifierLivreWindow(DataRowView livreRow)
        {
            InitializeComponent();
            InitializeComponent();
            this.livreRow = livreRow;

            AfficherAuteurs();

            if (livreRow != null)
            {
                TitreTxt.Text = livreRow["Titre"].ToString();
                AuteursCbx.Text = livreRow["Auteur"].ToString();
                NbPagesTxt.Text = livreRow["NbPages"].ToString();
                PrixTxt.Text = livreRow["Prix"].ToString();
            }
        }

        private void AfficherAuteurs()
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, nom FROM auteurs", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AuteursCbx.Items.Add(new { Id = reader.GetInt32("id"), Nom = reader.GetString("nom") });
                    }
                }
            }

            AuteursCbx.DisplayMemberPath = "Nom";
            AuteursCbx.SelectedValuePath = "Id";
        }

        private void SaveLivre_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TitreTxt.Text) || String.IsNullOrWhiteSpace(NbPagesTxt.Text) || String.IsNullOrWhiteSpace(PrixTxt.Text) || AuteursCbx.SelectedItem == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }
            if (int.TryParse(NbPagesTxt.Text, out int pages) && double.TryParse(PrixTxt.Text, out double prix))
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (livreRow == null)
                    {
                        var cmd = new MySqlCommand("INSERT INTO livres (titre, auteur_id, nb_page, prix) VALUES (@titre, @id_auteur, @nb_pages, @prix)", conn);
                        cmd.Parameters.AddWithValue("@titre", TitreTxt.Text);
                        cmd.Parameters.AddWithValue("@id_auteur", AuteursCbx.SelectedValue);
                        cmd.Parameters.AddWithValue("@nb_pages", pages);
                        cmd.Parameters.AddWithValue("@prix", prix);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        var cmd = new MySqlCommand("UPDATE livres SET titre = @titre, auteur_id = @id_auteur, nb_page = @nb_page, prix = @prix WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@titre", TitreTxt.Text);
                        cmd.Parameters.AddWithValue("@id_auteur", AuteursCbx.SelectedValue);
                        cmd.Parameters.AddWithValue("@nb_page", pages);
                        cmd.Parameters.AddWithValue("@prix", prix);
                        cmd.Parameters.AddWithValue("@id", livreRow["id"]);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez enter un nombre pour nombre de pages et prix");
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
