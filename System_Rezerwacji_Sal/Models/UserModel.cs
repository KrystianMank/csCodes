using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System_Rezerwacji_Sal.Data;

namespace System_Rezerwacji_Sal.Models
{
    public class UserModel : BaseModel
    {
        private static int _nextId = 2;
        private int _id;
        private string _name;
        private Data.Data.AccessType _type;
        private string _email;
        private string _password;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(_id));
                }
            }
        }

        public string Email
        {
            get => _email;
            private set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(_email));
                }
            }
        } 
        public string Password
        {
            get => _password;
            private set
            {
                if(_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(_password));
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

        public Data.Data.AccessType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(_type));
                }
            }
        }

        public UserModel()
        {
            Id = _nextId++;
            OnPropertyChanged(nameof(Id));
        }

        public UserModel(string name, Data.Data.AccessType type, string email, string hashedPassword)
        {
            Id = _nextId++;
            Name = name;
            Type = type;
            Email = email;
            Password = hashedPassword;
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
        }
    }
}
