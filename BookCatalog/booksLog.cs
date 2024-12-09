using iText.Layout.Element;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class booksLog : Form
    {
        public booksLog()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void booksLog_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";
            string query = "SELECT * FROM Books";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            main mn = new main();
            mn.Show();
            this.Close();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            addLog al = new addLog();
            al.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                string selectedbookID = selectedRow.Cells[0].Value.ToString(); 
  
                string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";
                string deleteQuery = $"DELETE FROM Books WHERE BookID = {selectedbookID}";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@bookID", selectedbookID);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Book deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            string query = "Select * from Books";
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                                DataTable dataTable = new DataTable();
                                dataAdapter.Fill(dataTable);

                                dataGridView1.DataSource = dataTable;
                                dataGridView1.Refresh();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
