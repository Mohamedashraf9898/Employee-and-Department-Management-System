 using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repostories;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private IEmployeeRepository _employeerepository;
        //private IDepartmentRepository _departmentrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmployeeController(/*IEmployeeRepository employeeRepository*/IUnitOfWork unitOfWork,IMapper mapper, IDepartmentRepository departmentrepository)
        {
            //_employeerepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper=mapper;
            //_departmentrepository = departmentrepository;
        }
        public async Task<IActionResult> Index(string SearchInput)
        {
            var employees= Enumerable.Empty<Employee>();
             _unitOfWork.Complete();
            if (string.IsNullOrEmpty(SearchInput))
            {
                //employees = _employeerepository.GetAll();
                employees =await _unitOfWork.EmployeeRepository.GetAll();

            }
            else
            {
                employees =await _unitOfWork.EmployeeRepository.GetByName(SearchInput.ToLower());       
            }
           var result =  _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
           
            // View's Dictionary --> Send Extra Information.

            //1-ViewData : Inherited from Controller Class.
            ViewData["Messege"] = "Hello ViewData";
            //2-ViewBag : Inherited from Controller Class.
            ViewBag.Message = "Hello ViewBag";
            //return View(employees);
            //3-tempData
            //TempData["Message"] = "Welcome To Our Company";
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        { 
            ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    HireDate = model.HireDate,
                //    Salary = model.Salary,
                //    Address = model.Address,
                //    PhoneNumber = model.PhoneNumber,
                //    DateOfCreation = model.DateOfCreation,
                //    DepartmentId = model.DepartmentId,
                //    Department = model.Department,
                //    IsDeleted = model.IsDeleted,
                //    IsActive = model.IsActive
                //};
                var employee = _mapper.Map<Employee>(model);
                //int count = _employeerepository.Add(employee);
                 _unitOfWork.EmployeeRepository.Add(employee);
                var count =await _unitOfWork.Complete();

                if (count > 0)
                {
                    TempData["Message"] = "Updated";
                }
                else
                {
                
                    TempData["Message"] = "Nothing Change";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee =_unitOfWork.EmployeeRepository.Get(id.Value);
            var employeeviewmodel = _mapper.Map<EmployeeViewModel>(employee);
            //EmployeeViewModel employeeViewmodel =(EmployeeViewModel) employee;
            if (employee is null)
                return NotFound();
            return View(ViewName, employeeviewmodel);
        }
        //public IActionResult Details(int? id, string ViewName = "Details")
        //{
        //    if (id is null)
        //        return BadRequest();
        //    var employee = _employeerepository.Get(id.Value);
        //    if (employee is null)
        //        return NotFound();
        //    return View(ViewName, employee);
        //}
        [HttpGet]
        public IActionResult Edit(int? id )
        {
            //ViewData["Departments"] = _departmentrepository.GetAll();
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit([FromRoute]int? id , EmployeeViewModel model) 
        { 
            if (id != model.Id)
                return BadRequest();
            if (model.ImageName is not null)
            {
                DocumentSettings.DeleteFile(model.ImageName, "images");
            }
            model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

            var employee = _mapper.Map<Employee>(model);
            if (ModelState.IsValid)
            {

                _unitOfWork.EmployeeRepository.Update(employee);
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
            return View (model);
        }
        [HttpGet]
        public IActionResult Delete(int? id) 
        {
            return Details(id , "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id , EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
                var employee = _mapper.Map<Employee>(model);

            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count =await _unitOfWork.Complete();

                if (count > 0)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
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
