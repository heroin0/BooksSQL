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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;



namespace BooksDBMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable IzdDataTable;
        private SqlDataAdapter IzdDataAdapter;
        string conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=Books;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            IzdDataTable = new DataTable();
        }

        private void OnPublistSelect(object sender, RoutedEventArgs e)
        {
            label.Content = "Образец инициализации объекта";
            publishersDataGrid.BeginInit();
            publishersDataGrid.DataContext = IzdDataTable;//скармливает сюда datatable
            publishersDataGrid.Items.Refresh();
            publishersDataGrid.EndInit();
            using (SqlConnection sc = new SqlConnection(conn))
            {
                SqlCommand comm = new SqlCommand(@"SELECT * FROM dbo.Izd", sc);
                IzdDataAdapter = new SqlDataAdapter(comm);
                IzdDataAdapter.Fill(IzdDataTable);
            }
           
        }

        private void publishersDataGrid_AutoGeneratingColumn(object sender, Microsoft.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Key")
                e.Column.Visibility = Visibility.Collapsed;
        }

        private void UpdateDB()
        {
            DataTable pubTable = publishersDataGrid.DataContext as DataTable;
            if (pubTable != null)
            {
                using (SqlConnection sc = new SqlConnection(conn))
                {
                    SqlCommand comm = new SqlCommand(@"SELECT * FROM dbo.Izd", sc);
                    IzdDataAdapter = new SqlDataAdapter(comm);
                    IzdDataAdapter.Update(pubTable);
                }
            }
        }

        private void publishersDataGrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {

        }
    }
}
