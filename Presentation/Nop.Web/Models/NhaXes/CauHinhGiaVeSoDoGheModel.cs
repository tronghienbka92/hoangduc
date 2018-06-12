using FluentValidation.Attributes;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Nop.Web.Models.NhaXes
{
    public class CauHinhGiaVeSoDoGheModel
    {
        public int HanhTrinhId { get; set; }
        public List<SelectListItem> HanhTrinhs { get; set; }
        public int LoaiXeId { get; set; }
        public List<SelectListItem> LoaiXes { get; set; }
        public List<SoDoGheGiaVe> SoDoGheGiaVes { get; set; }
        public class SoDoGheGiaVe
        {
            public int SoDoGheId { get; set; }
            public string SoGhe { get; set; }
            public decimal GiaVe { get; set; }
        }

    }
}