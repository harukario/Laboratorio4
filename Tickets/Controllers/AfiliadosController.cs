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
using static System.Net.Mime.MediaTypeNames;

namespace Tickets.Controllers
{
    
    public class AfiliadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
      

        public AfiliadosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Afiliadoes
        public async Task<IActionResult> Index(string buscNombre, string buscApellido, int? buscaDni)
        {
           
            var afiliados = _context.Afiliado.AsQueryable();

           
            if (!string.IsNullOrEmpty(buscNombre))
            {
                afiliados = afiliados.Where(a => a.Nombres.Contains(buscNombre));
            }

            if (!string.IsNullOrEmpty(buscApellido))
            {
                afiliados = afiliados.Where(a => a.Apellido.Contains(buscApellido));
            }

            if (buscaDni.HasValue)
            {
                afiliados = afiliados.Where(a => a.Dni.ToString().Contains(buscaDni.ToString()));
            }

            var listaFiltrada = await afiliados.ToListAsync();

          
            return View(listaFiltrada);
        }

        //IMPORTAR
        [Authorize]
        public async Task<IActionResult> Importar()
        {
            
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivo = archivos[0];
                if (archivo.Length > 0)
                {
                    var pathDestino = Path.Combine(_env.WebRootPath, "importaciones");
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivo.FileName);
                    var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                    using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                    {
                        archivo.CopyTo(filestream);
                    }
                    using (var file = new FileStream(rutaDestino, FileMode.Open))
                    {


                        List<string> renglones = new List<string>();
                        List<Afiliado> AfiliadosData = new List<Afiliado>();

                        StreamReader fileContent = new StreamReader(file, System.Text.Encoding.Default);
                        do
                        {
                            renglones.Add(fileContent.ReadLine());
                        }
                        while (!fileContent.EndOfStream);

                        if (renglones.Count() > 0)
                        {
                            foreach (var renglon in renglones)
                            {
                                string[] data = renglon.Split(';');
                                if (data.Length == 5)
                                {
                                    Afiliado afiliado = new Afiliado();
                                    afiliado.Apellido = data[1].Trim();
                                    afiliado.Nombres = data[2].Trim();
                                    afiliado.Dni = int.Parse(data[3].Trim());
                                    afiliado.FechaNacimiento = DateTime.Parse(data[4].Trim());
                                    AfiliadosData.Add(afiliado);
                                }
                            }
                            if (AfiliadosData.Count() >0)
                            {
                                _context.AddRange(AfiliadosData);
                                await _context.SaveChangesAsync();
                            }
                            
                        }
                    }
                }
            }
                return _context.Afiliado != null ?
                    View("Index", await _context.Afiliado.ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Afiliado'  is null.");
            
        }

        // GET: Afiliadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Afiliado == null)
            {
                return NotFound();
            }

            var afiliado = await _context.Afiliado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (afiliado == null)
            {
                return NotFound();
            }

            return View(afiliado);
        }

        // GET: Afiliadoes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Afiliadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Apellido,Nombres,Dni,FechaNacimiento")] Afiliado afiliado)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    { 
                    var pathDestino = Path.Combine(_env.WebRootPath, "images","afiliados");
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);


                    using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                    {
                        archivoFoto.CopyTo(filestream);
                        afiliado.foto = archivoDestino;
                    }
                }
            }

            _context.Add(afiliado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
            return View(afiliado);

        }

        // GET: Afiliadoes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Afiliado == null)
            {
                return NotFound();
            }

            var afiliado = await _context.Afiliado.FindAsync(id);
            if (afiliado == null)
            {
                return NotFound();
            }
            return View(afiliado);
        }

        // POST: Afiliadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Apellido,Nombres,Dni,FechaNacimiento,foto")] Afiliado afiliado)
        {
            if (id != afiliado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(_env.WebRootPath, "images", "afiliados");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        string fotoAnterior = Path.Combine(pathDestino, afiliado.foto);
                        if (
                            System.IO.File.Exists(fotoAnterior))
                            System.IO.File.Delete(fotoAnterior);
                        

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            afiliado.foto = archivoDestino;
                        }
                    }
                } 
                try
                {
                    _context.Update(afiliado);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AfiliadoExists(afiliado.Id))
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
            return View(afiliado);
        }

        // GET: Afiliadoes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Afiliado == null)
            {
                return NotFound();
            }

            var afiliado = await _context.Afiliado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (afiliado == null)
            {
                return NotFound();
            }

            return View(afiliado);
        }

        // POST: Afiliadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Afiliado == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Afiliado'  is null.");
            }
            var afiliado = await _context.Afiliado.FindAsync(id);
            if (afiliado != null)
            {
                _context.Afiliado.Remove(afiliado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AfiliadoExists(int id)
        {
          return (_context.Afiliado?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
