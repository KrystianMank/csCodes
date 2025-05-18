using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TransactionModel
{
        public class Tranaction
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public bool IsIncome { get; set; }
            public string Source { get; set; }
        }

        public class TranactionViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<Tranaction> _transactions;
            public ObservableCollection<Tranaction> Transactions
            {
                get => _transactions;
                set
                {
                    _transactions = value;
                    OnPropertyChanged();
                }
            }



            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
