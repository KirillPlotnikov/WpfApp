using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Semestralka
{
    public class Meeting : INotifyPropertyChanged
    {
        private int expectedPersonsCount;
        private string customer;
        private bool videoPreparation;
        private string note;
        private TimeSpan timeFrom;
        private TimeSpan timeTo;

        public event PropertyChangedEventHandler PropertyChanged;

        public MeetingRoom MeetingRoom { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeFrom { get=> timeFrom;
            set
            {
                timeFrom = value;
                OnPropertyChanged("TimeFrom");
            }
        }
        public TimeSpan TimeTo { get => timeTo;
            set 
            {
                timeTo = value;
                OnPropertyChanged("TimeTo");
            }
        }
        public int ExpectedPersonsCount { get => expectedPersonsCount;
            set
            {
                if(value >= 1 && value <= MeetingRoom.Capacity)
                {
                    expectedPersonsCount = value;
                    OnPropertyChanged("ExpectedPersonsCount");
                }
            }
        }
        public string Customer { get => customer;
            set
            {
                if(value.Length >= 2 && value.Length <= 100)
                {
                    customer = value;
                    OnPropertyChanged("Customer");
                }
            }
        }
        public bool VideoPreparation { get => videoPreparation;
            set
            {
                if(value == true && MeetingRoom.VideoConference == true)
                {
                    videoPreparation = value;
                } else if(value == true && MeetingRoom.VideoConference == false)
                {
                    videoPreparation = false;
                }
                else
                {
                    videoPreparation = value;
                }
                OnPropertyChanged("VideoPreparation");
            }
        }
        public string Note { get => note;
            set
            {
                if(value.Length >= 0 && value.Length <= 300)
                {
                    note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        public Meeting(MeetingRoom meetingRoom, DateTime date, TimeSpan timefrom, TimeSpan timeto, int expectedpersonscount, string customer, bool videoPreparation, string note)
        {
            this.MeetingRoom = meetingRoom;
            this.Date = date;
            this.TimeFrom = timefrom;
            this.TimeTo = timeto;
            this.ExpectedPersonsCount = expectedpersonscount;
            this.Customer = customer;
            this.VideoPreparation = videoPreparation;
            this.Note = note;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            string hoursFrom = this.TimeFrom.Hours.ToString().Length == 1 ? $"0{this.TimeFrom.Hours.ToString()}" : this.TimeFrom.Hours.ToString();
            string hoursTo = this.TimeTo.Hours.ToString().Length == 1 ? $"0{this.TimeTo.Hours.ToString()}" : this.TimeTo.Hours.ToString();
            string minutesFrom = this.TimeFrom.Minutes.ToString().Length == 1 ? $"0{this.TimeFrom.Minutes.ToString()}" : this.TimeFrom.Minutes.ToString();
            string minutesTo = this.TimeTo.Minutes.ToString().Length == 1 ? $"0{this.TimeTo.Minutes.ToString()}" : this.TimeTo.Minutes.ToString();
            return $"od {hoursFrom}:{minutesFrom} - do {hoursTo}:{minutesTo}";
        }
    }
}
