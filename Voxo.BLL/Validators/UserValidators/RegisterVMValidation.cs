
namespace Voxo.BLL.Validators.UserValidators
{
    public class RegisterVMValidation : AbstractValidator<RegisterVM>
    {
        public RegisterVMValidation()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Enter email").NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmedPassword).NotEmpty().NotNull()
                .Equal(x => x.Password).WithMessage("Confirm password not match to password");
        }
    }
}
