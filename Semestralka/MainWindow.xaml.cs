using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Semestralka
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<MeetingCentre> meetingCentres;
        ObservableCollection<MeetingRoom> meetingRooms;
        ObservableCollection<Meeting> meetings;
        bool changesApplied = false;
        public MainWindow()
        {
            meetingCentres = new ObservableCollection<MeetingCentre>();
            meetingRooms = new ObservableCollection<MeetingRoom>();
            meetings = new ObservableCollection<Meeting>();
            InitializeComponent();
            
            LoadDataCSV();
            LoadDataXML();
        }

        private void btnNewMeetingCentre_Click(object sender, RoutedEventArgs e)
        {
            formMeetingCentre mcForm = new formMeetingCentre();

            if (mcForm.ShowDialog() == true && mcForm.DialogResult == true)
            {
                string name = mcForm.nameTextBox.Text;
                string code = mcForm.codeTextBox.Text;
                string description = mcForm.descriptionTextBox.Text;
                if (name != "" && code != "" && description != "")
                {
                    MeetingCentre newMC = new MeetingCentre(name, code, description);
                    meetingCentres.Add(newMC);
                    changesApplied = true;
                    meetingCentresListBox.ItemsSource = meetingCentres;
                    meetingCentreComboBox.ItemsSource = meetingCentres;
                }
            }

        }

        private void btnEditMeetingCentre_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                MeetingCentre selectedMC = (MeetingCentre)meetingCentresListBox.SelectedItem;
                formMeetingCentre mcForm = new formMeetingCentre();
                mcForm.nameTextBox.Text = selectedMC.Name;
                mcForm.codeTextBox.Text = selectedMC.Code;
                mcForm.descriptionTextBox.Text = selectedMC.Description;

                if (mcForm.ShowDialog() == true && mcForm.DialogResult == true)
                {
                    selectedMC.Name = mcForm.nameTextBox.Text;
                    selectedMC.Code = mcForm.codeTextBox.Text;
                    selectedMC.Description = mcForm.descriptionTextBox.Text;
                    changesApplied = true;
                }
            }

        }

        private void btnDeleteMeetingCentre_Click(object sender, RoutedEventArgs e)
        {
            if (meetingCentresListBox.SelectedItem != null)
            {
                MeetingCentre selectedMC = (MeetingCentre)meetingCentresListBox.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete this Meeting Centre?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    selectedMC.meetingRooms = null;
                    meetingCentres.Remove(selectedMC);
                    changesApplied = true;
                }
            }
            else
            {
                MessageBox.Show("You have to choose Meeting Centre first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void meetingCentresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MeetingCentre mc = (MeetingCentre)meetingCentresListBox.SelectedItem;
            if (mc != null)
            {
                meetingCentreName.Text = mc.Name;
                meetingCentreCode.Text = mc.Code;
                meetingCentreDescription.Text = mc.Description;
                meetingRoomsListBox.ItemsSource = mc.meetingRooms;
            }
            else
            {
                meetingCentreName.Text = "";
                meetingCentreCode.Text = "";
                meetingCentreDescription.Text = "";
                meetingRoomsListBox.ItemsSource = null;
            }
        }

        private void newMeetingRoom_Click(object sender, RoutedEventArgs e)
        {
            MeetingCentre mc = (MeetingCentre)meetingCentresListBox.SelectedItem;

            if (mc != null)
            {
                formMeetingRoom formMR = new formMeetingRoom();
                if (formMR.ShowDialog() == true && formMR.DialogResult == true)
                {
                    string name = formMR.nameTextBox.Text;
                    string code = formMR.codeTextBox.Text;
                    string description = formMR.descriptionTextBox.Text;
                    bool convertResult = Int32.TryParse(formMR.capacityTextBox.Text, out int capacity);
                    bool videoConference = (bool)formMR.videoConferenceCheckBox.IsChecked;

                    if (name != "" && code != "" && description != "" && convertResult)
                    {
                        MeetingRoom mroom = new MeetingRoom(name, code, description, capacity, videoConference, mc);
                        mc.meetingRooms.Add(mroom);
                        meetingRooms.Add(mroom);
                        changesApplied = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("You have to choose Meeting Centre", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void meetingRoomsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MeetingRoom mr = (MeetingRoom)meetingRoomsListBox.SelectedItem;

            if (mr != null)
            {
                meetingRoomName.Text = mr.Name;
                meetingRoomCode.Text = mr.Code;
                meetingRoomDescription.Text = mr.Description;
                meetingRoomCapacity.Text = mr.Capacity.ToString();
                videConferenceCheckBox.IsChecked = mr.VideoConference;
            }
            else
            {
                meetingRoomName.Text = "";
                meetingRoomCode.Text = "";
                meetingRoomDescription.Text = "";
                meetingRoomCapacity.Text = "";
                videConferenceCheckBox.IsChecked = false;
            }
        }

        private void editMeetingRoom_Click(object sender, RoutedEventArgs e)
        {
            if (meetingRoomsListBox.SelectedItem != null)
            {
                MeetingRoom selectedMR = (MeetingRoom)meetingRoomsListBox.SelectedItem;
                formMeetingRoom mrForm = new formMeetingRoom();
                mrForm.nameTextBox.Text = selectedMR.Name;
                mrForm.codeTextBox.Text = selectedMR.Code;
                mrForm.descriptionTextBox.Text = selectedMR.Description;
                mrForm.capacityTextBox.Text = selectedMR.Capacity.ToString();
                mrForm.videoConferenceCheckBox.IsChecked = selectedMR.VideoConference;

                if (mrForm.ShowDialog() == true && mrForm.DialogResult == true)
                {
                    selectedMR.Name = mrForm.nameTextBox.Text;
                    selectedMR.Code = mrForm.codeTextBox.Text;
                    selectedMR.Description = mrForm.descriptionTextBox.Text;

                    if (Int32.TryParse(mrForm.capacityTextBox.Text, out int capacity))
                    {
                        selectedMR.Capacity = capacity;
                    }
                    selectedMR.VideoConference = (bool)mrForm.videoConferenceCheckBox.IsChecked;

                    meetingRoomName.Text = selectedMR.Name;
                    meetingRoomCode.Text = selectedMR.Code;
                    meetingRoomDescription.Text = selectedMR.Description;
                    meetingRoomCapacity.Text = selectedMR.Capacity.ToString();
                    videConferenceCheckBox.IsChecked = selectedMR.VideoConference;
                    changesApplied = true;
                }
            }
        }

        private void deleteMeetingRoom_Click(object sender, RoutedEventArgs e)
        {
            if (meetingRoomsListBox.SelectedItem != null)
            {
                MeetingCentre selectedMC = (MeetingCentre)meetingCentresListBox.SelectedItem;
                MeetingRoom selectedMR = (MeetingRoom)meetingRoomsListBox.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete this Meeting Room?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    selectedMC.meetingRooms.Remove(selectedMR);
                    changesApplied = true;
                }
            }
            else
            {
                MessageBox.Show("You have to choose Meeting Room first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void menuImportItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                meetingCentres.Clear();
                StreamReader sr = new StreamReader(openFile.FileName, System.Text.Encoding.UTF8);
                LoadDataCSV(sr);
                
            }
        }

        private void menuSaveItem_Click(object sender, RoutedEventArgs e)
        {
            SaveDataCSV();
            SaveDataXML();
        }

        private void menuExitItem_Click(object sender, RoutedEventArgs e)
        {
            if (changesApplied)
            {
                bool result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
                if (result)
                {
                    SaveDataCSV();
                    SaveDataXML();
                    changesApplied = false;
                }
            }
            
            Application.Current.Shutdown();
            

            
        }
        private void SaveDataCSV()
        {
            string writePath = Directory.GetCurrentDirectory() + "\\Export.csv";
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("MEETING_CENTRES,,,,,");

                foreach (MeetingCentre mc in meetingCentres)
                {
                    sw.WriteLine($"{mc.Name},{mc.Code},{mc.Description},,,");
                }

                sw.WriteLine("MEETING_ROOMS,,,,,");

                foreach (MeetingCentre mc in meetingCentres)
                {
                    foreach (MeetingRoom mr in mc.meetingRooms)
                    {
                        string vc = mr.VideoConference == true ? "YES" : "NO";
                        sw.WriteLine($"{mr.Name},{mr.Code},{mr.Description},{mr.Capacity},{vc},{mc.Code}");
                    }
                }
            }
        }
        private void LoadDataCSV()
        {
            string file = Directory.GetCurrentDirectory() + "\\Export.csv";
            StreamReader sr = new StreamReader(file, System.Text.Encoding.UTF8);
            string line = "";
            string entity = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();

                string[] s = line.Split(',');

                if (s[0] == "MEETING_CENTRES")
                {
                    entity = "MEETING_CENTRES";
                    continue;
                }
                else if (s[0] == "MEETING_ROOMS")
                {
                    entity = "MEETING_ROOMS";
                    continue;
                }

                if (entity == "MEETING_CENTRES")
                {
                    meetingCentres.Add(new MeetingCentre(s[0], s[1], s[2]));
                }
                else
                {
                    MeetingCentre mc = meetingCentres.Where(i => i.Code == s[5]).FirstOrDefault();
                    bool videoConference = s[4] == "YES" ? true : false;
                    MeetingRoom mr = new MeetingRoom(s[0], s[1], s[2], Int32.Parse(s[3]), videoConference, mc);
                    mc.meetingRooms.Add(mr);
                    meetingRooms.Add(mr);
                }
            }
            meetingCentresListBox.ItemsSource = meetingCentres;
            meetingCentreComboBox.ItemsSource = meetingCentres;
        }

        private void LoadDataCSV(StreamReader sr)
        {
       
          
            string line = "";
            string entity = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();

                string[] s = line.Split(',');

                if (s[0] == "MEETING_CENTRES")
                {
                    entity = "MEETING_CENTRES";
                    continue;
                }
                else if (s[0] == "MEETING_ROOMS")
                {
                    entity = "MEETING_ROOMS";
                    continue;
                }

                if (entity == "MEETING_CENTRES")
                {
                    meetingCentres.Add(new MeetingCentre(s[0], s[1], s[2]));
                }
                else
                {
                    MeetingCentre mc = meetingCentres.Where(i => i.Code == s[5]).FirstOrDefault();
                    bool videoConference = s[4] == "YES" ? true : false;
                    MeetingRoom mr = new MeetingRoom(s[0], s[1], s[2], Int32.Parse(s[3]), videoConference, mc);
                    mc.meetingRooms.Add(mr);
                    meetingRooms.Add(mr);
                }
            }
            meetingCentresListBox.ItemsSource = meetingCentres;
            meetingCentreComboBox.ItemsSource = meetingCentres;
        }
        private void SaveDataXML()
        {
            XDocument xdoc = new XDocument();
            XElement meetingsElement = new XElement("meetings");
            foreach(Meeting m in meetings)
            {
                XElement meeting = new XElement("meeting");
               
                meeting.Add(new XElement("date", m.Date.ToString("d.M.yyy")));
                meeting.Add(new XElement("timeFrom", $"{m.TimeFrom.Hours}:{m.TimeFrom.Minutes}"));
                meeting.Add(new XElement("timeTo", $"{m.TimeTo.Hours}:{m.TimeTo.Minutes}"));
                meeting.Add(new XElement("expectedPersonsCount", m.ExpectedPersonsCount));
                meeting.Add(new XElement("customer", m.Customer));
                meeting.Add(new XElement("videoPreparation", m.VideoPreparation));
                meeting.Add(new XElement("note", m.Note));
                meeting.Add(new XElement("meetingRoom", m.MeetingRoom.Code));

                meetingsElement.Add(meeting);
            }
            xdoc.Add(meetingsElement);
            xdoc.Save("meetings.xml");
        }
        private void LoadDataXML()
        {
            XDocument xdoc = XDocument.Load("meetings.xml");
            foreach(XElement meetingElement in xdoc.Element("meetings").Elements("meeting"))
            {
                string[] dateString = meetingElement.Element("date").Value.Split('.');
                string[] timeToString = meetingElement.Element("timeTo").Value.Split(':');
                string[] timeFromString = meetingElement.Element("timeFrom").Value.Split(':');
                DateTime date = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[1]), int.Parse(dateString[0]));
                TimeSpan timeTo = new TimeSpan(int.Parse(timeToString[0]), int.Parse(timeToString[1]), 0);
                TimeSpan timeFrom = new TimeSpan(int.Parse(timeFromString[0]), int.Parse(timeFromString[1]), 0);
                int personCount = int.Parse(meetingElement.Element("expectedPersonsCount").Value);
                string customer = meetingElement.Element("customer").Value;
                bool videoPrep = bool.Parse(meetingElement.Element("videoPreparation").Value);
                string note = meetingElement.Element("note").Value;

                MeetingRoom mroom = null;
                foreach(MeetingRoom mr in meetingRooms)
                {
                    if (mr.Code == meetingElement.Element("meetingRoom").Value)
                    {
                        mroom = mr;
                    }
                }

                Meeting m = new Meeting(mroom, date, timeFrom, timeTo, personCount, customer, videoPrep, note);
                mroom.meetings.Add(m);
                meetings.Add(m);
            }
        }

        private void ExportJSON(string path)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
           using(StreamWriter sw = File.CreateText(path))
            {
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {


                    foreach (Meeting m in meetings)
                    {
                        JObject something = JObject.FromObject(new
                        {
                            meetingRoom = new
                            {
                                meetingCentre = m.MeetingRoom.meetingCentre.Code,
                                meetingRoom = m.MeetingRoom.Code,
                                reservations = from meeting in m.MeetingRoom.meetings
                                               orderby meeting.Date
                                               select new { date = meeting.Date, timeFrom = meeting.TimeFrom, timeTo = meeting.TimeTo, expectedPersonsCount = meeting.ExpectedPersonsCount, customer = meeting.Customer, videoConference = meeting.VideoPreparation, note = meeting.Note }
                            }
                        });

                        something.WriteTo(writer);
                        
                    }
                }
           




            }
        }
        private void meetingCentreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MeetingCentre mc = (MeetingCentre)meetingCentreComboBox.SelectedItem;

            meetingRoomComboBox.ItemsSource = mc.meetingRooms;
        }

        private void newMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            MeetingRoom mr = (MeetingRoom)meetingRoomComboBox.SelectedItem;

            if (mr != null)
            {

                formMeetings meetingForm = new formMeetings(mr);

                if (meetingForm.ShowDialog() == true && meetingForm.DialogResult == true)
                {
                    TimeSpan timeTo = new TimeSpan(int.Parse(meetingForm.timeToHoursTextBox.Text), int.Parse(meetingForm.timeToMinutesTextBox.Text), 0);
                    TimeSpan timeFrom = new TimeSpan(int.Parse(meetingForm.timeFromHoursTextBox.Text), int.Parse(meetingForm.timeFromMinutesTextBox.Text), 0);
                    string customer = meetingForm.customerTextBox.Text;
                    string note = meetingForm.noteTextBox.Text;
                    int personsCount = int.Parse(meetingForm.expectedPersonsCountTextBox.Text);
                    bool videoPreparation = (bool)meetingForm.videoPreparationCheckBox.IsChecked;
                    DateTime wantedDate = (DateTime)datePicker.SelectedDate;
                    Meeting m = new Meeting(mr, wantedDate, timeFrom, timeTo, personsCount, customer, videoPreparation, note);
                    mr.meetings.Add(m);
                    meetings.Add(m);

                    var meetingsToShow = from meeting in mr.meetings
                                         where meeting.Date == wantedDate
                                         orderby meeting.TimeFrom
                                         select meeting;

                    meetingsListBox.ItemsSource = meetingsToShow;
                    changesApplied = true;
                }
            }
            else
            {
                MessageBox.Show("You have to choose Meeting Centre, Meeting room and Date first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            MeetingRoom mr = (MeetingRoom)meetingRoomComboBox.SelectedItem;
            DateTime wantedDate = (DateTime)datePicker.SelectedDate;

            var meetingsToShow = from m in mr.meetings
                                 where m.Date == wantedDate
                                 orderby m.TimeFrom
                                 select m;

            meetingsListBox.ItemsSource = meetingsToShow;
        }

        private void meetingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Meeting m = (Meeting)meetingsListBox.SelectedItem;

            
            fromHoursTextBox.Text = m?.TimeFrom.Hours.ToString().Length == 1? $"0{m?.TimeFrom.Hours.ToString()}" : m?.TimeFrom.Hours.ToString();
            fromMinutesTextBox.Text = m?.TimeFrom.Minutes.ToString().Length == 1 ? $"0{m?.TimeFrom.Minutes.ToString()}" : m?.TimeFrom.Minutes.ToString();
            toHoursTextBox.Text = m?.TimeTo.Hours.ToString().Length == 1 ? $"0{m?.TimeTo.Hours.ToString()}" : m?.TimeTo.Hours.ToString();
            toMinutesTextBox.Text = m?.TimeTo.Minutes.ToString().Length == 1 ? $"0{m?.TimeTo.Minutes.ToString()}" : m?.TimeTo.Minutes.ToString();
            expectedPersonsCountTextBox.Text = m?.ExpectedPersonsCount.ToString();
            customerTextBox.Text = m?.Customer;
            videConferenceCheckBox.IsChecked = m?.VideoPreparation;
            noteTextBox.Text = m?.Note;
             
        }

        private void meetingRoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MeetingRoom mr = (MeetingRoom)meetingRoomComboBox.SelectedItem;

            if (datePicker.SelectedDate != null)
            {
                DateTime wantedDate = (DateTime)datePicker.SelectedDate;

                var meetingsToShow = from m in mr.meetings
                                     where m.Date == wantedDate
                                     orderby m.TimeFrom
                                     select m;

                meetingsListBox.ItemsSource = meetingsToShow;
            }
        }

        private void editMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            Meeting m = (Meeting)meetingsListBox.SelectedItem;
            MeetingRoom mr = (MeetingRoom)meetingRoomComboBox.SelectedItem;

            if (m != null)
            {

                formMeetings meetingForm = new formMeetings(mr);
                meetingForm.timeFromHoursTextBox.Text = m.TimeFrom.Hours.ToString();
                meetingForm.timeFromMinutesTextBox.Text = m.TimeFrom.Minutes.ToString();
                meetingForm.timeToHoursTextBox.Text = m.TimeTo.Hours.ToString();
                meetingForm.timeToMinutesTextBox.Text = m.TimeTo.Minutes.ToString();
                meetingForm.expectedPersonsCountTextBox.Text = m.ExpectedPersonsCount.ToString();
                meetingForm.customerTextBox.Text = m.Customer;
                meetingForm.noteTextBox.Text = m.Note;
                meetingForm.videoPreparationCheckBox.IsChecked = m.VideoPreparation;

                if (meetingForm.ShowDialog() == true && meetingForm.DialogResult == true)
                {
                    
                    TimeSpan timeTo = new TimeSpan(int.Parse(meetingForm.timeToHoursTextBox.Text), int.Parse(meetingForm.timeToMinutesTextBox.Text), 0);
                    TimeSpan timeFrom = new TimeSpan(int.Parse(meetingForm.timeFromHoursTextBox.Text), int.Parse(meetingForm.timeFromMinutesTextBox.Text), 0);
                    string customer = meetingForm.customerTextBox.Text;
                    string note = meetingForm.noteTextBox.Text;
                    int personsCount = int.Parse(meetingForm.expectedPersonsCountTextBox.Text);
                    bool videoPreparation = (bool)meetingForm.videoPreparationCheckBox.IsChecked;
                    DateTime wantedDate = (DateTime)datePicker.SelectedDate;

                    m.TimeFrom = timeFrom;
                    m.TimeTo = timeTo;
                    m.Customer = customer;
                    m.Note = note;
                    m.ExpectedPersonsCount = personsCount;
                    m.VideoPreparation = videoPreparation;

                    

                    var meetingsToShow = from meeting in mr.meetings
                                         where meeting.Date == wantedDate
                                         orderby meeting.TimeFrom
                                         select meeting;

                    meetingsListBox.ItemsSource = meetingsToShow;

                    fromHoursTextBox.Text = m.TimeFrom.Hours.ToString();
                    fromMinutesTextBox.Text = m.TimeFrom.Hours.ToString();
                    toHoursTextBox.Text = m.TimeTo.Hours.ToString();
                    toMinutesTextBox.Text = m.TimeTo.Minutes.ToString();
                    expectedPersonsCountTextBox.Text = m.ExpectedPersonsCount.ToString();
                    customerTextBox.Text = m.Customer;
                    videConferenceCheckBox.IsChecked = m.VideoPreparation;
                    noteTextBox.Text = m.Note;
                    changesApplied = true;
                }
            }
            else
            {
                MessageBox.Show("You have to choose Meeting first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void deleteMeetingButton_Click(object sender, RoutedEventArgs e)
        {
            if (meetingsListBox.SelectedItem != null)
            {
                MeetingRoom selectedMR = (MeetingRoom)meetingRoomComboBox.SelectedItem;
                Meeting m = (Meeting)meetingsListBox.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure that you want to delete this Meeting?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                DateTime wantedDate = (DateTime)datePicker.SelectedDate;

                if (result == MessageBoxResult.Yes)
                {
                    selectedMR.meetings.Remove(m);
                    var meetingsToShow = from meeting in selectedMR.meetings
                                         where meeting.Date == wantedDate
                                         orderby meeting.TimeFrom
                                         select meeting;

                    meetingsListBox.ItemsSource = meetingsToShow;
                    changesApplied = true;
                }
                
            }
            else
            {
                MessageBox.Show("You have to choose Meeting first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changesApplied)
            {
                bool result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
                if (result)
                {
                    SaveDataCSV();
                    SaveDataXML();
                }
            }
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
            {
                ExportJSON(saveFileDialog.FileName);
            }
        }
    }
}
