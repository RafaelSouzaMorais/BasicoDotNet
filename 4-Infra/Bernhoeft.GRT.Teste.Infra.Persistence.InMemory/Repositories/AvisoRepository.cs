using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.Attributes;
using Bernhoeft.GRT.Core.EntityFramework.Infra;
using Bernhoeft.GRT.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bernhoeft.GRT.ContractWeb.Infra.Persistence.SqlServer.ContractStore.Repositories
{
    [InjectService(Interface: typeof(IAvisoRepository))]
    public class AvisoRepository : Repository<AvisoEntity>, IAvisoRepository
    {
        public AvisoRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<AvisoEntity> AdicionarAvisoAsync(AvisoEntity aviso, CancellationToken cancellationToken = default)
        {
            var entry = await Set.AddAsync(aviso, cancellationToken);
            return entry.Entity;
        }

        public async Task<AvisoEntity> AtualizarAvisoAsync(AvisoEntity aviso, CancellationToken cancellationToken = default)
        {
            Set.Update(aviso);
            return await Task.FromResult(aviso);
        }

        public async Task<AvisoEntity?> ObterAvisoPorIdAsync(int id, TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default)
        {
            //verificar está ativo
            return await (tracking is TrackingBehavior.NoTracking ? Set.AsNoTrackingWithIdentityResolution() : Set)
                .Where(a => a.Ativo == true)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<AvisoEntity>> ObterAvisosAtivosAsync(TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default)
        {
            return await (tracking is TrackingBehavior.NoTracking ? Set.AsNoTrackingWithIdentityResolution() : Set)
                .Where(a => a.Ativo)
                .ToListAsync(cancellationToken);
        }

        public Task<List<AvisoEntity>> ObterTodosAvisosAsync(TrackingBehavior tracking = TrackingBehavior.Default, CancellationToken cancellationToken = default)
        {
            var query = tracking is TrackingBehavior.NoTracking ? Set.AsNoTrackingWithIdentityResolution() : Set;
            return query.ToListAsync();
        }
    }
}