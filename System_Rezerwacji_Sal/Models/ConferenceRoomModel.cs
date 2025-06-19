using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_Rezerwacji_Sal.Data;

namespace System_Rezerwacji_Sal.Models
{
    public class ConferenceRoomModel : BaseModel
    {
        private static int _nextId = 1;
        private int _id;
        private string _name = string.Empty;
        private int _capacity;
        private bool _isReserved;
        private List<string> _roomEquipment = Data.Data.RoomEquipment;

        public int Id
        {
            get => _id;
            private set
            {
                if(_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(_id));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(_name));
                }
            }
        }

        public int Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    OnPropertyChanged(nameof(_capacity));
                }
            }
        }

        public bool IsReserved
        {
            get => _isReserved;
            set
            {
                if (_isReserved != value)
                {
                    _isReserved = value;
                    OnPropertyChanged(nameof(_isReserved));
                }
            }
        }

        public List<string> RoomEquipment
        {
            get => _roomEquipment;
            set
            {
                if (_roomEquipment != value)
                {
                    _roomEquipment = value;
                    OnPropertyChanged(nameof(_roomEquipment));
                }
            }
        }

        public ConferenceRoomModel()
        {
            Id = _nextId++;
            IsReserved = false;
            _roomEquipment = new List<string>();
        }

        public ConferenceRoomModel(string name, int capacity, List<string> roomEquipment)
        {
            Id = _nextId++;
            IsReserved = false;
            Name = name;
            Capacity = capacity;
            RoomEquipment = roomEquipment;

            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Capacity));
            OnPropertyChanged(nameof(IsReserved));
            OnPropertyChanged(nameof(RoomEquipment));
        }
    }
}
