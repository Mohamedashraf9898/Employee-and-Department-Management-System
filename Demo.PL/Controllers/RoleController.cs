using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        

        public RolesController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index(string SearchInput)
        {
            var Roles = Enumerable.Empty<RoleviewModel>();
            if (string.IsNullOrEmpty(SearchInput))
            {
                Roles  = await _roleManager.Roles.Select(R => new RoleviewModel()
                {
                    Id=R.Id,
                    RoleName=R.Name,
                }).ToListAsync();
            }
            else
            {
                Roles = await _roleManager.Roles.Where(R => R.Name
                .ToLower()
                .Contains(SearchInput.ToLower()))
                    .Select(R => new RoleviewModel()
                    {
                        Id = R.Id,
                        RoleName=R.Name
                    }).ToListAsync();
            }

            return View(Roles);
        }
        [HttpGet]

        public IActionResult create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> create(RoleviewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name=model.RoleName,
                };
               await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var RolefromDb = await _roleManager.FindByIdAsync(id);
            if (RolefromDb is null)
                return NotFound();
            var Role = new RoleviewModel()
            {
                Id = RolefromDb.Id,
                RoleName=RolefromDb.Name,
               
            };
            return View(ViewName, Role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleviewModel model)
        {
            if (id != model.Id)
                return BadRequest();




            if (ModelState.IsValid)
            {
                var roleFromDb = await _roleManager.FindByIdAsync(id);
                if (User is null)
                    return NotFound(model);

                roleFromDb.Name = model.RoleName;

                await _roleManager.UpdateAsync(roleFromDb);

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
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleviewModel model)
        {
            if (id != model.Id)
                return BadRequest();




            if (ModelState.IsValid)
            {
                var roleFromDb = await _roleManager.FindByIdAsync(id);
                if (User is null)
                    return NotFound(model);



                await _roleManager.DeleteAsync(roleFromDb);

                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
             var role = await  _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                { 
                    UserId=user.Id,
                    UserName=user.UserName,
                };

                if(await _userManager.IsInRoleAsync(user,role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected=false;
                }

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId , List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (! user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new {id=roleId});
            }
            return View(users);
        }
    }
}
