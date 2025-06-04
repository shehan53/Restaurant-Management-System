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

namespace RestaurantManagementSystem
{
    public partial class Invoice : Form
    {
        public Invoice()
        {
            InitializeComponent();
            listProducts.Visible = false;
        }

        private void txtItemCode_KeyUp(object sender, KeyEventArgs e)
        {
            SearchProductByID(txtItemCode.Text.Trim());
        }

        private void SearchProductByID(string itemcode)
        {
            string filePath = @"Records\Products\Products.txt";

            /* if (!File.Exists(filePath))
             {
                 MessageBox.Show("Product file not found.");
                 return;
             }
            */
            listProducts.Items.Clear();
            if (!itemcode.Equals(""))
            {
                listProducts.Visible = true;
       
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');

                if (parts.Length == 4 && parts[0] == itemcode)
                {

                    var item = new ListViewItem(parts[0]); // ID
                    item.SubItems.Add(parts[1]); // Name
                    item.SubItems.Add(parts[2]); // Price
                    item.SubItems.Add(parts[3]); // Qty
                    listProducts.Items.Add(item);
                    Console.WriteLine(parts[0] + parts[1] + parts[2] + (parts[3]));
                    return;
                }



            }
            Console.WriteLine(listProducts);
            listProducts.Items.Clear();
        } 
        }

       

        private void panelSalesControl_MouseClick(object sender, MouseEventArgs e)
        {
            listProducts.Visible = false;
        }

      
        
        private void listProducts_MouseClick(object sender, MouseEventArgs e)
        {
            populateFields();

        }

        private void populateFields()
        {
            if (listProducts.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listProducts.SelectedItems[0];
                txtItemName.Text = selectedItem.SubItems[1].Text; // Name
                txtUnitPrice.Text = selectedItem.SubItems[2].Text;    // Price
                txtQty.Text = selectedItem.SubItems[3].Text.Trim(); // Qty
                txtPID.Text = selectedItem.SubItems[0].Text;  // PID
                txtItemCode.Text = "";
                txtQty.Focus();
                txtQty.SelectionStart = txtQty.Text.Length; // Move cursor to end of text
                txtQty.SelectionLength = 0;
                listProducts.Visible = false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            txtItemName.Text = "";
            txtUnitPrice.Text = "";
            txtQty.Text = "";
            txtPID.Text = "";
            txtItemCode.Text = "";
            lblTotQty.Text = "0";
            lblItemCount.Text = "0";
            txtInvoiceSearch.Text = "";
            listProducts.Visible=false;
            txtCash.Text = "";
            txtCard.Text = "";
            txtName.Text = "";
            txtTpno.Text = "";
            lblBalance.Text = "0.00";
            lblDue.Text = "0.00";
            txtTotal.Text = "";
            txtDiscount.Text = "";
            txtNetTotal.Text = "";
            table.Rows.Clear();

        }

        private void txtItemName_KeyUp(object sender, KeyEventArgs e)
        {
            SearchProductByName(txtItemName.Text.Trim());
        }

        private void SearchProductByName(string name)
        {
            string filePath = @"Records\Products\Products.txt";

            listProducts.Items.Clear();

            if (!string.IsNullOrWhiteSpace(name))
            {
                listProducts.Visible = true;

                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');

                    if (parts.Length == 4)
                    {
                        string productName = parts[1];

                        // Case-insensitive "starts with" match (like 'ab%')
                        if (productName.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                        {
                            var item = new ListViewItem(parts[0]); // ID
                            item.SubItems.Add(parts[1]);           // Name
                            item.SubItems.Add(parts[2]);           // Price
                            item.SubItems.Add(parts[3]);           // Qty
                            listProducts.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                listProducts.Visible = false;
            }
        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            addItemsToTable(e);
        }

        private void addItemsToTable(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the ding sound
                e.SuppressKeyPress = true;

                string code = txtPID.Text.Trim();
                string name = txtItemName.Text.Trim();
                double unitPrice = Double.Parse(txtUnitPrice.Text.Trim());
                double qty = Double.Parse(txtQty.Text.Trim());
                double total = qty * unitPrice;
                table.Rows.Add(code, name, unitPrice.ToString("0.00"), qty, total.ToString("0.00"));
                txtItemCode.Clear();
                txtItemName.Clear();
                txtUnitPrice.Clear();
                txtQty.Clear();
                txtPID.Clear();
                txtItemCode.Focus();
            }
        }

      

      
    }
}
