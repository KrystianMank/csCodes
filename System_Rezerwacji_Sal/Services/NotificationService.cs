using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_Rezerwacji_Sal.Models;

namespace System_Rezerwacji_Sal.Services
{
    public static class NotificationService
    {
        public static void SendNotificationForUser(ReservationModel[] reservations, UserModel user)
        {
            foreach (ReservationModel reservation in reservations)
            {
                if (reservation.BeginDate.Date == DateTime.Now.Date)
                {
                    Console.WriteLine($"Przypomnienie: Masz dzisiaj rezerwację sali {reservation.ConferenceRoomId} o {reservation.BeginDate:HH:mm}");
                }
            }
        }
    }
}
