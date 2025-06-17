using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly LeaveManagementSystemWebContext _context;
        private readonly IMapper _mapper;
        private const string NameExistsValidationMessage = "This leave type already exists in the database.";

        public LeaveTypesController(LeaveManagementSystemWebContext context, IMapper mapper) // dependency injection
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {

            var data = await _context.LeaveTypes.ToListAsync(); // SELECT * FROM LeaveTypes

            // convert the data model to a view model - Manually
            //var viewData = data.Select(q => new IndexVM
            //{
            //    LeaveTypeId = q.LeaveTypeId,
            //    LeaveTypeName = q.LeaveTypeName,
            //    NumberOfDays = q.NumberOfDays,
            //});

            // convert the data model to a view model - Using AutoMapper
            var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
            // return the view model to the view
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // parameterized query to prevent SQL injection attacks
            // like "SELECT * FROM LeaveTypes WHERE LeaveTypeId = id"
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.LeaveTypeId == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

            return View(viewData);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {
            // custom validation and model state 
            if (await CheckIfLeaveTypeNameExists(leaveTypeCreate.LeaveTypeName))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.LeaveTypeName), NameExistsValidationMessage);
            }

            // very important to validate the model state before processing
            if (ModelState.IsValid)
            {
                // convert to data model
                var leaveType = _mapper.Map<LeaveType>(leaveTypeCreate);
                
                _context.Add(leaveType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate);
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType);
            return View(viewData);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {
            if (id != leaveTypeEdit.LeaveTypeId)
            {
                return NotFound();
            }

            // custom validation and model state
            if (await CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.LeaveTypeName), NameExistsValidationMessage);
            }


            if (ModelState.IsValid)
            { 
                try
                {
                    var leaveType = _mapper.Map<LeaveType>(leaveTypeEdit);
                    _context.Update(leaveType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveTypeEdit.LeaveTypeId))
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
            return View(leaveTypeEdit);
        }


        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.LeaveTypeId == id);
            if (leaveType == null)
            {
                return NotFound();
            }

            var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);
            return View(viewData);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.LeaveTypeId == id);
        }

        private async Task<bool> CheckIfLeaveTypeNameExists(string leaveTypeName)
        {
            return await _context.LeaveTypes.AnyAsync(q => q.LeaveTypeName.ToLower().Equals(leaveTypeName.ToLower()));

            // could do this, but it is not recommended to use string comparison in LINQ queries
            // LINQ queries have to be translated to SQL, and SQL does not support StringComparison.InvariantCultureIgnoreCase
            // .Equals(leaveTypeName, StringComparison.InvariantCultureIgnoreCase
        }

        private async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
        {
            return await _context.LeaveTypes.AnyAsync(q => q.LeaveTypeName.ToLower()
                .Equals(leaveTypeEdit.LeaveTypeName.ToLower())
                && q.LeaveTypeId != leaveTypeEdit.LeaveTypeId
            );
        }
    }
}
