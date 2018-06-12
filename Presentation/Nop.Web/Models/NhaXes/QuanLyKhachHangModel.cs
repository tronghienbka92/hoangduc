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
    public class QuanLyKhachHangModel
    {
        public QuanLyKhachHangModel()
        {
            isQuanTri = false;
            LoaiTimKiemId = 0;
            LoaiTimKiems = new List<SelectListItem>();
            LoaiTimKiems.Add(new SelectListItem
            {
                Value = "0",
                Text = "Tìm kiếm chung",
                Selected = true
            });
            LoaiTimKiems.Add(new SelectListItem
            {
                Value = "1",
                Text = "Khách có SL vé đặt nhiều nhất"
            });
            LoaiTimKiems.Add(new SelectListItem
            {
                Value = "2",
                Text = "Khách có SL vé hủy nhiều nhất"
            });
        }
        public bool isQuanTri { get; set; }
        public string ThongTinKhachHang { get; set; }
        public int LoaiTimKiemId { get; set; }
        public IList<SelectListItem> LoaiTimKiems { get; set; }
    }
}