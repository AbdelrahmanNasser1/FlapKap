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
    public class DepositModelValidator : AbstractValidator<DepositModel>
    {
        public DepositModelValidator()
        {

            RuleFor(x => x.Deposit).NotNull().NotEmpty()
                .WithMessage("Note!. User name  is required")
                .GreaterThan(0)
                .WithMessage("Note!. cost shoud be greater than 0 .");
    
        }
    }
    public class BuyModelValidator : AbstractValidator<BuyModel>
    {
        public BuyModelValidator()
        {
            RuleFor(x => x.product_list)
                .NotNull()
                .WithMessage("Note!. You must add at least one product.")
                .NotEmpty()
                .WithMessage("Note!. You must add at least one product.")
                .Must(s => s.Count() > 0)
                .WithMessage("Note!. You must add at least one product.");
                
        }
    }
    public class ProductBuyListValidator : AbstractValidator<ProductBuyList>
    {
        public ProductBuyListValidator()
        {
            RuleFor(x => x.ProductAmount).GreaterThan(0)
                .WithMessage("Note!. Product amount shoud be greater than 0.");
            RuleFor(x => x.ProductId).NotEmpty().NotNull()
                .WithMessage("Note!. Product Id  is required");

        }
    }
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull()
                .WithMessage("Note!. UserName  is required");
            RuleFor(x => x.Password).NotEmpty().NotNull()
                .WithMessage("Note!. Password  is required");
            RuleFor(x => x.Deposit).GreaterThan(0)
                .WithMessage("Note!. Deposit amount shoud be greater than 0");
            RuleFor(x => x.RoleId).GreaterThan(0)
                .WithMessage("Note!. Deposit amount shoud be greater than 0");

        }
    }
}
