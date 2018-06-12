using FluentValidation.Attributes;
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
    public class ChuyenDiModel : BaseNopEntityModel
    {
        public string Ma { get; set; }
        public DateTime NgayDi { get; set; }
        public DateTime NgayDiThuc { get; set; }
        public int LichTrinhId { get; set; }
        public string TenLichTrinh { get; set; }
        public List<SelectListItem> LichTrinhs { get; set; }
        public int? LaiXeId { get; set; }
        public String TenLaiXe { get; set; }
        public decimal TienChuaThanhToan { get; set; }
        public String TenLaiXeRutGon { get; set; }
        public int? XeVanChuyenId { get; set; }
        public string BienSoXe { get; set; }
        public string BienSoXe3So { get; set; }
        public int HanhTrinhId { get; set; }
        public string TenHanhTrinh { get; set; }
        public DateTime NgayTao { get; set; }
        public int NguoiTaoId { get; set; }
        public String TenNguoiTao { get; set; }
        public int TrangThaiId { get; set; }
        public ENTrangThaiXeXuatBen trangthai
        {
            get
            {
                return (ENTrangThaiXeXuatBen)TrangThaiId;
            }
            set
            {
                TrangThaiId = (int)value;
            }
        }
        public string TrangThaiText { get; set; }
        public string GhiChu { get; set; }
        public bool isEdit { get; set; }
        public int SoKhach { get; set; }
        public int SoGhe { get; set; }
        public string TenLoaiXe { get; set; }
        public int LichTrinhLoaiXeId { get; set; }
        public int LoaiXeId { get; set; }
        public List<SelectListItem> LoaiXes { get; set; }
        public decimal GiaVe { get; set; }
    }
}