using Nop.Core.Domain.Chonves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.NhaXes
{
    public class ThongKeItem 
    {
        public string Nhan { get; set; }
        public string NhanSapXep { get; set; }
        public int TrangThaiPhoiVeId { get; set; }
        public ENTrangThaiPhoiVe TrangThai
        {
            get
            {
                return (ENTrangThaiPhoiVe)TrangThaiPhoiVeId;
            }
            set
            {
                TrangThaiPhoiVeId = (int)value;
            }
        }
        public Decimal GiaTri { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDat { get; set; }
        public int SoLuongChuyen { get; set; }
        public int SoLuongHuy { get; set; }
        public int NguoiTaoId { get; set; }
        public Decimal GiaTri1 { get; set; }
        public Decimal GiaTri2 { get; set; }
        public int ItemId { get; set; }
        public DateTime ItemDataDate { get; set; }
        public int ItemDataYear { get; set; }
        public int ItemDataMonth { get; set; }
        public int ItemDataDay { get; set; }
    }
    
    public class DoanhThuItem
    {
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public Decimal DoanhThu { get; set; }
       
    }
    public class DoanhThuTheoXeItem
    {
        public int NguonVeXeId { get; set; }
        public string ThongTinChuyenDi { get; set; }
        public int XeId { get; set; }
        public string Nhan { get; set; }
        public string NhanSapXep { get; set; }
        public Decimal GiaTri { get; set; }
        public int SoLuong { get; set; }
        public string KyHieuGhe { get; set; }
        public int TrangThaiPhoiVeId { get; set; }
        public ENTrangThaiPhoiVe TrangThai
        {
            get
            {
                return (ENTrangThaiPhoiVe)TrangThaiPhoiVeId;
            }
            set
            {
                TrangThaiPhoiVeId = (int)value;
            }
        }
        public Decimal GiaTri1 { get; set; }
        public Decimal GiaTri2 { get; set; }
        public Nop.Core.Domain.Chonves.NguonVeXe NguonVeXe { get; set; }
        public int ItemId { get; set; }
        public DateTime ItemDataDate { get; set; }
        public int ItemDataYear { get; set; }
        public int ItemDataMonth { get; set; }
        public int ItemDataDay { get; set; }
        public decimal GiaVe { get; set; }
    }
    public class KhachHangMuaVeItem
    {
        public int CustomerId { get; set; }
        public Customers.Customer customer { get; set; }
        public Nop.Core.Domain.Chonves.NguonVeXe nguonve { get; set; }
        public int NguonVeXeId { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string ThongTinChuyenDi { get; set; }
        public int TrangThaiPhoiVeId { get; set; }
        public ENTrangThaiPhoiVe TrangThai
        {
            get
            {
                return (ENTrangThaiPhoiVe)TrangThaiPhoiVeId;
            }
            set
            {
                TrangThaiPhoiVeId = (int)value;
            }
        }
        public string KyHieuGhe { get; set; }
        public bool isChonVe { get; set; }
        public decimal GiaVe { get; set; }
        public DateTime NgayDi { get; set; }
    }
    public class ThongKeLuotXuatBenItem
    {
        public int[] NhanVienIds { get; set; }
        public int LaiXeId { get; set; }
        public int SoLuot { get; set; }
        public int Ngay { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }


    }
    public class DoanhThuNhanVienTheoXe
    {
        public string BienSoXe { get; set; }
        public string TenLaiXe { get; set; }
        public int TongLuot { get; set; }
        public int SoKhach { get; set; }
        public decimal ThanhTien { get; set; }
        public string TienFomat { get; set; }


    }
    public class DoanhThuTheoNgay
    {
        public string NgayBan { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDat { get; set; }
        public int SoLuongChuyen { get; set; }
        public int SoLuongHuy { get; set; }
        public decimal ThanhTien { get; set; }
    }
    public class ChuyenDiGroup
    {
        
        public string BienSoXe { get; set; }
        public string TenLaiXe { get; set; }
        public int XeVanChuyenId { get; set; }
        public virtual XeVanChuyen XeVanChuyen { get; set; }
        public int LaiXeId { get; set; }
        public virtual NhanVien LaiXe { get; set; }
        public List<ChuyenDiItem> ChuyenDis { get; set; }
        public int TongLuot { get; set; }
    }
    public class ChuyenDiItem
    {

       
        public int xeid { get; set; }
        public virtual XeVanChuyen xevanchuyen { get; set; }
        public int laixeid { get; set; }
        public virtual NhanVien laixe { get; set; }

        public int idchuyen { get; set; }
    }
    public class DoanhThuChiTietNgay
    {
        public string BienSoXe { get; set; }
        public string BatDau { get; set; }
        public string KetThuc { get; set; }
        public int SoLuot { get; set; }
        public int SoKhach { get; set; }
        public decimal SoTien { get; set; }
        public string TienChu { get; set; }
        public string DiemDau { get; set; }
        public string DiemCuoi { get; set; }
        public string TenLai { get; set; }


    }
     public class ListDoanhThuTongHop
     {
         public DateTime NgayDi { get; set; }
         public decimal DTHopDongChuyen { get; set; }
         public DoanhThuTongHop DT_HN_ND { get; set; }

     }
    public class DoanhThuTongHop
    {
      

      
        public int TongLuot { get; set; }
        public int SoKhach { get; set; }
        public decimal ThanhTien { get; set; }
        public string TienFomat { get; set; }


    }
    public class XeDiTrongNgay
    {

        public int XeId { get; set; }
        public string BienSoXe { get; set; }
        public List<DoanhThuChiTietNgay> doanhthus { get; set; }
        public int TongLuot { get; set; }
        public int TongKhach { get; set; }
        public decimal TongTien { get; set; }
        public string TenLaiXe { get; set; }

    }
    public class KhachHangThongKe 
    {
        public int Id { get; set; }
        public string DienThoai { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string ThongTin { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuongDat { get; set; }
        public int SoLuongHuy { get; set; }
        //lay thong tin diem don lan dat cuoi
        public string TenDiemDon { get; set; }
        public string TenDiemTra { get; set; }
        public string ChuyenDiTrongNgay { get; set; }
    }
}
