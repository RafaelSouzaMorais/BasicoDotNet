using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Enums;

namespace Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories
{
    public interface IAvisoRepository : IRepository<AvisoEntity>
    {
        Task<List<AvisoEntity>> ObterTodosAvisosAsync(TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default);
        Task<List<AvisoEntity>> ObterAvisosAtivosAsync(TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default);
        Task<AvisoEntity?> ObterAvisoPorIdAsync(int id, TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default);
        Task<AvisoEntity> AdicionarAvisoAsync(AvisoEntity aviso, CancellationToken cancellationToken = default);
        Task<AvisoEntity> AtualizarAvisoAsync(AvisoEntity aviso, CancellationToken cancellationToken = default);
    }
}