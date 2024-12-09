using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library
{
    public partial class addLog : Form
    {
        public addLog()
        {
            InitializeComponent();
        }

        private void LogBtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open(); 

                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Books ([BookName], [Author], [ISBN], [Status]) " +
                        "VALUES (@bookname, @author, @isbn, @status);", con
                    );


                    cmd.Parameters.AddWithValue("@bookname", booknameTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@author", authorTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@isbn", isbnTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", "Available"); 

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                        booknameTb.Clear();
                        authorTb.Clear();
                        isbnTb.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add the book. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            booksLog bl = new booksLog();
            bl.Show();
            this.Close();
        }

        private void isbnTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
