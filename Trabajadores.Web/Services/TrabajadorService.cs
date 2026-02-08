using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Trabajadores.Web.Data;
using Trabajadores.Web.Models;

namespace Trabajadores.Web.Services
{
    public class TrabajadorService : ITrabajadorService
    {
        private readonly ApplicationDbContext _context;

        public TrabajadorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trabajador>> ListarTrabajadores(string? sexo = null)
        {
            var sexoParam = new SqlParameter("@Sexo", SqlDbType.Char, 1)
            {
                Value = string.IsNullOrEmpty(sexo) ? DBNull.Value : sexo
            };

            var trabajadores = await _context.Trabajadores
                .FromSqlRaw("EXEC sp_listar_trabajadores @Sexo", sexoParam)
                .ToListAsync();

            return trabajadores;
        }

        public async Task<Trabajador?> ObtenerTrabajadorPorId(int id)
        {
            return await _context.Trabajadores
                .FirstOrDefaultAsync(t => t.IdTrabajador == id);
        }

        public async Task<bool> InsertarTrabajador(Trabajador trabajador)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Nombres", trabajador.Nombres),
                    new SqlParameter("@Apellidos", trabajador.Apellidos),
                    new SqlParameter("@TipoDocumento", trabajador.TipoDocumento),
                    new SqlParameter("@NumeroDocumento", trabajador.NumeroDocumento),
                    new SqlParameter("@Sexo", trabajador.Sexo),
                    new SqlParameter("@FechaNacimiento", trabajador.FechaNacimiento),
                    new SqlParameter("@Foto", (object?)trabajador.Foto ?? DBNull.Value),
                    new SqlParameter("@Direccion", (object?)trabajador.Direccion ?? DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_insertar_trabajador @Nombres, @Apellidos, @TipoDocumento, @NumeroDocumento, @Sexo, @FechaNacimiento, @Foto, @Direccion",
                    parameters);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarTrabajador(Trabajador trabajador)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@IdTrabajador", trabajador.IdTrabajador),
                    new SqlParameter("@Nombres", trabajador.Nombres),
                    new SqlParameter("@Apellidos", trabajador.Apellidos),
                    new SqlParameter("@TipoDocumento", trabajador.TipoDocumento),
                    new SqlParameter("@NumeroDocumento", trabajador.NumeroDocumento),
                    new SqlParameter("@Sexo", trabajador.Sexo),
                    new SqlParameter("@FechaNacimiento", trabajador.FechaNacimiento),
                    new SqlParameter("@Foto", (object?)trabajador.Foto ?? DBNull.Value),
                    new SqlParameter("@Direccion", (object?)trabajador.Direccion ?? DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_actualizar_trabajador @IdTrabajador, @Nombres, @Apellidos, @TipoDocumento, @NumeroDocumento, @Sexo, @FechaNacimiento, @Foto, @Direccion",
                    parameters);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarTrabajador(int id)
        {
            try
            {
                var idParam = new SqlParameter("@IdTrabajador", id);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_eliminar_trabajador @IdTrabajador",
                    idParam);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
