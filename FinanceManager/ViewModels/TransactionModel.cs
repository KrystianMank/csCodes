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
using System.Windows.Input;
using FinanceManager.Models;

namespace TransactionModel
{
    // ViewModel transakcji
    public class TransactionViewModel : INotifyPropertyChanged
    {

        // Aktualna transakcja
        private Transaction _currentTransaction = new Transaction();
        
        // Tablica transakcji
        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>();

        // Inicjacja właściwości 
        public string Title
        {
            get => _currentTransaction.Title;
            set
            {
                _currentTransaction.Title = value;
                OnPropertyChanged();
                
            }
        }
        public string Description
        {
            get => _currentTransaction.Description;
            set
            {
                _currentTransaction.Description = value;
                OnPropertyChanged();
            }
        }
        public decimal Amount
        {
            get => _currentTransaction.Amount;
            set
            {
                _currentTransaction.Amount = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get => _currentTransaction.Date;
            set
            {
                _currentTransaction.Date = value;
                OnPropertyChanged();
            }
        }

        public string Source
        {
            get => _currentTransaction.Source;
            set
            {
                _currentTransaction.Source = value;
                OnPropertyChanged();
            }
        }

        public bool IsIncome
        {
            get => _currentTransaction.IsIncome;
            set
            {
                _currentTransaction.IsIncome = value;
                OnPropertyChanged();
            }
        }

        // Utworzenie zdarzeń dodania transakcji i resetu formularza
        public ICommand AddTransactionCommand { get; }
        public ICommand ResetFormCommand { get; }

        public TransactionViewModel()
        {
            AddTransactionCommand = new RelayCommand(AddTransaction);
            ResetFormCommand = new RelayCommand(ResetForm);
            Date = DateTime.Now;
        }

        // Dodanie transakcji
        private void AddTransaction(object parameter)
        {
            if (Amount <= 0) return;
            Transactions.Add(_currentTransaction);
            ResetForm(null);
        }

        // Reset formularza
        private void ResetForm(object parameter)
        {
            _currentTransaction = new Transaction
            {
                Date = DateTime.Now
            };
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(IsIncome));
            OnPropertyChanged(nameof(Source));
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
