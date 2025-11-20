using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations
{
    public class UpdateAvisoValidator : AbstractValidator<UpdateAvisoRequest>
    {
        private const int TituloMaxLength = 100;
        private const int MensagemMaxLength = 1000;

        public UpdateAvisoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("O ID do aviso é obrigatório e deve ser maior que zero.");

            RuleFor(x => x.Mensagem)
                .NotEmpty().WithMessage("A mensagem do aviso é obrigatória.")
                .MaximumLength(MensagemMaxLength).WithMessage("A mensagem do aviso não pode exceder 1000 caracteres.");
        }
    }
}

