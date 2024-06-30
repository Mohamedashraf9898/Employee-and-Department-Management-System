using Demo.BLL.Interfaces;
using Demo.BLL.Repostories;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepository departmentRepository; //NULL
        private IEmployeeRepository employeeRepository; //NULL
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            departmentRepository = new DepartmentRepository(_context);
            employeeRepository =  new EmployeeRepository(_context);
        }
        public IDepartmentRepository DepartmentRepository => departmentRepository;
        public IEmployeeRepository EmployeeRepository =>employeeRepository;



        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
