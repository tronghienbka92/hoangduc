using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Core.Domain.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Seo;
using Nop.Core.Domain.Directory;
using Nop.Data;


namespace Nop.Services.NhaXes
{
    public class HopDongChuyenService : IHopDongChuyenService
    {
       
        #region Init
        private readonly IRepository<KhachHangChuyen> _khachhangchuyenRepository;
        private readonly IRepository<DatVe> _datveRepository;
        private readonly IRepository<HopDongChuyen> _hopdongchuyenRepository;
        private readonly IRepository<DiemDon> _diemdonRepository;
        private readonly IRepository<HanhTrinhDiemDon> _hanhtrinhdiemdonRepository;
        private readonly IRepository<ChuyenDi> _chuyendiRepository;
        private readonly IDbContext _dbContext;
        private readonly IRepository<NhanVien> _nhanvienRepository;
        private readonly IRepository<LichTrinhLoaiXe> _lichtrinhloaixeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<DatVeNote> _datvenoteRepository;
        private readonly IRepository<HanhTrinhLoaiXeGiaVe> _hanhtrinhloaixeRepository;
        private readonly IRepository<HanhTrinh> _hanhtrinhRepository;
        public HopDongChuyenService(IRepository<KhachHangChuyen> khachhangchuyenRepository,
            IRepository<DatVe> datveRepository,
             IRepository<DiemDon> diemdonRepository,
             IRepository<HopDongChuyen> hopdongchuyenRepository,
             IRepository<HanhTrinhDiemDon> hanhtrinhdiemdonRepository,
            IRepository<ChuyenDi> chuyendiRepository,
             IRepository<NhanVien> nhanvienRepository,
            IDbContext dbContext,
            IRepository<LichTrinhLoaiXe> lichtrinhloaixeRepository,
             IRepository<DatVeNote> datvenoteRepository,
            ICacheManager cacheManager,
            IRepository<HanhTrinh> hanhtrinhRepository,
            IRepository<HanhTrinhLoaiXeGiaVe> hanhtrinhloaixeRepository
            )
        {
            this._hanhtrinhRepository = hanhtrinhRepository;
            this._hanhtrinhloaixeRepository = hanhtrinhloaixeRepository;
            this._hopdongchuyenRepository = hopdongchuyenRepository;
            this._cacheManager = cacheManager;
            this._lichtrinhloaixeRepository = lichtrinhloaixeRepository;
            this._khachhangchuyenRepository = khachhangchuyenRepository;
            this._datveRepository = datveRepository;
            this._diemdonRepository = diemdonRepository;
            this._hanhtrinhdiemdonRepository = hanhtrinhdiemdonRepository;
            this._dbContext = dbContext;
            this._chuyendiRepository = chuyendiRepository;
            this._nhanvienRepository = nhanvienRepository;
            this._datvenoteRepository = datvenoteRepository;
        }
        #endregion
        #region "Hanh khach"
        public virtual KhachHangChuyen InsertKhachHangChuyen(KhachHangChuyen item)
        {
            if (item == null)
                throw new ArgumentNullException("HopDongChuyenLimousine");
            //kiem tra ton tai so dien thoai chua



            _khachhangchuyenRepository.Insert(item);
            return item;
        }
        public virtual void UpdateKhachHangChuyen(KhachHangChuyen item)
        {
            if (item == null)
                throw new ArgumentNullException("HopDongChuyenLimousine");
            _khachhangchuyenRepository.Update(item);
        }
        public virtual void DeleteKhachHangChuyen(KhachHangChuyen item)
        {
            if (item == null)
                throw new ArgumentNullException("HopDongChuyenLimousine");
            _khachhangchuyenRepository.Delete(item);
        }
        public virtual KhachHangChuyen GetKhachHangChuyenById(int itemId)
        {
            if (itemId == 0)
                return null;
            //string key = string.Format(LIMOUSINE_KHACH_HANG_BY_ID_KEY, itemId);
            //return _cacheManager.Get(key, () => _khachhangRepository.GetById(itemId));
            return _khachhangchuyenRepository.GetById(itemId);
        }

