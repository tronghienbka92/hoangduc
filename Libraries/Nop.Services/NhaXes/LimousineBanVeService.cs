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
using System.Web.Mvc;


namespace Nop.Services.NhaXes
{
    public class LimousineBanVeService : ILimousineBanVeService
    {
        #region Constants
        private const string LIMOUSINE_KHACH_HANG_BY_ID_KEY = "Chonve.limousine.khachhang.id-{0}";
        private const string LIMOUSINE_KHACH_HANG_ALL_KEY = "Chonve.limousine.khachhang.all-{0}";
        private const string LIMOUSINE_KHACH_HANG_PATTERN_KEY = "Chonve.limousine.khachhang.";
        #endregion
        #region Init
        private readonly IRepository<KhachHang> _khachhangRepository;
        private readonly IRepository<DatVe> _datveRepository;
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
        private readonly IRepository<HanhTrinhKhuVuc> _hanhtrinhkhuvucRepository;
        public LimousineBanVeService(IRepository<KhachHang> khachhangRepository,
            IRepository<DatVe> datveRepository,
             IRepository<DiemDon> diemdonRepository,
             IRepository<HanhTrinhDiemDon> hanhtrinhdiemdonRepository,
            IRepository<ChuyenDi> chuyendiRepository,
             IRepository<NhanVien> nhanvienRepository,
            IDbContext dbContext,
            IRepository<LichTrinhLoaiXe> lichtrinhloaixeRepository,
             IRepository<DatVeNote> datvenoteRepository,
            ICacheManager cacheManager,
            IRepository<HanhTrinh> hanhtrinhRepository,
            IRepository<HanhTrinhLoaiXeGiaVe> hanhtrinhloaixeRepository,
            IRepository<HanhTrinhKhuVuc> hanhtrinhkhuvucRepository
            )
        {
            this._hanhtrinhRepository = hanhtrinhRepository;
            this._hanhtrinhloaixeRepository = hanhtrinhloaixeRepository;
            this._cacheManager = cacheManager;
            this._lichtrinhloaixeRepository = lichtrinhloaixeRepository;
            this._khachhangRepository = khachhangRepository;
            this._datveRepository = datveRepository;
            this._diemdonRepository = diemdonRepository;
            this._hanhtrinhdiemdonRepository = hanhtrinhdiemdonRepository;
            this._dbContext = dbContext;
            this._chuyendiRepository = chuyendiRepository;
            this._nhanvienRepository = nhanvienRepository;
            this._datvenoteRepository = datvenoteRepository;
            this._hanhtrinhkhuvucRepository = hanhtrinhkhuvucRepository;
        }
        #endregion

        #region "Hanh khach"
        public virtual KhachHang InsertKhachHang(KhachHang item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            //kiem tra ton tai so dien thoai chua

            var query = _khachhangRepository.Table.Where(c => c.DienThoai == item.DienThoai).FirstOrDefault();
            if (query != null)
            {
                query.Ten = item.Ten;
                query.DiaChi = item.DiaChi;
                UpdateKhachHang(query);
                return query;
            }
            else
                _khachhangRepository.Insert(item);
            return item;
        }
        public virtual void UpdateKhachHang(KhachHang item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            _khachhangRepository.Update(item);            
        }
        public virtual void DeleteKhachHang(KhachHang item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            _khachhangRepository.Delete(item);
        }
        public virtual KhachHang GetKhachHangById(int itemId)
        {
            if (itemId == 0)
                return null;
            //string key = string.Format(LIMOUSINE_KHACH_HANG_BY_ID_KEY, itemId);
            //return _cacheManager.Get(key, () => _khachhangRepository.GetById(itemId));
            return _khachhangRepository.GetById(itemId);
        }
        public virtual KhachHang GetKhachHangByDienThoai(int NhaXeId, string DienThoai)
        {
            var query = _khachhangRepository.Table.Where(c => c.NhaXeId == NhaXeId);
            query = query.Where(c => c.DienThoai.Contains(DienThoai));
            return query.FirstOrDefault();
        }
        public virtual List<KhachHang> GetAllHanhKhach(int NhaXeId, string ThongTin, int NumRow = 100)
        {
            //string key = string.Format(LIMOUSINE_KHACH_HANG_ALL_KEY, NhaXeId);
            //var query = _cacheManager.Get(key, () => _khachhangRepository.Table.Where(c => c.NhaXeId == NhaXeId).ToList());
            //if (!string.IsNullOrEmpty(ThongTin))
            //{
            //    query = query.Where(c => (c.DienThoai.Contains(ThongTin) || c.Ten.Contains(ThongTin))).ToList();
            //}
            //if (NumRow == 0)
            //    return query.OrderByDescending(c => c.Id).ToList();
            //return query.OrderByDescending(c => c.Id).Take(NumRow).ToList();
            var query = _khachhangRepository.Table.Where(c => c.NhaXeId == NhaXeId);
            if (!string.IsNullOrEmpty(ThongTin))
            {
                query = query.Where(c => (c.DienThoai.Contains(ThongTin) || c.Ten.Contains(ThongTin)));
            }
            if (NumRow == 0)
                return query.OrderByDescending(c => c.Id).ToList();
            return query.OrderByDescending(c => c.Id).Take(NumRow).ToList();
        }
        
        #endregion

        #region "Dat ve"
        public virtual void InsertDatVe(DatVe item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            _datveRepository.Insert(item);
            //cap nhat ma dat ve
            item.Ma = item.Id.ToString().PadLeft(7, '0');
            _datveRepository.Update(item);
            var note = "HD " + item.Ma + " được đặt bởi ";
            if(item.nguoitao!=null)
            {
               note=note+ item.nguoitao.HoVaTen;                
            }
            else
            {
                note = note + "ID=" + item.NguoiTaoId.ToString();
            }
            InsertDatVeNote(item.Id, note);
           
        }
        public virtual void UpdateDatVe(DatVe item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            _datveRepository.Update(item);
        }
        public virtual void DeleteDatVe(DatVe item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            //delete all log
            var datvelog = _datvenoteRepository.Table.Where(c => c.DatVeId == item.Id).ToList();
            _datvenoteRepository.Delete(datvelog);
            _datveRepository.Delete(item);
        }
        public virtual DatVe GetDatVeById(int itemId)
        {
            if (itemId == 0)
                return null;
            return _datveRepository.GetById(itemId);
        }

