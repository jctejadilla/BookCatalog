using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Library
{
    public partial class viewLog : Form
    {
        public viewLog()
        {
            InitializeComponent();
        }

        private void viewLog_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";
            string query = "SELECT * FROM borrowTable";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            ExportToPdf();
        }

        private void ExportToPdf()
        {
            try
            {
                string pdfFilePath = @"C:\Users\jctej\Downloads\borrowTable.pdf";

                using (FileStream fs = new FileStream(pdfFilePath, FileMode.Create))
                {
                    using (PdfDocument pdfDoc = new PdfDocument())
                    {
                        PdfPage page = pdfDoc.Pages.Add();
                        PdfGraphics graphics = page.Graphics;
                        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                        float yPos = 10;  
                        float xPos = 10;  
                        float rowHeight = 20;
                        float columnWidth = 100; 
                        float tableWidth = dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + 20;

                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            graphics.DrawString(column.HeaderText, font, PdfBrushes.Black, xPos, yPos);
                            xPos += columnWidth;  
                        }

                        yPos += rowHeight; 
                        xPos = 10; 

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.IsNewRow) continue; 

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                graphics.DrawString(cell.Value?.ToString(), font, PdfBrushes.Black, xPos, yPos);
                                xPos += columnWidth; 
                            }

                            yPos += rowHeight; 
                            xPos = 10; 

                            if (yPos + rowHeight > page.GetClientSize().Height)
                            {
                                page = pdfDoc.Pages.Add(); 
                                yPos = 10; 
                            }
                        }

                        pdfDoc.Save(fs); 
                    }

                    MessageBox.Show("Data saved to PDF successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv()
        {
            try
            {
                string csvFilePath = @"C:\Users\jctej\Downloads\Log.csv";

                StringBuilder csvContent = new StringBuilder();

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    csvContent.Append(column.HeaderText + ",");
                }

                csvContent.Length--;

                csvContent.AppendLine();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue; 

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellValue = cell.Value?.ToString().Replace(",", ""); 
                        csvContent.Append(cellValue + ",");
                    }

                    csvContent.Length--;
                    csvContent.AppendLine();
                }

                File.WriteAllText(csvFilePath, csvContent.ToString());

                MessageBox.Show("Data saved to Excel successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            main mn = new main();
            mn.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void excelBtn_Click(object sender, EventArgs e)
        {
            ExportToCsv();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                
               // int selectedrentalID = int.Parse(selectedRow.Cells[0].Value.ToString());

                string connectionString = @"Data Source=JOANNE;Initial Catalog=librarydb;Integrated Security=True;TrustServerCertificate=True";
                string deleteQuery = "DELETE FROM borrowTable WHERE [Rental ID] = @rentalid";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                        {
                            if (int.TryParse(selectedRow.Cells[0].Value.ToString(), out int selectedrentalID))
                            {
                                // Get Book's ID
                                string bookIDquery = $"SELECT BookID from Books WHERE BookName = '{selectedRow.Cells[3].Value.ToString()}'";
                                string rentalBookID = "";
                                using (SqlConnection conn = new SqlConnection(connectionString))
                                {
                                    SqlDataAdapter dataAdapter = new SqlDataAdapter(bookIDquery, conn);
                                    DataTable dataTable = new DataTable();
                                    dataAdapter.Fill(dataTable);
                                    rentalBookID = dataTable.Rows[0]["BookID"].ToString();
                                }

                                // Execute Delete
                                cmd.Parameters.AddWithValue("@rentalid", selectedrentalID);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Log deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Update Books
                                    string updateQuery = "UPDATE Books SET Status = 'Available' WHERE BookID = @rentalbookID";
                                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                                    updateCmd.Parameters.AddWithValue("@rentalbookID", rentalBookID);
                                    updateCmd.ExecuteNonQuery();
                                }

                                //Refresh borrowTable
                                string query = "Select * from borrowTable";
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
