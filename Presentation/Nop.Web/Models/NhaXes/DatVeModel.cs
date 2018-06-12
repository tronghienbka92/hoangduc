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
    public class DatVeModel : BaseNopEntityModel
    {
        public string Ma { get; set; }
        public int KhachHangId { get; set; }
        public string TenKhachHang { get; set; }
        public string DienThoai { get; set; }
        [UIHint("Date")]
        public DateTime NgayDi { get; set; }
        public DateTime GioDi { get; set; }
        public int LichTrinhId { get; set; }
        public List<SelectListItem> LichTrinhs { get; set; }
        public string TenLichTrinh { get; set; }
        public int HanhTrinhId { get; set; }
        public string TenHanhTrinh { get; set; }
        public int ChuyenDiId { get; set; }
        public string TenChuyenDi { get; set; }
        public int SoDoGheId { get; set; }
        public string sodoghekyhieu { get; set; }
        public int TrangThaiId { get; set; }
        public ENTrangThaiDatVe trangthai
        {
            get
            {
                return (ENTrangThaiDatVe)TrangThaiId;
            }
            set
            {
                TrangThaiId = (int)value;
            }
        }
        public string TrangThaiText { get; set; }
        public bool isDonTaxi { get; set; }
        public bool isLenhDonTaXi { get; set; }
        public string MaTaXi { get; set; }
        public int DiemDonId { get; set; }
        public string TenDiemDon { get; set; }
        public List<SelectListItem> DiemDons { get; set; }
        public string DiaChiNha { get; set; }
        public string GhiChu { get; set; }
        public int NguoiTaoId { get; set; }
        public String TenNguoiTao { get; set; }
        public DateTime NgayTao { get; set; }
        public Decimal GiaTien { get; set; }
        public bool isEdit { get; set; }
        public bool isThanhToan { get; set; }
        public bool isKhachHuy { get; set; }
        public bool isNoiBai { get; set; }
        public bool isDaXacNhan { get; set; }
        public int VeChuyenDenId { get; set; }
        public string TenDiemTra { get; set; }
        public List<BangGiaVe> BangGiaVes { get; set; }
        public bool isAdmin { get; set; }
    }
    public class DatVeCopyModel
    {
        public int Id { get; set; }
        public string Ma { get; set; }
    }
}