        public virtual List<DatVe> GetAllDatVe(int NhaXeId, DateTime NgayDi, int HanhTrinhId = 0, int LichTrinhId = 0, ENTrangThaiDatVe TrangThai = ENTrangThaiDatVe.ALL, string ThongTinDatVe = "")
        {
            var query = _datveRepository.Table.Where(c => c.NhaXeId == NhaXeId && c.TrangThaiId != (int)ENTrangThaiDatVe.MOI
                && c.NgayDi.Day == NgayDi.Day
                && c.NgayDi.Month == NgayDi.Month
                && c.NgayDi.Year == NgayDi.Year
                );
            if (HanhTrinhId > 0)
                query = query.Where(c => c.HanhTrinhId == HanhTrinhId);
            if (LichTrinhId > 0)
                query = query.Where(c => c.LichTrinhId == LichTrinhId);
            if (TrangThai != ENTrangThaiDatVe.ALL)
                query = query.Where(c => c.TrangThaiId == (int)TrangThai);
            else
                query = query.Where(c => c.TrangThaiId != (int)ENTrangThaiDatVe.HUY);
            if (!string.IsNullOrEmpty(ThongTinDatVe))
            {
                query = query.Where(c => (c.khachhang.Ten.Contains(ThongTinDatVe) || c.khachhang.DienThoai.Contains(ThongTinDatVe) || c.DiaChiNha.Contains(ThongTinDatVe)));
            }
            return query.OrderByDescending(c=>c.Id).ToList();
        }
        public virtual List<DatVe> GetDatVeBySession(int NhaXeId, int ChuyenDiId, string SessionId)
        {
            var query = _datveRepository.Table.Where(c => c.NhaXeId == NhaXeId && c.TrangThaiId != (int)ENTrangThaiDatVe.HUY
                && c.ChuyenDiId == ChuyenDiId
                && c.SessionID == SessionId
                );
            return query.ToList();
        }
        public virtual List<DatVe> GetDatVeByChuyenDi(int ChuyenDiId)
        {
            var query = _datveRepository.Table 
                .Where(c => c.ChuyenDiId == ChuyenDiId
                && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO));

