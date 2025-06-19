using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Rezerwacji_Sal.Data
{
    public class ReservationIdGenerator : ValueGenerator<int>
    {
        private readonly ConcurrentQueue<int> _avaibleIds = new();
        private int _maxUsedId = 0;
        private readonly object _lock = new();

        public override int Next(EntityEntry entry)
        {
            lock (_lock)
            {
                if (_avaibleIds.TryDequeue(out int nextId))
                {
                    return nextId;
                }
                return ++_maxUsedId;
            }
        }

        public void AddAvaibleId(int id)
        {
            lock (_lock)
            {
                _avaibleIds.Enqueue(id);
                if (id > _maxUsedId)
                {
                    _maxUsedId = id;
                }
            }
        }
        public override bool GeneratesTemporaryValues => false;
    }
}
