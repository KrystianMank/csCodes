using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinanceManager;
using FinanceManager.Models;
using FinanceManager.Services;

namespace FinanceManager.ViewModels
{
    // ViewModel transakcji
    public class TransactionViewModel : INotifyPropertyChanged
    {
        // Singleton dla TransactionViewModel
        public static TransactionViewModel Instance { get; } = new TransactionViewModel();

        // Repozytorium transakcji
        protected internal TransactionRepository _repository = new TransactionRepository();

        // Aktualna transakcja
        private Transaction _currentTransaction = new Transaction();

        // Tablica transakcji
        private ObservableCollection<Transaction> _transactions = new ObservableCollection<Transaction>();  
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                if (_transactions != value)
                {
                    _transactions = value;
                    OnPropertyChanged();
                }
            }
        }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (_selectedTransaction != value)
                {
                    _selectedTransaction = value;
                    OnPropertyChanged();
                }
            }
        }

        // Inicjacja właściwości 

        public string Title
        {
            get => _currentTransaction.Title;
            set
            {
                if (_currentTransaction.Title != value)
                {
                    _currentTransaction.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _currentTransaction.Description;
            set
            {
                if (_currentTransaction.Description != value)
                {
                    _currentTransaction.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Amount
        {
            get => _currentTransaction.Amount;
            set
            {
                if (_currentTransaction.Amount != value)
                {
                    _currentTransaction.Amount = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _currentTransaction.Date;
            set
            {
                if (_currentTransaction.Date != value)
                {
                    _currentTransaction.Date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Source
        {
            get => _currentTransaction.Source;
            set
            {
                if (_currentTransaction.Source != value)
                {
                    _currentTransaction.Source = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsIncome
        {
            get => _currentTransaction.IsIncome;
            set
            {
                if (_currentTransaction.IsIncome != value)
                {
                    _currentTransaction.IsIncome = value;
                    OnPropertyChanged();
                }
            }
        }

        // Utworzenie zdarzeń dodania transakcji i resetu formularza
        public ICommand AddTransactionCommand { get; }
        public ICommand ResetFormCommand { get; }


        public TransactionViewModel()
        {
            AddTransactionCommand = new RelayCommand(AddTransaction);
            ResetFormCommand = new RelayCommand(ResetForm);
            _currentTransaction = new Transaction
            {
                Date = DateTime.Now,
                //Source = "Karta",
            };
            foreach (var transaction in _repository.GetAll())
            {
                Transactions.Add(transaction);
            }

        }

        // Dodanie transakcji
        private void AddTransaction(object parameter)
        {
            if (Amount <= 0) return;
            if (string.IsNullOrWhiteSpace(Title)) return;

            var newTransaction = new Transaction
            {
                Title = this.Title,
                Amount = this.Amount,
                Date = this.Date,
                Description = this.Description,
                IsIncome = this.IsIncome,
                Source = this.Source
            };

            _repository.AddTransaction(newTransaction);
            
            Transactions.Add(newTransaction);

            ResetForm(null);
        }

        // Reset formularza
        private void ResetForm(object parameter)
        {
            _currentTransaction = new Transaction
            {
                Date = DateTime.Now,
            };

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(IsIncome));
            OnPropertyChanged(nameof(Source));
            OnPropertyChanged(nameof(Transactions));
        }

        // Implementacja metody klasy INotifyProperyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    // Klasa umożliwiająca powiązanie interfejsu użytkownika z logiką ViewModel
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        public readonly Predicate<object> _canExecute;
        
        // Konstruktor z możliwością wykonania (execute) i sprawdzenia warunku (canExecute)
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Implemetacja metod klasy ICommand
        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
