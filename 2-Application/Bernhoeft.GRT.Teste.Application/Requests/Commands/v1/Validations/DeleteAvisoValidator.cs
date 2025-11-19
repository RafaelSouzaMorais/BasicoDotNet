using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations
{
    public class DeleteAvisoValidator : AbstractValidator<DeleteAvisoRequest>
    {
        private const int TituloMaxLength = 100;
        private const int MensagemMaxLength = 1000;

        public DeleteAvisoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("O ID do aviso é obrigatório e deve ser maior que zero.");
        }
    }
}

