using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.NhaXes;
using Nop.Core.Domain.Chonves;


namespace Nop.Services.NhaXes
{
    public partial class NhaXeService : INhaXeService
    {
        private const string NHAXE_BY_ID_KEY = "Nop.NhaXe.id-{0}";
        private readonly IRepository<NguonVeXe> _NguonVeXeRepository;
        private readonly IRepository<NhaXe> _nhaxeRepository;
        private readonly IRepository<HanhTrinhKhuVuc> _hahtrinhkhuvucRepository;
        private readonly IRepository<HistoryXeXuatBen> _historyxexuatbenRepository;
        private readonly IRepository<ChuyenDi> _chuyendiRepository;
        private readonly IRepository<HistoryXeXuatBenLog> _historyxexuatbenlogRepository;
        private readonly IRepository<HistoryXeXuatBen_NhanVien> _historyxexuatbennhanvienRepository;
        private readonly IRepository<NhanVien> _nhanvienRepository;
        private readonly IRepository<HopDong> _hopdongRepository;
        private readonly IRepository<NhaXePicture> _pictureRepository;
        private readonly IRepository<VanPhong> _vanphongRepository;
        private readonly IRepository<LichTrinh> _LichTrinhRepository;
        private readonly IRepository<ChotKhach> _chotkhachRepository;
        private readonly IXeInfoService _xeinfoService;

        private readonly IGiaoDichKeVeXeService _giadichkeveService;
        private readonly IRepository<XeVanChuyen> _XeVanChuyenRepository;
        private readonly IRepository<NhaXeCauHinh> _nhaxecauhinhRepository;


        private readonly ICacheManager _cacheManager;
        public NhaXeService(ICacheManager cacheManager,
            IRepository<NhaXe> nhaxeRepository,
             IRepository<HistoryXeXuatBen> historyxexuatbenRepository,
             IRepository<ChuyenDi> chuyendiRepository,
              IRepository<HanhTrinhKhuVuc> hanhtrinhkhuvucRepository,
            IRepository<NhanVien> nhanvienRepository,
             IRepository<NguonVeXe> NguonVeXeRepository,
            IRepository<HopDong> hopdongRepository,
            IRepository<NhaXePicture> pictureRepository,
                IXeInfoService xeinfoService,
            IGiaoDichKeVeXeService giaodichkeveService,
            IRepository<VanPhong> vanphongRepository,
              IRepository<LichTrinh> LichTrinhRepository,
             IRepository<XeVanChuyen> XeVanChuyenRepository,
             IRepository<NhaXeCauHinh> nhaxecauhinhRepository,
             IRepository<HistoryXeXuatBenLog> historyxexuatbenlogRepository,
             IRepository<ChotKhach> chotkhachRepository,
            IRepository<HistoryXeXuatBen_NhanVien> historyxexuatbennhanvienRepository
            )
        {
            this._historyxexuatbennhanvienRepository = historyxexuatbennhanvienRepository;
            this._chotkhachRepository = chotkhachRepository;
            this._chuyendiRepository = chuyendiRepository;
            this._hahtrinhkhuvucRepository = hanhtrinhkhuvucRepository;
            this._historyxexuatbenlogRepository = historyxexuatbenlogRepository;
            this._nhaxecauhinhRepository = nhaxecauhinhRepository;
            this._historyxexuatbenRepository = historyxexuatbenRepository;
            this._nhaxeRepository = nhaxeRepository;
            this._nhanvienRepository = nhanvienRepository;
            this._cacheManager = cacheManager;
            this._xeinfoService = xeinfoService;
            this._NguonVeXeRepository = NguonVeXeRepository;
            this._hopdongRepository = hopdongRepository;
            this._pictureRepository = pictureRepository;
            this._vanphongRepository = vanphongRepository;
            this._LichTrinhRepository = LichTrinhRepository;
            this._XeVanChuyenRepository = XeVanChuyenRepository;

            this._giadichkeveService = giaodichkeveService;

        }

