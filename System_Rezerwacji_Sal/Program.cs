using System_Rezerwacji_Sal.Models;
using System_Rezerwacji_Sal.Data;
using System_Rezerwacji_Sal.Services;
using Microsoft.EntityFrameworkCore;
/*
    •   Dodanie metod do zarządzania rezerwacjami (usuwanie, modyfikacja) -> done
    •	Implementację prostego systemu wyszukiwania wolnych terminów -> done
    •	Dodanie logiki uprawnień bazującej na AccessType -> done
    •   Dodanie systemu zapisu (baza danych lub json) -> done
    Odpowiednia implementacja modyfikacji 
 */
class Program
{  
    public static async Task Main(string[] args)
    {
        using var db = new DbMapper();
        db.Database.EnsureCreated();
        
        bool appliactionRunning = true;
        while (appliactionRunning)
        { 
            #region LoginScreen
            bool isLoginScreenRunning = true;
            UserModel user = null;
            while (isLoginScreenRunning)
            {
                Console.WriteLine("====== Witaj w systemie rezerwacji sal ========");
                Console.WriteLine("1. Zaloguj się");
                Console.WriteLine("2. Zarejestruj się");
                Console.WriteLine("0. Wyjście");
                Console.Write("\nWybierz opcję: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Podaj email: ");
                            string email = Console.ReadLine() ?? string.Empty;
                            Console.Write("Podaj hasło: ");
                            string password = Console.ReadLine() ?? string.Empty;
                            var loggedUser = UserSystem.Login(email, password);
                            if (loggedUser != null)
                            {
                                user = loggedUser;
                                isLoginScreenRunning = false;
                            }
                            else Console.WriteLine("Brak użytkownika w bazie");

                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case 2:
                            Console.WriteLine("===Rejestracja użytkownika===");
                            Console.Write("Podaj email: ");
                            string registerEmail = Console.ReadLine() ?? string.Empty;
                            Console.Write("Podaj haslo: ");
                            string registerPassword = Console.ReadLine() ?? string.Empty;
                            Console.Write("Nazwa użytkownika: ");
                            string registerUserName = Console.ReadLine() ?? string.Empty;
                            Data.AccessType accessType = Data.AccessType.Klient_zewnetrzny;

                            UserSystem.AddUser(registerUserName, accessType, registerEmail, registerPassword);
                            user = UserSystem.Login(registerEmail, registerPassword);
                            isLoginScreenRunning = false;
                            break;
                        default:
                            Console.WriteLine("Dziękuje za korzystanie z programu");
                            return;

                    }
                }
            }
            #endregion
            #region Reservation system
            ReservationSystem reservationSystem = new ReservationSystem(db);
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine($"Witaj {user.Name}");
                var reservationsForUser = db.Reservations.Where(r => r.UserId == user.Id).ToArray();
                NotificationService.SendNotificationForUser(reservationsForUser, user);

