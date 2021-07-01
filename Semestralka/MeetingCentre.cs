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
    public class MeetingCentre : INotifyPropertyChanged
    {
        
        private string name;
        private string code;
        private string description;


        public ObservableCollection<MeetingRoom> meetingRooms;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { 
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
            set
            {
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
            set
            {
                if(value.Length >= 10 && value.Length <= 300)
                {
                    description = value;
                    OnPropertyChanged("Description");
                   
                }
               
            }
        }

        public string Error => throw new NotImplementedException();

        

        public bool ValidateCode(string value)
        {
            string validChars = "qwertyuiopasdfghjklzxcvbnm0123456789.-:_";
            if(value.Length >= 5 && value.Length <= 50)
            {
                bool valid = value.ToLower().All(c => validChars.Contains(c));
                return valid;
            }
            else
            {
                throw new Exception("Message");
            }
            
        }

        public MeetingCentre(string name, string code, string description)
        {
            this.Name = name;
            this.Code = code;
            this.Description = description;
            meetingRooms = new ObservableCollection<MeetingRoom>();
        }

        public void AddMeetingRoom(MeetingRoom m) => meetingRooms.Add(m);


        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return $"Name: {this.Name}    Code: {this.Code}";
        }

    }
}