        #region Nha Xe
        public virtual List<NhaXe> GetAllNhaXe()
        {
            var query = _nhaxeRepository.Table;
            return query.ToList();
        }
        public virtual IPagedList<NhaXe> GetAllNhaXe(string nhaxeName = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false,
            int OwnerID = 0)
        {
            var query = _nhaxeRepository.Table;
            if (!showHidden)
                query = query.Where(m => !m.isDelete);
            if (!String.IsNullOrWhiteSpace(nhaxeName))
                query = query.Where(m => m.TenNhaXe.Contains(nhaxeName));
            if (OwnerID > 0)
                query = query.Where(m => m.NguoiTaoID == OwnerID);

            query = query.OrderBy(m => m.MaNhaXe);

            return new PagedList<NhaXe>(query, pageIndex, pageSize);


        }
        /// <summary>
        /// Gets a NhaXe
        /// </summary>
        /// <param name="NhaXeId">NhaXe identifier</param>
        /// <returns>NhaXe</returns>
        public virtual NhaXe GetNhaXeById(int NhaXeId)
        {
            if (NhaXeId == 0)
                return null;
            return _nhaxeRepository.GetById(NhaXeId);
        }
        /// <summary>
        /// Gets a NhaXe
        /// </summary>
        /// <param name="NhaXeId">NhaXe identifier</param>
        /// <returns>NhaXe</returns>
        public virtual NhaXe GetNhaXeByCustommerId(int CustommerId)
        {
            if (CustommerId == 0)
                return null;
            //lay thong tin hop dong
            var hopdongs = _hopdongRepository.Table.Where(x => x.KhachHangID == CustommerId);

            foreach (var item in hopdongs.ToList())
            {
                if (item.NhaXeID == 0)
                    return null;
                return GetNhaXeById(item.NhaXeID);
            }
            if (hopdongs.Count() == 0)
            {
                var nhaviens = _nhanvienRepository.Table.Where(a => a.CustomerID == CustommerId).ToList();
                foreach (var item in nhaviens)
                {
                    if (item.NhaXeID == 0)
                        return null;
                    return GetNhaXeById(item.NhaXeID);
                }
            }
            return null;
        }
        /// <summary>
        /// Inserts NhaXe
        /// </summary>
        /// <param name="NhaXe">NhaXe</param>
        public virtual void InsertNhaXe(NhaXe _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _item.CreatedOn = DateTime.Now;
            _item.LastUpdate = DateTime.Now;
            _item.DieuKhoanGuiHang = "";
            _nhaxeRepository.Insert(_item);
            _item.MaNhaXe = string.Format("NX{0}", _item.Id.ToString().PadLeft(10, '0'));
            _nhaxeRepository.Update(_item);


        }

        /// <summary>
        /// Updates the NhaXe
        /// </summary>
        /// <param name="NhaXe">NhaXe</param>
        public virtual void UpdateNhaXe(NhaXe _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _item.LastUpdate = DateTime.Now;
            _nhaxeRepository.Update(_item);

        }
        public virtual void DeleteNhaXe(NhaXe _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");

            _item.isDelete = true;
            UpdateNhaXe(_item);
        }

        #endregion

        #region Nha Xe hinh anh
        public virtual void InsertNhaXePicture(NhaXePicture _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXePicture");

            _pictureRepository.Insert(_item);
        }
        public virtual void UpdateNhaXePicture(NhaXePicture _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXePicture");

            _pictureRepository.Update(_item);
        }
        public virtual void DeleteNhaXePicture(NhaXePicture _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXePicture");

            _pictureRepository.Delete(_item);
        }

        public virtual IList<NhaXePicture> GetNhaXePicturesByNhaXeId(int NhaXeId)
        {
            var query = from pp in _pictureRepository.Table
                        where pp.NhaXe_Id == NhaXeId
                        orderby pp.DisplayOrder
                        select pp;
            var nhaxePictures = query.ToList();
            return nhaxePictures;
        }


