using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Regularidad.Models;
using Tickets.Data;

namespace Tickets.Controllers
{
    public class TicketDetallesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketDetallesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketDetalles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketDetalle.Include(t => t.estado).Include(t => t.ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TicketDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TicketDetalle == null)
            {
                return NotFound();
            }

            var ticketDetalle = await _context.TicketDetalle
                .Include(t => t.estado)
                .Include(t => t.ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketDetalle == null)
            {
                return NotFound();
            }

            return View(ticketDetalle);
        }

        // GET: TicketDetalles/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Descripcion");
            ViewData["TicketId"] = new SelectList(_context.Set<Ticket>(), "Id", "NombreTicket");
            return View();
        }

        // POST: TicketDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,DescripcionPedido,EstadoId,FechaEstado")] TicketDetalle ticketDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", ticketDetalle.EstadoId);
            ViewData["TicketId"] = new SelectList(_context.Set<Ticket>(), "Id", "Id", ticketDetalle.TicketId);
            return View(ticketDetalle);
        }

        // GET: TicketDetalles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TicketDetalle == null)
            {
                return NotFound();
            }

            var ticketDetalle = await _context.TicketDetalle.FindAsync(id);
            if (ticketDetalle == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Descripcion", ticketDetalle.EstadoId);
            ViewData["TicketId"] = new SelectList(_context.Set<Ticket>(), "Id", "NombreTicket", ticketDetalle.TicketId);
            return View(ticketDetalle);
        }

        // POST: TicketDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,DescripcionPedido,EstadoId,FechaEstado")] TicketDetalle ticketDetalle)
        {
            if (id != ticketDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketDetalleExists(ticketDetalle.Id))
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
            ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Descripcion", ticketDetalle.EstadoId);
            ViewData["TicketId"] = new SelectList(_context.Set<Ticket>(), "Id", "NombreTicket", ticketDetalle.TicketId);
            return View(ticketDetalle);
        }

        // GET: TicketDetalles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TicketDetalle == null)
            {
                return NotFound();
            }

            var ticketDetalle = await _context.TicketDetalle
                .Include(t => t.estado)
                .Include(t => t.ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketDetalle == null)
            {
                return NotFound();
            }

            return View(ticketDetalle);
        }

        // POST: TicketDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TicketDetalle == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TicketDetalle'  is null.");
            }
            var ticketDetalle = await _context.TicketDetalle.FindAsync(id);
            if (ticketDetalle != null)
            {
                _context.TicketDetalle.Remove(ticketDetalle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketDetalleExists(int id)
        {
          return (_context.TicketDetalle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
