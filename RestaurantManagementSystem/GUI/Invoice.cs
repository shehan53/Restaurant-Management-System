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
            }
            else
            {
                listProducts.Visible = false;
            }
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
                    Console.WriteLine(parts[0] + parts[1] +parts[2]+(parts[3]));
                    return;
                }
               

   
            }
           Console.WriteLine(listProducts);
            listProducts.Items.Clear();
            
        }

        private void panelSalesControl_MouseClick(object sender, MouseEventArgs e)
        {
            listProducts.Visible = false;
        }

      
        
        private void listProducts_MouseClick(object sender, MouseEventArgs e)
        {
            if (listProducts.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listProducts.SelectedItems[0];

                //txtItemCode.Text = selectedItem.SubItems[0].Text; // ID
                txtItemName.Text = selectedItem.SubItems[1].Text; // Name
                txtUnitPrice.Text = selectedItem.SubItems[2].Text;    // Price
                txtQty.Text = selectedItem.SubItems[3].Text.Trim(); // Qty
                txtPID.Text = selectedItem.SubItems[0].Text;  // PID
                txtItemCode.Text = "";
                txtQty.Focus();
                txtQty.SelectionStart = txtQty.Text.Length; // Move cursor to end of text
                txtQty.SelectionLength = 0;  
                // Optionally hide the ListView after selection:
                listProducts.Visible = false;
            }
        }

      

      
    }
}
