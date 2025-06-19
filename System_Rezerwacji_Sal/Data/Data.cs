using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Rezerwacji_Sal.Data
{ 
    public static class Data
    {
        public static List<string>RoomEquipment { get; } = new List<string>
        {
            "Rzutnik multimedialny",
            "Ekran projekcyjny",
            "Tablica suchościeralna",
            "Flipchart",
            "System nagłośnieniowy",
            "Mikrofon bezprzewodowy",
            "Kamera konferencyjna",
            "Komputer stacjonarny",
            "Wi-Fi (high-speed)",
            "Gniazda elektryczne przy stołach",
            "Klimatyzacja",
            "Rolety zaciemniające",
            "Stoliki w układzie boardroom",
            "Ładowarki USB",
            "Woda i szklanki"
        };

        public enum AccessType
        {
            Administrator,
            Pracownik,
            Klient_zewnetrzny,
        }

        public enum ModifableReservationVariable
        {
            BeginDate,
            EndDate,
            ConferenceRoomId,
            Purpose
        }
    }
}
