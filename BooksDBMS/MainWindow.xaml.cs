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

        public MainWindow()
        {
            InitializeComponent();
            IzdDataTable = new DataTable();

        }

        private void OnPublistSelect(object sender, RoutedEventArgs e)
        {
            label.Content = "Образец инициализации объекта";
            string conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=Books;Integrated Security=True";
            publishersDataGrid.DataContext = IzdDataTable.DefaultView;//скармливает сюда datatable
            publishersDataGrid.Items.Refresh();
            using (SqlConnection sc = new SqlConnection(conn))
            {
                SqlCommand comm = new SqlCommand(@"SELECT [Value] FROM dbo.Izd", sc);
                IzdDataAdapter = new SqlDataAdapter(comm);
                IzdDataAdapter.Fill(IzdDataTable);
            }
            
        }

        private void OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            SqlCommandBuilder cb = new SqlCommandBuilder(IzdDataAdapter);//автоматически создаёт insert,delete и update.
        }
    }
}
