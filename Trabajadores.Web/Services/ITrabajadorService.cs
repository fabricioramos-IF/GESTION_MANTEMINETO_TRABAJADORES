using Trabajadores.Web.Models;

namespace Trabajadores.Web.Services
{
    public interface ITrabajadorService
    {
        Task<List<Trabajador>> ListarTrabajadores(string? sexo = null);
        Task<Trabajador?> ObtenerTrabajadorPorId(int id);
        Task<bool> InsertarTrabajador(Trabajador trabajador);
        Task<bool> ActualizarTrabajador(Trabajador trabajador);
        Task<bool> EliminarTrabajador(int id);
    }
}
