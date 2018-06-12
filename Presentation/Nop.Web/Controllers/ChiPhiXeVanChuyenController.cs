using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Tax;
using Nop.Services.Authentication;
using Nop.Services.Authentication.External;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Web.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.UI.Captcha;
using Nop.Web.Models.Common;
using Nop.Web.Models.Customer;
using WebGrease.Css.Extensions;
using Nop.Web.Models.NhaXes;
using Nop.Core.Data;
using Nop.Services.NhaXes;
using Nop.Core.Caching;
using Nop.Core.Domain.News;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using Nop.Services.Chonves;
using Nop.Services.Security;
using Nop.Core.Domain.Security;
using System.Globalization;
using Nop.Services.Catalog;
using Nop.Web.Models.VeXeKhach;
using Nop.Core.Domain.Chonves;
using Nop.Web.Models.NhaXeBanVe;
using Nop.Web.Infrastructure.Cache;
using System.IO;
using Nop.Web.Models.ChiPhiXeVanChuyen;

namespace Nop.Web.Controllers
{
    public class ChiPhiXeVanChuyenController : BaseNhaXeController
    {
        #region Khoi Tao
        private readonly IStateProvinceService _stateProvinceService;
        private readonly INhaXeService _nhaxeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly IChonVeService _chonveService;
        private readonly IDiaChiService _diachiService;
        private readonly INhanVienService _nhanvienService;
        private readonly IPermissionService _permissionService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly CustomerSettings _customerSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreService _storeService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IXeInfoService _xeinfoService;
        private readonly IHanhTrinhService _hanhtrinhService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ILimousineBanVeService _limousinebanveService;
        private readonly IChiPhiXeVanChuyenService _chiphixeService;

