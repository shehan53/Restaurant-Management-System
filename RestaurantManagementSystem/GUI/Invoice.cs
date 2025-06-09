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

                if (parts.Length == 3 && parts[0] == itemcode)
                {

                    var item = new ListViewItem(parts[0]); // ID
                    item.SubItems.Add(parts[1]); // Name
                    item.SubItems.Add(parts[2]); // Price
                    
                    listProducts.Items.Add(item);
                    Console.WriteLine(parts[0] + parts[1] + parts[2]);
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
            txtCash.Text = "0";
            txtCard.Text = "0";
            txtName.Text = "";
            txtTpno.Text = "";
            lblBalance.Text = "0.00";
            lblDue.Text = "0.00";
            txtTotal.Text = "0";
            txtDiscount.Text = "0";
            txtNetTotal.Text = "0";
            table.Rows.Clear();
            btnSave.Enabled = true;
            btnRemove.Enabled = true;
            txtDiscount.Enabled = true;
            txtCash.Enabled = false;
            txtCard.Enabled = false;

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

                    if (parts.Length == 3)
                    {
                        string productName = parts[1];

                        // Case-insensitive "starts with" match (like 'ab%')
                        if (productName.StartsWith(name, StringComparison.OrdinalIgnoreCase))
                        {
                            var item = new ListViewItem(parts[0]); // ID
                            item.SubItems.Add(parts[1]);           // Name
                            item.SubItems.Add(parts[2]);           // Price
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
                txtCash.Enabled = true;
                txtCard.Enabled = true;
            }
            calItemCountAndQtyCount();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeProducts();
        }

        private void removeProducts()
        {
            if (table.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in table.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        table.Rows.Remove(row);
                    }
                }
            }
            else
            {
           
                MessageBox.Show("Please select a row to remove", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCash.Enabled = false;
                txtCard.Enabled = false;
            }
            calItemCountAndQtyCount();
            calDueAndBalance();
            if (table.SelectedRows.Count < 1)
            {
                txtCash.Enabled = false;
                txtCard.Enabled = false;
            }
        }

        private void calItemCountAndQtyCount()
        {
            double totalQty = 0;
            int itemCount = 0;
            double totalAmount = 0;

            foreach (DataGridViewRow row in table.Rows)
            {
                var totqty = row.Cells[3].Value;
                var amount = row.Cells[4].Value;
                if (totqty != null && !string.IsNullOrWhiteSpace(totqty.ToString()))
                {

                    double qty = double.Parse(totqty.ToString());
                    double totAmount = double.Parse(amount.ToString());
                    totalQty += qty;
                    totalAmount += totAmount;
                    itemCount++;
                }
            }
            lblItemCount.Text=(itemCount.ToString());
            lblTotQty.Text = (totalQty.ToString());
            txtTotal.Text = (totalAmount.ToString());
            txtNetTotal.Text = (totalAmount - double.Parse(txtDiscount.Text)).ToString();


        }

        private void txtDiscount_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtDiscount.Text.Equals("")) {
                txtDiscount.Text = "0";
            }
            calNetTotal();
            calDueAndBalance();
        }

        private void calNetTotal()
        {
            if (double.Parse(txtNetTotal.Text) >= double.Parse(txtDiscount.Text))
            {

                    txtNetTotal.Text = (double.Parse(txtTotal.Text) - double.Parse(txtDiscount.Text)).ToString();
            }
            else
            {
                MessageBox.Show("You can't add discount more than the Net Total", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiscount.Text = "0";
            }
        }

        private void txtCash_KeyUp(object sender, KeyEventArgs e)
        {
          
            calDueAndBalance();
        }

        private void calDueAndBalance()
        {
            if (txtCash.Text.Equals(""))
            {
                txtCash.Text = "0";
            }
            if (txtCard.Text.Equals(""))
            {
                txtCard.Text = "0";
            }

            if (!txtCash.Text.Equals("0") && txtCard.Text.Equals("0"))
            {
                if (double.Parse(txtNetTotal.Text) < double.Parse(txtCash.Text))
                {
                    lblBalance.Text = (double.Parse(txtCash.Text) - double.Parse(txtNetTotal.Text)).ToString("0.00");
                    lblDue.Text = "0.00";
                }
                else if (double.Parse(txtNetTotal.Text) > double.Parse(txtCash.Text))
                {
                    lblDue.Text = (double.Parse(txtNetTotal.Text) - double.Parse(txtCash.Text)).ToString("0.00");
                    lblBalance.Text = "0.00";
                }
                else
                {
                    lblDue.Text = "0.00";
                    lblBalance.Text = "0.00";
                }
            }
            else if (!txtCard.Text.Equals("0") && txtCash.Text.Equals("0"))
            {
                if (double.Parse(txtNetTotal.Text) < double.Parse(txtCard.Text))
                {
                    lblBalance.Text = (double.Parse(txtCard.Text) - double.Parse(txtNetTotal.Text)).ToString("0.00");
                    lblDue.Text = "0.00";
                }
                else if (double.Parse(txtNetTotal.Text) > double.Parse(txtCard.Text))
                {
                    lblDue.Text = (double.Parse(txtNetTotal.Text) - double.Parse(txtCard.Text)).ToString("0.00");
                    lblBalance.Text = "0.00";
                }
                else
                {
                    lblDue.Text = "0.00";
                    lblBalance.Text = "0.00";
                }
            }
            else 
                {
                    double CashCardTotal = double.Parse(txtCash.Text) + double.Parse(txtCard.Text);
                    if (double.Parse(txtNetTotal.Text) < CashCardTotal)
                    {
                        lblBalance.Text = (CashCardTotal - double.Parse(txtNetTotal.Text)).ToString("0.00");
                        lblDue.Text = "0.00";
                    }
                    else if (double.Parse(txtNetTotal.Text) > CashCardTotal)
                    {
                        lblDue.Text = (double.Parse(txtNetTotal.Text) - CashCardTotal).ToString("0.00");
                        lblBalance.Text = "0.00";
                    }
                    else
                    {
                        lblDue.Text = "0.00";
                        lblBalance.Text = "0.00";
                    }

                }
            
        }

        private void txtCard_KeyUp(object sender, KeyEventArgs e)
        {
            
            calDueAndBalance();
        }

        private void brnRefreshFields_Click(object sender, EventArgs e)
        {
            txtItemName.Text = "";
            txtUnitPrice.Text = "";
            txtQty.Text = "";
            txtPID.Text = "";
            txtItemCode.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (double.Parse(lblDue.Text) == 0)
            {
                saveInvoice();
            }
            else {
                MessageBox.Show("Please make the full payment", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveInvoice()
        {
            if (table.RowCount > 0)
            {

                string directoryPath = @"Records\Invoices\";
                


    // Ensure directory exists
    if (!Directory.Exists(directoryPath))
    {
        Directory.CreateDirectory(directoryPath);
    }

    // Find next available invoice number
    int invoiceNo = 1;
    string filePath;

    do
    {
        filePath = Path.Combine(directoryPath, "invoice-" + invoiceNo + ".csv");
        invoiceNo++;
    } while (File.Exists(filePath));

    invoiceNo--; // Step back to actual invoice number

    // Prepare CSV content using StringBuilder
    StringBuilder sb = new StringBuilder();
    string today = DateTime.Now.ToString("yyyy-MM-dd");
    // First lines with invoice info (can be separate or part of CSV)
    sb.AppendLine("Date-" + today);
    sb.AppendLine("Customer Name-" + txtName.Text);
    sb.AppendLine("Customer TP No-" + txtTpno.Text);
    sb.AppendLine("Invoice No-" + invoiceNo);
    sb.AppendLine("Total Amount-" + txtTotal.Text);
    sb.AppendLine("Total Net Amount-" + txtNetTotal.Text);
    sb.AppendLine("Discount-" + txtDiscount.Text);
    sb.AppendLine();
    sb.AppendLine("ItemCode | ProductName | UnitPrice | Quantity | Amount");

foreach (DataGridViewRow row in table.Rows)
{
    if (row.IsNewRow) continue;

    string itemCode = EscapeCsv(row.Cells[0].Value == null ? "" : row.Cells[0].Value.ToString());
    string productName = EscapeCsv(row.Cells[1].Value == null ? "" : row.Cells[1].Value.ToString());
    string unitPrice = EscapeCsv(row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString());
    string qty = EscapeCsv(row.Cells[3].Value == null ? "" : row.Cells[3].Value.ToString());
    string amount = EscapeCsv(row.Cells[4].Value == null ? "" : row.Cells[4].Value.ToString());

    sb.AppendLine(itemCode + " | " + productName + " | " + unitPrice + " | " + qty+ " | " + amount);
}

    // Write CSV to file
    File.WriteAllText(filePath, sb.ToString());

    MessageBox.Show("Invoice saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    refresh();

            }
            else {
                MessageBox.Show("Please add products to save", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EscapeCsv(string field)
        {
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                return "\"" + field + "\"";
            }
            return field;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(!txtInvoiceSearch.Text.Equals("")){
            loadInvoiceById(int.Parse(txtInvoiceSearch.Text));
            }else{
            MessageBox.Show("Please enter the invoice no", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadInvoiceById(int invNo)
        {
           string directoryPath = @"Records\Invoices\";
           string filePath = Path.Combine(directoryPath, "invoice-" + invNo + ".csv");

    if (!File.Exists(filePath))
    {
        MessageBox.Show("Invoice file not found.");
        return;
    }

    string[] lines = File.ReadAllLines(filePath);

 
    refresh();
    txtInvoiceSearch.Text = invNo.ToString();
    // Optional: variables to hold invoice info
    double totalAmount = 0;
    double totalNetAmount = 0;
    double discount = 0;
    string customerName = "";
    string customerTP = "";

    // Parse header info
    foreach (var line in lines)
    {
        if (line.StartsWith("Customer Name-"))
        {
            customerName=line.Substring("Customer Name-".Length);
        }
        else if (line.StartsWith("Customer TP No-"))
        {
            customerTP=line.Substring("Customer TP No-".Length);
        }
        else if (line.StartsWith("Total Amount-"))
        {
            double.TryParse(line.Substring("Total Amount-".Length), out totalAmount);
        }
        else if (line.StartsWith("Total Net Amount-"))
        {
            double.TryParse(line.Substring("Total Net Amount-".Length), out totalNetAmount);
        }
        else if (line.StartsWith("Discount-"))
        {
            double.TryParse(line.Substring("Discount-".Length), out discount);
        }
    }

    // Find the line index where the item table header starts
    int itemStartIndex = -1;
    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i].StartsWith("ItemCode"))
        {
            itemStartIndex = i + 1; // the next line contains first item
            break;
        }
    }

    if (itemStartIndex == -1)
    {
        MessageBox.Show("Invoice data not found in file.");
        return;
    }

    // Read each item line and add to DataGridView
    for (int i = itemStartIndex; i < lines.Length; i++)
    {
        string line = lines[i].Trim();
        if (string.IsNullOrEmpty(line)) continue;

        // Split by '|' and trim spaces
        var parts = line.Split('|').Select(p => p.Trim()).ToArray();

        if (parts.Length >= 5)
        {
            // Add row to DataGridView
            table.Rows.Add(parts[0], parts[1], parts[2], parts[3], parts[4]);
        }
    }

    // Optionally, set invoice info textboxes
    txtNetTotal.Text = totalNetAmount.ToString("F2");
    txtDiscount.Text = discount.ToString("F2");
    txtTotal.Text = totalAmount.ToString("F2");
    txtName.Text = customerName;
    txtTpno.Text = customerTP;
    btnSave.Enabled = false;
    btnRemove.Enabled = false;
    txtDiscount.Enabled = false;
    calItemCountAndQtyCount();
        }

        private void txtItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtPID_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtInvoiceSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtTpno_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowLettersOnly(e);
        }

        private void txtCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.numbersWithDecimals(sender, e);
        }

        private void txtCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.numbersWithDecimals(sender,e);
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.numbersWithDecimals(sender, e);
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowLettersOnly(e);

        } 
      

      
    }
}
