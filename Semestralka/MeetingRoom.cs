using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Semestralka
{
    public class MeetingRoom : INotifyPropertyChanged
    {
        private string name;
        private string code;
        private string description;
        private int capacity;
        private bool videoconference;

        public ObservableCollection<Meeting> meetings;

        public event PropertyChangedEventHandler PropertyChanged;

        public MeetingCentre meetingCentre { get; private set; }

        public string Name
        {
            get { return name; }
            set
            {
                if(value.Length >= 2 && value.Length <= 100)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Code
        {
            get { return code; }
            set {
                if (ValidateCode(value))
                {
                    code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set {
                if (value.Length >= 10 && value.Length <= 300)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if(value >= 1 && value <= 100)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }

        public bool VideoConference { get { return videoconference; } 
            set
            {
                videoconference = value;
                OnPropertyChanged("VideoConference");
            }
        }

        public MeetingRoom(string name, string code, string description, int capacity, bool videoConference, MeetingCentre mc)
        {
            Name = name;
            Code = code;
            Description = description;
            Capacity = capacity;
            VideoConference = videoConference;
            meetingCentre = mc;
            meetings = new ObservableCollection<Meeting>();
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

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return $"Name: {this.Name}   Code: {this.Code}";
        }
    }
}
