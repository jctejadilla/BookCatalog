using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Library
{
    public partial class deleteLog : Form
    {
        public deleteLog()
        {
            InitializeComponent();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = "DELETE FROM borrowTable WHERE [Rental ID] = @rentalid";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (studentidTb.Text != "")
                        {
                    
                            if (int.TryParse(studentidTb.Text, out int rentalID))
                            {
                                cmd.Parameters.AddWithValue("@rentalid", rentalID);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("No records found for the given Student ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please enter a valid Student ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter a Student ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (this.Owner is borrow borrowForm)
                    {
                        borrowForm.RefreshBooksComboBox();  
                    }

                    studentidTb.Clear();
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
    }
}
