using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Partidos
{
    public interface IPartidoCommand
    {
        Task<int> CrearPartido(Partido partido, CancellationToken ct = default);
        Task ModificarPartido(Partido partido, CancellationToken ct = default);
        Task EliminarPartido(Partido partido, CancellationToken ct = default);
        Task<IEnumerable<Partido>> AgregarPartidos(List<Partido> fixture, CancellationToken ct = default);
    }
}