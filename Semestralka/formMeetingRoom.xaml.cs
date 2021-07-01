using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Semestralka
{
    /// <summary>
    /// Interakční logika pro formMeetingRoom.xaml
    /// </summary>
    public partial class formMeetingRoom : Window
    {
        public formMeetingRoom()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = new List<string>();

            if (nameTextBox.Text.Length < 2 || nameTextBox.Text.Length > 100)
            {
                errors.Add("Name must be longer than 2 and shorter than 100 characters!");
            }

            if (!ValidateCode(codeTextBox.Text))
            {
                errors.Add("Code must contains english letters or symbols \" - : _ ");
            }

            if (descriptionTextBox.Text.Length < 10 || descriptionTextBox.Text.Length > 300)
            {
                errors.Add("Desrtiption must be longer than 10 and shorter than 300 characters");
            }

            if(!Int32.TryParse(capacityTextBox.Text, out int capacity) || capacity < 1 || capacity > 100)
            {
                errors.Add("Capacity must be a number between 1 and 100");
            }

            if (errors.Count != 0)
            {
                string errorMessage = "";
                foreach (string error in errors)
                {
                    errorMessage += error + "\n";
                }
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }
        public bool ValidateCode(string value)
        {
            string validChars = "qwertyuiopasdfghjklzxcvbnm0123456789.-:_";
            if (value.Length >= 5 && value.Length <= 50)
            {
                bool valid = value.ToLower().All(c => validChars.Contains(c));
                return valid;
            }
            else
            {
                return false;
            }

        }
    }
}
