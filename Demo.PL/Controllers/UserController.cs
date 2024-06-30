using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //Get , GetAll , Add , Update , Delete
        //Index , Details , Edit , Delete

        public UserController(UserManager<ApplicationUser> usermanager)
        {
           _userManager = usermanager;
        }
        public async Task<IActionResult> Index(string SearchInput)
        {
            var users = Enumerable.Empty<UserViewModel>();
            if (string.IsNullOrEmpty(SearchInput))
            {
               users = await _userManager.Users.Select( U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles =  _userManager.GetRolesAsync(U).Result
                }).ToListAsync();  
            }
            else
            {
                users = await _userManager.Users.Where(U => U.Email
                .ToLower()
                .Contains(SearchInput.ToLower()))
                    .Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
            }
          
            return View(users);
        }
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

           var userfromDb = await _userManager.FindByIdAsync(id);
            if (userfromDb is null)
                return NotFound();
            var user = new UserViewModel()
            {
                Id=userfromDb.Id,
                FirstName=userfromDb.FirstName,
                LastName=userfromDb.LastName,
                Email = userfromDb.Email,
                Roles = _userManager.GetRolesAsync(userfromDb).Result
            };
            return View(ViewName, user);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            
                

                
                if (ModelState.IsValid)
                {
                    var userFromDb =await _userManager.FindByIdAsync(id);
                if (User is null)
                    return NotFound(model);

                userFromDb.FirstName = model.FirstName;
                userFromDb.LastName = model.LastName;
                userFromDb.Email = model.Email;

                await _userManager.UpdateAsync(userFromDb);
                   
                    return RedirectToAction(nameof(Index));
                }
            
            
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();




            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);
                if (User is null)
                    return NotFound(model);

               

                await _userManager.DeleteAsync(userFromDb);

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }
    }
}
