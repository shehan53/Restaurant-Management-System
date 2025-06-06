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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
            
        }

        private void login()
        {
            if (txtUsername.Text == "admin" && txtPassword.Text == "1111")
            {
                Home home = new Home();
                home.Show();
                this.Hide();

                
            }

            else if (txtUsername.Text == "staff" && txtPassword.Text == "0000")
            {
                Home home = new Home();
                home.Show();
                this.Hide();
                
            }
                
            else
            {
                MessageBox.Show("Wrong username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
  
        }
    }
}
