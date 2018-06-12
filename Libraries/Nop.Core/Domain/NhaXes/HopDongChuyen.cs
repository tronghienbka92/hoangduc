using System;
using System.Collections.Generic;


namespace Nop.Core.Domain.NhaXes
{
    public class HopDongChuyen:BaseEntity
    {
        public string SoHopDong { get; set; }
        public string TenHopDong  { get; set; }
        public int XeVanChuyenId { get; set; }
        public virtual XeVanChuyen XeInfo { get; set; }
      
        public decimal GiaTri { get; set; }
        //cac tien ich
        public DateTime? ThoiGianDonKhach { get; set; }
        public DateTime? ThoiGianTraKhach { get; set; }
        public string DiemDonKhach { get; set; }
        public string DiemTraKhach { get; set; }
        public string LoTrinh { get; set; }
        public string ChieuVe { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThaiId { get; set; }
        public int NguoiTaoId { get; set; }
        public virtual NhanVien NguoiTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
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
        public string KmXuat { get; set; }
        public int? LaiXeId { get; set; }
        public virtual NhanVien laixe { get; set; }
        public string GhiChu { get; set; }

        public int? HinhThucThanhToanId { get; set; }
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

    }
}
