using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;

namespace Bernhoeft.GRT.Teste.Application.Responses.Commands.v1
{
    public class CreateAvisoResponse
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }

        public static implicit operator CreateAvisoResponse(AvisoEntity entity) => new()
        {
            Id = entity.Id,
            Ativo = entity.Ativo,
            Titulo = entity.Titulo,
            Mensagem = entity.Mensagem,
            DataCriacao = entity.DataCriacao,
            DataEdicao = entity.DataEdicao
        };
    }
}