        public virtual List<KhachHangChuyen> GetAllHanhKhachChuyen(int NhaXeId, string ThongTin, int NumRow = 100)
        {
            
            var query = _khachhangchuyenRepository.Table.Where(c => c.NhaXeId == NhaXeId);
            if (!string.IsNullOrEmpty(ThongTin))
            {
                query = query.Where(c => (c.SoDienThoai.Contains(ThongTin) || c.TenKhachHang.Contains(ThongTin)));
            }
            if (NumRow == 0)
                return query.OrderByDescending(c => c.Id).ToList();
            return query.OrderByDescending(c => c.Id).Take(NumRow).ToList();
        }

        #endregion

        #region "hop dong chuyen"
        public virtual void InsertChuyenDiHopDong(HopDongChuyen item)
        {
            if (item == null)
                throw new ArgumentNullException("HopDongChuyenLimousine");
            item.NgayTao = DateTime.Now;
            _hopdongchuyenRepository.Insert(item);
            _hopdongchuyenRepository.Update(item);
            //tao log

        }
        public virtual void UpdateChuyenDiHopDong(HopDongChuyen item)
        {
            if (item == null)
                throw new ArgumentNullException("HopDongChuyenLimousine");
            item.NgayCapNhat = DateTime.Now;
            _hopdongchuyenRepository.Update(item);
        }
        public virtual void DeleteChuyenDiHopDong(HopDongChuyen item)
        {
            if (item == null)
                throw new ArgumentNullException("HopDongChuyenLimousine");
            item.TrangThaiId = (int)ENTrangThaiHopDongChuyen.HUY;
            _hopdongchuyenRepository.Update(item);
        }
        public virtual HopDongChuyen GetChuyenDiHopDongById(int itemId)
        {
            if (itemId == 0)
                return null;
            return _hopdongchuyenRepository.GetById(itemId);
        }
        public virtual List<KhachHangChuyen> GetAllKhachHangByHopDongId(int Id)
        {
            var query = _khachhangchuyenRepository.Table.Where(c => c.HopDongChuyenId == Id);
            return query.ToList();
        }
        public virtual List<HopDongChuyen> GetHopDongChuyenToBaoCao(int NhaXeId,DateTime tungay, DateTime denngay)
        {
            var query = _hopdongchuyenRepository.Table.Where(c =>c.NhaXeId==NhaXeId && (c.TrangThaiId != (int)ENTrangThaiHopDongChuyen.HUY) && c.ThoiGianDonKhach>=tungay && c.ThoiGianDonKhach<=denngay);
            return query.ToList();
        }
        public virtual List<HopDongChuyen> GetHopDongChuyenByDayIndex(int NhaXeId, DateTime NgayDi)
        {
            var query = _hopdongchuyenRepository.Table.Where(c => c.NhaXeId == NhaXeId
                && (c.TrangThaiId != (int)ENTrangThaiHopDongChuyen.HUY)
                && c.ThoiGianDonKhach.Value.Year==NgayDi.Year
                 && c.ThoiGianDonKhach.Value.Month == NgayDi.Month
                  && c.ThoiGianDonKhach.Value.Day == NgayDi.Day);
            return query.ToList();
        }
        public virtual List<HopDongChuyen> GetHopDongChuyenTrongThang(int NhaXeId, DateTime NgayDi)
        {
            var query = _hopdongchuyenRepository.Table.Where(c => c.NhaXeId == NhaXeId
                && (c.TrangThaiId != (int)ENTrangThaiHopDongChuyen.HUY)
                && c.ThoiGianDonKhach.Value.Year == NgayDi.Year
                 && c.ThoiGianDonKhach.Value.Month == NgayDi.Month
                 );
            return query.ToList();
        }
        public virtual   PagedList<HopDongChuyen> GetAllHopDongChuyen(int NhaXeId = 0, string BienSo="",string SoHopDong="",
        int pageIndex = 0,
        int pageSize = int.MaxValue)
        {
            var query = _hopdongchuyenRepository.Table;
            query = query.Where(m => m.NhaXeId == NhaXeId && m.TrangThaiId != (int)ENTrangThaiHopDongChuyen.HUY);
            if (!string.IsNullOrWhiteSpace(BienSo))
                query = query.Where(c => c.XeInfo.BienSo.Contains(BienSo));
            if (!string.IsNullOrWhiteSpace(SoHopDong))
                query = query.Where(c => c.SoHopDong.Contains(SoHopDong));
            query = query.OrderBy(m => m.Id);
            return new PagedList<HopDongChuyen>(query, pageIndex, pageSize);
        }
       
        #endregion
   
    }
}