        public virtual NhaXePicture GetNhaXePictureById(int nhaxePictureId)
        {
            if (nhaxePictureId == 0)
                return null;

            return _pictureRepository.GetById(nhaxePictureId);
        }

        #endregion
        #region VanPhong
        public virtual IPagedList<VanPhong> GetAllVanPhong(int NhaXeId = 0, string tenvanphong = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _vanphongRepository.Table;
            query = query.Where(m => m.NhaXeId == NhaXeId);
            if (!showHidden)
                query = query.Where(m => !m.isDelete);

            if (!String.IsNullOrWhiteSpace(tenvanphong))
                query = query.Where(m => m.TenVanPhong.Contains(tenvanphong));
            query = query.OrderBy(m => m.Id);
            return new PagedList<VanPhong>(query, pageIndex, pageSize);
        }
        public virtual List<VanPhong> GetAllVanPhongByNhaXeId(int NhaXeId = 0)
        {
            var query = _vanphongRepository.Table;
            query = query.Where(m => m.NhaXeId == NhaXeId);
            query = query.Where(m => !m.isDelete);
            return query.ToList();
        }
        public virtual void InsertVanPhong(VanPhong _item)
        {
            if (_item == null)
                throw new ArgumentNullException("VanPhong");
            _item.CreatedOn = DateTime.Now;
            _item.LastUpdate = DateTime.Now;
            _vanphongRepository.Insert(_item);
        }
        public virtual void UpdateVanPhong(VanPhong _item)
        {
            if (_item == null)
                throw new ArgumentNullException("VanPhong");
            _item.LastUpdate = DateTime.Now;
            _vanphongRepository.Update(_item);
        }
        public virtual void DeleteVanPhong(VanPhong _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXePicture");
            _item.isDelete = true;
            UpdateVanPhong(_item);
        }
        public virtual VanPhong GetVanPhongById(int VanPhongId)
        {
            if (VanPhongId == 0)
                return null;
            return _vanphongRepository.GetById(VanPhongId);
        }
        #endregion
        #region xexuatben

        public List<NhanVien> GetAllNhanVienByNhaXe(int NhaXeId, ENKieuNhanVien[] kieunvs, string TenNhanVien = "")
        {
            var query = _nhanvienRepository.Table.Where(m => m.NhaXeID == NhaXeId && !m.isDelete);
            if(kieunvs!=null && kieunvs.Length>0)
            {
                int[] _kieunvs = kieunvs.Cast<int>().ToArray();
                query = query.Where(m=>_kieunvs.Contains(m.KieuNhanVienID));
            }
            if (!string.IsNullOrEmpty(TenNhanVien))
                query = query.Where(c => c.HoVaTen.Contains(TenNhanVien));
            return query.ToList();
        }
        public List<XeVanChuyen> GetAllBienSoXeByNhaXeId(int NhaXeId,int LoaiXeId=0)
        {

            var query = _XeVanChuyenRepository.Table.Where(m => m.NhaXeId == NhaXeId );
            if (LoaiXeId > 0)
                query = query.Where(c => c.LoaiXeId == LoaiXeId);
            return query.ToList();
        }

