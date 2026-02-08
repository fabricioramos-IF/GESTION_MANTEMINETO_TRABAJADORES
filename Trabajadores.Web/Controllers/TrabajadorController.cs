using Microsoft.AspNetCore.Mvc;
using Trabajadores.Web.Models;
using Trabajadores.Web.Services;
using Trabajadores.Web.ViewModels;

namespace Trabajadores.Web.Controllers
{
    public class TrabajadorController : Controller
    {
        private readonly ITrabajadorService _trabajadorService;
        private readonly IFileUploadService _fileUploadService;

        public TrabajadorController(ITrabajadorService trabajadorService, IFileUploadService fileUploadService)
        {
            _trabajadorService = trabajadorService;
            _fileUploadService = fileUploadService;
        }

        public async Task<IActionResult> Index(string? sexo = null)
        {
            var trabajadores = await _trabajadorService.ListarTrabajadores(sexo);
            ViewBag.FiltroSexo = sexo;
            return View(trabajadores);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int id)
        {
            var trabajador = await _trabajadorService.ObtenerTrabajadorPorId(id);
            
            if (trabajador == null)
            {
                return NotFound(new { mensaje = "Trabajador no encontrado" });
            }

            var viewModel = new TrabajadorViewModel
            {
                IdTrabajador = trabajador.IdTrabajador,
                Nombres = trabajador.Nombres,
                Apellidos = trabajador.Apellidos,
                TipoDocumento = trabajador.TipoDocumento,
                NumeroDocumento = trabajador.NumeroDocumento,
                Sexo = trabajador.Sexo,
                FechaNacimiento = trabajador.FechaNacimiento,
                FotoUrl = trabajador.Foto,
                Direccion = trabajador.Direccion
            };

            return Json(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(TrabajadorViewModel model)
        {
            ModelState.Remove("FotoArchivo");
            
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value!.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();
                return Json(new { exito = false, mensaje = string.Join(". ", errores) });
            }

            string? fotoUrl = null;

            if (model.FotoArchivo != null && model.FotoArchivo.Length > 0)
            {
                if (!_fileUploadService.ValidateImage(model.FotoArchivo, out string errorMessage))
                {
                    return Json(new { exito = false, mensaje = errorMessage });
                }

                fotoUrl = await _fileUploadService.UploadImageAsync(model.FotoArchivo);
                
                if (string.IsNullOrEmpty(fotoUrl))
                {
                    return Json(new { exito = false, mensaje = "Error al subir imagen" });
                }
            }

            var trabajador = new Trabajador
            {
                Nombres = model.Nombres?.Trim() ?? "",
                Apellidos = model.Apellidos?.Trim() ?? "",
                TipoDocumento = model.TipoDocumento ?? "",
                NumeroDocumento = model.NumeroDocumento?.Trim() ?? "",
                Sexo = model.Sexo ?? "",
                FechaNacimiento = model.FechaNacimiento,
                Foto = fotoUrl,
                Direccion = model.Direccion?.Trim()
            };

            var resultado = await _trabajadorService.InsertarTrabajador(trabajador);

            if (resultado)
            {
                return Json(new { exito = true, mensaje = "Trabajador registrado exitosamente" });
            }

            return Json(new { exito = false, mensaje = "Error al registrar. Verifique que el documento no esté duplicado." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(TrabajadorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return Json(new { exito = false, mensaje = string.Join(", ", errores) });
            }

            var trabajadorExistente = await _trabajadorService.ObtenerTrabajadorPorId(model.IdTrabajador);
            
            if (trabajadorExistente == null)
            {
                return Json(new { exito = false, mensaje = "Trabajador no encontrado" });
            }

            string? fotoUrl = trabajadorExistente.Foto;

            if (model.FotoArchivo != null && model.FotoArchivo.Length > 0)
            {
                if (!_fileUploadService.ValidateImage(model.FotoArchivo, out string errorMessage))
                {
                    return Json(new { exito = false, mensaje = errorMessage });
                }

                var nuevaFoto = await _fileUploadService.UploadImageAsync(model.FotoArchivo);
                
                if (!string.IsNullOrEmpty(nuevaFoto))
                {
                    await _fileUploadService.DeleteImageAsync(trabajadorExistente.Foto);
                    fotoUrl = nuevaFoto;
                }
            }

            var trabajador = new Trabajador
            {
                IdTrabajador = model.IdTrabajador,
                Nombres = model.Nombres.Trim(),
                Apellidos = model.Apellidos.Trim(),
                TipoDocumento = model.TipoDocumento,
                NumeroDocumento = model.NumeroDocumento.Trim(),
                Sexo = model.Sexo,
                FechaNacimiento = model.FechaNacimiento,
                Foto = fotoUrl,
                Direccion = model.Direccion?.Trim()
            };

            var resultado = await _trabajadorService.ActualizarTrabajador(trabajador);

            if (resultado)
            {
                return Json(new { exito = true, mensaje = "Trabajador actualizado" });
            }

            return Json(new { exito = false, mensaje = "Error al actualizar trabajador" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var trabajador = await _trabajadorService.ObtenerTrabajadorPorId(id);
            
            if (trabajador == null)
            {
                return Json(new { exito = false, mensaje = "Trabajador no encontrado" });
            }

            var resultado = await _trabajadorService.EliminarTrabajador(id);

            if (resultado)
            {
                if (!string.IsNullOrEmpty(trabajador.Foto))
                {
                    await _fileUploadService.DeleteImageAsync(trabajador.Foto);
                }
                return Json(new { exito = true, mensaje = "Trabajador eliminado" });
            }

            return Json(new { exito = false, mensaje = "Error al eliminar trabajador" });
        }
    }
}
