using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void borrowBtn_Click(object sender, EventArgs e)
        {
            borrow br = new borrow();
            br.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateLog ul = new updateLog();
            ul.Show();
            this.Hide();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            deleteLog dl = new deleteLog();
            dl.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            viewLog vl = new viewLog();
            vl.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            booksLog bl = new booksLog();
            bl.Show();
            this.Hide();
        }
    }
}
