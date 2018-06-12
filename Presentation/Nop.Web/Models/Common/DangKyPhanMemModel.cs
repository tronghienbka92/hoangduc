using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.NhaXes;
using Nop.Web.Models.VeXeKhach;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Validators.Common;


namespace Nop.Web.Models.Common
{
    [Validator(typeof(DangKyPhanMemValidator))]
    public class DangKyPhanMemModel : BaseNopEntityModel
    {
        public string Ten { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string GhiChu { get; set; }
    }
}