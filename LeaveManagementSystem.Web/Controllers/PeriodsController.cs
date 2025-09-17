using LeaveManagementSystem.Application.Models.Periods;
using LeaveManagementSystem.Application.Services.Periods;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class PeriodsController(IPeriodsService _periodsService) : Controller
    {
        // GET: Periods
        public async Task<IActionResult> Index()
        {
            var viewData = await _periodsService.GetAll(); // encapsulate the logic in the service layer
            return View(viewData);
        }

        // GET: Periods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _periodsService.Get<PeriodReadOnlyVM>(id.Value);

            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }
  
        // GET: Periods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var period = await _periodsService.Get<PeriodReadOnlyVM>(id.Value);

            if (period == null)
            {
                return NotFound();
            }

            return View(period);
        }

        // POST: Periods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _periodsService.Remove(id);
            return RedirectToAction(nameof(Index));
        }  
    }
}
