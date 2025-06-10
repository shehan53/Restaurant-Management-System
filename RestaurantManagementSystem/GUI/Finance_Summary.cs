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
using System.Globalization;
namespace RestaurantManagementSystem.GUI
{
    public partial class Finance_Summary : Form
    {
        public Finance_Summary()
        {
            InitializeComponent();
            loadFinanceSummary();
        }

        private void loadFinanceSummary()
        {
            string directoryPath = @"Records\Invoices\";
            string[] files = Directory.GetFiles(directoryPath, "invoice-*.csv");

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            decimal totalAmount = 0;
            int invoiceCount = 0;

            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);

                if (lines.Length < 6) continue; // Skip if file is incomplete

                // Parse date from first line: "Date-2025-06-09"
                string dateLine = lines[0].Trim(); // Ex: "Date-2025-06-09"

                if (!dateLine.StartsWith("Date-")) continue;

                string dateStr = dateLine.Substring(5); // Get just "2025-06-09"
                DateTime invoiceDate;
                if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate))
                    continue;



                // Check if invoice date is within range
                if (invoiceDate >= fromDate && invoiceDate <= toDate)
                {
                    invoiceCount++;

                    // Get amount from 6th line: "Total Net Amount-300"
                    string amountLine = lines[5];
                    string[] amountParts = amountLine.Split('-');
                    decimal amount;
                    if (amountParts.Length == 2 && decimal.TryParse(amountParts[1], out amount))
                    {
                        totalAmount += amount;
                    }
                }
                loadPopularFoods();
            }

            // Set the values to text boxes
            txtTotalRs.Text = totalAmount.ToString("0.00");
            txtTotalCount.Text = invoiceCount.ToString();
        }

        private void loadPopularFoods()
        {
            string directoryPath = @"Records\Invoices\";
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("Invoice directory not found.");
                return;
            }

            string[] files = Directory.GetFiles(directoryPath, "invoice-*.csv");

            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date;

            Dictionary<string, int> foodSummary = new Dictionary<string, int>();

            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);

                if (lines.Length < 8) continue;

                string dateLine = lines[0].Trim(); // Ex: "Date-2025-06-08"
                if (!dateLine.StartsWith("Date-")) continue;

                string dateStr = dateLine.Substring(5);
                DateTime invoiceDate;
                if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate))
                    continue;

                if (invoiceDate < fromDate || invoiceDate > toDate)
                    continue;

                int headerIndex = Array.FindIndex(lines, line => line.StartsWith("ItemCode"));
                if (headerIndex == -1 || headerIndex + 1 >= lines.Length) continue;

                for (int i = headerIndex + 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('|');
                    if (parts.Length < 5) continue;

                    string productName = parts[1].Trim();

                    decimal quantityDecimal;
                    if (!decimal.TryParse(parts[3].Trim(), out quantityDecimal)) continue;

                    int quantity = (int)quantityDecimal;

                    if (foodSummary.ContainsKey(productName))
                        foodSummary[productName] += quantity;
                    else
                        foodSummary[productName] = quantity;
                }
            }

            // Clear existing rows
            dgvFood.Rows.Clear();

            // Add rows directly to the existing grid
            foreach (var item in foodSummary.OrderByDescending(f => f.Value))
            {
                dgvFood.Rows.Add(item.Key, item.Value);
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            refresh();
            loadFinanceSummary();
        }

        private void refresh()
        {
            dgvFood.Rows.Clear();
            txtTotalRs.Text = "";
            txtTotalCount.Text = "";
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            refresh();
            loadFinanceSummary();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            refresh();
            loadFinanceSummary();
        }
    }
}
