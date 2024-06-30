using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
		Task<IEnumerable<Employee>> GetAll();
		#region MyRegion
		////IEnumerable<Employee> GetAll();
		////Employee Get(int Id);
		////int Add(Employee entity);
		////int Update(Employee entity);
		////int Delete(Employee entity); 
		#endregion
		Task<IEnumerable<Employee>> GetByName(string name);
    }
}
