using FlapKap.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }
        public void Delete(object id)
        {
            T exist = entities.Find(id);
            entities.Remove(exist);
        }

        public IQueryable<T> GetAll(Func<T, bool> expr)
        {
            return entities.Where(expr).AsQueryable();
        }

        public T GetById(object id)
        {
            return entities.Find(id);
        }

        public void Insert(T obj)
        {
            entities.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            _context.ChangeTracker.Clear();
            entities.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
    }
}
