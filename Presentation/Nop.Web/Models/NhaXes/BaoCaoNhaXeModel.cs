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
    public class BaoCaoNhaXeModel : BaseNopEntityModel
    {
        public BaoCaoNhaXeModel()
        {
            ListLoai1 = new List<SelectListItem>();
            ListLoai2 = new List<SelectListItem>();
            ListLoai3 = new List<SelectListItem>();
            ListQuy = new List<SelectListItem>();
            ListMonth = new List<SelectListItem>();
            ListYear = new List<SelectListItem>();
            VanPhongs = new List<SelectListItem>();
            Xe = new List<SelectListItem>();
        }
        public int Loai1Id { get; set; }
        public int Loai2Id { get; set; }
        public string Loai2Ids { get; set; }
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.Quy")]
        public int QuyId { get; set; }
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.Thang")]
        public int ThangId { get; set; }
        public int HanhTrinhId { get; set; }

        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.Nam")]
        public int NamId { get; set; }
        [UIHint("DateNullable")]
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.TuNgay")]
        public DateTime TuNgay { get; set; }
        [UIHint("DateNullable")]
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.DenNgay")]
        public DateTime DenNgay { get; set; }

        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.ChonVe")]
        public bool isChonVe { get; set; }

        public string GioBan { get; set; }
        public string NgayBan { get; set; }
        public string SearchName { get; set; }
        public int VanPhongId { get; set; }
        public int XeId { get; set; }
        public IList<SelectListItem> VanPhongs { get; set; }
        public IList<SelectListItem> Xe { get; set; }
        public IList<SelectListItem> ListLoai1 { get; set; }
        public IList<SelectListItem> ListLoai2 { get; set; }
        public IList<SelectListItem> ListLoai3 { get; set; }
        public IList<SelectListItem> ListQuy { get; set; }
        public IList<SelectListItem> ListMonth { get; set; }
        public IList<SelectListItem> ListYear { get; set; }
        public class BaoCaoDoanhThuModel : ThongKeItem
        {
            public string ThoiGian { get; set; }
            public string ThoiGianOrder
            {
                get
                {
                    return ThoiGian.PadLeft(5,'0');
                }
            }
            public decimal TongDoanhThu { get; set; }
            public decimal DoanhThuChonVe { get; set; }
            public decimal DoanhThuNhaXe { get; set; }

        }
        public class BaoCaoDoanhThuNhanVienModel : ThongKeItem
        {
            public int NhanVienId { get; set; }
            public int NguonVeId { get; set; }
            public string TenNhanVien { get; set; }
            public string TrangThaiPhoiVeText { get; set; }
            public decimal TongDoanhThu { get; set; }
            public decimal DoanhThuChuaThanhToan { get; set; }
            public decimal DoanhThuChonVe { get; set; }
            public decimal DoanhThuNhaXe { get; set; }
            public string NgayBan { get; set; }

        }
        public class BaoCaoDoanhThuXeTungNgayModel : DoanhThuTheoXeItem
        {

            public string BienSo { get; set; }
            public string TrangThaiPhoiVeText { get; set; }
            public decimal TongDoanhThu { get; set; }
            public decimal DoanhThuXe { get; set; }
            public string NgayBan { get; set; }
            public string NgayDen { get; set; }

        }
        public class BaoCaoDetailDoanhThuKiGuiModel : ThongKeItem
        {
            public string NotPay { get; set; }
            public int NhanVienId { get; set; }
            public string NgayBan { get; set; }
        }
        public class KhachHangMuaVeModel
        {
            public int CustomerId { get; set; }
            public int NguonVeXeId { get; set; }
            public string TenKhachHang { get; set; }
            public string SoDienThoai { get; set; }
            public string ThongTinChuyenDi { get; set; }
            public int TrangThaiPhoiVeId { get; set; }
            public string TrangThaiPhoiVeText { get; set; }
            public string KyHieuGhe { get; set; }
            public bool isChonVe { get; set; }
            public decimal GiaVe { get; set; }
            public DateTime NgayDi { get; set; }
        }

    }
}