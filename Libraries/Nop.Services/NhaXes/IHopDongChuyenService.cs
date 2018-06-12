using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.NhaXes;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.Directory;
using System;


namespace Nop.Services.NhaXes
{
    public partial interface IHopDongChuyenService
    {
        #region "Hanh khach"
        KhachHangChuyen InsertKhachHangChuyen(KhachHangChuyen item);
        void UpdateKhachHangChuyen(KhachHangChuyen item);
        void DeleteKhachHangChuyen(KhachHangChuyen item);
        KhachHangChuyen GetKhachHangChuyenById(int itemId);      
        List<KhachHangChuyen> GetAllHanhKhachChuyen(int NhaXeId, string ThongTin, int NumRow = 100);
        #endregion
        #region "hop dong chuyen"
        void InsertChuyenDiHopDong(HopDongChuyen item);
        void UpdateChuyenDiHopDong(HopDongChuyen item);
        void DeleteChuyenDiHopDong(HopDongChuyen item);
        HopDongChuyen GetChuyenDiHopDongById(int itemId);
        List<KhachHangChuyen> GetAllKhachHangByHopDongId(int Id);
        List<HopDongChuyen> GetHopDongChuyenToBaoCao(int NhaXeId,DateTime tungay, DateTime denngay);
        List<HopDongChuyen> GetHopDongChuyenByDayIndex(int NhaXeId, DateTime NgayDi);
        List<HopDongChuyen> GetHopDongChuyenTrongThang(int NhaXeId, DateTime NgayDi);
        PagedList<HopDongChuyen> GetAllHopDongChuyen(int NhaXeId = 0, string BienSo="",string SoHopDong="",
        int pageIndex = 0,
        int pageSize = int.MaxValue);
       
        #endregion
        
    }
}
