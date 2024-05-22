using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace WinFormsPrak4
{
    public partial class Form1 : Form
    {
        OleDbConnection myConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\fedye\\source\\repos\\WinFormsPrak4\\DBPrak4.accdb");
        OleDbDataAdapter adapter = new OleDbDataAdapter();

        public Form1()
        {
            InitializeComponent();
            Books();
            ComboAuthor();
            ComboBook();
        }

        private void GetInfo()
        {
            myConn.Open();
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            ClearDataGridView();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            else
            {
                MessageBox.Show("Данные не найдены");
            }
            myConn.Close();
        }

        private void ClearDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void Books()
        {
            string query = "SELECT * FROM books";
            OleDbCommand command = new OleDbCommand(query, myConn);
            adapter.SelectCommand = command;
            GetInfo();
        }

        private void FindAuthor(object sender, EventArgs e)
        {
            string author = comboBox1.Text;
            myConn.Open();

            string query = "SELECT * FROM books WHERE (author = ?)";
            OleDbCommand command = new OleDbCommand(query, myConn);
            command.Parameters.AddWithValue("?", author);
            adapter.SelectCommand = command;

            myConn.Close();
            GetInfo();
        }

        private void AddBook(object sender, EventArgs e)
        {
            string author = textBox1.Text;
            string book = textBox2.Text;
            if (author == "" || book == "")
            {
                return;
            }

            myConn.Open();

            string query = "INSERT INTO books (Book, Author) VALUES (?, ?)";
            OleDbCommand command = new OleDbCommand(query, myConn);
            command.Parameters.AddWithValue("?", book);
            command.Parameters.AddWithValue("?", author);
            command.ExecuteNonQuery();

            myConn.Close();

            textBox1.Text = "";
            textBox2.Text = "";
            ComboAuthor();
            ComboBook();
            Books();
        }

        private void DelBook(object sender, EventArgs e)
        {
            string book = comboBox2.Text;

            myConn.Open();

            string query = "DELETE FROM books WHERE Book = ?";
            OleDbCommand command = new OleDbCommand(query, myConn);
            command.Parameters.AddWithValue("?", book);
            command.ExecuteNonQuery();

            myConn.Close();
            ComboAuthor();
            ComboBook();
            Books();
        }

        private void ComboAuthor()
        {
            myConn.Open();
            string query = "SELECT DISTINCT Author FROM books";
            OleDbCommand command = new OleDbCommand(query, myConn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            comboBox1.DataSource = dataTable;
            comboBox1.DisplayMember = "Author";
            myConn.Close();
        }

        private void ComboBook()
        {
            myConn.Open();
            string query = "SELECT DISTINCT Book FROM books";
            OleDbCommand command = new OleDbCommand(query, myConn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            comboBox2.DataSource = dataTable;
            comboBox2.DisplayMember = "Book";
            myConn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Books();
        }
    }
}