                switch (user.Type)
                {
                    #region Admin
                    case Data.AccessType.Administrator:
                        Console.WriteLine("1. Dodaj rezerwację");
                        Console.WriteLine("2. Pokaż wszystkie rezerwacje");
                        Console.WriteLine("3. Pokaż szczegóły rezerwacji");
                        Console.WriteLine("4. Modyfikuj rezerwację");
                        Console.WriteLine("5. Usuń rezerwację");
                        Console.WriteLine("6. Pokaż szczegóły sali");
                        Console.WriteLine("7. Pokaż wszystkich użytkowników");
                        Console.WriteLine("8. Usuń użytkownika");
                        Console.WriteLine("9. Dodaj użytkownika");
                        Console.WriteLine("10. Wyszukaj wolne terminy");
                        Console.WriteLine("0. Wyloguj się");
                        Console.Write("\nWybierz opcję: ");

                        if (int.TryParse(Console.ReadLine(), out int choice))
                        {
                            Console.Clear();
                            switch (choice)
                            {
                                case 1:
                                    Console.WriteLine("=== Dodawanie rezerwacji ===");

                                    Console.Write("ID sali (1 lub 2): ");
                                    int roomId = int.Parse(Console.ReadLine() ?? "0");
                                    if (!await reservationSystem.RoomExists(roomId))
                                    {
                                        Console.WriteLine("Brak pokoju o podanym numerze");
                                    }

                                    Console.WriteLine("Podaj datę (dd.MM.yyyy): ");
                                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime addDate))
                                    {
                                        Console.WriteLine("Nieprawidłowy format daty");
                                        break;
                                    }

                                    Console.Write("Godzina rozpoczęcia: ");
                                    double beginHour = double.Parse(Console.ReadLine() ?? "8");

                                    if(beginHour < 8 || beginHour > 19)
                                    {
                                        Console.WriteLine("Godzina rozpoczęcia nie może byc wcześniejsza od 8 rano lub późniejsza od 20");
                                        break;
                                    }

                                    addDate = addDate.AddHours(beginHour);

                                    Console.Write("Czas trwania: ");
                                    double hours = double.Parse(Console.ReadLine() ?? "1");

                                    Console.Write("Cel rezerwacji: ");
                                    string purpose = Console.ReadLine() ?? string.Empty;

                                    Console.Write("Ilość uczestników spotkania: ");
                                    int participants = int.Parse(Console.ReadLine() ?? "0");

                                    await reservationSystem.AddReservationAsync(
                                        addDate,
                                        addDate.AddHours(hours),
                                        user.Id,
                                        roomId,
                                        purpose,
                                        participants);
                                    break;

                                case 2:
                                    Console.WriteLine("=== Lista wszystkich rezerwacji ===");
                                    await reservationSystem.ShowAllReservationsAsync();
                                    break;

                                case 3:
                                    Console.Write("Podaj ID rezerwacji: ");
                                    int reservationId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.ShowReservationAsync(reservationId);
                                    break;

                                case 4:
                                    Console.WriteLine("=== Modyfikacja rezerwacji ===");
                                case4:
                                    Console.Write("Podaj ID rezerwacji: ");
                                    if(!int.TryParse(Console.ReadLine(), out int modifyId))
                                    {
                                        Console.WriteLine("Nieprawidłowe dane");
                                        goto case4;
                                    }
                                    if(!await db.Reservations.AnyAsync(r => r.Id == modifyId))
                                    {
                                        Console.WriteLine("Nie znaleziono takiej rezerwacji");
                                        goto case4;
                                    }

                                    Console.WriteLine("Co chcesz zmodyfikować?");
                                    Console.WriteLine("1. Data rozpoczęcia");
                                    Console.WriteLine("2. Data zakończenia");
                                    Console.WriteLine("3. Sala konferencyjna");
                                    Console.WriteLine("4. Cel rezerwacji");

                                    if (int.TryParse(Console.ReadLine(), out int modChoice))
                                    {
                                        switch (modChoice)
                                        {
                                            case 1:
                                                Console.Write("Podaj liczbę godzin do przesunięcia: ");
                                                double hoursOffset = double.Parse(Console.ReadLine() ?? "0");
                                                DateTime newBeginDate = await db.Reservations.Where(r => r.Id == modifyId).Select(r => r.BeginDate).FirstOrDefaultAsync();
                                                newBeginDate = newBeginDate.AddHours(hoursOffset);
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.BeginDate, newBeginDate);
                                                break;
                                            case 2:
                                                Console.Write("Podaj nową liczbę godzin trwania (wartość ujemna - odjęcie czasu): ");
                                                double newHours = double.Parse(Console.ReadLine() ?? "0");
                                                DateTime newEndDate = await db.Reservations.Where(r => r.Id == modifyId).Select(r => r.EndDate).FirstOrDefaultAsync();
                                                newEndDate = newEndDate.AddHours(newHours);
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.EndDate,
                                                    newEndDate);
                                                break;
                                            case 3:
                                                Console.Write("Podaj nowe ID sali (1 lub 2): ");
                                                int newRoomId = int.Parse(Console.ReadLine() ?? "0");
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.ConferenceRoomId,
                                                    newRoomId);
                                                break;
                                            case 4:
                                                Console.Write("Podaj nowy cel rezerwacji: ");
                                                string newPurpose = Console.ReadLine() ?? string.Empty;
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.Purpose,
                                                    newPurpose);
                                                break;
                                        }
                                    }
                                    break;

                                case 5:
                                    Console.Write("Podaj ID rezerwacji do usunięcia: ");
                                    int deleteId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.DeleteReservationAsync(deleteId);
                                    break;

                                case 6:
                                    Console.Write("Podaj numer pokoju (ID): ");
                                    int conferenceRoomId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.ShowRoomDetailAsync(conferenceRoomId);
                                    break;

                                case 7:
                                    Console.WriteLine("Lista użytkowników");
                                    foreach (var getUser in UserSystem.GetUsers())
                                    {
                                        Console.WriteLine($"Dane użytkownuika ID.{getUser.Id}" +
                                            $"\n\tNazwa: {getUser.Name}" +
                                            $"\n\tDostęp: {getUser.Type}" +
                                            $"\n\tEmail: {getUser.Email}");
                                    }
                                    break;

                                case 8:
                                    Console.Write("Podaj id użytkownika do usunięcia: ");
                                    if (int.TryParse(Console.ReadLine(), out int userToDeleteId))
                                    {
                                        if (user.Id != userToDeleteId)
                                            UserSystem.DeleteUser(userToDeleteId);
                                        else
                                            Console.WriteLine("Nie można usunąć konta na którym jest się aktualnie");
                                    }
                                    break;

                                case 9:
                                    Console.WriteLine("===Rejestracja użytkownika===");
                                    Console.Write("Podaj email: ");
                                    string registerEmail = Console.ReadLine() ?? string.Empty;
                                    Console.Write("Podaj haslo: ");
                                    string registerPassword = Console.ReadLine() ?? string.Empty;
                                    Console.Write("Nazwa użytkownika: ");
                                    string registerUserName = Console.ReadLine() ?? string.Empty;
                                    Console.Write("Wybierz rolę: 1 - Administrator, 2 - Pracownik, 3 - Klient zewnętrzny: ");
                                    Data.AccessType accessType = Data.AccessType.Klient_zewnetrzny;
                                    if (int.TryParse(Console.ReadLine(), out int role))
                                    {
                                        switch (role)
                                        {
                                            case 1:
                                                accessType = Data.AccessType.Administrator;
                                                break;
                                            case 2:
                                                accessType = Data.AccessType.Pracownik;
                                                break;
                                            case 3:
                                                accessType = Data.AccessType.Klient_zewnetrzny;
                                                break;
                                            default:
                                                accessType = Data.AccessType.Klient_zewnetrzny;
                                                break;
                                        }
                                    }
                                    UserSystem.AddUser(registerUserName, accessType, registerEmail, registerPassword);
                                    break;

                                case 10:
                                    Console.WriteLine("=== Wyszukiwanie wolnych terminów ===");
                                    Console.Write("Podaj ID sali (1 lub 2): ");
                                    int searchRoomId = int.Parse(Console.ReadLine() ?? "1");

                                    Console.WriteLine("Podaj datę (dd.MM.yyyy): ");
                                    if(DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                                    {
                                        Console.Write("Podaj wymaganą liczbę godzin (domyślnie 1): ");
                                        int.TryParse(Console.ReadLine(), out int duration);
                                        duration = duration <= 0 ? 1 : duration;

                                        await reservationSystem.FindAvaiableTimeSlotsAsync(searchDate, searchRoomId, duration);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nieprawidłowy format daty");
                                    }
                                        break;

                                case 0:
                                    UserSystem.Logout(ref user);
                                    Console.WriteLine("Pomyślnie wylogowano");
                                    isRunning = false;
                                    break;
                            }

                            if (isRunning)
                            {
                                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                                Console.ReadKey();
                            }

                        }
                        break;
                    #endregion
                    #region Pracownik
                    case Data.AccessType.Pracownik:
                        Console.WriteLine("1. Dodaj rezerwację");
                        Console.WriteLine("2. Pokaż wszystkie rezerwacje");
                        Console.WriteLine("3. Pokaż szczegóły rezerwacji");
                        Console.WriteLine("4. Modyfikuj rezerwację");
                        Console.WriteLine("5. Usuń rezerwację");
                        Console.WriteLine("6. Pokaż szczegóły sali");
                        Console.WriteLine("7. Wyszukaj wolne terminy");
                        Console.WriteLine("0. Wyloguj się");
                        Console.Write("\nWybierz opcję: ");

                        if (int.TryParse(Console.ReadLine(), out int choice1))
                        {
                            Console.Clear();
                            switch (choice1)
                            {
                                case 1:
                                    Console.WriteLine("=== Dodawanie rezerwacji ===");

                                    Console.Write("ID sali (1 lub 2): ");
                                    int roomId = int.Parse(Console.ReadLine() ?? "0");

                                    Console.WriteLine("Podaj datę (dd.MM.yyyy): ");
                                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime addDate))
                                    {
                                        Console.WriteLine("Nieprawidłowy format daty");
                                    }

                                    Console.Write("Godzina rozpoczęcia: ");
                                    double beginHour = double.Parse(Console.ReadLine() ?? "8");

                                    if (beginHour < 8 || beginHour > 19)
                                    {
                                        Console.WriteLine("Godzina rozpoczęcia nie może byc wcześniejsza od 8 rano lub późniejsza od 20");
                                        break;
                                    }

                                    addDate = addDate.AddHours(beginHour);

                                    Console.Write("Czas trwania: ");
                                    double hours = double.Parse(Console.ReadLine() ?? "1");

                                    Console.Write("Cel rezerwacji: ");
                                    string purpose = Console.ReadLine() ?? string.Empty;

                                    Console.Write("Ilość uczestników spotkania: ");
                                    int participants = int.Parse(Console.ReadLine() ?? "0");

                                    await reservationSystem.AddReservationAsync(
                                        addDate,
                                        addDate.AddHours(hours),
                                        user.Id,
                                        roomId,
                                        purpose,
                                        participants);
                                    break;

                                case 2:
                                    Console.WriteLine("=== Lista wszystkich rezerwacji ===");
                                    await reservationSystem.ShowAllReservationsAsync();
                                    break;

                                case 3:
                                    Console.Write("Podaj ID rezerwacji: ");
                                    int reservationId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.ShowReservationAsync(reservationId);
                                    break;

                                case 4:
                                    Console.WriteLine("=== Modyfikacja rezerwacji ===");
                                case4:
                                    Console.Write("Podaj ID rezerwacji: ");
                                    if(!int.TryParse(Console.ReadLine(), out int modifyId))
                                    {
                                        Console.WriteLine("Nieprawidłowe dane");
                                        goto case4;
                                    }
                                    if(!await db.Reservations.AnyAsync(r => r.Id == modifyId))
                                    {
                                        Console.WriteLine("Nie znaleziono takiej rezerwacji");
                                        goto case4;
                                    }

                                    Console.WriteLine("Co chcesz zmodyfikować?");
                                    Console.WriteLine("1. Data rozpoczęcia");
                                    Console.WriteLine("2. Data zakończenia");
                                    Console.WriteLine("3. Sala konferencyjna");
                                    Console.WriteLine("4. Cel rezerwacji");

                                    if (int.TryParse(Console.ReadLine(), out int modChoice))
                                    {
                                        switch (modChoice)
                                        {
                                            case 1:
                                                Console.Write("Podaj liczbę godzin do przesunięcia: ");
                                                double hoursOffset = double.Parse(Console.ReadLine() ?? "0");
                                                DateTime newBeginDate = await db.Reservations.Where(r => r.Id == modifyId).Select(r => r.BeginDate).FirstOrDefaultAsync();
                                                newBeginDate = newBeginDate.AddHours(hoursOffset);
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.BeginDate, newBeginDate);
                                                break;
                                            case 2:
                                                Console.Write("Podaj nową liczbę godzin trwania (wartość ujemna - odjęcie czasu): ");
                                                double newHours = double.Parse(Console.ReadLine() ?? "0");
                                                DateTime newEndDate = await db.Reservations.Where(r => r.Id == modifyId).Select(r => r.EndDate).FirstOrDefaultAsync();
                                                newEndDate = newEndDate.AddHours(newHours);
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.EndDate,
                                                    newEndDate);
                                                break;
                                            case 3:
                                                Console.Write("Podaj nowe ID sali (1 lub 2): ");
                                                int newRoomId = int.Parse(Console.ReadLine() ?? "0");
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.ConferenceRoomId,
                                                    newRoomId);
                                                break;
                                            case 4:
                                                Console.Write("Podaj nowy cel rezerwacji: ");
                                                string newPurpose = Console.ReadLine() ?? string.Empty;
                                                await reservationSystem.ModifyReservationAsync(modifyId,
                                                    Data.ModifableReservationVariable.Purpose,
                                                    newPurpose);
                                                break;
                                        }
                                    }
                                    break;

                                case 5:
                                    Console.Write("Podaj ID rezerwacji do usunięcia: ");
                                    int deleteId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.DeleteReservationAsync(deleteId);
                                    break;

                                case 6:
                                    Console.Write("Podaj numer pokoju (ID): ");
                                    int conferenceRoomId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.ShowRoomDetailAsync(conferenceRoomId);
                                    break;

                                case 7:
                                    Console.WriteLine("=== Wyszukiwanie wolnych terminów ===");
                                    Console.Write("Podaj ID sali (1 lub 2): ");
                                    int searchRoomId = int.Parse(Console.ReadLine() ?? "1");

                                    Console.WriteLine("Podaj datę (dd.MM.yyyy): ");
                                    if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                                    {
                                        Console.Write("Podaj wymaganą liczbę godzin (domyślnie 1): ");
                                        int.TryParse(Console.ReadLine(), out int duration);
                                        duration = duration <= 0 ? 1 : duration;

                                        await reservationSystem.FindAvaiableTimeSlotsAsync(searchDate, searchRoomId, duration);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nieprawidłowy format daty");
                                    }
                                    break;

                                case 0:
                                    UserSystem.Logout(ref user);
                                    Console.WriteLine("Pomyślnie wylogowano");
                                    isRunning = false;
                                    break;
                            }

                            if (isRunning)
                            {
                                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                                Console.ReadKey();
                            }

                        }
                        break;
                    #endregion
                    #region Klient zewnętrzny
                    case Data.AccessType.Klient_zewnetrzny:
                        Console.WriteLine("1. Pokaż wszystkie rezerwacje");
                        Console.WriteLine("2. Pokaż szczegóły rezerwacji");
                        Console.WriteLine("3. Pokaż szczegóły sali");
                        Console.WriteLine("4. Wyszukaj wolne terminy");
                        Console.WriteLine("0. Wyloguj się");
                        Console.Write("\nWybierz opcję: ");

                        if (int.TryParse(Console.ReadLine(), out int choice2))
                        {
                            Console.Clear();
                            switch (choice2)
                            {
                                case 1:
                                    Console.WriteLine("=== Lista wszystkich rezerwacji ===");
                                    await reservationSystem.ShowAllReservationsAsync();
                                    break;

                                case 2:
                                    Console.Write("Podaj ID rezerwacji: ");
                                    int reservationId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.ShowReservationAsync(reservationId);
                                    break;

                                case 3:
                                    Console.Write("Podaj numer pokoju (ID): ");
                                    int conferenceRoomId = int.Parse(Console.ReadLine() ?? "0");
                                    await reservationSystem.ShowRoomDetailAsync(conferenceRoomId);
                                    break;

                                case 4:
                                    Console.WriteLine("=== Wyszukiwanie wolnych terminów ===");
                                    Console.Write("Podaj ID sali (1 lub 2): ");
                                    int searchRoomId = int.Parse(Console.ReadLine() ?? "1");

                                    Console.WriteLine("Podaj datę (dd.MM.yyyy): ");
                                    if (DateTime.TryParse(Console.ReadLine(), out DateTime searchDate))
                                    {
                                        Console.Write("Podaj wymaganą liczbę godzin (domyślnie 1): ");
                                        int.TryParse(Console.ReadLine(), out int duration);
                                        duration = duration <= 0 ? 1 : duration;

                                        await reservationSystem.FindAvaiableTimeSlotsAsync(searchDate, searchRoomId, duration);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nieprawidłowy format daty");
                                    }
                                    break;

                                case 0:
                                    UserSystem.Logout(ref user);
                                    Console.WriteLine("Pomyślnie wylogowano");
                                    isRunning = false;
                                    break;
                            }

                            if (isRunning)
                            {
                                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                                Console.ReadKey();
                            }

                        }
                        break;
                        #endregion
                }
  
            }
            #endregion
        }
    }
}