        public ChiPhiXeVanChuyenController(
            IChiPhiXeVanChuyenService chiphixeService,
            IStateProvinceService stateProvinceService,
            INhaXeService nhaxeService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            ICustomerService customerService,
            IChonVeService chonveService,
            IDiaChiService diachiService,
            INhanVienService nhanvienService,
            IPermissionService permissionService,
            IDateTimeHelper dateTimeHelper,
            CustomerSettings customerSettings,
            DateTimeSettings dateTimeSettings,
            ICustomerActivityService customerActivityService,
            IGenericAttributeService genericAttributeService,
            IStoreService storeService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IXeInfoService xeinfoService,
            IHanhTrinhService hanhtrinhService,
            IPriceFormatter priceFormatter,
            ILimousineBanVeService limousinebanveService
            )
        {
            this._chiphixeService = chiphixeService;
            this._stateProvinceService = stateProvinceService;
            this._nhaxeService = nhaxeService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._customerService = customerService;
            this._chonveService = chonveService;
            this._diachiService = diachiService;
            this._nhanvienService = nhanvienService;
            this._permissionService = permissionService;
            this._dateTimeHelper = dateTimeHelper;
            this._customerSettings = customerSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._customerActivityService = customerActivityService;
            this._genericAttributeService = genericAttributeService;
            this._storeService = storeService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._xeinfoService = xeinfoService;
            this._hanhtrinhService = hanhtrinhService;
            this._priceFormatter = priceFormatter;
            this._limousinebanveService = limousinebanveService;

        }
        #endregion
        #region common
        ChiPhiXeModel toModel(ChiPhiXe item)
        {
            var model = new ChiPhiXeModel();
            model.Id = item.Id;
            model.Ma = item.Ma;
            model.NhaXeId = item.NhaXeId;
            model.NgayGiaoDich = item.NgayGiaoDich;
            model.NgayTao = item.NgayTao;
            model.HangMucChiPhiId = item.HangMucChiPhiId;
            model.TenHangMuc = item.hangmuc.Ten;
            model.TenCongViec = item.TenCongViec;
            model.ThoiGian = item.ThoiGian;
            model.XeVanChuyenId = item.XeVanChuyenId;
            model.BienSo = item.xevanchuyen.BienSo;
            model.TenLaiXe = "";
            if (item.xevanchuyen.laixe != null)
                model.TenLaiXe = item.xevanchuyen.laixe.HoVaTen;
            model.GhiChu = item.GhiChu;
            model.ChiPhi = item.ChiPhi;
            return model;

        }
        void toEntity(ChiPhiXe entity, ChiPhiXeModel item)
        {
            if (entity == null)
                entity = new ChiPhiXe();
            entity.NhaXeId = _workContext.NhaXeId;
            entity.NgayGiaoDich = item.NgayGiaoDich;
            entity.HangMucChiPhiId = item.HangMucChiPhiId;
            entity.TenCongViec = item.TenCongViec;
            entity.ThoiGian = item.ThoiGian;
            entity.XeVanChuyenId = item.XeVanChuyenId;
            entity.GhiChu = item.GhiChu;
            entity.ChiPhi = item.ChiPhi;            


        }
        #endregion
        public ActionResult List()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVBaoCao))
                return AccessDeniedView();
            var model = new ChiPhiXeListModel();
            var hangmucs = _chiphixeService.GetAllHangMucChiPhi(_workContext.NhaXeId);
            model.hangmucs = hangmucs.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Ten
            }).ToList();
            model.hangmucs.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "----Chọn hạng mục----"
            });
            model.AllXeInfo = _xeinfoService.GetAllXeInfoByNhaXeId(_workContext.NhaXeId).Select(c =>
            {
                return new XeXuatBenItemModel.XeVanChuyenInfo(c.Id, c.BienSo);
            }).ToList();
            model.TuNgay = DateTime.Now.AddMonths(-1);
            model.DenNgay = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult ListChiPhi(DataSourceRequest command, ChiPhiXeListModel model)
        {

            var items = _chiphixeService.GetAllChiPhiXes(_workContext.NhaXeId, model.XeVanChuyenListId, model.HangMucChiPhiListId, model.TuNgay, model.DenNgay);
            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {
                    var modelcp = toModel(x);
                    return modelcp;
                }),
                Total = items.Count
            };
            return Json(gridModel);

        }
        public ActionResult _ChiPhiXeChinhSua(int Id, int HangMucId, int XeId)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVBaoCao))
                return AccessDeniedView();
            var model = new ChiPhiXeModel();
            if (Id > 0)
            {
                var item = _chiphixeService.GetById(Id);
                model = toModel(item);
                //bin theo thong tin
            }
            else
            {
                model.HangMucChiPhiId = HangMucId;
                model.XeVanChuyenId = XeId;
                model.NgayGiaoDich = DateTime.Now.Date;
                if(model.XeVanChuyenId>0)
                {
                    var xeinfo = _xeinfoService.GetXeInfoById(model.XeVanChuyenId);
                    model.BienSo = xeinfo.BienSo;
                    if (xeinfo.laixe!=null)
                        model.TenLaiXe = xeinfo.laixe.HoVaTen;
                }
            }

            model.hangmucs = _chiphixeService.GetAllHangMucChiPhi(_workContext.NhaXeId).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Ten,
                Selected=c.Id==model.HangMucChiPhiId
            }).ToList();
            //bin theo thong tin
            return PartialView(model);


        }
        [HttpPost]
        public ActionResult ChiPhiXeChinhSua(ChiPhiXeModel model)
        {

            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVBaoCao))
                return AccessDeniedView();
            if (model.Id > 0)
            {
                var item = _chiphixeService.GetById(model.Id);
                toEntity(item, model);
                _chiphixeService.Update(item);
            }
            else
            {
                var item = new ChiPhiXe();
                toEntity(item, model);
                item.NguoiTaoId = _workContext.CurrentNhanVien.Id;
                _chiphixeService.Insert(item);
            }

            return ThanhCong();
        }
        [HttpPost]
        public ActionResult XoaChiPhiXe(int Id)
        {

            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVBaoCao))
                return AccessDeniedView();
            if (Id > 0)
            {
                var item = _chiphixeService.GetById(Id);
                item.IsDeleted = true;
                _chiphixeService.Update(item);
            }
            return ThanhCong();
        }
       
        public ActionResult BaoCao()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVBaoCao))
                return AccessDeniedView();
            var model = new ChiPhiXeListModel();
            var hangmucs = _chiphixeService.GetAllHangMucChiPhi(_workContext.NhaXeId);
            model.hangmucs = hangmucs.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Ten
            }).ToList();
            model.hangmucs.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "----Chọn hạng mục----"
            });
            model.AllXeInfo = _xeinfoService.GetAllXeInfoByNhaXeId(_workContext.NhaXeId).Select(c =>
            {
                return new XeXuatBenItemModel.XeVanChuyenInfo(c.Id, c.BienSo);
            }).ToList();
            model.TuNgay = DateTime.Now.AddMonths(-1);
            model.DenNgay = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult ListChiPhiBaoCao(DataSourceRequest command, ChiPhiXeListModel model)
        {

            var items = _chiphixeService.GetAllChiPhiXes(_workContext.NhaXeId, model.XeVanChuyenListId, model.HangMucChiPhiListId, model.TuNgay, model.DenNgay);
            var xeids = items.Select(c => c.XeVanChuyenId).ToArray();
            var xeinfos = _xeinfoService.GetAllXeInfoByNhaXeId(_workContext.NhaXeId).Where(c=>xeids.Contains(c.Id)).ToList();
            var models = new List<ChiPhiXeModel>();
            foreach (var xe in xeinfos)
            {
                var m = new ChiPhiXeModel();
                m.BienSo = xe.BienSo;
                m.TenLaiXe = xe.laixe != null ? xe.laixe.HoVaTen : "";
                var chiphis = items.Where(c => c.XeVanChuyenId == xe.Id).ToList();
                if (chiphis.Count>0)
                {
                    m.TenHangMuc = chiphis[0].hangmuc.Ten;
                    m.ChiPhi = chiphis.Sum(c => c.ChiPhi);
                }
                models.Add(m);
            }
            var gridModel = new DataSourceResult
            {
                Data = models,
                Total = models.Count
            };
            return Json(gridModel);

        }
    }
}