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
namespace RestaurantManagementSystem.GUI
{
    public partial class Employee_Management : Form
    {
        public Employee_Management()
        {
            InitializeComponent();
            loadEmployeeData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtEmpCode.Text.Equals("") && !txtEmpName.Text.Equals("") && !txtRole.Text.Equals(""))
            {
                string empCode = txtEmpCode.Text.Trim();
                string empName = txtEmpName.Text.Trim();
                string empRole = txtRole.Text.Trim();

                empTable.Rows.Add(empCode, empName, empRole);
                saveEmpDetails();
                refresh();
            }
            else
            {
                MessageBox.Show("Please fill all the fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveEmpDetails()
        {
            string directoryPath = @"Records\Employees\";

            // Ensure directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, "Employees.csv");
            bool fileExists = File.Exists(filePath);
            bool hasCsvFile = Directory.GetFiles(directoryPath, "*.csv").Length > 0;

            // Prepare data
            string empCode = txtEmpCode.Text.Trim();
            string empName = txtEmpName.Text.Trim();
            string empRole = txtRole.Text.Trim();
            string dataLine = empCode + " | " + empName + " | " + empRole;

            // First time: write header + data
            if (!fileExists && !hasCsvFile)
            {
                using (StreamWriter sw = new StreamWriter(filePath, false)) // false = overwrite
                {
                    sw.WriteLine("Emp Code | Employee Name | Role");
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

            MessageBox.Show("Employee saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadEmployeeData();
        }

        private void refresh()
        {
            txtEmpCode.Clear();
            txtEmpName.Clear();
            txtRole.Clear();
            txtEmpid.Clear();
            txtEmpCode.Enabled = true;
           
        }
        private void loadEmployeeData()
        {
            string filePath = @"Records\Employees\Employees.csv";

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Employee file not found.");
                return;
            }

            // Clear existing rows/columns
            empTable.Rows.Clear();
            empTable.Columns.Clear();

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                MessageBox.Show("Employee file is empty.");
                return;
            }

            // Set up columns from the header
            string[] headers = lines[0].Split('|');
            foreach (string header in headers)
            {
                empTable.Columns.Add(header.Trim(), header.Trim());
            }

            // Add data rows
            for (int i = 1; i < lines.Length; i++) // Start from 1 to skip the header
            {
                string[] fields = lines[i].Split('|');
                for (int j = 0; j < fields.Length; j++)
                {
                    fields[j] = fields[j].Trim(); // Trim whitespace
                }
                empTable.Rows.Add(fields);
            }
        }

        private void empTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            fetchEmpDetailsFromDatagrid(e);
        }

        private void fetchEmpDetailsFromDatagrid(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = empTable.Rows[e.RowIndex];

                if (row.Cells[0].Value != null)
                    txtEmpCode.Text = row.Cells[0].Value.ToString();
                        txtEmpCode.Enabled = false;
                if (row.Cells[1].Value != null)
                    txtEmpName.Text = row.Cells[1].Value.ToString();

                if (row.Cells[2].Value != null)
                    txtRole.Text = row.Cells[2].Value.ToString();
            }
        }
        private void UpdateEmployeeDetails()
        {
            string filePath = @"Records\Employees\Employees.csv";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Employee records not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string empCode = txtEmpCode.Text.Trim();
            string empName = txtEmpName.Text.Trim();
            string empRole = txtRole.Text.Trim();

            var lines = File.ReadAllLines(filePath).ToList();

            bool updated = false;

            // Keep header line and search for the record to update
            for (int i = 1; i < lines.Count; i++) // Start at 1 to skip header
            {
                var parts = lines[i].Split('|');

                if (parts.Length >= 3 && parts[0].Trim() == empCode)
                {
                    // Update the line
                    lines[i] = empCode + " | " + empName + " | " + empRole;
                    updated = true;
                    break;
                }
            }

            if (updated)
            {
                File.WriteAllLines(filePath, lines);
                MessageBox.Show("Employee updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Employee not found for update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadEmployeeData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!txtEmpCode.Text.Equals(""))
            {
                UpdateEmployeeDetails();
                refresh();
            }
            else
            {
                MessageBox.Show("Please enter employee code to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        private void deleteEmployeeDetails()
        {
            string filePath = @"Records\Employees\Employees.csv";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Employee file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string empCodeToDelete = txtEmpCode.Text.Trim();
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
                MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Employee not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            loadEmployeeData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!txtEmpCode.Text.Equals(""))
            {
                deleteEmployeeDetails();
                refresh();
            }
            else
            {
                MessageBox.Show("Please enter employee code to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }
        private void searchEmployeeById()
        {
            string filePath = @"Records\Employees\Employees.csv";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Employee file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtEmpCode.Enabled = false;
            string searchCode = txtEmpid.Text.Trim();
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
                        txtEmpName.Text = empName;
                        txtRole.Text = empRole;
                        txtEmpCode.Text = empCode;

                        
                        return;
                    }
                }
            }

            // Not found
            MessageBox.Show("Employee not found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtEmpName.Clear();
            txtRole.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!txtEmpid.Text.Equals(""))
            {
                searchEmployeeById();
            }
            else
            {
                MessageBox.Show("Please enter employee code to search", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void txtEmpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtEmpid.Text = "";
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtEmpid_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowDigitsOnly(e);
        }

        private void txtEmpName_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowLettersOnly(e);
        }

        private void txtRole_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateFields.allowLettersOnly(e);
        }



    }
}
