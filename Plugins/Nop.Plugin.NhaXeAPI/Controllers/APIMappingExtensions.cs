using Nop.Core.Domain.NhaXes;
using Nop.Plugin.NhaXeAPI.Models;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.NhaXes;

namespace Nop.Plugin.NhaXeAPI.Controllers
{
    public static class APIMappingExtensions
    {
        public static string ToCVEnumText<T>(this T enumValue, ILocalizationService localizationService) where T : struct
        {
            if (localizationService == null)
                throw new ArgumentNullException("localizationService");
            //localized value
            string resourceName = string.Format("Enums.{0}.{1}",
                typeof(T).ToString(),
                enumValue.ToString());
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            return localizationService.GetResource(resourceName);


        }
        public static DatVeModel toModel(this DatVe e, ILocalizationService localizationService)
        {
            var m = new DatVeModel();
            m.Id = e.Id;
            m.Ma = e.Ma;
            m.KhachHangId = e.KhachHangId.GetValueOrDefault(0);
            if (e.khachhang != null)
            {
                m.TenKhachHang = e.khachhang.Ten;
                m.DienThoai = e.khachhang.DienThoai;
            }
            if (!String.IsNullOrEmpty(e.TenKhachHangDiKem))
            {
                m.TenKhachHang = e.TenKhachHangDiKem;
                m.KhachHangId = -1;//khong upd khach hang
            }
            m.NgayDi = e.NgayDi;
            m.LichTrinhId = e.LichTrinhId;
            m.TenLichTrinh = e.chuyendi!=null?e.chuyendi.NgayDiThuc.ToString("HH:mm"):"";
            m.HanhTrinhId = e.HanhTrinhId;
            m.TenHanhTrinh = e.hanhtrinh!=null ? e.hanhtrinh.toText():"";
            m.ChuyenDiId = e.ChuyenDiId.GetValueOrDefault(0);
            m.TenChuyenDi = e.chuyendi != null ? e.chuyendi.toText() : "";
               
            m.SoDoGheId = e.SoDoGheId.GetValueOrDefault(0);
            m.sodoghekyhieu = e.sodoghe != null ? e.sodoghe.Val:"";
            m.trangthai = e.trangthai;
            m.TrangThaiText = e.trangthai.ToCVEnumText(localizationService);
            m.isDonTaxi = e.isDonTaxi;
            m.isLenhDonTaXi = e.isLenhDonTaXi;
            m.MaTaXi = e.MaTaXi;
            m.DiemDonId = e.DiemDonId.GetValueOrDefault(0);
            //if(e.diemdon!=null)
            m.TenDiemDon = e.TenDiemDon;
            m.TenDiemTra = e.TenDiemTra;
            m.DiaChiNha = e.DiaChiNha;
            m.GhiChu = e.GhiChu;
            m.NguoiTaoId = e.NguoiTaoId;
            m.TenNguoiTao = e.nguoitao!=null ? e.nguoitao.HoVaTen : "";
            m.NgayTao = e.NgayTao;
            m.GiaTien = e.GiaTien;
            m.isThanhToan = e.isThanhToan;
            m.isNoiBai = e.isNoiBai;
            m.isKhachHuy = e.isKhachHuy;
            m.isDaXacNhan = e.isDaXacNhan;
            m.VeChuyenDenId = e.VeChuyenDenId;
            m.GioDi = e.chuyendi.NgayDiThuc;

            //trang thai nay luc ke toan cap nhat doanh thu se chuyen sang trang thai da di hay Huy
            m.isEdit = true;
            if (e.trangthai == ENTrangThaiDatVe.DA_DI)
                m.isEdit = false;
            return m;
        }
        public static ChuyenDiModel toModel(this ChuyenDi e, ILocalizationService localizationService)
        {
            var m = new ChuyenDiModel();
            m.Id = e.Id;
            m.Ma = e.Ma;
            m.LaiXeId = e.LaiXeId;
            m.TenLaiXeRutGon = "";
            if (e.laixe != null)
            {
                m.TenLaiXe = e.laixe.ThongTin();
                m.TenLaiXeRutGon = e.laixe.HoVaTen;
            }
            else
                m.TenLaiXe = "";
            //chi lay ten
            if (!string.IsNullOrEmpty(m.TenLaiXeRutGon))
            {
                string[] arrten = m.TenLaiXeRutGon.Split(' ');
                m.TenLaiXeRutGon = arrten[arrten.Length - 1].Trim();
            }
            else
                m.TenLaiXeRutGon = "---";
            m.NgayDi = e.NgayDi;
            m.NgayDiThuc = e.NgayDiThuc;

            m.LichTrinhId = e.LichTrinhId;
            m.TenLichTrinh = e.lichtrinh.toText(false);
            m.HanhTrinhId = e.HanhTrinhId;
            m.TenHanhTrinh = e.hanhtrinh.MaHanhTrinh;
            m.XeVanChuyenId = e.XeVanChuyenId;
            if (e.xevanchuyen != null)
            {
                m.BienSoXe = e.xevanchuyen.BienSo;
            }
            else
            {
                m.BienSoXe = "---------";
            }
            if (m.BienSoXe.Length >= 4)
                m.BienSoXe3So = m.BienSoXe.Substring(m.BienSoXe.Length - 4);
            else
                m.BienSoXe3So = m.BienSoXe;
            m.NgayTao = e.NgayTao;
            m.NguoiTaoId = e.NguoiTaoId;
            m.TenNguoiTao = e.nguoitao.HoVaTen;
            m.trangthai = e.trangthai;
            m.TrangThaiText = e.trangthai.ToCVEnumText(localizationService);
            m.GhiChu = e.GhiChu;
            m.SoKhach = e.DatVeHopLes().Count();
            m.SoGhe = e.lichtrinhloaixe.loaixe.sodoghe.SoLuongGhe;
            m.TenLoaiXe = e.lichtrinhloaixe.loaixe.TenLoaiXe;
            m.GiaVe = e.lichtrinhloaixe.GiaVe;
            m.isEdit = true;
            if (e.trangthai == ENTrangThaiXeXuatBen.KET_THUC)
                m.isEdit = false;

            return m;
        }
    }
}
