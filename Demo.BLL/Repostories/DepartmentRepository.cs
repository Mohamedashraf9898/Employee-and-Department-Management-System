using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repostories
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        #region MyRegion
        //private readonly AppDbContext _context;
        //public DepartmentRepository(AppDbContext context)
        //{
        //    _context = context;
        //    //_context = new DataDpContext();
        //}
        //public int Add(Department entity)
        //{
        //    _context.Departments.Add(entity);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department entity)
        //{
        //     _context.Departments.Remove(entity);
        //    return _context.SaveChanges();
        //}

        //public Department Get(int id)
        //{
        //    var departmen = _context.Departments.FirstOrDefault(D=>D.Id == id);
        //    return departmen;
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //   var departments = _context.Departments.ToList();
        //    return departments;
        //}

        //public int Update(Department entity)
        //{
        //    _context.Departments.Update(entity);
        //    return _context.SaveChanges();
        //} 
        #endregion
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }
        
        public IEnumerable<Department> GetByName(string name)
        {
            return _context.Departments.Where(D => D.Name.ToLower().Contains(name));
        }
    }
}
