using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(Func<T,bool>expr);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
