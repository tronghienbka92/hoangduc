using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.VeXeKhach;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Models.NhaXes
{
    public class LichSuDatVeModel : BaseNopEntityModel
    {
        public LichSuDatVeModel()
        {
            ListHanhTrinh = new List<SelectListItem>();
        }
        [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.HanhTrinhId")]
        public int HanhTrinhId { get; set; }
         [UIHint("Date")]
        public DateTime NgayDi { get; set; }
        public string StringSearch { get; set; }
        public IList<SelectListItem> ListHanhTrinh { get; set; }
    }
    public class DatVeNoteItem : BaseNopEntityModel
    {

        public string MaHD { get; set; }
        public string KhachHang { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string Note { get; set; }
    }
   
}