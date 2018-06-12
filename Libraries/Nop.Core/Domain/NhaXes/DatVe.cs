using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class DatVe : BaseEntity
    {
        public DatVe()
        {
            NgayTao = DateTime.Now;
        }
        public string SessionID { get; set; }
        public int NhaXeId { get; set; }
        public string Ma { get; set; }
        public int? KhachHangId { get; set; }
        public virtual KhachHang khachhang { get; set; }
        public DateTime NgayDi { get; set; }
        public int LichTrinhId { get; set; }
        public virtual LichTrinh lichtrinh { get; set; }
        public int HanhTrinhId { get; set; }
        public int ThuTuDon { get; set; }
       
        public virtual HanhTrinh hanhtrinh { get; set; }
        public int? ChuyenDiId { get; set; }
        public virtual ChuyenDi chuyendi { get; set; }
        public int? SoDoGheId { get; set; }
        public virtual SoDoGheXeQuyTac sodoghe { get; set; }
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
        public bool isDonTaxi { get; set; }
        public bool isLenhDonTaXi { get; set; }
        public string MaTaXi { get; set; }
        public int? DiemDonId { get; set; }
        public virtual DiemDon diemdon { get; set; }
        public string DiaChiNha { get; set; }
        public string GhiChu { get; set; }
        public int NguoiTaoId { get; set; }
        public int? NguoiChuyenId { get; set; }
        public int? NguoiHuyId { get; set; }
        public virtual NhanVien nguoitao { get; set; }
        public virtual NhanVien nguoichuyen { get; set; }
        public virtual NhanVien nguoihuy { get; set; }
        public DateTime NgayTao { get; set; }
        public Decimal GiaTien { get; set; }
        public bool isThanhToan { get; set; }
        public bool isKhachHuy { get; set; }
        public bool isNoiBai { get; set; }
        public bool isDaXacNhan { get; set; }
        public int VeChuyenDenId { get; set; }
        public string TenKhachHangDiKem { get; set; }
        public string TenDiemDon { get; set; }
        public string TenDiemTra { get; set; }
       
    }
    public class MauChuyenTaxi : BaseEntity
    {
        
        public int? KhachHangId { get; set; }
        public virtual KhachHang khachhang { get; set; }
        public DateTime NgayDi { get; set; }
       
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
        
        public int SoLuongKhach { get; set; }
        
        public string DiaChiNha { get; set; }
        public string DiemTraTaxi { get; set; }

        public string GhiChu { get; set; }
        

    }
}
