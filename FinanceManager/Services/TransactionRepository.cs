using FinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace FinanceManager.Services
{
    public class TransactionRepository
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=transactionsdatabase";
        // Metoda do pobierania wszystkich transakcji z bazy danych
        public IEnumerable<Transaction> GetAll()
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=transactionsdatabase"))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM transactions", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new Transaction
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            Date = reader.GetDateTime(3),
                            Description = reader.GetString(4),
                            IsIncome = reader.GetBoolean(5),
                            Source = reader.GetString(6)
                        };
                    }
                }
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=transactionsdatabase"))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO transactions (title, amount, date, description, is_income, source) VALUES (@title, @amount, @date, @description, @is_income, @source)", connection))
                {
                    command.Parameters.AddWithValue("title", transaction.Title);
                    command.Parameters.AddWithValue("amount", transaction.Amount);
                    command.Parameters.AddWithValue("date", transaction.Date);
                    command.Parameters.AddWithValue("description", transaction.Description);
                    command.Parameters.AddWithValue("is_income", transaction.IsIncome);
                    command.Parameters.AddWithValue("source", transaction.Source);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
