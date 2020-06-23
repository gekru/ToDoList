using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToDoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the current application user
        /// </summary>
        /// <returns></returns>
        public IdentityUser GetCurrentUser()
        {
            // Define current user Id
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Find user from DB
            IdentityUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);

            return currentUser;
        }

        /// <summary>
        /// Get the current user ToDoList
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ToDo> GetMyToDos()
        {
            return _context.ToDos.ToList().Where(x => x.User == GetCurrentUser());
        }

        // GET: ToDoes
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable", GetMyToDos());
        }

        // GET: ToDoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // GET: ToDoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,IsDone")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                // Associate current user with model user
                toDo.User = GetCurrentUser();

                _context.Add(toDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDo);
        }

        #region TODO! PARTIALVIEW
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> InstantCreate([Bind("Id,Description")] ToDo toDo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Associate current user with model user
        //        toDo.User = GetCurrentUser();
        //        // IsDone uqual to false such as todo just created
        //        toDo.IsDone = false;

        //        _context.Add(toDo);
        //        await _context.SaveChangesAsync();
        //    }
        //    return BuildToDoTable();
        //}
        #endregion

        // GET: ToDoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            return View(toDo);
        }

        // POST: ToDoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,IsDone")] ToDo toDo)
        {
            if (id != toDo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(toDo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDo);
        }

        // GET: ToDoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // POST: ToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDo = await _context.ToDos.FindAsync(id);
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoExists(int id)
        {
            return _context.ToDos.Any(e => e.Id == id);
        }
    }
}
