using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestaurantManagementSystem.Classes;
namespace RestaurantManagementSystem
{
    public partial class Product_Management : Form
    {
        public Product_Management()
        {
            InitializeComponent();
            loadProductData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtItemCode.Text.Equals("") && !txtItemName.Text.Equals("") && !txtPrice.Text.Equals(""))
            {
                string itemCode = txtItemCode.Text.Trim();
                string itemName = txtItemName.Text.Trim();
                string price = txtPrice.Text.Trim();

                tableProduct.Rows.Add(itemCode, itemName, price);
                saveProductDetails();
                refresh();
            }
            else
            {
                MessageBox.Show("Please fill all the fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void refresh()
        {
            txtItemCode.Text = "";
            txtItemName.Text = "";
            txtPrice.Text = "";
            txtProductID.Text = "";
        }

        private void saveProductDetails()
        {
            string directoryPath = @"Records\Products\";

            // Ensure directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, "Products.txt");
            bool fileExists = File.Exists(filePath);
            bool hasCsvFile = Directory.GetFiles(directoryPath, "*.txt").Length > 0;

            // Prepare data
            string itemCode = txtItemCode.Text.Trim();
            string itemName = txtItemName.Text.Trim();
            string price = txtPrice.Text.Trim();
            string dataLine = itemCode + "|" + itemName + "|" + price;

            // First time: write header + data
            if (!fileExists && !hasCsvFile)
            {
                using (StreamWriter sw = new StreamWriter(filePath, false)) // false = overwrite
                {
                    sw.WriteLine("ID|Name|Price");
                    sw.WriteLine(dataLine);
                }
            }
            else
            {
                // File exists or any csv exists in the folder: append only data
                using (StreamWriter sw = new StreamWriter(filePath, true)) // true = append
                {
                    sw.WriteLine(dataLine);
                }
            }

            MessageBox.Show("Product saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadProductData();
        }

        private void loadProductData()
        {
            string filePath = @"Records\Products\Products.txt";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Product file not found.");
                return;
            }

            // Clear existing rows/columns
            tableProduct.Rows.Clear();
            tableProduct.Columns.Clear();

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                MessageBox.Show("Product file is empty.");
                return;
            }

            // Set up columns from the header
            string[] headers = lines[0].Split('|');
            foreach (string header in headers)
            {
                tableProduct.Columns.Add(header.Trim(), header.Trim());
            }

            // Add data rows
            for (int i = 1; i < lines.Length; i++) // Start from 1 to skip the header
            {
                string[] fields = lines[i].Split('|');
                for (int j = 0; j < fields.Length; j++)
                {
                    fields[j] = fields[j].Trim(); // Trim whitespace
                }
                tableProduct.Rows.Add(fields);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!txtItemCode.Text.Equals(""))
            {
                UpdateProductDetails();
                refresh();
            }
            else
            {
                MessageBox.Show("Please enter product code to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateProductDetails()
        {
             string filePath = @"Records\Products\Products.txt";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Product records not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string itemCode = txtItemCode.Text.Trim();
            string itemName = txtItemName.Text.Trim();
            string price = txtPrice.Text.Trim();

            var lines = File.ReadAllLines(filePath).ToList();

            bool updated = false;

            // Keep header line and search for the record to update
            for (int i = 1; i < lines.Count; i++) // Start at 1 to skip header
            {
                var parts = lines[i].Split('|');

                if (parts.Length >= 3 && parts[0].Trim() == itemCode)
                {
                    // Update the line
                    lines[i] = itemCode + "|" + itemName + "|" + price;
                    updated = true;
                    break;
                }
            }
            if (updated)
            {
                File.WriteAllLines(filePath, lines);
                MessageBox.Show("Product updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Product not found for update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadProductData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!txtItemCode.Text.Equals(""))
            {
                deleteProductDetails();
                refresh();
            }
            else
            {
                MessageBox.Show("Please enter product code to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void deleteProductDetails()
        {
            string filePath = @"Records\Products\Products.txt";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Product file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string empCodeToDelete = txtItemCode.Text.Trim();
            var lines = File.ReadAllLines(filePath).ToList();

            bool deleted = false;

            // Keep the header and filter out the matching row
            var newLines = new List<string>();
            newLines.Add(lines[0]); // Keep header

            for (int i = 1; i < lines.Count; i++)
            {
                var parts = lines[i].Split('|');
                if (parts.Length >= 3 && parts[0].Trim() != empCodeToDelete)
                {
                    newLines.Add(lines[i]); // Keep line if it's not the one to delete
                }
                else if (parts[0].Trim() == empCodeToDelete)
                {
                    deleted = true; // Mark as deleted
                }
            }

            if (deleted)
            {
                File.WriteAllLines(filePath, newLines);
                MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Product not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadProductData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!txtProductID.Text.Equals(""))
            {
                searchProductById();
            }
            else
            {
                MessageBox.Show("Please enter product code to search", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void searchProductById()
        {
            string filePath = @"Records\Products\Products.txt";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Product file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string searchCode = txtProductID.Text.Trim();
            var lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++) // Skip header
            {
                var parts = lines[i].Split('|');
                if (parts.Length >= 3)
                {
                    string empCode = parts[0].Trim();
                    string empName = parts[1].Trim();
                    string empRole = parts[2].Trim();

                    if (empCode.Equals(searchCode, StringComparison.OrdinalIgnoreCase))
                    {
                        txtItemName.Text = empName;
                        txtPrice.Text = empRole;
                        txtItemCode.Text = empCode;


                        return;
                    }
                }
            }
        }

        private void tableProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            fetchProductsFromDatagrid(e);
        }

        private void fetchProductsFromDatagrid(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = tableProduct.Rows[e.RowIndex];

                if (row.Cells[0].Value != null)
                    txtItemCode.Text = row.Cells[0].Value.ToString();

                if (row.Cells[1].Value != null)
                    txtItemName.Text = row.Cells[1].Value.ToString();

                if (row.Cells[2].Value != null)
                    txtPrice.Text = row.Cells[2].Value.ToString();
            }   
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtProductID.Text = "";
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowLettersOnly(e);
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.numbersWithDecimals(sender, e);
        }

        private void txtProductID_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }
    }
}
