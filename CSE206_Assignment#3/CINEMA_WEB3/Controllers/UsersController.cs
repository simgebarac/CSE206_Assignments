using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore;
using CINEMA_WEB3.Models;
using System.ComponentModel.DataAnnotations; 

namespace CINEMA_WEB3.Controllers
{
    public class UsersController : Controller
    {
        private readonly CinemaContext _context;

        public UsersController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return _context.Users != null ?
                            View(await _context.Users.ToListAsync()) :
                            Problem("Entity set 'CinemaContext.Users' is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            PopulateRolesDropdown(); 
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,PasswordHash,FullName,Role")] User user)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbEx)
                {
                    if (dbEx.InnerException != null && (dbEx.InnerException.Message.Contains("duplicate key") || dbEx.InnerException.Message.Contains("UNIQUE constraint failed")))
                    {
                        TempData["ErrorMessage"] = "Bu kullanıcı adı zaten mevcut. Lütfen farklı bir kullanıcı adı seçin.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Kullanıcı eklenirken bir veritabanı hatası oluştu: {dbEx.Message}";
                    }
                    PopulateRolesDropdown(user.Role); 
                    return View(user);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Kullanıcı eklenirken beklenmedik bir hata oluştu: {ex.Message}";
                    PopulateRolesDropdown(user.Role); 
                    return View(user);
                }
            }

            PopulateRolesDropdown(user.Role); 
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            PopulateRolesDropdown(user.Role); 
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,PasswordHash,FullName,Role")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        TempData["ErrorMessage"] = "Güncellenecek kullanıcı bulunamadı.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Kullanıcı güncellenirken bir eşzamanlılık hatası oluştu.";
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Kullanıcı güncellenirken bir hata oluştu: {ex.Message}";
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateRolesDropdown(user.Role);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                TempData["ErrorMessage"] = "Kullanıcılar veritabanı seti mevcut değil.";
                return Problem("Entity set 'CinemaContext.Users' is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Kullanıcı silinirken bir hata oluştu: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Silinecek kullanıcı bulunamadı.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

       
        private void PopulateRolesDropdown(string selectedRole = null)
        {
            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "admin", Text = "Admin" },
                new SelectListItem { Value = "customer", Text = "Customer" }
            };

          
            if (!string.IsNullOrEmpty(selectedRole))
            {
                var selectedItem = roles.FirstOrDefault(r => r.Value == selectedRole);
                if (selectedItem != null)
                {
                    selectedItem.Selected = true;
                }
            }

            ViewBag.RoleList = roles;
        }
    }
}