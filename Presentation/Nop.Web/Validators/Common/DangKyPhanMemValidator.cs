using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Common;

namespace Nop.Web.Validators.Common
{
    public class DangKyPhanMemValidator : BaseNopValidator<DangKyPhanMemModel>
    {
        public DangKyPhanMemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SoDienThoai).NotEmpty().WithMessage("Bạn chưa nhập thông tin điện thoại liên hệ");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Bạn chưa nhập thông tin email");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email không đúng");
            RuleFor(x => x.Ten).NotEmpty().WithMessage("Bạn chưa nhập thông tin nhà xe");
            RuleFor(x => x.GhiChu).NotEmpty().WithMessage("Bạn chưa nhập thông tin phản hồi");
        }}
}