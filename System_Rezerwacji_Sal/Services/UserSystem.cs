using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System_Rezerwacji_Sal.Data;
using System_Rezerwacji_Sal.Models;

namespace System_Rezerwacji_Sal.Services
{
    public static class UserSystem
    {
        private static readonly DbMapper _db;
        static UserSystem()
        {
            _db = new DbMapper();
        }
        public static void ShowAllUsers()
        {
            var users = GetUsers();
            if (!users.Any())
            {
                Console.WriteLine("Debug: Brak użytkowników w bazie!");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Email: {user.Email}, Name: {user.Name}, " +
                    $"Type: {user.Type}, PasswordHash: {user.Password}");
            }
        }

        public static void AddUser(string name, Data.Data.AccessType accessType, string email, string password)
        {
            try
            {
                if (!IsEmailValid(email) || !IsPasswordValid(password))
                {
                    return;
                }
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Nie podano nazwy użytkownika");
                    return;
                }
                if(name.Length < 3)
                {
                    Console.WriteLine("Nazwa użytkownika musi mieć minimum 3 znaki");
                    return;
                }

                string hashedPassword = HashPassword(password);
                var user = new UserModel(name, accessType, email, hashedPassword);
                _db.Users.Add(user);
                _db.SaveChanges();
                Console.WriteLine("Zarejestrowano pomyślnie");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas dodawania użytkownika: {ex.Message}");
            }
        }

        public static bool IsEmailValid(string email)
        {
            try
            {
                if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    Console.WriteLine("Nieprawidłowy email");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email nie może być pusty!");
                    return false;
                }
                return true;
            }
            catch(RegexParseException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            return false;
        }

        private static bool IsPasswordValid(string password)
        {
            try
            {
                if (!Regex.IsMatch(password, @"^[A-Za-z0-9._-]{5,}$"))
                {
                    Console.WriteLine("Hasło musi składać się z liter, cyfr lub znaków specjalnych: . _ -");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Hasło nie może być puste!");
                    return false;
                }
                return true;
            }
            catch (RegexParseException ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }
            return false;
        }

        

        public static void DeleteUser(int userId)
        {
            var user = _db.Users.Find(userId);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Nie ma takiego użytkownika");
                return;
            }
        }

        //public static void DebugLoginAttempt(string email, string password)
        //{
        //    try
        //    {
        //        var hashedPassword = HashPassword(password);
        //        var user = _db.Users.FirstOrDefault(u => u.Email == email);

        //        if (user == null)
        //        {
        //            Console.WriteLine("Debug: Użytkownik nie istnieje w bazie");
        //            return;
        //        }

        //        Console.WriteLine($"Debug: Znaleziono użytkownika: {user.Email}");
        //        Console.WriteLine($"Debug: Hash wprowadzonego hasła: {hashedPassword}");
        //        Console.WriteLine($"Debug: Hash hasła w bazie: {user.Password}");
        //        Console.WriteLine($"Debug: Hasła {(hashedPassword == user.Password ? "się zgadzają" : "się NIE zgadzają")}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Debug: Błąd podczas debugowania: {ex.Message}");
        //    }
        //}

        public static UserModel Login(string email, string password)
        {
            try
            {
                string hashedInputPassword = HashPassword(password);
              //  Console.WriteLine($"DEBUG: Generated hash: {hashedInputPassword}");
                var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedInputPassword);
                if (user != null)
                {
                    Console.WriteLine($"Zalogowano pomyślnie jako {user.Name} ({user.Type})");
                    return user;
                }
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Błąd podczas logowania: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
                return null;
            }
        }

        public static List<UserModel> GetUsers()
        {
            return _db.Users.ToList();
        }

        public static void Logout(ref UserModel user)
        {
            user = null;
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
