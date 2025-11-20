using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations
{
    public class CreateAvisoValidator : AbstractValidator<CreateAvisoRequest>
    {
        private const int TituloMaxLength = 100;
        private const int MensagemMaxLength = 1000;

        public CreateAvisoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título do aviso é obrigatório.")
                .MaximumLength(TituloMaxLength).WithMessage("O título do aviso não pode exceder 100 caracteres.");

            RuleFor(x => x.Mensagem)
                .NotEmpty().WithMessage("A mensagem do aviso é obrigatória.")
                .MaximumLength(MensagemMaxLength).WithMessage("A mensagem do aviso não pode exceder 1000 caracteres.");
        }
    }
}
