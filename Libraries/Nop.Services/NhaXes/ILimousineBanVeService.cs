using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.NhaXes;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.Directory;
using System;
using System.Web.Mvc;


namespace Nop.Services.NhaXes
{
    public partial interface ILimousineBanVeService
    {
        #region "Hanh khach"
        KhachHang InsertKhachHang(KhachHang item);
        void UpdateKhachHang(KhachHang item);
        void DeleteKhachHang(KhachHang item);
        KhachHang GetKhachHangById(int itemId);
        KhachHang GetKhachHangByDienThoai(int NhaXeId, string DienThoai);
        List<KhachHang> GetAllHanhKhach(int NhaXeId, string ThongTin,int NumRow=100);
        #endregion
        #region "Dat ve"
        void InsertDatVe(DatVe item);
        void UpdateDatVe(DatVe item);
        void DeleteDatVe(DatVe item);
        DatVe GetDatVeById(int itemId);
        List<DatVe> GetAllDatVe(int NhaXeId,DateTime NgayDi,int HanhTrinhId=0,int LichTrinhId=0,ENTrangThaiDatVe TrangThai=ENTrangThaiDatVe.ALL,string ThongTinDatVe="");
        List<DatVe> GetDatVeBySession(int NhaXeId,int ChuyenDiId, string SessionId);
        List<DatVe> GetDatVeByChuyenDi(int ChuyenDiId);
        int GetSoLuongDatVeTheoKhachHang(int KhachHangId, out int SoLuongHuy);
        List<ThongKeItem> GetHanhKhachTheoSoLuongDatVe(int NhaXeId, string ThongTin, int LoaiId);
        DatVe GetDatVeCuoiTheoKhachHangId(int KhachHangId,int HanhTrinhId);
        #endregion
        #region "Chuyen Di"
        void InsertChuyenDi(ChuyenDi item);
        void UpdateChuyenDi(ChuyenDi item);
        void DeleteChuyenDi(ChuyenDi item);
        ChuyenDi GetChuyenDiById(int itemId);
        List<ChuyenDi> GetAllChuyenDi(int NhaXeId, int HanhTrinhId = 0, int LichTrinhId = 0, ENKhungGio KhungGioId = ENKhungGio.All, DateTime? NgayDi = null, string ThongTin = "", string ThongTinKhachHang="");
        List<ChuyenDi> GetAllChuyenDi(int NhaXeId, int HanhTrinhId, DateTime NgayDi);
        List<ChuyenDi> GetAllChuyenDiTheoXe(int XeVanChuyenId);
        List<ChuyenDi> GetAllChuyenDiTheoKhachHang(int KhachHangId,DateTime NgayDi);
        int GetSTTChuyenDi(int NhaXeId, DateTime NgayDi);
        #endregion
        #region Bao Cao
        void GetLimousineIndex(int NhaXeId, DateTime ngaydi, ENBaoCaoChuKyThoiGian loaithongke, out int SoLuongDatVe, out Decimal DoanhThu, out int SoLuongChuyen);
        List<ThongKeItem> GetDoanhThuBanVeTheoNgay(DateTime tuNgay, DateTime denNgay, int nhaxeid, int VanPhongId);
        List<ThongKeItem> GetDoanhThuBanVeTheoNhanVien(int nhaxeid, int VanPhongId, DateTime NgayBan);
        List<KhachHangMuaVeItem> GetDetailDoanhThu(int nhaxeid, DateTime ngaydi, int nhanvienid = 0);
        List<ThongKeItem> GetBaoCaoDoanhThu(int thang, int nam, int nhaxeid, ENBaoCaoChuKyThoiGian ChuKyThoiGianId,string HanhTrinhIds="");
        decimal DoanhThuTuyen(int HanhTrinhId, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId,string GioBan,string NgayBan, out int SoLuong);
        decimal DoanhThuLichTrinh(int lichtrinhid,int HanhTrinhId, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId,string NgayBan, out int SoLuong);
        List<DoanhThuNhanVienTheoXe> GetAllChuyenTheoNgay(DateTime TuNgay, DateTime DenNgay, string SerchName, int NhaXeId);
        List<DatVe> GetAllDatVeByKhachHang(DateTime TuNgay, DateTime DenNgay, int KhachHangId);
        List<XeDiTrongNgay> GetAllChuyenTheoXeTrongNgay(DateTime NgayDi, string SerchName,int NhaXeId);
        ListDoanhThuTongHop GetDoanhThuTongHopTheoNgay(DateTime NgayDi, int NhaXeId);
        #endregion
        #region Lich Trinh Loai Xe
        LichTrinhLoaiXe GetLichTrinhLoaiXe(int HanhTrinhId, int LichTrinhId, int LoaiXeId);
        LichTrinhLoaiXe GetLichTrinhLoaiXeById(int Id);
        #endregion 
        #region dat ve note
        void InsertDatVeNote(int DatVeId, string note);
        PagedList<DatVeNote> GetLichSuDatVe(DateTime? NgayGiaoDich = null, int HanhTrinhId = 0,string StringSearch="", int pageIndex = 0,
              int pageSize = int.MaxValue);
        List<DatVeNote> GetLichSuDatVeByDatVeId(int DatVeId);
        #endregion
        #region Cau Hinh Gia Ve
        //co 2 hinh thuc cau hinh gia ve
        //1. Cau hinh gia ve the lich trinh, hanh trinh, loai xe (cv_lichtrinh_loaixe) ->tat ca cung gia ve
        //2. Cau hinh gia ve theo so do ghe xe, loai xe, hanh trinh(cv_hanhtrinh_loaixe)
        //==>viec lay gia ve luc dat ghe se dua vao chuyen di dang ap dung theo cau hinh gia ve nao 
        //muc uu tien lay gia ve khi co cau hinh gia ve theo so do, neu khong co se lay gia ve chung
        List<LichTrinhLoaiXe> GetAllLichTrinhLoaiXe(int NhaXeId);
        void UpdateGiaVe(int HanhTrinhId, int LichTrinhId, int LoaiXeId, decimal GiaVe);
        List<HanhTrinhLoaiXeGiaVe> GetAllHanhTrinhLoaiXeGiaVe(int HanhTrinhId, int LoaiXeId);
        void UpdateGiaVeSoDo(int HanhTrinhId, int LoaiXeId, int SoDoGheId, decimal GiaVe);
        Decimal GetGiaVeTheoSoDoId(int HanhTrinhId, int LoaiXeId, int SoDoGheId);
        #endregion

    }
}
