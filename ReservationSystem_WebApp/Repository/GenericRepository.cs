using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace ReservationSystem_WebApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context = null;

        protected DbSet<T> table = null;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            if(existing != null)
            {
                table.Remove(existing);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Update(obj);
        }
    }
}
