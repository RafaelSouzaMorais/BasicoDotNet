using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations
{
    public class UpdateAvisoValidator : AbstractValidator<UpdateAvisoRequest>
    {
        private const int TituloMaxLength = 100;
        private const int MensagemMaxLength = 1000;

        public UpdateAvisoValidator()
        {
            RuleFor(x => x.Mensagem)
                .NotEmpty().WithMessage("A mensagem do aviso é obrigatória.")
                .MaximumLength(MensagemMaxLength).WithMessage("A mensagem do aviso não pode exceder 1000 caracteres.");
        }
    }
}

