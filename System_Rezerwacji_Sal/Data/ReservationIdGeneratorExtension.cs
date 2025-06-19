using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System_Rezerwacji_Sal.Data
{
    public static class ReservationIdGeneratorExtension
    {
        public static void InitializeMaxId(this ReservationIdGenerator generator, int maxId)
        {
            var field = generator.GetType()
                .GetField("_maxUsedId", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(generator, maxId);
        }
    }
}
