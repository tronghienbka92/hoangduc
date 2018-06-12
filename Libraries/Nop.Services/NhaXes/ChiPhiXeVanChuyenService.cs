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
    public class ChiPhiXeVanChuyenService : IChiPhiXeVanChuyenService
    {
        #region Init
        private readonly IDbContext _dbContext;
        private readonly IRepository<ChiPhiXe> _chiphixeRepository;
        private readonly IRepository<HangMucChiPhi> _hangmucchiphiRepository;
        public ChiPhiXeVanChuyenService(
            IRepository<ChiPhiXe> chiphixeRepository,
             IRepository<HangMucChiPhi> hangmucchiphiRepository,
            IDbContext dbContext
            )
        {
            this._chiphixeRepository = chiphixeRepository;
            this._hangmucchiphiRepository = hangmucchiphiRepository;
            this._dbContext = dbContext;

        }
        #endregion
        public virtual ChiPhiXe GetById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _chiphixeRepository.GetById(itemId);
        }
        public virtual void Insert(ChiPhiXe _item)
        {
            if (_item == null)
                throw new ArgumentNullException("ChiPhiXe");
            _chiphixeRepository.Insert(_item);
        }
        public virtual void Update(ChiPhiXe _item)
        {
            if (_item == null)
                throw new ArgumentNullException("ChiPhiXe");
            _chiphixeRepository.Update(_item);
        }
        public virtual void Delete(ChiPhiXe _item)
        {
            if (_item == null)
                throw new ArgumentNullException("ChiPhiXe");
            _chiphixeRepository.Delete(_item);
        }
        public virtual List<ChiPhiXe> GetAllChiPhiXes(int NhaXeId, int XeVanChuyenId = 0, int HangMucId = 0, DateTime? TuNgay = null, DateTime? DenNgay = null)
        {
            var query = _chiphixeRepository.Table.Where(c => c.NhaXeId == NhaXeId && !c.IsDeleted);
            if (XeVanChuyenId > 0)
                query = query.Where(c => c.XeVanChuyenId == XeVanChuyenId);
            if (HangMucId > 0)
                query = query.Where(c => c.HangMucChiPhiId == HangMucId);
            if (TuNgay.HasValue)
            {
                var _tungay = TuNgay.Value.Date;
                query = query.Where(c => c.NgayGiaoDich >= _tungay);
            }
            if (DenNgay.HasValue)
            {
                var _denngay = DenNgay.Value.Date;
                query = query.Where(c => c.NgayGiaoDich <= _denngay);
            }
            return query.ToList();
        }

        public virtual List<HangMucChiPhi> GetAllHangMucChiPhi(int NhaXeId)
        {
            return _hangmucchiphiRepository.Table.Where(c => c.NhaXeId == NhaXeId).ToList();
        }
        public virtual HangMucChiPhi GetHangMucById(int itemid)
        {
            if (itemid == 0)
                return null;

            return _hangmucchiphiRepository.GetById(itemid);
        }
        public virtual void InsertHangMuc(HangMucChiPhi _item)
        {
            if (_item == null)
                throw new ArgumentNullException("ChiPhiXe");
            _hangmucchiphiRepository.Insert(_item);
        }
        public virtual void UpdateHangMuc(HangMucChiPhi _item)
        {
            if (_item == null)
                throw new ArgumentNullException("ChiPhiXe");
            _hangmucchiphiRepository.Update(_item);
        }
        public virtual void DeleteHangMuc(HangMucChiPhi _item)
        {
            if (_item == null)
                throw new ArgumentNullException("ChiPhiXe");
            _hangmucchiphiRepository.Delete(_item);
        }
    }
}
