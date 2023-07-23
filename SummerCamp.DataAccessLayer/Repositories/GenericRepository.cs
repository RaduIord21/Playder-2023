using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
namespace SummerCamp.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected SummerCampDbContext context;

        protected DbSet<T> dataSet;

        public GenericRepository(SummerCampDbContext context)
        {
            this.context = context;
            dataSet = this.context.Set<T>();
        }

        public IList<T> GetAll()
        {
             return dataSet.ToList();
        }

        public T? GetById(int id)
        {
            return dataSet.Find(id);
        }

        public void Update(T obj)
        {
            dataSet.Update(obj);
        }

        public void Delete(T obj)
        {
            dataSet.Remove(obj);
        }

        public IList<T> Get(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>()
            .Where(expression)
            .ToList();
        }

        public void Add(T entity)
        {
            dataSet.Add(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
 