using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Chonves;

namespace Nop.Services.Chonves
{
    public class ChonVeService : IChonVeService
    {

        #region Ctor
        private readonly IRepository<HopDong> _hopdongRepository;
        private readonly IRepository<DangKyPhanMem> _DangKyPhanMemRepository;
        private readonly ICacheManager _cacheManager;

        public ChonVeService(ICacheManager cacheManager,
            IRepository<HopDong> HopDongRepository,
            IRepository<DangKyPhanMem> DangKyPhanMemRepository
            )
        {
            this._DangKyPhanMemRepository = DangKyPhanMemRepository;
            this._hopdongRepository = HopDongRepository;
            this._cacheManager = cacheManager;
        }
        #endregion

        #region Hop Dong
        public virtual PagedList<HopDong> GetAllHopDong(string mahopdong = "", 
            string tenhopdong = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            bool showHidden = false,
            int OwnerID =0,
            ENTrangThaiHopDong trangthai = ENTrangThaiHopDong.TatCa)
        {
            var query = _hopdongRepository.Table;
            if (!showHidden)
                query = query.Where(m => m.TrangThaiID!=(int)ENTrangThaiHopDong.Huy);
            if (!String.IsNullOrWhiteSpace(mahopdong))
                query = query.Where(m => m.MaHopDong.Contains(mahopdong));
            if (!String.IsNullOrWhiteSpace(tenhopdong))
                query = query.Where(m => m.TenHopDong.Contains(tenhopdong));
            if (trangthai != ENTrangThaiHopDong.TatCa)
            {
                query = query.Where(m => m.TrangThaiID == (int)trangthai);
            }
            if (OwnerID > 0)
                query = query.Where(m => m.NguoiTaoID == OwnerID);
           
            query = query.OrderBy(m => m.MaHopDong);

            return new PagedList<HopDong>(query, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets a HopDong
        /// </summary>
        /// <param name="HopDongId">HopDong identifier</param>
        /// <returns>HopDong</returns>
        public virtual HopDong GetHopDongById(int HopDongId)
        {
            if (HopDongId == 0)
                return null;

            return _hopdongRepository.GetById(HopDongId);
        }
       

        /// <summary>
        /// Inserts HopDong
        /// </summary>
        /// <param name="HopDong">HopDong</param>
        public virtual void InsertHopDong(HopDong _item)
        {
            if (_item == null)
                throw new ArgumentNullException("HopDong");

            _hopdongRepository.Insert(_item);
            _item.MaHopDong = string.Format("HD{0}{1}",DateTime.Now.ToString("yyyyMM"), _item.Id.ToString().PadLeft(6, '0'));
            _hopdongRepository.Update(_item);

        }

        /// <summary>
        /// Updates the HopDong
        /// </summary>
        /// <param name="HopDong">HopDong</param>
        public virtual void UpdateHopDong(HopDong _item)
        {
            if (_item == null)
                throw new ArgumentNullException("HopDong");
            _hopdongRepository.Update(_item);
            
        }
        public virtual void DeleteHopDong(HopDong _item)
        {
            if (_item == null)
                throw new ArgumentNullException("HopDong");

            _item.TrangThai = ENTrangThaiHopDong.Huy;
            UpdateHopDong(_item);
        }
        #endregion

        #region XuLyHopDong
        public List<int> GetNguoiTaoIds()
        {
            var query = _hopdongRepository.Table.Select(c => c.NguoiTaoID).Distinct();
            return query.ToList();
        }

        #endregion

        #region Khac
        public PagedList<DangKyPhanMem> GetAllDangKyPhanMem(string thongtin = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            var query = _DangKyPhanMemRepository.Table;
            
            if (!String.IsNullOrWhiteSpace(thongtin))
                query = query.Where(m => m.Ten.Contains(thongtin) || m.DiaChi.Contains(thongtin) || m.SoDienThoai.Contains(thongtin) || m.GhiChu.Contains(thongtin) || m.Email.Contains(thongtin));
            query = query.OrderByDescending(m => m.Id);

            return new PagedList<DangKyPhanMem>(query, pageIndex, pageSize);
        }
        /// <summary>
        /// Gets a DangKyPhanMem
        /// </summary>
        /// <param name="DangKyPhanMemId">DangKyPhanMem identifier</param>
        /// <returns>DangKyPhanMem</returns>
        public virtual DangKyPhanMem GetDangKyPhanMemById(int DangKyPhanMemId)
        {
            if (DangKyPhanMemId == 0)
                return null;

            return _DangKyPhanMemRepository.GetById(DangKyPhanMemId);
        }


        /// <summary>
        /// Inserts DangKyPhanMem
        /// </summary>
        /// <param name="DangKyPhanMem">DangKyPhanMem</param>
        public virtual void InsertDangKyPhanMem(DangKyPhanMem _item)
        {
            if (_item == null)
                throw new ArgumentNullException("DangKyPhanMem");

            _DangKyPhanMemRepository.Insert(_item);

        }

        /// <summary>
        /// Updates the DangKyPhanMem
        /// </summary>
        /// <param name="DangKyPhanMem">DangKyPhanMem</param>
        public virtual void UpdateDangKyPhanMem(DangKyPhanMem _item)
        {
            if (_item == null)
                throw new ArgumentNullException("DangKyPhanMem");
            _DangKyPhanMemRepository.Update(_item);

        }
        public virtual void DeleteDangKyPhanMem(DangKyPhanMem _item)
        {
            if (_item == null)
                throw new ArgumentNullException("DangKyPhanMem");

            _DangKyPhanMemRepository.Delete(_item);
        }
        #endregion
    }
}
