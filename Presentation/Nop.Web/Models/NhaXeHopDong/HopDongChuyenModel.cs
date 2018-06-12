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

namespace Nop.Web.Models.NhaXes
{

    public class HopDongChuyenModel : BaseNopEntityModel
    {
        public HopDongChuyenModel()
        {
            KhachHangs = new List<KhachHangChuyenModel>();

        }
        [NopResourceDisplayName("Số hợp đồng")]
        public string SoHopDong { get; set; }
        [NopResourceDisplayName("Tên hợp đồng")]
        public string TenHopDong { get; set; }
          [NopResourceDisplayName("Xe vận chuyển")]
        public string BienSoXe { get; set; }
        [NopResourceDisplayName("Xe vận chuyển")]
        public int XeVanChuyenId { get; set; }
        public virtual XeVanChuyen XeInfo { get; set; }
        [NopResourceDisplayName("Giá trị")]
        public decimal GiaTri { get; set; }       
        public DateTime GioDonKhach { get; set; }
        public DateTime GioTraKhach { get; set; }
        //cac tien ich
           [NopResourceDisplayName("Thời gian đón khách")]
         [UIHint("Date")]       
        public DateTime ThoiGianDonKhach { get; set; }
           [NopResourceDisplayName("Thời gian trả khách")]
         [UIHint("Date")]
       
        public DateTime  ThoiGianTraKhach { get; set; }
           [NopResourceDisplayName("Điểm đón khách")]
        public string DiemDonKhach { get; set; }
           [NopResourceDisplayName("Điểm trả khách")]
        public string DiemTraKhach { get; set; }
           [NopResourceDisplayName("Lộ trình")]
        public string LoTrinh { get; set; }
           [NopResourceDisplayName("Chiều về")]
        public string ChieuVe { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThaiId { get; set; }
        public int NguoiTaoId { get; set; }
        public string NguoiTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public ENTrangThaiHopDongChuyen TrangThai
        {
            get
            {
                return (ENTrangThaiHopDongChuyen)TrangThaiId;
            }
            set
            {
                TrangThaiId = (int)value;
            }
        }


        public int NhaXeId { get; set; }
        public List<KhachHangChuyenModel> KhachHangs { get; set; }
        public string KmXuat { get; set; }
        public int LaiXeId { get; set; }
        public string TenLaiXe { get; set; }
        public int HinhThucThanhToanId { get; set; }
        public ENHinhThucThanhToan HinhThucThanhToan
        {
            get
            {
                return (ENHinhThucThanhToan)HinhThucThanhToanId;
            }
            set
            {
                HinhThucThanhToanId = (int)value;
            }
        }
        public string GhiChu { get; set; }

        public IList<SelectListItem> HTThanhToans { get; set; }
    }
    public class KhachHangChuyenModel : BaseNopEntityModel
    {
           [NopResourceDisplayName("Họ và tên")]
        public string TenKhachHang { get; set; }
           [NopResourceDisplayName("Số điện thoại")]
        public string SoDienThoai { get; set; }
           [NopResourceDisplayName("Năm sinh")]
        public int NamSinh { get; set; }
        public int HopDongChuyenId { get; set; }

          [NopResourceDisplayName("Người đại diện")]
        public bool isDaiDien { get; set; }
        //cac tien ich
           [NopResourceDisplayName("Ghi chú")]
        public string GhiChu { get; set; }

        public int NhaXeId { get; set; }
    }
}