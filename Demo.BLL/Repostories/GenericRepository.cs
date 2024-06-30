using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repostories
{
    public class GenericRepository<T>:IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            //_context = new DataDpContext();
        }
        public void Add(T entity)
        {
            _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<T> Get(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task< IEnumerable<T> >GetAll()
        {
            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E=>E.Department).ToListAsync();
            }
            //var departments = _context.Departments.ToList();
            //return departments;
             
            return await _context.Set<T>().ToListAsync();;
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
