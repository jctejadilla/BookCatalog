using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library
{
    public partial class updateLog : Form
    {
        public updateLog()
        {
            InitializeComponent();
        }

        private void updateLog_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT BookName FROM Books WHERE Status = 'Rented'", con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    booksComboBox.Items.Add(reader["BookName"].ToString());
                }

                reader.Close();
            }
        }

        private void LogBtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string query = @"UPDATE borrowTable 
                             SET [Book Name] = @bookborrowed, 
                                 [Date Borrowed] = @dateborrowed, 
                                 [Date of Return] = @dateofreturn,
                                 [Returned] = @returned 
                             WHERE [Rental ID] = @rentalid";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        if (!int.TryParse(rentalidTb.Text, out int rentalID))
                        {
                            MessageBox.Show("Please enter a valid Rental ID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        cmd.Parameters.AddWithValue("@rentalid", rentalID);
                        

                        if (booksComboBox.SelectedItem == null)
                        {
                            MessageBox.Show("Please select a book.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        cmd.Parameters.AddWithValue("@bookborrowed", booksComboBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@dateborrowed", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@dateofreturn", dateTimePicker2.Value);

                        if (returnedCb.SelectedItem == null)
                        {
                            MessageBox.Show("Please select the returned status.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        cmd.Parameters.AddWithValue("@returned", returnedCb.SelectedItem.ToString());

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            if (returnedCb.SelectedItem.ToString() == "Returned")
                            {
                                
                                string updateBookStatusQuery = "UPDATE Books SET Status = 'Available' WHERE BookName = @bookname";
                                using (SqlCommand updateBookCmd = new SqlCommand(updateBookStatusQuery, con))
                                {
                                    updateBookCmd.Parameters.AddWithValue("@bookname", booksComboBox.SelectedItem.ToString());
                                    updateBookCmd.ExecuteNonQuery();
                                }

                                MessageBox.Show("Book Returned and Information Updated!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Information Updated!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the Rental ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
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
            main mn = new main();
            mn.Show();
            this.Hide();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            int rentID = int.Parse(rentalidTb.Text);
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";
            string query = $"select * from borrowTable where [Rental ID] = {rentID};";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0) {
                    studentidTb.Text = dataTable.Rows[0]["Student ID"].ToString();
                    booksComboBox.Text = dataTable.Rows[0]["Book Name"].ToString();
                    dateTimePicker1.Text = dataTable.Rows[0]["Date Borrowed"].ToString();
                    dateTimePicker2.Text = dataTable.Rows[0]["Date of Return"].ToString();
                    returnedCb.Text = dataTable.Rows[0]["Returned"].ToString();
                }
                else
                {
                    MessageBox.Show($"Rental ID not found.", "Information.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    studentidTb.Clear();
                }
            }
        }

        private void rentalidTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void studentidTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void rentalidTb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
