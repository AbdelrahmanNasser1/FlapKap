using FlapKap.Response;
using FlapKap.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Validators
{
    public class UpdateProductValidator :AbstractValidator<UpdateProductModel>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.AvaliableAmount).NotNull().NotEmpty()
                .WithMessage("Note!. number of Product available is required")
                .GreaterThan(0)
                .WithMessage("Note!. number of team members must be greater than 0");

            RuleFor(x => x.Cost).NotNull().NotEmpty()
                .GreaterThanOrEqualTo(5)
                .WithMessage("Note!. cost should equal to 5 or 10 or 20 or 50 or 100.");

            RuleFor(x => x.ProductName).NotNull()
                .WithMessage("Note!. Product name  is required");
        }
    }
    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {

            RuleFor(x => x.UserName).NotNull().NotEmpty()
                .WithMessage("Note!. User name  is required");
            RuleFor(x => x.Password).NotNull()
                .WithMessage("Note!. Password  is required")
                .MinimumLength(8)
                .WithMessage("Note!. Password length shouldn't less than 8 characters");
        }
    }
}
