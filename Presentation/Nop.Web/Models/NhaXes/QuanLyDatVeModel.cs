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
    public class QuanLyDatVeModel
    {
        public QuanLyDatVeModel()
        {
            LoaiDonKhachId = 0;
            LoaiDonKhachs = new List<SelectListItem>();
            LoaiDonKhachs.Add(new SelectListItem
            {
                Value = "0",
                Text = "----------------",
                Selected = true
            });
            LoaiDonKhachs.Add(new SelectListItem
            {
                Value = "1",
                Text = "Vé có đón khách"
            });
            LoaiDonKhachs.Add(new SelectListItem
            {
                Value = "2",
                Text = "Vé không đón khách"
            });
        }
        public string ThongTinDatVe { get; set; }
        [UIHint("Date")]
        public DateTime NgayDatVe { get; set; }
        public int HanhTrinhId { get; set; }
        public List<SelectListItem> HanhTrinhs { get; set; }
        public int LichTrinhId { get; set; }
        public List<SelectListItem> LichTrinhs { get; set; }
        public int TrangThaiId { get; set; }
        public ENTrangThaiDatVe trangthai
        {
            get
            {
                return (ENTrangThaiDatVe)TrangThaiId;
            }
        }
        public IList<SelectListItem> TrangThais { get; set; }
        public int LoaiDonKhachId { get; set; }
        public IList<SelectListItem> LoaiDonKhachs { get; set; }
    }
}