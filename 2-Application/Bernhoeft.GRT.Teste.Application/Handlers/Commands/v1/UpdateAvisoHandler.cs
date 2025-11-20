using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Commands.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Commands.v1
{
    public class UpdateAvisoHandler : IRequestHandler<UpdateAvisoRequest, IOperationResult<UpdateAvisoResponse>>
    {
        private readonly IServiceProvider _serviceProvider;
        private IContext _context => _serviceProvider.GetRequiredService<IContext>();
        private IAvisoRepository _avisoRepository => _serviceProvider.GetRequiredService<IAvisoRepository>();
        public UpdateAvisoHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IOperationResult<UpdateAvisoResponse>> Handle(UpdateAvisoRequest request, CancellationToken cancellationToken)
        {
            var existingAviso = await _avisoRepository.ObterAvisoPorIdAsync(request.Id, TrackingBehavior.Default, cancellationToken);

            if(existingAviso is null
                || !existingAviso.Ativo)
            {
                return OperationResult<UpdateAvisoResponse>.ReturnNotFound();
            }

            existingAviso.Mensagem = request.Mensagem;
            existingAviso.DataEdicao = DateTime.UtcNow;

            var response = await _avisoRepository.AtualizarAvisoAsync(existingAviso, cancellationToken);
            if(response == null) 
            {
                return OperationResult<UpdateAvisoResponse>.ReturnBadRequest();
            }

            if (await _context.SaveChangesAsync(cancellationToken) <= 0)
            {
                return OperationResult<UpdateAvisoResponse>.ReturnBadRequest();
            }

            return OperationResult<UpdateAvisoResponse>.Return(CustomHttpStatusCode.Ok, (UpdateAvisoResponse)response);
        }
    }
}