            return query.ToList();
        }
        public virtual int GetSoLuongDatVeTheoKhachHang(int KhachHangId, out int SoLuongHuy)
        {
            SoLuongHuy = _datveRepository.Table.Where(c => c.KhachHangId == KhachHangId && c.TrangThaiId == (int)ENTrangThaiDatVe.HUY && c.isKhachHuy).Count();
            var query = _datveRepository.Table.Where(c => c.KhachHangId == KhachHangId && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || (c.TrangThaiId == (int)ENTrangThaiDatVe.HUY && c.isKhachHuy))).Count();
            return query;
        }

        /// <summary>
        /// LoaiId=1: so luong dat ve nhieu nhat
        /// LoaiId=2: so luong huy ve nhieu nhat
        /// </summary>
        /// <param name="LoaiId"></param>
        /// <returns></returns>
        public virtual List<ThongKeItem> GetHanhKhachTheoSoLuongDatVe(int NhaXeId, string ThongTin, int LoaiId)
        {
            var query = _datveRepository.Table.Where(c=>c.NhaXeId==NhaXeId);
            if(!string.IsNullOrEmpty(ThongTin))
                query = query.Where(c => (c.khachhang.Ten.Contains(ThongTin) || c.khachhang.DienThoai.Contains(ThongTin)));
            if(LoaiId==1)
            {
                query = query.Where(c => (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO));
            }
            else
            {
                query = query.Where(c => (c.TrangThaiId == (int)ENTrangThaiDatVe.HUY && c.isKhachHuy));
            }
            var items = query.Select(c => new { c.KhachHangId, c.Id })
                    .GroupBy(g => g.KhachHangId)
                    .Select(s => new ThongKeItem
                    {
                        ItemId = s.Key.Value,
                        SoLuong = s.Count()
                    })
              .OrderByDescending(sx => sx.SoLuong)
              .ToList();
            return items;
        }
        /// <summary>
        /// Lay thong tin dat ve cuoi cung cua khach hang
        /// </summary>
        /// <param name="KhachHangId"></param>
        /// <returns></returns>
        public virtual DatVe GetDatVeCuoiTheoKhachHangId(int KhachHangId, int HanhTrinhId)
        {
            var query = _datveRepository.Table.Where(c =>c.HanhTrinhId==HanhTrinhId && c.KhachHangId == KhachHangId && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)).OrderByDescending(c=>c.NgayDi).ThenByDescending(c=>c.Id);
            return query.FirstOrDefault();
        }

        #endregion

        #region "Chuyen Di"
        public virtual void InsertChuyenDi(ChuyenDi item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            item.NgayTao = DateTime.Now;
            _chuyendiRepository.Insert(item);
            item.Ma = item.Id.ToString();// "CV" + item.Id.ToString().PadLeft(7, '0');
            _chuyendiRepository.Update(item);
            //tao log

        }
        public virtual void UpdateChuyenDi(ChuyenDi item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            _chuyendiRepository.Update(item);
        }
        public virtual void DeleteChuyenDi(ChuyenDi item)
        {
            if (item == null)
                throw new ArgumentNullException("LimousineBanVeService");
            _chuyendiRepository.Delete(item);
        }
        public virtual ChuyenDi GetChuyenDiById(int itemId)
        {
            if (itemId == 0)
                return null;
            return _chuyendiRepository.GetById(itemId);
        }
        public virtual List<ChuyenDi> GetAllChuyenDi(int NhaXeId, int HanhTrinhId = 0, int LichTrinhId = 0, ENKhungGio KhungGioId = ENKhungGio.All, DateTime? NgayDi = null, string ThongTin = "", string ThongTinKhachHang = "")
        {
            var query = _chuyendiRepository.Table.Where(c => c.NhaXeId == NhaXeId && c.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY);
            if (HanhTrinhId > 0)
                query = query.Where(c => c.HanhTrinhId == HanhTrinhId);
            if (LichTrinhId > 0)
                query = query.Where(c => c.LichTrinhId == LichTrinhId);
            if (KhungGioId != ENKhungGio.All)
            {
                var khungtg = new KhungThoiGian(KhungGioId);
                query = query.Where(c => c.NgayDiThuc.Hour >= khungtg.GioTu
                  && c.NgayDiThuc.Hour < khungtg.GioDen
               );
            }
            if (NgayDi.HasValue)
                query = query.Where(c => c.NgayDiThuc.Day == NgayDi.Value.Day
                    && c.NgayDiThuc.Month == NgayDi.Value.Month
                    && c.NgayDiThuc.Year == NgayDi.Value.Year
                    );
            if (!String.IsNullOrEmpty(ThongTin))
            {
                query = query.Where(c => (c.xevanchuyen.BienSo.Contains(ThongTin) || c.laixe.HoVaTen.Contains(ThongTin)));
            }
            if (!String.IsNullOrEmpty(ThongTinKhachHang))
            {
                query = query.Where(c => (c.DatVes.Any(dv=>dv.TrangThaiId!=(int)ENTrangThaiDatVe.HUY && (dv.khachhang.Ten.Contains(ThongTinKhachHang) || dv.khachhang.DienThoai.Contains(ThongTinKhachHang) || dv.TenKhachHangDiKem.Contains(ThongTinKhachHang)))));
            }
            return query.OrderBy(c=>c.NgayDiThuc).ToList();
        }
        public virtual List<ChuyenDi> GetAllChuyenDi(int NhaXeId, int HanhTrinhId, DateTime NgayDi)
        {
            NgayDi = NgayDi.Date;
            var query = _chuyendiRepository.Table.Where(c => c.NhaXeId == NhaXeId && c.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY 
                && c.HanhTrinhId == HanhTrinhId
                && c.NgayDi>=NgayDi
                );
            return query.OrderBy(c=>c.NgayDi).ThenBy(c=>c.Id).ToList();
        }
        /// <summary>
        /// Lay so tt chuyen di trong ngay de update vao chuyen di
        /// </summary>
        /// <param name="NhaXeId"></param>
        /// <param name="NgayDi"></param>
        /// <returns></returns>
        public virtual int GetSTTChuyenDi(int NhaXeId, DateTime NgayDi)
        {
            int[] trangthaiids = { (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN, (int)ENTrangThaiXeXuatBen.DANG_DI, (int)ENTrangThaiXeXuatBen.KET_THUC };
            var query = _chuyendiRepository.Table.Where(c => c.NhaXeId == NhaXeId
                && trangthaiids.Contains(c.TrangThaiId)
                && c.NgayDi.Day == NgayDi.Day
                    && c.NgayDi.Month == NgayDi.Month
                    && c.NgayDi.Year == NgayDi.Year);
           
            return query.Count()+1;
        }
        public virtual List<ChuyenDi> GetAllChuyenDiTheoXe(int XeVanChuyenId)
        {
            //lay tat ca chuyen di trong ngay
            DateTime NgayDi = DateTime.Now.Date;
            var query = _chuyendiRepository.Table.Where(m => m.XeVanChuyenId == XeVanChuyenId
                && (m.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN || m.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI)
                && m.NgayDi >= NgayDi).OrderByDescending(m => m.Id).ToList();
            return query;
            
        }
        public virtual List<ChuyenDi> GetAllChuyenDiTheoKhachHang(int KhachHangId, DateTime NgayDi)
        {
            //lay tat ca chuyen di cua khach hang trong ngay
            var chuyendiids=_datveRepository.Table.Where(c=>c.KhachHangId==KhachHangId 
                && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                && c.NgayDi.Day==NgayDi.Day
                && c.NgayDi.Month==NgayDi.Month
                && c.NgayDi.Year==NgayDi.Year).Select(g=>g.ChuyenDiId).Distinct().ToList();
            var query = _chuyendiRepository.Table.Where(m => chuyendiids.Contains(m.Id)
                && m.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY).OrderByDescending(m => m.NgayDi).ToList();
            return query;
        }
        public virtual List<DatVe> GetAllDatVeByKhachHang(DateTime TuNgay,DateTime DenNgay,int KhachHangId)
        {

            var query = _datveRepository.Table.Where(m => m.KhachHangId==KhachHangId
              && (m.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || m.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                && m.NgayDi >= TuNgay 
                && m.NgayDi<=DenNgay
                ).OrderByDescending(m => m.NgayDi).ToList();
            return query;

        }
        public virtual List<DoanhThuNhanVienTheoXe> GetAllChuyenTheoNgay(DateTime TuNgay, DateTime DenNgay, string SerchName, int NhaXeId)
        {
            //lay tat ca chuyen di trong ngay
            DenNgay = DenNgay.Date.AddDays(1);
            var query = _chuyendiRepository.Table.Where(m =>m.NhaXeId==NhaXeId &&
                  m.NgayDi < DenNgay
                   && (m.NgayDi >= TuNgay)
                   //&& (m.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN || m.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI))
                   &&(m.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY))
                   .Select(c => new ChuyenDiItem
                   { 
                       xeid=c.XeVanChuyenId.Value,
                       xevanchuyen=c.xevanchuyen,
                       laixeid=c.LaiXeId.Value,
                       laixe=c.laixe,
                       idchuyen=c.Id,
                   })
                   .GroupBy(c => new { c.xeid, c.laixeid })
                   .Select(g => new ChuyenDiGroup
                   {

                       XeVanChuyen = g.FirstOrDefault().xevanchuyen,
                       BienSoXe = g.FirstOrDefault().xevanchuyen.BienSo,
                       LaiXe = g.FirstOrDefault().laixe,
                       TenLaiXe = g.FirstOrDefault().laixe.HoVaTen,
                       TongLuot = g.Count(),
                       ChuyenDis=g.ToList(),
                      

                   })
                   .Where(m=>(m.TenLaiXe.Contains(SerchName)) || m.BienSoXe.Contains(SerchName)).ToList();
            var doanhthus = new List<DoanhThuNhanVienTheoXe>();
            foreach (var m in query)
            {
                var item = new DoanhThuNhanVienTheoXe();
                item.BienSoXe = m.XeVanChuyen.BienSo;
                item.TenLaiXe = m.LaiXe.HoVaTen;
                item.TongLuot = m.TongLuot;
                var arrChuyenId = m.ChuyenDis.Select(c => c.idchuyen);
                // get doanh thu cho tung nhom chuyen di
                var _doanhthu = _datveRepository.Table.Where(c => arrChuyenId.Contains(c.ChuyenDiId.Value)
                && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)).ToList();
                item.SoKhach = _doanhthu.Count();
                item.ThanhTien = _doanhthu.Sum(c => c.GiaTien);
                doanhthus.Add(item);

            }
            return doanhthus;

        }
        public virtual ListDoanhThuTongHop GetDoanhThuTongHopTheoNgay(DateTime NgayDi, int NhaXeId)
        {
            var query = _chuyendiRepository.Table.Where(m => m.NhaXeId == NhaXeId &&
                 m.NgayDi.Year == NgayDi.Year
                  && m.NgayDi.Month == NgayDi.Month
                 && m.NgayDi.Day == NgayDi.Day
                  && (m.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY));
            var item = new ListDoanhThuTongHop();
            item.NgayDi = NgayDi;
            var HN_NDhanhtrinhids = _hanhtrinhkhuvucRepository.Table.Where(c => c.KhuVucId == (int)ENKhuVuc.HA_NOI).Select(c => c.HanhTrinhId);
           // var ND_HNhanhtrinhids = _hanhtrinhkhuvucRepository.Table.Where(c => c.KhuVucId == (int)ENKhuVuc.NAM_DINH).Select(c => c.HanhTrinhId);
            var chuyendiHN_ND = query.Where(c => HN_NDhanhtrinhids.Contains(c.HanhTrinhId)).Select(c => c.Id).ToArray();
           // var chuyendiND_HN = query.Where(c => ND_HNhanhtrinhids.Contains(c.HanhTrinhId)).Select(c => c.Id).ToArray();
            //doanh thu ha noi
           item.DT_HN_ND = new DoanhThuTongHop();
           var datvesnb = _datveRepository.Table.Where(c => chuyendiHN_ND.Contains(c.ChuyenDiId.Value)
                && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO));
           item.DT_HN_ND.TongLuot = datvesnb.Select(c => c.ChuyenDiId).Distinct().Count();
           item.DT_HN_ND.SoKhach = datvesnb.Count();
           item.DT_HN_ND.ThanhTien = 0;
           if (datvesnb.Count() > 0)
           {
               item.DT_HN_ND.ThanhTien = datvesnb.Sum(c => c.GiaTien);
           }
           //doanh thu  tam ky da nang
           //item.DT_ND_HN = new DoanhThuTongHop();
           //var datvetkdns = _datveRepository.Table.Where(c => chuyendiND_HN.Contains(c.ChuyenDiId.Value)
           //     && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO));
           //item.DT_ND_HN.TongLuot = datvetkdns.Select(c => c.ChuyenDiId).Distinct().Count();
           //item.DT_ND_HN.SoKhach = datvetkdns.Count();
           //item.DT_ND_HN.ThanhTien = 0;
           //if (datvetkdns.Count() > 0)
           //{
           //    item.DT_ND_HN.ThanhTien = datvetkdns.Sum(c => c.GiaTien);
           //}

            
          
            return item;
        }
        public virtual List<XeDiTrongNgay> GetAllChuyenTheoXeTrongNgay(DateTime NgayDi, string SerchName,int NhaXeId)
        {
            //lay tat ca chuyen di trong ngay

            var query = _chuyendiRepository.Table.Where(m => m.NhaXeId == NhaXeId &&
                  m.NgayDi.Year == NgayDi.Year
                   && m.NgayDi.Month == NgayDi.Month
                  && m.NgayDi.Day == NgayDi.Day
                   && (m.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY)
                   && (m.laixe.HoVaTen.Contains(SerchName) || m.xevanchuyen.BienSo.Contains(SerchName)))
                  .GroupBy(c => new { c.XeVanChuyenId })
                  .Select(g => new XeDiTrongNgay
                   {

                       XeId = g.Key.XeVanChuyenId.Value,
                       BienSoXe = g.FirstOrDefault().xevanchuyen.BienSo
                   }).ToList();
                    //.Where(m => m.BienSoXe.Contains(SerchName)).ToList();
            var doanhthus = new List<XeDiTrongNgay>();
            foreach (var m in query)
            {
                var item = new XeDiTrongNgay();
                item.TongLuot = 0;
                item.TongKhach = 0;
                item.TongTien = 0;
                item.BienSoXe = m.BienSoXe;
              //lay toan bo chuyen cua xe trong ngay
                var _items = _chuyendiRepository.Table.Where(c =>
                c.NgayDi.Year == NgayDi.Year
                 && c.NgayDi.Month == NgayDi.Month
                && c.NgayDi.Day == NgayDi.Day
                // && (c.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI)
                 && (c.TrangThaiId != (int)ENTrangThaiXeXuatBen.HUY)
                 && c.XeVanChuyenId == m.XeId).ToList();
                item.TongLuot = _items.Count();
                item.doanhthus = new List<DoanhThuChiTietNgay>();
               foreach(var chuyen in _items)
               {
                   var chuyeninfo=new DoanhThuChiTietNgay();
                   chuyeninfo.BienSoXe=m.BienSoXe;
                   chuyeninfo.BatDau=chuyen.NgayDiThuc.ToString("dd/MM/yyyy HH:mm");
                   chuyeninfo.KetThuc = chuyen.NgayDiThuc.ToString("dd/MM/yyy") + " " + chuyen.lichtrinh.ThoiGianDen.ToString("HH:mm");
                   //chuyeninfo.KetThuc = chuyen.lichtrinh.ThoiGianDen.ToString("HH:mm");
                   chuyeninfo.SoLuot=1;
                   var hopdongs=_datveRepository.Table.Where(c => c.ChuyenDiId==chuyen.Id
                && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)).ToList();
                   chuyeninfo.SoKhach = hopdongs.Count();
                   chuyeninfo.SoTien = hopdongs.Sum(c => c.GiaTien);
                   item.TongKhach = item.TongKhach + chuyeninfo.SoKhach;
                   item.TongTien = item.TongTien + chuyeninfo.SoTien;
                   chuyeninfo.DiemDau = chuyen.hanhtrinh.DiemDons.First().diemdon.TenDiemDon;
                   chuyeninfo.DiemCuoi = chuyen.hanhtrinh.DiemDons.Last().diemdon.TenDiemDon;
                   chuyeninfo.TenLai = chuyen.laixe.HoVaTen;
                   item.doanhthus.Add(chuyeninfo);
               }
               doanhthus.Add(item);
                

            }
            return doanhthus;

        }
        #endregion
        #region Bao Cao
        public void GetLimousineIndex(int NhaXeId, DateTime ngaydi, ENBaoCaoChuKyThoiGian loaithongke, out int SoLuongDatVe, out Decimal DoanhThu, out int SoLuongChuyen)
        {
            SoLuongDatVe = 0;
            DoanhThu = 0;
            SoLuongChuyen = 0;
            var datves = _datveRepository.Table.Where(c => c.NhaXeId == NhaXeId
                && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO));
            var chuyendis = _chuyendiRepository.Table.Where(c => c.NhaXeId == NhaXeId
                    && (c.TrangThaiId == (int)ENTrangThaiXeXuatBen.DANG_DI
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.KET_THUC
                    || c.TrangThaiId == (int)ENTrangThaiXeXuatBen.CHO_XUAT_BEN)                    
                    );
            switch (loaithongke)
            {
                case ENBaoCaoChuKyThoiGian.HangNgay:
                    {
                        datves = datves.Where(c => c.NgayDi.Day == ngaydi.Day
                            && c.NgayDi.Month == ngaydi.Month
                            && c.NgayDi.Year == ngaydi.Year);
                        chuyendis = chuyendis.Where(c => c.NgayDi.Day == ngaydi.Day
                            && c.NgayDi.Month == ngaydi.Month
                            && c.NgayDi.Year == ngaydi.Year);
                        break;
                    }
                case ENBaoCaoChuKyThoiGian.HangThang:
                    {
                        datves = datves.Where(c => c.NgayDi.Month == ngaydi.Month
                            && c.NgayDi.Year == ngaydi.Year);
                        chuyendis = chuyendis.Where(c => c.NgayDi.Month == ngaydi.Month
                            && c.NgayDi.Year == ngaydi.Year);
                        break;
                    }
                case ENBaoCaoChuKyThoiGian.HangNam:
                    {
                        datves = datves.Where(c => c.NgayDi.Year == ngaydi.Year);
                        chuyendis = chuyendis.Where(c => c.NgayDi.Year == ngaydi.Year);
                        break;
                    }
            }
            SoLuongDatVe = datves.Count();
            if (datves.Any())
                DoanhThu = datves.Sum(c => c.GiaTien);
            SoLuongChuyen = chuyendis.Count();



        }
        public List<ThongKeItem> GetDoanhThuBanVeTheoNgay(DateTime tuNgay, DateTime denNgay, int nhaxeid, int VanPhongId)
        {
            /*denNgay = denNgay.Date.AddDays(1);
            var phoives = _datveRepository.Table
               .Where(c => c.NhaXeId == nhaxeid
                   && c.nguoitao.VanPhongID == VanPhongId
                    && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId == (int)ENTrangThaiDatVe.HUY)
                    && (c.NgayDi < denNgay)
                     && (c.NgayDi >= tuNgay)).ToList()
             .Select(c => new
             {
                 GiaTien = c.GiaTien,
                 NgayDi = c.NgayDi.Date,
                 TrangThaiId = c.TrangThaiId,
                 VeChuyenDenId=c.VeChuyenDenId
             })
             .GroupBy(c => new { c.NgayDi })
             .Select(g => new ThongKeItem
             {
                 ItemDataDate = g.Key.NgayDi,
                 GiaTri = g.Sum(a => a.GiaTien),
                 SoLuong = g.Count(),
                 SoLuongDat=g.Count(a=>a.TrangThaiId != (int)ENTrangThaiDatVe.HUY),
                 SoLuongChuyen = g.Count(a => a.VeChuyenDenId > 0 && a.TrangThaiId != (int)ENTrangThaiDatVe.HUY),
                 SoLuongHuy = g.Count(a => a.TrangThaiId == (int)ENTrangThaiDatVe.HUY)
             })
             .OrderByDescending(sx => sx.ItemDataDate)
             //.OrderByDescending(sx => sx.GiaTri)
             .ToList();
            var tknhanvien = new List<ThongKeItem>();
            foreach (var item in phoives)
            {
                item.Nhan = item.ItemDataDate.ToString("dd-MM-yyyy");
                item.NhanSapXep = item.ItemDataDate.ToString("yyyyMMdd");
                //item.SoLuongChuyen
                tknhanvien.Add(item);
            }
            return tknhanvien;*/
            denNgay = denNgay.Date.AddDays(1);
            var phoives = _datveRepository.Table
               .Where(c => c.NhaXeId == nhaxeid
                   && c.nguoitao.VanPhongID == VanPhongId
                    && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId == (int)ENTrangThaiDatVe.HUY)
                    && (c.NgayDi < denNgay)
                     && (c.NgayDi >= tuNgay)).ToList()
             .Select(c => new
             {
                 GiaTien = c.GiaTien,
                 NgayDi = c.NgayDi.Date,
                 TrangThaiId = c.TrangThaiId,
                 NguoiChuyenId = c.NguoiChuyenId,
                 NguoiHuyId = c.NguoiHuyId,
             })
             .GroupBy(c => new { c.NgayDi })
             .Select(g => new ThongKeItem
             {
                 ItemDataDate = g.Key.NgayDi,
                 GiaTri = g.Where(a => a.TrangThaiId != (int)(int)ENTrangThaiDatVe.HUY).Sum(a => a.GiaTien),
                 SoLuong = g.Count(),
                 SoLuongDat = g.Count(a => a.TrangThaiId != (int)ENTrangThaiDatVe.HUY),
                 SoLuongChuyen = g.Count(a => a.NguoiChuyenId > 0),
                 SoLuongHuy = g.Count(a => a.NguoiHuyId > 0)
             })
             .OrderByDescending(sx => sx.ItemDataDate)
                //.OrderByDescending(sx => sx.GiaTri)
             .ToList();
            var tknhanvien = new List<ThongKeItem>();
            foreach (var item in phoives)
            {
                item.Nhan = item.ItemDataDate.ToString("dd-MM-yyyy");
                item.NhanSapXep = item.ItemDataDate.ToString("yyyyMMdd");
                //item.SoLuongChuyen
                tknhanvien.Add(item);
            }
            return tknhanvien;
        }
        public List<ThongKeItem> GetDoanhThuBanVeTheoNhanVien(int nhaxeid, int VanPhongId, DateTime NgayBan)
        {

            //var phoives = _datveRepository.Table
            //   .Where(c => c.NhaXeId == nhaxeid
            //        && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId==(int)ENTrangThaiDatVe.HUY)
            //        && (c.NgayDi.Year == NgayBan.Year)
            //         && (c.NgayDi.Month == NgayBan.Month)
            //         && (c.NgayDi.Day == NgayBan.Day))
            // .Select(c => new
            // {
            //     NhanVienId = c.NguoiTaoId,
            //     GiaTien = c.GiaTien,
            //     TrangThaiId = c.TrangThaiId,
            //     VeChuyenDenId = c.VeChuyenDenId
            // })
            // .GroupBy(c => new { c.NhanVienId })
            // .Select(g => new ThongKeItem
            // {
            //     ItemId = g.Key.NhanVienId,
            //     GiaTri = g.Sum(a => a.GiaTien),
            //     SoLuong = g.Count(),
            //     SoLuongDat = g.Count(a =>a.TrangThaiId != (int)ENTrangThaiDatVe.HUY),
            //     SoLuongChuyen = 0,
            //     SoLuongHuy = 0
            // }) 
            // .OrderByDescending(sx => sx.GiaTri)
            // .ToList();

            
            //foreach(var item in phoives)
            //{
            //    item.SoLuongChuyen = _datveRepository.Table
            //   .Where(c => c.NhaXeId == nhaxeid
            //        && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId == (int)ENTrangThaiDatVe.HUY)
            //        && (c.NgayDi.Year == NgayBan.Year)
            //         && (c.NgayDi.Month == NgayBan.Month)
            //         && (c.NgayDi.Day == NgayBan.Day) && (c.NguoiChuyenId==item.ItemId)
            //         ).Count();
            //    item.SoLuongHuy = _datveRepository.Table
            //   .Where(c => c.NhaXeId == nhaxeid
            //        && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId == (int)ENTrangThaiDatVe.HUY)
            //        && (c.NgayDi.Year == NgayBan.Year)
            //         && (c.NgayDi.Month == NgayBan.Month)
            //         && (c.NgayDi.Day == NgayBan.Day) && (c.NguoiHuyId == item.ItemId)
            //         ).Count();
            //}

            //var tknhanvien = new List<ThongKeItem>();
            //foreach (var item in phoives)
            //{
            //    item.Nhan = item.GiaTri.ToString();
            //    item.NhanSapXep = item.GiaTri.ToString();
            //    var checknhanvien = _nhanvienRepository.Table.Where(c => c.Id == item.ItemId && c.VanPhongID == VanPhongId).Count();
            //    if (checknhanvien > 0)
            //        tknhanvien.Add(item);
            //}
            //return tknhanvien;


            var tknhanvien = new List<ThongKeItem>();
            var query = _datveRepository.Table
               .Where(c => c.NhaXeId == nhaxeid
                    && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO || c.TrangThaiId == (int)ENTrangThaiDatVe.HUY)
                    && (c.NgayDi.Year == NgayBan.Year)
                     && (c.NgayDi.Month == NgayBan.Month)
                     && (c.NgayDi.Day == NgayBan.Day));
            //lay tat ca nhan vien tham gia vao giao dich dat ve, chuyen ve, huy ve
            var nhanvienids = query.Select(c => c.NguoiTaoId).Distinct().ToList();
            nhanvienids.AddRange(query.Where(c => c.NguoiHuyId > 0).Select(c => c.NguoiHuyId).Distinct().ToList().Select(c => c.GetValueOrDefault(0)));
            nhanvienids.AddRange(query.Where(c => c.NguoiChuyenId > 0).Select(c => c.NguoiChuyenId).Distinct().ToList().Select(c => c.GetValueOrDefault(0)));

            //var LoaiNhanVienBanVe=new int[] {(int)ENKieuNhanVien.LaiXe,(int)ENKieuNhanVien.PhuXe};
            var listNhanVien = _nhanvienRepository.Table.Where(c => c.VanPhongID == VanPhongId && nhanvienids.Contains(c.Id)).ToList();
            foreach (var item in listNhanVien)
            {
                var nhanvien = new ThongKeItem();
                nhanvien.ItemId = item.Id;
                nhanvien.SoLuongDat = query.Where(c => c.NguoiTaoId == item.Id && c.TrangThaiId != (int)ENTrangThaiDatVe.HUY).Count();
                nhanvien.SoLuongChuyen = query.Where(c => c.NguoiChuyenId == item.Id).Count();
                nhanvien.SoLuongHuy = query.Where(c => c.NguoiHuyId == item.Id).Count();
                var _giatri = query.Where(c => c.NguoiTaoId == item.Id && c.TrangThaiId != (int)ENTrangThaiDatVe.HUY).Select(g => g.GiaTien).ToList();
                if (_giatri.Count > 0)
                    nhanvien.GiaTri = _giatri.Sum();
                tknhanvien.Add(nhanvien);
            }
            return tknhanvien.OrderByDescending(c => c.GiaTri).ToList();
        }
        public List<KhachHangMuaVeItem> GetDetailDoanhThu(int nhaxeid, DateTime ngaydi, int nhanvienid = 0)
        {
            var phoives = _datveRepository.Table
              .Where(c => c.NhaXeId == nhaxeid && c.NgayDi.Year == ngaydi.Year
                  && c.NgayDi.Month == ngaydi.Month && c.NgayDi.Day == ngaydi.Day
                  && (c.NguoiTaoId == nhanvienid || nhanvienid == 0)
                  && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                  )
                  .ToList()
                .Select(g => new KhachHangMuaVeItem
                {
                    TrangThaiPhoiVeId = g.TrangThaiId,
                    KyHieuGhe = g.sodoghe.Val,
                    GiaVe = g.GiaTien,
                    NgayDi = g.NgayDi,
                    TenKhachHang=g.khachhang.Ten,
                    SoDienThoai=g.khachhang.DienThoai,
                    ThongTinChuyenDi = g.chuyendi.ChuyenDitoText(true)
                }).ToList();

            return phoives;
        }
        public List<ThongKeItem> GetBaoCaoDoanhThu(int thang, int nam, int nhaxeid, ENBaoCaoChuKyThoiGian ChuKyThoiGianId, string HanhTrinhIds = "")
        {

            if (ChuKyThoiGianId == ENBaoCaoChuKyThoiGian.HangThang)
                thang = 0;
            if (ChuKyThoiGianId == ENBaoCaoChuKyThoiGian.HangNam)
                nam = 0;
            var _queryphoives = _datveRepository.Table
                .Where(c => c.NhaXeId == nhaxeid && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                    && (c.NgayDi.Month == thang || thang == 0) && (c.NgayDi.Year == nam || nam == 0));            
            if(!string.IsNullOrEmpty(HanhTrinhIds))
            {
                var hanhtrinhids = Array.ConvertAll(HanhTrinhIds.Split(','), int.Parse);
                _queryphoives = _queryphoives.Where(c => hanhtrinhids.Contains(c.HanhTrinhId));
            }

            var phoives = _queryphoives
             .Select(c => new
             {
                 Ngay = c.NgayDi.Day,
                 Thang = c.NgayDi.Month,
                 Nam = c.NgayDi.Year,
                 TongDoanhThu = c.GiaTien,
                 ischonve = false,
             }).ToList();
            if (ChuKyThoiGianId == ENBaoCaoChuKyThoiGian.HangNgay)
            {
                var doanhthungay = phoives.GroupBy(c => c.Ngay).Select(g => new ThongKeItem
                {
                    Nhan = g.Key.ToString(),
                    NhanSapXep = g.Key.ToString().PadLeft(9,'0'),
                    GiaTri = g.Sum(a => a.TongDoanhThu),
                    SoLuong = g.Count(),
                    GiaTri1 = g.Where(a => a.ischonve == true).Sum(a => a.TongDoanhThu),
                    GiaTri2 = g.Where(a => a.ischonve == false).Sum(a => a.TongDoanhThu)

                }).ToList();

                return doanhthungay;

            }
            if (ChuKyThoiGianId == ENBaoCaoChuKyThoiGian.HangThang)
            {
                var doanhthuthang = phoives.GroupBy(c => c.Thang).Select(g => new ThongKeItem
                {
                    Nhan = g.Key.ToString(),
                    NhanSapXep = g.Key.ToString().PadLeft(9, '0'),
                    GiaTri = g.Sum(a => a.TongDoanhThu),
                    SoLuong = g.Count(),
                    GiaTri1 = g.Where(a => a.ischonve == true).Sum(a => a.TongDoanhThu),
                    GiaTri2 = g.Where(a => a.ischonve == false).Sum(a => a.TongDoanhThu)
                }).ToList();
                return doanhthuthang;
            }
            if (ChuKyThoiGianId == ENBaoCaoChuKyThoiGian.HangNam)
            {
                var doanhthunam = phoives.GroupBy(c => c.Nam).Select(g => new ThongKeItem
                {
                    Nhan = g.Key.ToString(),
                    NhanSapXep = g.Key.ToString().PadLeft(9, '0'),
                    GiaTri = g.Sum(a => a.TongDoanhThu),
                    SoLuong = g.Count(),
                    GiaTri1 = g.Where(a => a.ischonve == true).Sum(a => a.TongDoanhThu),
                    GiaTri2 = g.Where(a => a.ischonve == false).Sum(a => a.TongDoanhThu)
                }).ToList();
                return doanhthunam;
            }

            return null;

        }
        void ProcessTime(int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId, out int Thang1, out int Thang2)
        {
            Thang1 = thang;
            Thang2 = thang;

            if (LoaiThoiGianId == ENBaoCaoLoaiThoiGian.TheoQuy)
            {
                switch (QuyId)
                {
                    case ENBaoCaoQuy.Quy1:
                        Thang1 = 1;
                        Thang2 = 3;
                        break;
                    case ENBaoCaoQuy.Quy2:
                        Thang1 = 4;
                        Thang2 = 6;
                        break;
                    case ENBaoCaoQuy.Quy3:
                        Thang1 = 7;
                        Thang2 = 9;
                        break;
                    case ENBaoCaoQuy.Quy4:
                        Thang1 = 10;
                        Thang2 = 12;
                        break;
                }
            }
            else if (LoaiThoiGianId == ENBaoCaoLoaiThoiGian.TheoNam)
            {
                Thang1 = 1;
                Thang2 = 12;
            }
        }
        decimal GetTongDoanhThu(List<DoanhThuItem> doanhthus, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId)
        {
            decimal _doanhthu = decimal.Zero;
            foreach (var item in doanhthus)
            {
                switch (LoaiThoiGianId)
                {
                    case ENBaoCaoLoaiThoiGian.TheoThang:
                        {
                            if (item.Nam == nam && item.Thang == thang)
                            {
                                _doanhthu = _doanhthu + item.DoanhThu;

                            }
                            break;
                        }
                    case ENBaoCaoLoaiThoiGian.TheoQuy:
                        {
                            thang = 0;
                            if (item.Nam == nam)
                            {
                                switch (QuyId)
                                {
                                    case ENBaoCaoQuy.Quy1:
                                        if (item.Thang >= 1 && item.Thang <= 3)
                                            _doanhthu = _doanhthu + item.DoanhThu;
                                        break;
                                    case ENBaoCaoQuy.Quy2:
                                        if (item.Thang >= 4 && item.Thang <= 6)
                                            _doanhthu = _doanhthu + item.DoanhThu;
                                        break;
                                    case ENBaoCaoQuy.Quy3:
                                        if (item.Thang >= 7 && item.Thang <= 9)
                                            _doanhthu = _doanhthu + item.DoanhThu;
                                        break;
                                    case ENBaoCaoQuy.Quy4:
                                        if (item.Thang >= 10 && item.Thang <= 12)
                                            _doanhthu = _doanhthu + item.DoanhThu;
                                        break;
                                }
                            }
                            break;
                        }
                    case ENBaoCaoLoaiThoiGian.TheoNam:
                        {
                            QuyId = 0;
                            thang = 0;
                            if (item.Nam == nam)
                            {
                                _doanhthu = _doanhthu + item.DoanhThu;
                            }
                            break;
                        }
                }

            }
            return _doanhthu;
        }
        public virtual decimal DoanhThuTuyen(int HanhTrinhId, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId,string GioBan,string NgayBan, out int SoLuong)
        {
            if(LoaiThoiGianId==ENBaoCaoLoaiThoiGian.TheoNgay)
            {
                decimal _doanhthu = decimal.Zero;
                var ngayban = Convert.ToDateTime(NgayBan);
                var phoives = _datveRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId
                    && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                    && c.NgayDi.Year == ngayban.Year && c.NgayDi.Month == ngayban.Month && c.NgayDi.Day == ngayban.Day).ToList();
                foreach (var item in phoives)
                {
                    _doanhthu = _doanhthu + item.GiaTien;
                }
                SoLuong = phoives.Count();
                return _doanhthu;
            }
            else
            {
                int Thang1, Thang2;
                ProcessTime(thang, nam, QuyId, LoaiThoiGianId, out Thang1, out Thang2);
                var phoives = _datveRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                    && c.NgayDi.Year == nam
                    && (c.NgayDi.Month >= Thang1 && c.NgayDi.Month <= Thang2)
                    )
                    .Select(c => new DoanhThuItem
                    {
                        Ngay = c.NgayDi.Day,
                        Thang = c.NgayDi.Month,
                        Nam = c.NgayDi.Year,
                        DoanhThu = c.GiaTien,
                    }).ToList();
                SoLuong = phoives.Count;
                return GetTongDoanhThu(phoives, thang, nam, QuyId, LoaiThoiGianId);
            }           
        }
        public virtual decimal DoanhThuLichTrinh(int lichtrinhid,int HanhTrinhId, int thang, int nam, ENBaoCaoQuy QuyId, ENBaoCaoLoaiThoiGian LoaiThoiGianId,string NgayBan, out int SoLuong)
        {
            if(HanhTrinhId==0)
            {
                if (LoaiThoiGianId == ENBaoCaoLoaiThoiGian.TheoNgay)
                {
                    var _ngayban = Convert.ToDateTime(NgayBan);
                    var phoives = _datveRepository.Table.Where(c => c.LichTrinhId == lichtrinhid
                    && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                    && c.NgayDi.Year == _ngayban.Year
                    && (c.NgayDi.Month == _ngayban.Month && c.NgayDi.Day == _ngayban.Day)
                    ).ToList();
                    decimal _doanhthu = decimal.Zero;
                    foreach (var item in phoives)
                    {
                        _doanhthu = _doanhthu + item.GiaTien;
                    }
                    SoLuong = phoives.Count();
                    return _doanhthu;
                    
                }
                else
                {
                    int Thang1, Thang2;
                    ProcessTime(thang, nam, QuyId, LoaiThoiGianId, out Thang1, out Thang2);
                    var phoives = _datveRepository.Table.Where(c => c.LichTrinhId == lichtrinhid
                        && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                        && c.NgayDi.Year == nam
                        && (c.NgayDi.Month >= Thang1 && c.NgayDi.Month <= Thang2)
                        )
                        .Select(c => new DoanhThuItem
                        {
                            Ngay = c.NgayDi.Day,
                            Thang = c.NgayDi.Month,
                            Nam = c.NgayDi.Year,
                            DoanhThu = c.GiaTien
                        }).ToList();
                    SoLuong = phoives.Count;
                    return GetTongDoanhThu(phoives, thang, nam, QuyId, LoaiThoiGianId);
                }               
            }
            else
            {
                if(LoaiThoiGianId==ENBaoCaoLoaiThoiGian.TheoNgay)
                {
                    var _ngayban = Convert.ToDateTime(NgayBan);
                    var phoives = _datveRepository.Table.Where(c => c.LichTrinhId == lichtrinhid && c.HanhTrinhId == HanhTrinhId
                        && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                        && c.NgayDi.Year == _ngayban.Year
                        && (c.NgayDi.Month == _ngayban.Month && c.NgayDi.Day == _ngayban.Day)
                        ).ToList();
                        decimal _doanhthu = decimal.Zero;
                        foreach (var item in phoives)
                        {
                            _doanhthu = _doanhthu + item.GiaTien;
                        }
                        SoLuong = phoives.Count();
                        return _doanhthu;
                }
                else
                {
                    int Thang1, Thang2;
                    ProcessTime(thang, nam, QuyId, LoaiThoiGianId, out Thang1, out Thang2);
                    var phoives = _datveRepository.Table.Where(c => c.LichTrinhId == lichtrinhid && c.HanhTrinhId == HanhTrinhId
                        && (c.TrangThaiId == (int)ENTrangThaiDatVe.DA_DI || c.TrangThaiId == (int)ENTrangThaiDatVe.DA_XEP_CHO)
                        && c.NgayDi.Year == nam
                        && (c.NgayDi.Month >= Thang1 && c.NgayDi.Month <= Thang2)
                        )
                        .Select(c => new DoanhThuItem
                        {
                            Ngay = c.NgayDi.Day,
                            Thang = c.NgayDi.Month,
                            Nam = c.NgayDi.Year,
                            DoanhThu = c.GiaTien
                        }).ToList();
                    SoLuong = phoives.Count;
                    return GetTongDoanhThu(phoives, thang, nam, QuyId, LoaiThoiGianId);
                }               
            }           
        }
        #endregion
        #region Lich Trinh Loai Xe
        public virtual LichTrinhLoaiXe GetLichTrinhLoaiXe(int HanhTrinhId, int LichTrinhId, int LoaiXeId)
        {
            return _lichtrinhloaixeRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && c.LichTrinhId == LichTrinhId && c.LoaiXeId == LoaiXeId).FirstOrDefault();
        }
        public virtual LichTrinhLoaiXe GetLichTrinhLoaiXeById(int Id)
        {
            if (Id == 0)
                return null;
            return _lichtrinhloaixeRepository.GetById(Id);
        }
        #endregion 
        #region Dat ve note
         public virtual  void InsertDatVeNote(int DatVeId, string note)
        {
            var item = new DatVeNote();
            item.NgayTao = DateTime.Now;
            item.DatVeId = DatVeId;
            item.Note = note;             
            _datvenoteRepository.Insert(item);
        }
         public virtual PagedList<DatVeNote> GetLichSuDatVe(DateTime? NgayGiaoDich = null, int HanhTrinhId = 0,string StringSearch="", int pageIndex = 0,
               int pageSize = int.MaxValue)
         {
             var query = _datvenoteRepository.Table
                  .Join(_datveRepository.Table, pvn => pvn.DatVeId, nv => nv.Id, (pvn, nv) => new { DatVenote = pvn, Datve = nv })
                .Where(c => c.Datve.HanhTrinhId == HanhTrinhId
                     && c.Datve.NgayDi.Year == NgayGiaoDich.Value.Year
                     && c.Datve.NgayDi.Month == NgayGiaoDich.Value.Month
                     && c.Datve.NgayDi.Day == NgayGiaoDich.Value.Day);
             if(!string.IsNullOrEmpty(StringSearch))
             {
                 query=query.Where(c=>c.Datve.Ma.Contains(StringSearch) || c.Datve.nguoitao.HoVaTen.Contains(StringSearch) || c.Datve.khachhang.Ten.Contains(StringSearch));
             }

             return new PagedList<DatVeNote>(query.Select(c => c.DatVenote).OrderByDescending(c=>c.Id), pageIndex, pageSize);
         }
         public virtual List<DatVeNote> GetLichSuDatVeByDatVeId(int DatVeId)
         {
             return _datvenoteRepository.Table.Where(c => c.DatVeId == DatVeId).OrderByDescending(c=>c.Id).ToList();
         }
        #endregion
         #region Cau Hinh Gia Ve
         //co 2 hinh thuc cau hinh gia ve
         //1. Cau hinh gia ve the lich trinh, hanh trinh, loai xe (cv_lichtrinh_loaixe) ->tat ca cung gia ve
         //2. Cau hinh gia ve theo so do ghe xe, loai xe, hanh trinh(cv_hanhtrinh_loaixe)
         //==>viec lay gia ve luc dat ghe se dua vao chuyen di dang ap dung theo cau hinh gia ve nao 
         //muc uu tien lay gia ve khi co cau hinh gia ve theo so do, neu khong co se lay gia ve chung
         public virtual List<LichTrinhLoaiXe> GetAllLichTrinhLoaiXe(int NhaXeId)
         {
             var hanhtrinhids = _hanhtrinhRepository.Table.Where(c => c.NhaXeId == NhaXeId).Select(c => c.Id).ToList();
             var query = _lichtrinhloaixeRepository.Table.Where(c => hanhtrinhids.Contains(c.HanhTrinhId)).Select(c => new { c.HanhTrinhId, c.hanhtrinh, c.LoaiXeId, c.loaixe, c.GiaVe }).Distinct().ToList().Select(g => new LichTrinhLoaiXe
             {
                 HanhTrinhId = g.HanhTrinhId,
                 hanhtrinh=g.hanhtrinh,
                 LoaiXeId = g.LoaiXeId,
                 loaixe=g.loaixe,
                 GiaVe = g.GiaVe
             }).ToList();
             return query;
         }
         public virtual void UpdateGiaVe(int HanhTrinhId, int LichTrinhId, int LoaiXeId, decimal GiaVe)
         {
             var item = _lichtrinhloaixeRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && c.LichTrinhId == LichTrinhId && c.LoaiXeId == LoaiXeId).FirstOrDefault();
             if(item==null)
             {
                 item = new LichTrinhLoaiXe();
                 item.HanhTrinhId = HanhTrinhId;
                 item.LichTrinhId = LichTrinhId;
                 item.LoaiXeId = LoaiXeId;
                 item.GiaVe = GiaVe;
                 _lichtrinhloaixeRepository.Insert(item);
             }
             else
             {
                 item.GiaVe = GiaVe;
                 _lichtrinhloaixeRepository.Update(item);
             }
         }
         public virtual List<HanhTrinhLoaiXeGiaVe> GetAllHanhTrinhLoaiXeGiaVe(int HanhTrinhId, int LoaiXeId)
         {
             var query = _hanhtrinhloaixeRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && c.LoaiXeId == LoaiXeId).ToList();
             return query;
         }
         public virtual void UpdateGiaVeSoDo(int HanhTrinhId, int LoaiXeId, int SoDoGheId, decimal GiaVe)
         {             
             var item = _hanhtrinhloaixeRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && c.LoaiXeId == LoaiXeId && c.SoDoGheId == SoDoGheId).FirstOrDefault();
             if (item == null)
             {
                 item = new HanhTrinhLoaiXeGiaVe();
                 item.HanhTrinhId = HanhTrinhId;
                 item.SoDoGheId = SoDoGheId;
                 item.LoaiXeId = LoaiXeId;
                 item.GiaVe = GiaVe;
                 _hanhtrinhloaixeRepository.Insert(item);
             }
             else
             {
                 item.GiaVe = GiaVe;
                 _hanhtrinhloaixeRepository.Update(item);
             }
         }
         public virtual Decimal GetGiaVeTheoSoDoId(int HanhTrinhId, int LoaiXeId, int SoDoGheId)
         {
             var item = _hanhtrinhloaixeRepository.Table.Where(c => c.HanhTrinhId == HanhTrinhId && c.LoaiXeId == LoaiXeId && c.SoDoGheId == SoDoGheId).FirstOrDefault();
             if(item!=null)
                return item.GiaVe;
             return decimal.Zero;
         }
         #endregion
    }
}
