
namespace Voxo.BLL.Validators.UserValidators
{
    public class ResetPasswordVMValidation : AbstractValidator<ResetPasswordVM>
    {
        public ResetPasswordVMValidation()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Enter email").NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull()
                .Equal(x => x.Password).WithMessage("Confirm password not match to password");
        }
    }
}
