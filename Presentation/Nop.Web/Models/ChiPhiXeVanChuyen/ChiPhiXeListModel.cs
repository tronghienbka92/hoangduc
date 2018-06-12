using FluentValidation.Attributes;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.NhaXes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Nop.Web.Models.ChiPhiXeVanChuyen
{
    public class ChiPhiXeListModel
    {
        public List<XeXuatBenItemModel.XeVanChuyenInfo> AllXeInfo { get; set; }
        public int XeVanChuyenListId { get; set; }
        public  IList<SelectListItem> hangmucs { get; set; }
        public int HangMucChiPhiListId { get; set; }
        [UIHint("Date")]
        public DateTime TuNgay { get; set; }
        [UIHint("Date")]
        public DateTime DenNgay { get; set; }
    }
}