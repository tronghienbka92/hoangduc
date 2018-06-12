using FluentValidation.Attributes;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.NhaXes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Models.ChiPhiXeVanChuyen
{
    public class ChiPhiXeModel : BaseNopEntityModel
    {
        public int NhaXeId { get; set; }
        public string TenHangMuc { get; set; }
        public IList<SelectListItem> hangmucs { get; set; }
        public int HangMucChiPhiId { get; set; }
        public DateTime NgayTao { get; set; }
        public string TenNguoiTao { get; set; }
        public int NguoiTaoId { get; set; }
        public string BienSo { get; set; }
        public string TenLaiXe { get; set; }
        public int XeVanChuyenId { get; set; }
        public string TenCongViec { get; set; }
        public Decimal ChiPhi { get; set; }
        public string ThoiGian { get; set; }
        [UIHint("Date")]
        public DateTime NgayGiaoDich { get; set; }
        public string GhiChu { get; set; }
        public string Ma { get; set; }
    }
}