using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Rezerwacji_Sal.Models
{
    public class ReservationModel : BaseModel
    {
        private int _id;
        private DateTime _beginDate;
        private DateTime _endDate;
        private int _conferenceRoomId;
        private int _userId;
        private string? _purpose;
        private int _participants;

        public ReservationModel()
        {
            _purpose = string.Empty;
            OnPropertyChanged(nameof(Purpose));
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public DateTime BeginDate
        {
            get => _beginDate;
            set
            {
                if (value != _beginDate)
                {
                    _beginDate = value;
                    OnPropertyChanged(nameof(_beginDate));
                }
            }
        }
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(_endDate));
                }
            }
        }

        public int ConferenceRoomId
        {
            get => _conferenceRoomId;
            set
            {
                if (_conferenceRoomId != value)
                {
                    _conferenceRoomId = value;
                    OnPropertyChanged(nameof(_conferenceRoomId));
                }
            }
        }

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged(nameof(_userId));
                }
            }
        }

        public string Purpose
        {
            get => _purpose ?? string.Empty;
            set
            {
                if (value != _purpose)
                {
                    _purpose = value;
                    OnPropertyChanged(nameof(_purpose));
                }
            }
        }

        public int Participants
        {
            get => _participants;
            set
            {
                if(value != _participants)
                {
                    _participants = value;
                    OnPropertyChanged(nameof(_participants));
                }
            }
        }

        public ReservationModel(DateTime beginDate, DateTime endDate, int userId, int conferenceRoomId, string? purpose, int participants)
        {
            BeginDate = beginDate;
            EndDate = endDate;
            UserId = userId;
            ConferenceRoomId = conferenceRoomId;
            Purpose = purpose ?? string.Empty;
            Participants = participants;

            OnPropertyChanged(nameof(BeginDate));
            OnPropertyChanged(nameof(EndDate));
            OnPropertyChanged(nameof(ConferenceRoomId));
            OnPropertyChanged(nameof(UserId));
            OnPropertyChanged(nameof(Purpose));
            OnPropertyChanged(nameof(Participants));
        }
    }
}
