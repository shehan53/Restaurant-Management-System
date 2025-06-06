using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RestaurantManagementSystem.Classes
{
    class ValidateFields
    {

        public static void numbersWithDecimals(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Allow digits, one dot, and control keys
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Only one decimal point allowed
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            }

            // Allow only 2 decimal places
            if (char.IsDigit(e.KeyChar) && txt.Text.Contains("."))
            {
                string[] parts = txt.Text.Split('.');
                if (parts.Length == 2 && parts[1].Length >= 2 && txt.SelectionStart > txt.Text.IndexOf("."))
                {
                    e.Handled = true;
                }
            }

        }

        public static void allowLettersOnly(KeyPressEventArgs e)
        {

            // Allow letters, space, and control keys
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        public static void allowDigitsOnly(KeyPressEventArgs e)
        {
            // Allow digits and control keys (e.g., backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // block the key
            }
        }

    }
}
