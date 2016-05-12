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
        private DataTable IzdDataTable,BooksDataTable,AuthorsDataTable;
        private SqlDataAdapter IzdDataAdapter, BooksDataAdapter, AuthorsDataAdapter;
        string conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=Books;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            IzdDataTable = new DataTable();
            BooksDataTable = new DataTable();
            AuthorsDataTable = new DataTable();
        }

        #region publishers
        private void OnPublistSelect(object sender, RoutedEventArgs e)
        {
            label.Content = "Образец инициализации объекта";
            publishersDataGrid.BeginInit();
            publishersDataGrid.Items.Refresh();
            publishersDataGrid.DataContext = IzdDataTable;//скармливает сюда datatable
            publishersDataGrid.EndInit();
            using (SqlConnection sc = new SqlConnection(conn))
            {
                SqlCommand comm = new SqlCommand(@"SELECT * FROM VIEW_FOR_DBSM_PUBLISHERS", sc);
                IzdDataAdapter = new SqlDataAdapter(comm);
                IzdDataTable.Clear();
                IzdDataAdapter.Fill(IzdDataTable);
            }
        }

        private void publishersDataGrid_AutoGeneratingColumn(object sender, Microsoft.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            //if (e.Column.Header.ToString() == "Key")
            //    e.Column.Visibility = Visibility.Collapsed;
        }

        private void UpdateDB()
        {
            DataTable pubTable = publishersDataGrid.DataContext as DataTable;
            if (pubTable != null)
            {
                using (SqlConnection sc = new SqlConnection(conn))
                {
                    sc.Open();
                    SqlCommand comm = new SqlCommand(@"SELECT Value AS Издательство FROM dbo.Izd", sc);
                    IzdDataAdapter = new SqlDataAdapter(comm);
                    SqlCommandBuilder commBuilder = new SqlCommandBuilder(IzdDataAdapter);
                    try
                    {
            //            pubTable.AcceptChanges();
                        IzdDataAdapter.Update(pubTable);
                    }
                    catch (DBConcurrencyException) { };

                }
            }
        }

        private void publishersDataGrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            (e.Row.Item as DataRowView).EndEdit();
            UpdateDB();
        }

        #endregion

        #region books

        private void booksDataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void booksDataGrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            (e.Row.Item as DataRowView).EndEdit();
            UpdateDB();
        }

        private void booksDataGrid_AutoGeneratingColumn(object sender, Microsoft.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            //if (e.Column.Header.ToString() == "Book Key")
            //    e.Column.Visibility = Visibility.Collapsed;
        }

        private void OnBooklistSelect(object sender, RoutedEventArgs e)
        {
            booksDataGrid.BeginInit();
            booksDataGrid.Items.Refresh();
            booksDataGrid.DataContext = BooksDataTable;//скармливает сюда datatable
            booksDataGrid.EndInit();
            using (SqlConnection sc = new SqlConnection(conn))
            {
                SqlCommand comm = new SqlCommand(@"SELECT * FROM dbo.VIEW_FOR_DBSM_BOOKS", sc);
                BooksDataAdapter = new SqlDataAdapter(comm);
                BooksDataTable.Clear();
                BooksDataAdapter.Fill(BooksDataTable);
            }
        }

        #endregion
    }
}
