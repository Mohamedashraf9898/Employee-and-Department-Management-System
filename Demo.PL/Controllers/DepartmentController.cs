using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repostories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {
        //Action : public Non Static Method
        // /Department/Index
        //private IDepartmentRepository _departmentrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentController(/*IDepartmentRepository departmentrepository*/ IUnitOfWork unitOfWork,IMapper mapper)
        {
            //_departmentrepository = departmentrepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchInput)
        {
            var departments=Enumerable.Empty<Department>();
            if(string.IsNullOrEmpty(SearchInput))
            {
                departments = await _unitOfWork.DepartmentRepository.GetAll();
            }
            else
            {
                departments=_unitOfWork.DepartmentRepository.GetByName(SearchInput);
            }
           var result =  _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);
           
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Department department = new Department()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    Code = model.Code,
                //    DateOfCreation = model.DateOfCreation,
                //    Employees = model.Employees,
                //};
                var department = _mapper.Map<Department>(model);
                 _unitOfWork.DepartmentRepository.Add(department);
                var count =await _unitOfWork.Complete();

                if (count > 0)
                    if (count > 0)
                    {
                        TempData["Message"] = "Created";
                    }
                    else
                    {
                        TempData["Message"] = "Nothing Change";
                    }
                return RedirectToAction(nameof(Index));  
            }

            return View(model);
        }
        public IActionResult Details(int? id,string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            var departmentviewmodel = _mapper.Map<DepartmentViewModel>(department);

            if (department is null)
                return NotFound();
            return View(ViewName , departmentviewmodel);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();

            //var department = _departmentrepository.Get(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit([FromRoute]int? id , Department model)
        {
            if (id != model.Id)
                return BadRequest();
            var department = _mapper.Map<Department>(model);

            if (ModelState.IsValid)
            {
                 _unitOfWork.DepartmentRepository.Update(model);
                var count =await _unitOfWork.Complete();

                if (count > 0)
                {

                    TempData["Message"] = "Updated";
                }
                else
                {
                    TempData["Message"] = "Nothing Changed";
                }
                    return RedirectToAction(nameof(Index));
              
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var department = _departmentrepository.Get(id.Value);
            //if (department is null)
            //    return NotFound();
            //return View(department);
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Delete(DepartmentViewModel model)
        {
            var department = _mapper.Map<Department>(model);
            if (ModelState.IsValid)
            {
                 _unitOfWork.DepartmentRepository.Delete(department);
                var count =await _unitOfWork.Complete();

                if (count > 0)
                    {
                     TempData["Message"] = "Deleted";
                    }
                else
                    {
                        TempData["Message"] = "Nothing Changed";
                    }
                    return RedirectToAction(nameof(Index));
                
            }
            
            return View(model);
        }

    }    
}
