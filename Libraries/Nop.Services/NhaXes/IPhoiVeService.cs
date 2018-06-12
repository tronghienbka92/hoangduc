using Nop.Core;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.NhaXes
{
    public partial interface IPhoiVeService
    {
        bool DatVe(PhoiVe item, ENTrangThaiPhoiVe trangthai = ENTrangThaiPhoiVe.DatCho);


        PhoiVe GetPhoiVeById(int Id);

        void DeletePhoiVe(PhoiVe item);
        void UpdatePhoiVe(PhoiVe item);
        void ThanhToanVeTaiQuay(PhoiVe item);
        bool DeletePhoiVeTaiQuay(string MaVe);
        /// <summary>
        /// có the doi ve truoc 24h từ thời gian khởi hành
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool CanDoiVe(PhoiVe item);

        /// <summary>
        /// su dung trong truong hop cho nha xe thiet dat thanh toan ve doi voi nhung nguoi dat ve qua dien thoai
        /// </summary>
        /// <param name="item"></param>
        void ThanhToanGiaoVe(PhoiVe item);
        void HuyPhoiVe(PhoiVe item);
        void DeletePhoiVe(int NguonVeXeId, string SessionId, int CustomerId, DateTime NgayDi);
        bool GiuChoPhoiVe(int NguonVeXeId, string SessionId, int CustomerId, DateTime NgayDi);
        bool GetPhoiVeByNguonVe(int NguonveId, string SessionId, int CustomerId, DateTime NgayDi);
        void NhaXeThanhToanNhanh(int NguonVeXeId, string SessionId, int NguoiDatVeId);
        void NhaXeThanhToanGiuChoPhoiVe(int NguonVeXeId, string SessionId, int NguoiDatVeId, bool DaThanhToan, int customerId, string GhiChu);
        List<PhoiVe> GetPhoiVeGiuChoBySession(object SessionId);
        List<PhoiVe> GetPhoiVeDatChoBySession(object SessionId);
        List<PhoiVe> GetPhoiVeByCustomer(int CustomerId);
        bool ThanhToan(string SessionId, int CustomerId, Address shippingAddress, out int OrderId);
        List<NhaXeCustomer> GetKhachHangInNhaXe(string SearchKhachhang, int nhaxeid);
        NhaXeCustomer GetKhachVangLaiInNhaXe(int nhaxeid);
        List<PhoiVe> GetPhoiVeByOrderId(int OrderId);
        decimal GetTongDoanhThu(List<DoanhThuItem> doanhthus, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId);
        PhoiVe GetPhoiVe(int NguonVeXeId, SoDoGheXeQuyTac vitri, DateTime ngaydi, bool isNewIfNull = false);
        void ProcessTime(int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int Thang1, out int Thang2);
        int GetAllSoNguoi(int NguonVeId, DateTime ngaydi);
        decimal GetRevenueHistoryXeXuatBen(int NguonVeXeId, DateTime ngaydi);
        List<ThongKeItem> GetAllPhoiVe(int thang, int nam, int nhaxeid, ENBaoCaoChuKyThoiGian ChuKyThoiGianId);
        List<KhachHangMuaVeItem> GetDetailDoanhThu(int nhaxeid, DateTime ngaydi, int nhanvienid = 0);

        List<ThongKeItem> GetDoanhThuBanVeTheoNgay(DateTime tuNgay, DateTime denNgay, int nhaxeid, int VanPhongId);
        List<DoanhThuTheoXeItem> GetDoanhThuBanVeTungXeTheoNgay(DateTime tuNgay, DateTime denNgay, int nhaxeid, int XeId);

        List<KhachHangMuaVeItem> GetDetailDoanhThuBanVeTungXeTheoNgay(DateTime ngaydi, int nhaxeid, int XeId = 0);
        PagedList<PhoiVeNote> GetLichSuPhoiVe(DateTime? NgayGiaoDich = null, int NguonVeXeId = 0, int pageIndex = 0,
               int pageSize = int.MaxValue);
        List<DoanhThuTheoXeItem> GetDoanhThuTheoTuyen(DateTime tuNgay, DateTime denNgay,  List<int> LichtrinhIds);

        List<KhachHangMuaVeItem> GetDetailDoanhThuTheoTuyen(DateTime ngaydi, int nhaxeid, int HanhTrinhId = 0);
        List<ThongKeItem> GetDoanhThuBanVeTheoNhanVien(int nhaxeid, int VanPhongId, DateTime NgayBan);
        List<ThongKeItem> GetDoanhThuBanVeTheoTrangThai(int nhaxeid, int VanPhongId, DateTime NgayBan, int NhanVienId);
        decimal DoanhThuVanPhong(List<int> nhavienids, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int SoLuong);
        decimal DoanhThuTuyen(List<int> LichtrinhIds, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int SoLuong);
        decimal DoanhThuXeTheoTuyen(List<int> HisXeXuatBen, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int SoLuong);
        decimal DoanhThuTuyenCon(List<int> NguonVeIds, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int SoLuong);
        decimal DoanhThuLichTrinh(int lichtrinhid, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int SoLuong);
        List<PhoiVe> GetPhoiVeByChuyenDi(int NguonVeXeId, DateTime NgayDi);
        List<PhoiVe> GetPhoiVeYeuCauHuy(int VanPhongId);
        List<NguonVeXe> GetAllNguonVeXeByLichTrinhId(int LichTrinhId);
        List<HistoryXeXuatBen> GetAllHisByNguonVeId(int NguonVeId);
    }
}
