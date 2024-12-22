using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TP10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "server=localhost;uid=root;pwd=;database=gestion_bibliotheque";
        private DataTable livresTable;

        public MainWindow()
        {
            InitializeComponent();
            AfficherLivres();
            AfficherAuteurs();
            LivreStatistique(livresTable);
        }

        private void AfficherLivres(string req = "SELECT livres.id AS Id, titre as Titre, auteur_id, auteurs.nom AS Auteur, nb_page AS NbPages, prix AS Prix FROM livres INNER JOIN auteurs ON livres.auteur_id = auteurs.id")
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(req, conn);
                livresTable = new DataTable();
                adapter.Fill(livresTable);
                LivresDataGrid.ItemsSource = livresTable.DefaultView;
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
                        AuteurFilterCmb.Items.Add(new { Id = reader.GetInt32("id"), Nom = reader.GetString("nom") });
                    }
                }
            }

            AuteurFilterCmb.DisplayMemberPath = "Nom";
            AuteurFilterCmb.SelectedValuePath = "Id";
        }
        private void FilterEtTrierLivres()
        {
            var filteredBooks = livresTable.AsEnumerable();
            string req = "SELECT livres.id AS Id, titre as Titre, auteur_id, auteurs.nom AS Auteur, nb_page AS NbPages, prix AS Prix FROM livres INNER JOIN auteurs ON livres.auteur_id = auteurs.id";

            if (!string.IsNullOrEmpty(TitreFilterTxb.Text) && AuteurFilterCmb.SelectedItem == null)
            {
                req = req + " WHERE titre LIKE '%" + TitreFilterTxb.Text + "%'";
            }

            if (AuteurFilterCmb.SelectedItem != null && string.IsNullOrEmpty(TitreFilterTxb.Text))
            {
                var selectedAuteur = AuteurFilterCmb.SelectedValue;
                req = req + " WHERE auteur_id = " +  Convert.ToInt32(selectedAuteur);
            }

            if (AuteurFilterCmb.SelectedItem != null && !string.IsNullOrEmpty(TitreFilterTxb.Text))
            {
                var selectedAuteur = AuteurFilterCmb.SelectedValue;
                req = req + " WHERE titre LIKE '%" + TitreFilterTxb.Text + "%'" + " AND auteur_id = " + selectedAuteur;
            }

            if (TrierParCmb.SelectedItem != null)
            {
                ComboBoxItem selectedItem = TrierParCmb.SelectedItem as ComboBoxItem;
                string sortBy = selectedItem.Content.ToString();
                bool ascending = AscendantRadioButton.IsChecked == true;

                switch (sortBy)
                {
                    case "Nombre de Pages":
                        req = ascending ? req + " ORDER BY nb_page" : req + " ORDER BY nb_page DESC";
                        break;
                    case "Prix":
                        req = ascending ? req + " ORDER BY prix" : req + " ORDER BY prix DESC";
                        break;
                    case "Titre":
                        req = ascending ? req + " ORDER BY titre" : req + " ORDER BY titre DESC";
                        break;
                }
            }
            AfficherLivres(req);
            LivreStatistique(livresTable);
        }

        private void LivreStatistique(DataTable livresStatistique)
        {
            if (livresStatistique.Rows.Count > 0)
            {
                TtlLivreLbl.Content = $"Total des livres : {livresStatistique.Rows.Count}";
                MoyenPrixLbl.Content = $"Moyenne des prix : {livresStatistique.AsEnumerable().Average(row => row.Field<double>("Prix")):F2}";
            } else
            {
                TtlLivreLbl.Content = $"Total des livres : 0";
                MoyenPrixLbl.Content = $"Moyenne des prix : 0";

            }
        }

        private void OuvrirCrudAuteur_Click(object sender, RoutedEventArgs e)
        {
            ListeAuteurWindow auteurs = new ListeAuteurWindow();
            auteurs.Show();
            this.Close();
        }

        private void OuvrirCrudLivre_Click(object sender, RoutedEventArgs e)
        {
            ListeLivreWindow livres = new ListeLivreWindow();
            livres.Show();
            this.Close();
        }

        private void Appliquer_Click(object sender, RoutedEventArgs e)
        {
            FilterEtTrierLivres();
        }

        private void Effacer_Click(object sender, RoutedEventArgs e)
        {
            TitreFilterTxb.Text = String.Empty;
            AuteurFilterCmb.SelectedValue = null;
            TrierParCmb.SelectedValue = null;
            AfficherLivres();
            LivreStatistique(livresTable);
        }
    }
}