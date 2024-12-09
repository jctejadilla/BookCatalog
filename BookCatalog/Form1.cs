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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True");
            con.Open();
            string username = userTxt.Text;
            string password = passTxt.Text;
            SqlCommand cmd = new SqlCommand("SELECT Username, Password FROM loginTable WHERE Username = @Username AND Password = @Password", con);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                main mn = new main();
                mn.Show();
                this.Hide();
                
            }
            else {
                MessageBox.Show("Invalid Username or Password");
            }
            con.Close();
        }
    }
}