        public virtual HistoryXeXuatBen GetHistoryXeXuatBenId(int HistoryXeXuatBenId)
        {
            if (HistoryXeXuatBenId == 0)
                return null;
            return _historyxexuatbenRepository.GetById(HistoryXeXuatBenId);
        }
        /// <summary>
        /// lay xe xuat ben theo nguon ve
        /// </summary>
        /// <param name="nguonveid"></param>
        /// <returns></returns>
        public virtual HistoryXeXuatBen GetXeXuatBenByNguonVeId(int nguonveid)
        {
            var ngaydi = DateTime.Now;
            var query = _historyxexuatbenRepository.Table.Where(c => c.NguonVeId == nguonveid && c.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY
                 && c.NgayDi.Year == ngaydi.Year
                && c.NgayDi.Month == ngaydi.Month
                && c.NgayDi.Day == ngaydi.Day).ToList();
            return query.FirstOrDefault();
        }
        public virtual HistoryXeXuatBen GetHistoryXeXuatBenByNguonVeId(int NguonVeId, DateTime ngaydi)
        {

            var query = _historyxexuatbenRepository.Table.Where(m => m.NguonVeId == NguonVeId && m.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY
                && m.NgayDi.Year == ngaydi.Year
                && m.NgayDi.Month == ngaydi.Month
                && m.NgayDi.Day == ngaydi.Day).ToList();
            return query.FirstOrDefault();
        }
        /// <summary>
        /// Lay thong tin xe xuat ben gan nhat
        /// </summary>
        /// <param name="XeVanChuyenId"></param>
        /// <param name="ngaydi"></param>
        /// <returns></returns>
        public virtual List<HistoryXeXuatBen> GetHistoryXeXuatBenByXeVanChuyen(int XeVanChuyenId)
        {
            //lay ngay di trc do 4 ngay, đi lâu nhất là từ HN ->HCM 3 ngày
            DateTime NgayDi = DateTime.Now.AddDays(-4);
            var query = _historyxexuatbenRepository.Table.Where(m => m.XeVanChuyenId == XeVanChuyenId 
                && (m.TrangThaiId==(int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN || m.TrangThaiId==(int)ENTrangThaiXeXuatBen.DANG_DI)
                && m.NgayDi >= NgayDi).OrderByDescending(m =>m.Id).ToList().Where(c=>c.NgayDi.AddHours(Convert.ToDouble(c.NguonVeInfo.LichTrinhInfo.SoGioChay)+2)>=DateTime.Now.Date).ToList();
            return query;
        }
        /// <summary>
        /// lay toan bo xe xuat ben trong nha xe
        /// </summary>
        /// <param name="XeVanChuyenId"></param>
        /// <param name="ngaydi"></param>
        /// <returns></returns>
        public virtual List<HistoryXeXuatBen> GetHistoryXeXuatBenByNhaXeId(int NhaXeId)
        {
           
            //var query = _historyxexuatbenRepository.Table.Where(m => m. == XeVanChuyenId
            //    && (m.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN || m.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI)
            //    && m.NgayDi >= NgayDi).OrderByDescending(m => m.Id).ToList().Where(c => c.NgayDi.AddHours(Convert.ToDouble(c.NguonVeInfo.LichTrinhInfo.SoGioChay) + 2) >= DateTime.Now.Date).ToList();
            return null;
        }
    
        public virtual List<HistoryXeXuatBen> GetAllXeXuatBenByNgayDi(int NhaXeId, DateTime NgayDi)
        {
            var query = _historyxexuatbenRepository.Table.Where(m =>m.NguonVeInfo.NhaXeId==NhaXeId && m.TrangThaiId!=(int)ENTrangThaiXeXuatBen.HUY
               && m.NgayDi.Year >= NgayDi.Year
               && m.NgayDi.Month >= NgayDi.Month
               && m.NgayDi.Day >= NgayDi.Day);
            return query.ToList();
        }
        public virtual void UpdateHistoryXeXuatBen(HistoryXeXuatBen _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");

            _historyxexuatbenRepository.Update(_item);

        }
        public virtual void InsertHistoryXeXuatBen(HistoryXeXuatBen _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            
            _historyxexuatbenRepository.Insert(_item);
            var _log = new HistoryXeXuatBenLog();
            _log.XeXuatBenId = _item.Id;
            _log.TrangThai = _item.TrangThai;
            _log.GhiChu = "Thiết đặt xe xuất bến";
            _log.NguoiTaoId = _item.NguoiTaoId;
            InsertHistoryXeXuatBenLog(_log);
        }
        public virtual void DeleteHistoryXeXuatBenNhanVien(int XeXuatBenId)
        {
            //xoa du lieu lai xe
            var laiphuxecus = _historyxexuatbennhanvienRepository.Table.Where(c => c.HistoryXeXuatBen_Id == XeXuatBenId).ToList();
            if (laiphuxecus.Count > 0)
                _historyxexuatbennhanvienRepository.Delete(laiphuxecus);
        }
        public virtual void InsertHistoryXeXuatBenLog(HistoryXeXuatBenLog _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _item.NgayTao = DateTime.Now;
            _historyxexuatbenlogRepository.Insert(_item);

        }
        public virtual void UpdateHistoryXeXuatBenLog(HistoryXeXuatBenLog _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _historyxexuatbenlogRepository.Update(_item);
        }
        public virtual void DeleteHistoryXeXuatBenLog(HistoryXeXuatBenLog _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _historyxexuatbenlogRepository.Delete(_item);
        }
        public virtual HistoryXeXuatBenLog GetHistoryXeXuatBenLogById(int Id)
        {
            if (Id == 0)
                throw new ArgumentNullException("NhaXe");
            return _historyxexuatbenlogRepository.GetById(Id);
        }
        public virtual void UpdateHistoryXeXuatBenTrangThai(int Id,int NguoiTaoId, ENTrangThaiXeXuatBen trangthai)
        {
            var item = GetHistoryXeXuatBenId(Id);
            item.TrangThai = trangthai;
            UpdateHistoryXeXuatBen(item);
            var _log = new HistoryXeXuatBenLog();
            _log.XeXuatBenId = item.Id;
            _log.TrangThai = trangthai;
            _log.NguoiTaoId = NguoiTaoId;
            switch(trangthai)
            {
                case ENTrangThaiXeXuatBen.DANG_DI:
                    {
                        _log.GhiChu = "Khởi hành";
                        break;
                    }
                case ENTrangThaiXeXuatBen.KET_THUC:
                    {
                        _log.GhiChu = "Về bến";
                        break;
                    }
            }
            InsertHistoryXeXuatBenLog(_log);
        }
        /// <summary>
        /// Lay thong tin luot ve
        /// </summary>
        /// <param name="LuotDiId"></param>
        /// <returns></returns>
        public virtual HistoryXeXuatBen GetChuyenVeByChuyenDi(int ChuyenDiId)
        {
            //lay thong tin luot di
            var luotdi = GetHistoryXeXuatBenId(ChuyenDiId);
            //lay thong tin luot tiep theo cua xe nay
            var luotves = _historyxexuatbenRepository.Table.Where(m =>
                                m.Id != ChuyenDiId && m.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY
                                && m.NgayDi >= luotdi.NgayDi
                                && m.XeVanChuyenId == luotdi.XeVanChuyenId                                
                                ).OrderBy(d => d.NgayDi).Take(3).ToList().FirstOrDefault();

            if (luotves != null)
            {
                //xem xet lan di tiep theo cua xe nay dang di tuyen nao
                //neu la tuyen di tiep (HN-CP), thi xe co the co van de, nen khong thuc hien tiep dc viec ve
                if (luotves.HanhTrinh.isTuyenDi)
                {
                    return null;
                }
                return luotves;
            }
            return null;

        }
        /// <summary>
        /// lay xe xuat ben theo nguon ve
        /// </summary>
        /// <param name="nguonveid"></param>
        /// <returns></returns>
        public virtual List<HistoryXeXuatBen> GetAllTuyenDiTrongNgay(DateTime NgayDi, ENKhungGio khunggio = ENKhungGio.All, string ThongTin = null)
        {
            var khungtg = new KhungThoiGian(khunggio);
            var query = _historyxexuatbenRepository.Table.Where(c =>
                c.NgayDi.Year == NgayDi.Year && c.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY
                && c.NgayDi.Month == NgayDi.Month
                && c.NgayDi.Day == NgayDi.Day
                && c.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY
                && c.HanhTrinh.isTuyenDi
                && c.NguonVeInfo.ThoiGianDi.Hour >= khungtg.GioTu
                && c.NguonVeInfo.ThoiGianDi.Hour < khungtg.GioDen
                );
            if (!String.IsNullOrEmpty(ThongTin))
            {
                query = query.Where(c => c.xevanchuyen.BienSo.Contains(ThongTin) || c.LaiPhuXes.Any(n => n.nhanvien.HoVaTen.Contains(ThongTin)));
            }
            var xexuatbens = query.OrderBy(c=>c.NgayDi).ToList();
            return xexuatbens;
        }

        public virtual List<ThongKeLuotXuatBenItem> GetThongKeLuotXuatBen(int ThangId, int NamId, int[] arrNhanVien)
        {
            //lay tat ca ngay trong thang 
            if (ThangId > 0 && NamId > 0)
            {
                var query = _chuyendiRepository.Table
                .Where(c => (c.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.KET_THUC)
                    && c.NgayDi.Month == ThangId
                    && c.NgayDi.Year == NamId
                    && (arrNhanVien.Contains(c.LaiXeId.Value))
                    ).ToList().Select(m =>
                    {
                        var item = new ThongKeLuotXuatBenItem();
                        item.LaiXeId = m.LaiXeId.Value;
                        item.Ngay = m.NgayDi.Day;
                        item.Thang = ThangId;
                        item.Nam = NamId;
                        item.SoLuot = 1;
                        return item;
                    }).ToList();
                return query;

            }
            //lay du lieu trong nam
            if (NamId > 0)
            {
                var query = _chuyendiRepository.Table
                .Where(c => (c.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.KET_THUC)
                    && c.NgayDi.Year == NamId
                    && (arrNhanVien.Contains(c.LaiXeId.Value))
                    ).ToList().Select(m =>
                    {
                        var item = new ThongKeLuotXuatBenItem();
                        item.LaiXeId = m.LaiXeId.Value;
                        item.Ngay = 0;
                        item.Thang = m.NgayDi.Month;
                        item.Nam = NamId;
                        item.SoLuot = 1;
                        return item;
                    }).ToList();
                return query;
            }

            //lay du lieu trong vong 5 nam
            int _nam = DateTime.Now.Year;
            var query1 = _chuyendiRepository.Table
                .Where(c => (c.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.KET_THUC)
                    && c.NgayDi.Year <= _nam
                     && (arrNhanVien.Contains(c.LaiXeId.Value))
                    ).ToList().Select(m =>
                    {
                        var item = new ThongKeLuotXuatBenItem();
                        item.LaiXeId = m.LaiXeId.Value;
                        item.Ngay = 0;
                        item.Thang = 0;
                        item.Nam = m.NgayDi.Year;
                        item.SoLuot = 1;
                        return item;
                    }).ToList();
            return query1;
        }
        #endregion
        #region "Cau hinh nha xe"
        public virtual List<NhaXeCauHinh> GetAllNhaXeCauHinh(int NhaXeId)
        {
            var query = _nhaxecauhinhRepository.Table;
            query = query.Where(m => m.NhaXeId == NhaXeId);
            return query.ToList();
        }

        public virtual void Insert(NhaXeCauHinh _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXeCauHinh");
            var item1 = GetNhaXeCauHinhByCode(_item.NhaXeId, _item.MaCauHinh);
            if(item1!=null)
            {
                item1.Ten = _item.Ten;
                item1.GiaTri = _item.GiaTri;
                _nhaxecauhinhRepository.Update(item1);
            }
            else
            _nhaxecauhinhRepository.Insert(_item);
        }
        public virtual void Update(NhaXeCauHinh _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXeCauHinh");
            _nhaxecauhinhRepository.Update(_item);
        }
        public virtual void Delete(NhaXeCauHinh _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXeCauHinh");
            _nhaxecauhinhRepository.Delete(_item);
        }
        public virtual NhaXeCauHinh GetNhaXeCauHinhById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("NhaXeCauHinh");
            return _nhaxecauhinhRepository.GetById(id);
        }
        public virtual NhaXeCauHinh GetNhaXeCauHinhByCode(int NhaXeId,ENNhaXeCauHinh cauhinh)
        {
            string ma = cauhinh.ToString();
            return GetNhaXeCauHinhByCode(NhaXeId, ma);
        }
        public virtual NhaXeCauHinh GetNhaXeCauHinhByCode(int NhaXeId, string cauhinh)
        {
            var query = _nhaxecauhinhRepository.Table;
            query = query.Where(m => m.NhaXeId == NhaXeId && m.Ma == cauhinh);
            return query.FirstOrDefault();
        }
        #endregion
        #region Chot khach
        public virtual void InsertChotKhach(ChotKhach _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _item.NgayChot = DateTime.Now;
            _chotkhachRepository.Insert(_item);
            _item.Ma = string.Format("CK{0}", _item.Id.ToString().PadLeft(7, '0'));
            UpdateChotKhach(_item);
        }
        public virtual void UpdateChotKhach(ChotKhach _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _chotkhachRepository.Update(_item);
        }
        public virtual void DeleteChotKhach(ChotKhach _item)
        {
            if (_item == null)
                throw new ArgumentNullException("NhaXe");
            _chotkhachRepository.Delete(_item);
        }
        public virtual ChotKhach GetChotKhachById(int Id)
        {
            if (Id == 0)
                return null;
            return _chotkhachRepository.GetById(Id);
        }
        public virtual List<ChotKhach> GetChotKhachs(int NhaXeId, string Ma = null, int HistoryXeXuatBenId = 0, int NguoiChotId = 0, int DiemChotId = 0, DateTime? NgayChotTu = null, DateTime? NgayChotDen = null, int NumTop = 100)
        {
            var query = _chotkhachRepository.Table.Where(c => c.NhaXeId == NhaXeId);

            if (!String.IsNullOrEmpty(Ma))
                query = query.Where(c => c.Ma.Contains(Ma));

            if (HistoryXeXuatBenId > 0)
            {
                query = query.Where(c => c.HistoryXeXuatBenId == HistoryXeXuatBenId);
            }
            if (NguoiChotId > 0)
            {
                query = query.Where(c => c.NguoiChotId == NguoiChotId);
            }
            if (DiemChotId > 0)
            {
                query = query.Where(c => c.DiemDonId == DiemChotId);
            }
            if (NgayChotTu.HasValue)
            {
                query = query.Where(c => c.NgayChot >= NgayChotTu.Value);
            }
            if (NgayChotDen.HasValue)
            {
                var dtto = NgayChotDen.Value.AddDays(1);
                query = query.Where(c => c.NgayChot <= dtto);
            }

            return query.OrderByDescending(c => c.Id).Take(NumTop).ToList();
        }
        #endregion
        #region hanhtrinh khu vuc
        public bool InsertHanhTrinhKhuVuc(HanhTrinhKhuVuc item)
        {
            if (item == null)
                return false;
            _hahtrinhkhuvucRepository.Insert(item);
            return true;
        }
        public bool UpdateHanhTrinhKhuVuc(HanhTrinhKhuVuc item)
        {
            if (item == null)
                return false;
            _hahtrinhkhuvucRepository.Update(item);
            return true;
        }
        public bool DeleteHanhTrinhKhuVuc(HanhTrinhKhuVuc item)
        {
            if (item == null)
                return false;
            _hahtrinhkhuvucRepository.Delete(item);
            return true;
        }
        public List<HanhTrinhKhuVuc> GetHanhTrinhKhuVucs(int HanhTrinhId)
        {
           return _hahtrinhkhuvucRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId).ToList();
        }
        public List<HanhTrinhKhuVuc> GetHanhTrinhKhuVucsByKhuvucId(int KhuVucId)
        {
            return _hahtrinhkhuvucRepository.Table.Where(c => c.KhuVucId == KhuVucId).ToList();
        }
        public HanhTrinhKhuVuc GetHanhTrinhKhuVucByhtkh(int HanhTrinhId,int KhuVucId)
        {
           var query= _hahtrinhkhuvucRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && c.KhuVucId==KhuVucId).ToList();
           if (query.Count() > 0)
               return query.First();
           return null;
        }
        #endregion
    }
}
