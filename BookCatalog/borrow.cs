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
    public partial class borrow : Form
    {
        public borrow()
        {
            InitializeComponent();
        }

        private void borrow_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT BookName, BookID FROM Books WHERE Status = 'Available'", con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    booksComboBox.Items.Add(reader["BookName"].ToString());
                }

                reader.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           
            DateTime borrowedDate = dateTimePicker1.Value;

            Console.WriteLine("Date Borrowed changed to: " + borrowedDate.ToShortDateString());
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                Console.WriteLine("Enter key pressed on Date Borrowed");
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime returnDate = dateTimePicker2.Value;

            Console.WriteLine("Date of Return changed to: " + returnDate.ToShortDateString());
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Handle Enter key
            {
                Console.WriteLine("Enter key pressed on Date of Return");
            }
        }

        private void LogBtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO borrowTable ([Student ID], [Student Name], [Book Name], [Date Borrowed], [Date of Return], [Returned]) " +
                    "VALUES (@studentid, @studentname, @bookborrowed, @dateborrowed, @dateofreturn, @returned); " +
                    "SELECT SCOPE_IDENTITY();", con);  

                cmd.Parameters.AddWithValue("@studentid", int.Parse(studentidTb.Text));
                cmd.Parameters.AddWithValue("@studentname", studentnameTb.Text);
                cmd.Parameters.AddWithValue("@bookborrowed", booksComboBox.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@dateborrowed", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@dateofreturn", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@returned", "not yet returned");      

                int rentalID = Convert.ToInt32(cmd.ExecuteScalar());

                SqlCommand updateCmd = new SqlCommand(
                    "UPDATE Books SET Status = 'Rented' WHERE BookName = @bookname", con);
                updateCmd.Parameters.AddWithValue("@bookname", booksComboBox.SelectedItem.ToString());
                updateCmd.ExecuteNonQuery();

                MessageBox.Show($"Book Rented successfully! Your Rental ID is: {rentalID} \nPlease Take Note!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void RefreshBooksComboBox()
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";

            booksComboBox.Items.Clear(); 

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT BookName FROM Books WHERE Status = 'Available'", con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    booksComboBox.Items.Add(reader["BookName"].ToString());
                }
                reader.Close();
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            main mn = new main();
            mn.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void studentidTb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
