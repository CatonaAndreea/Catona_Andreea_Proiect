using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-IOJHVEI\\SQLEXPRESS;Initial Catalog=FinalProjectDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string SELECT_CMD = "SELECT id, title FROM books";
        
        SqlConnection conn;
        public MainWindow()
        {
            InitializeComponent();
            
            this.conn = new SqlConnection(connectionString);
            this.loadListBox();
        }

        private void loadListBox()
        {
            this.booksList.Items.Clear();
            conn.Open();

            SqlCommand command = new SqlCommand(SELECT_CMD, conn);

            SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                this.booksList.Items.Add(reader.GetInt32(0) + " - " + reader.GetString(1));
            }

            conn.Close();
        }

        private void InsertBook(object sender, RoutedEventArgs e)
        {
            string sqlCmd = this.generateInsertCommand(this.title.Text, this.author.Text, this.publishedBy.Text, int.Parse(this.pages.Text));

            SqlDataAdapter adapter = new SqlDataAdapter();

            conn.Open();
            adapter.InsertCommand = new SqlCommand(sqlCmd, conn);
            adapter.InsertCommand.ExecuteNonQuery();

            conn.Close();
            this.loadListBox();

            this.title.Clear();
            this.author.Clear();
            this.publishedBy.Clear();
            this.pages.Clear();
        }

        
        private void UpdateBook(object sender, RoutedEventArgs e)
        {

            try
            {
                string[] fields = this.booksList.SelectedItem.ToString().Trim().Split(" ");
                int id = int.Parse(fields[0]);
                string sqlCmd = this.generateUpdateCommand(id, this.title.Text, this.author.Text, this.publishedBy.Text, int.Parse(this.pages.Text));

                SqlDataAdapter adapter = new SqlDataAdapter();

                conn.Open();
                adapter.UpdateCommand = new SqlCommand(sqlCmd, conn);
                adapter.UpdateCommand.ExecuteNonQuery();

                conn.Close();
                this.loadListBox();

                this.title.Clear();
                this.author.Clear();
                this.publishedBy.Clear();
                this.pages.Clear();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
        }

        private void DeleteBook(object sender, RoutedEventArgs e)
        {

            try
            {
                string[] fields = this.booksList.SelectedItem.ToString().Trim().Split(" ");
                int id = int.Parse(fields[0]);
                string sqlCmd = this.generateDeleteCommand(id);

                SqlDataAdapter adapter = new SqlDataAdapter();

                conn.Open();
                adapter.DeleteCommand = new SqlCommand(sqlCmd, conn);
                adapter.DeleteCommand.ExecuteNonQuery();

                conn.Close();
                this.loadListBox();

                this.title.Clear();
                this.author.Clear();
                this.publishedBy.Clear();
                this.pages.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void onBookSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string[] fields= this.booksList.SelectedItem.ToString().Trim().Split(" ");
                int id = int.Parse(fields[0]);

                string sql = generateSelectBookById(id);

                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    this.title.Text = reader.GetString(1);
                    this.author.Text = reader.GetString(2);
                    this.publishedBy.Text = reader.GetString(3);
                    this.pages.Text = reader.GetInt32(4).ToString();
                }

                conn.Close();
            }catch (Exception)
            {
                Console.WriteLine("Error at book selection");
            }
            
        }


        private string generateInsertCommand(string title, string author, string publishedBy, int noPages)
        {
            return string.Format("insert into books values ('{0}', '{1}', '{2}', {3})", title, author, publishedBy, noPages);
        }

        private string generateSelectBookById(int id)
        {
            return string.Format("select * from books where id={0}", id);
        }

        private string generateDeleteCommand(int id)
        {
            return string.Format("delete from books where id={0}", id);
        }

        private string generateUpdateCommand(int id, string title, string author, string publishedBy, int noPages)
        {
            return string.Format("update books set title='{0}', author='{1}', publishedBy='{2}', pages={3} where id={4}", title, author, publishedBy, noPages, id);
        }
    }
}
