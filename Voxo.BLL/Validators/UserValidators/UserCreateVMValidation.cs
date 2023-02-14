
namespace Voxo.BLL.Validators.UserValidators
{
    public class UserCreateVMValidation : AbstractValidator<UserCreateVM>
    {
        public UserCreateVMValidation()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Enter email").NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull()
                .Equal(x => x.Password).WithMessage("Confirm password not match to password");
        }
    }
}
