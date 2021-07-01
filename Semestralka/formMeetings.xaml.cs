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
    /// Interakční logika pro formMeetings.xaml
    /// </summary>
    public partial class formMeetings : Window
    {
        public MeetingRoom MeetingRoom { get; set; }
        public formMeetings()
        {
            InitializeComponent();
        }
        public formMeetings(MeetingRoom mr) : this()
        {
            MeetingRoom = mr;
        }
        

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = new List<string>();

            int parsedHoursFrom = int.Parse(timeFromHoursTextBox.Text);
            int parsedHoursTo = int.Parse(timeToHoursTextBox.Text);
            int parsedMinutesFrom = int.Parse(timeFromMinutesTextBox.Text);
            int parsedMinutesTo = int.Parse(timeToMinutesTextBox.Text);
            int expectedPersons = int.Parse(expectedPersonsCountTextBox.Text);
            TimeSpan timeTo = new TimeSpan(int.Parse(timeToHoursTextBox.Text), int.Parse(timeToMinutesTextBox.Text), 0);
            TimeSpan timeFrom = new TimeSpan(int.Parse(timeFromHoursTextBox.Text), int.Parse(timeFromMinutesTextBox.Text), 0);

            if (parsedHoursFrom > 23 || parsedHoursFrom < 0 || parsedHoursTo > 23 || parsedHoursTo < 0 || parsedMinutesFrom > 59 || parsedMinutesFrom < 0 || parsedMinutesTo < 0 || parsedMinutesTo > 59)
            {
                errors.Add("Please input a valid time");
            }

            if(parsedHoursFrom > parsedHoursTo)
            {
                errors.Add("Time from must be before time to");
            }

            if(parsedHoursTo == parsedHoursFrom && parsedHoursFrom > parsedMinutesTo)
            {
                errors.Add("Time from must be before time to");
            }

            if(expectedPersons < 1 || expectedPersons > MeetingRoom.Capacity)
            {
                errors.Add("There must be atleast 1 person but not over the Meeting Room capacity");
            }

            if(customerTextBox.Text.Length < 2 || customerTextBox.Text.Length > 100)
            {
                errors.Add("Customer name must be between 2 and 100 characters long");
            }

            if(noteTextBox.Text.Length > 300)
            {
                errors.Add("Note must be maximum 300 characters long");
            }

            bool isOKay = MeetingRoom.meetings.All(m => m.TimeTo < timeFrom || m.TimeFrom > timeTo);

            if (!isOKay)
            {
                errors.Add("There is a meeting for this time already");
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
    }
}
