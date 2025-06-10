using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantManagementSystem.GUI
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            if(!Login.hasPrivilege){
                btnFinanceSummary.Visible = false;
            }
        }

        private void btnSalesControl_Click(object sender, EventArgs e)
        {
            Invoice invoice = new Invoice();
            invoice.Show();
           
        }

        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            Product_Management pm = new Product_Management();
            pm.Show();
        }

        private void btnEmployeeManagement_Click(object sender, EventArgs e)
        {
            Employee_Management em = new Employee_Management();
            em.Show();
        }

        private void btnFinanceSummary_Click(object sender, EventArgs e)
        {
            Finance_Summary fs = new Finance_Summary();
            fs.Show();
        }
    }
}
