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

namespace Nop.Web.Controllers
{
    public class HopDongChuyenController : BaseNhaXeController
    {
       
        #region Khoi Tao
        private readonly IStateProvinceService _stateProvinceService;
        private readonly INhaXeService _nhaxeService;
        private readonly IHopDongChuyenService _hopdongchuyenService;
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
        private readonly ICacheManager _cacheManager;

        public HopDongChuyenController(
            ICacheManager cacheManager,
            IStateProvinceService stateProvinceService,
            IHopDongChuyenService hopdongchuyenService,
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
            this._cacheManager = cacheManager;
            this._stateProvinceService = stateProvinceService;
            this._nhaxeService = nhaxeService;
            this._localizationService = localizationService;
            this._hopdongchuyenService = hopdongchuyenService;
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
        #region Common
        public ActionResult Index()
        {
            var model = new LimousineIndexModel();
            int sldv, slchuyen;
            decimal doanhthu;
            _limousinebanveService.GetLimousineIndex(_workContext.NhaXeId, DateTime.Now, ENBaoCaoChuKyThoiGian.HangNgay, out sldv, out doanhthu, out slchuyen);
            model.ngayhientai = new LimousineIndexModel.ThongKe(sldv, doanhthu, slchuyen);
            _limousinebanveService.GetLimousineIndex(_workContext.NhaXeId, DateTime.Now.AddDays(-1), ENBaoCaoChuKyThoiGian.HangNgay, out sldv, out doanhthu, out slchuyen);
            model.ngayhomqua = new LimousineIndexModel.ThongKe(sldv, doanhthu, slchuyen);
            _limousinebanveService.GetLimousineIndex(_workContext.NhaXeId, DateTime.Now, ENBaoCaoChuKyThoiGian.HangThang, out sldv, out doanhthu, out slchuyen);
            model.trongthang = new LimousineIndexModel.ThongKe(sldv, doanhthu, slchuyen);
            return View(model);
        }

        List<SelectListItem> PrepareHanhTrinhList(bool isAll = true, bool isChonHanhTrinh = false, int HanhTrinhId = 0)
        {
            List<HanhTrinh> hanhtrinhs = new List<HanhTrinh>();
            if (isAll || _workContext.CurrentNhanVien.isQuanTri)
                hanhtrinhs = _hanhtrinhService.GetAllHanhTrinhByNhaXeId(_workContext.NhaXeId);
            else
                hanhtrinhs = _hanhtrinhService.GetAllHanhTrinhByNhaXeId(_workContext.NhaXeId, _workContext.CurrentNhanVien.VanPhongs.Select(c => c.Id).ToArray());
            var ddls = hanhtrinhs.Select(c =>
            {
                var item = new SelectListItem();
                item.Text = string.Format("{0} ({1})", c.MoTa, c.MaHanhTrinh);
                item.Value = c.Id.ToString();
                item.Selected = c.Id == HanhTrinhId;
                return item;
            }).ToList();

            if (isChonHanhTrinh)
                ddls.Insert(0, new SelectListItem { Text = "--------Chọn--------", Value = "0", Selected = 0 == HanhTrinhId });
            return ddls;
        }
        List<SelectListItem> PrepareLichTrinhList(int NhaXeId, ref int LichTrinhId, bool isChonLichTrinh = true)
        {
            var _lichtrinhid = LichTrinhId;
            var items = _hanhtrinhService.GetAllLichTrinhByKhungGio(NhaXeId);
            var ddls = items.Select(c =>
           {
               var item = new SelectListItem();
               item.Text = c.toText(false);
               item.Value = c.Id.ToString();
               if (_lichtrinhid > 0)
                   item.Selected = c.Id == _lichtrinhid;
               else
               {
                   //kiem tra thoi gian hien tai de lay thong tin lich trinh hien tai
                   if (!isChonLichTrinh)
                   {
                       DateTime dtfrom = DateTime.Now.Date.AddHours(c.ThoiGianDi.Hour).AddMinutes(c.ThoiGianDi.Minute);
                       DateTime dtto = dtfrom.AddMinutes(c.KhungThoiGian);
                       if (DateTime.Now > dtfrom && DateTime.Now < dtto)
                       {
                           _lichtrinhid = c.Id;
                           item.Selected = true;
                       }

                   }
               }
               return item;
           }).ToList();

            if (isChonLichTrinh)
                ddls.Insert(0, new SelectListItem { Text = "--------Chọn--------", Value = "0", Selected = 0 == LichTrinhId });
            LichTrinhId = _lichtrinhid;
            return ddls;
        }
        List<SelectListItem> PrepareDiemDonList(int HanhTrinhId, int DiemDonId)
        {
            var hanhtrinh = _hanhtrinhService.GetHanhTrinhById(HanhTrinhId);

            var diemdons = hanhtrinh.DiemDons.OrderBy(c=>c.ThuTu).ToList();
            var ddls = diemdons.Select(c =>
        {
            var item = new SelectListItem();
            item.Text = c.diemdon.TenDiemDon;
            item.Value = c.diemdon.Id.ToString();
            item.Selected = c.diemdon.Id == DiemDonId;
            return item;
        }).ToList();
            //bo diem cuoi
            //ddls.RemoveAt(ddls.Count - 1);
            return ddls;
        }
        public ActionResult GetLichTrinh(int HanhTrinhId, bool isAddSelect = true)
        {
            var items = _hanhtrinhService.GetAllLichTrinhByKhungGio(_workContext.NhaXeId);
            var itemdatas = items.Select(c => new
            {
                Id = c.Id,
                MoTa = c.toText(false)
            }).ToList();
            if (isAddSelect)
            {
                itemdatas.Insert(0, new
                {
                    Id = 0,
                    MoTa = "--------Chọn--------"
                });
            }
            return Json(itemdatas, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetKhachHangInNhaXe(string ThongTin,bool? isAdv=false,DateTime? NgayDi=null,int HanhTrinhId=0)
        {
            var khachhangs = _limousinebanveService.GetAllHanhKhach(_workContext.NhaXeId, ThongTin,10).Select(m =>
            {
                var khm = m.toModel();
                //lay thong tin cuoi
                if (isAdv.HasValue && isAdv.Value)
                {
                    var _lasdv = _limousinebanveService.GetDatVeCuoiTheoKhachHangId(m.Id, HanhTrinhId);
                    if(_lasdv!=null)
                    {
                        khm.TenDiemDon = _lasdv.TenDiemDon;
                        khm.TenDiemTra = _lasdv.TenDiemTra;
                    }
                    //lay thong di trong ngay
                    if (NgayDi.HasValue)
                    {
                        var chuyendis = _limousinebanveService.GetAllChuyenDiTheoKhachHang(m.Id, NgayDi.Value);
                        khm.ChuyenDiTrongNgay = "";
                        foreach(var cd in chuyendis)
                        {
                            var cdmodel=cd.toModel(_localizationService);
                            if (String.IsNullOrEmpty(khm.ChuyenDiTrongNgay))
                                khm.ChuyenDiTrongNgay = string.Format("{0}({1})",cd.NgayDiThuc.ToString("HH:mm"), cd.hanhtrinh.MaHanhTrinh);
                            else
                                khm.ChuyenDiTrongNgay = khm.ChuyenDiTrongNgay + string.Format("; {0}({1})", cd.NgayDiThuc.ToString("HH:mm"), cd.hanhtrinh.MaHanhTrinh);
                        }
                    }
                }
                return khm;
            }).ToList();
            return Json(khachhangs, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region hopdongchuyen
        #region Quản lý gửi hàng

        public ActionResult ListHopDongChuyen()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            var model = new ListHopdongChuyenModels();
           
            return View(model);
        }
        [HttpPost]
        public ActionResult ListHopDongChuyen(DataSourceRequest command, ListHopdongChuyenModels model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var items = _hopdongchuyenService.GetAllHopDongChuyen(_workContext.NhaXeId,model.BienSoXe,model.SoHopDong,  command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {
                    var khachhangs = _hopdongchuyenService.GetAllKhachHangByHopDongId(x.Id);
                    var m = x.toModel(_localizationService, khachhangs);
                    return m;
                }),
                Total = items.TotalCount
            };

            return Json(gridModel);            
        }
       
        public ActionResult HopDongChuyenTao()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            var model = new HopDongChuyenModel();
            model.ThoiGianDonKhach = DateTime.Now;
            model.ThoiGianTraKhach = DateTime.Now;
            model.GioDonKhach = DateTime.Now;
            model.GioTraKhach = DateTime.Now;
            model.HTThanhToans = this.GetCVEnumSelectList<ENHinhThucThanhToan>(_localizationService, model.HinhThucThanhToanId);
           
            
            return View(model);
        }
        void HopDongChuyenToEntity (HopDongChuyen e, HopDongChuyenModel model)
        {
            e.SoHopDong = model.SoHopDong;
            e.TenHopDong = model.TenHopDong;
            e.XeVanChuyenId = model.XeVanChuyenId;
            e.ThoiGianDonKhach = model.ThoiGianDonKhach;
            e.ThoiGianTraKhach = model.ThoiGianTraKhach;
            e.DiemDonKhach = model.DiemDonKhach;
            e.DiemTraKhach = model.DiemTraKhach;
            e.GiaTri = model.GiaTri;
            e.LoTrinh = model.LoTrinh;
            e.ChieuVe = model.ChieuVe;
            e.GhiChu = model.GhiChu;
            e.HinhThucThanhToanId = model.HinhThucThanhToanId;
            if (model.LaiXeId > 0)
                e.LaiXeId = model.LaiXeId;
            else
                e.LaiXeId = null;
            e.KmXuat = model.KmXuat;
        }
        void KhachHangChuyenToEntity(KhachHangChuyen e, KhachHangChuyenModel model)
        {
            e.TenKhachHang = model.TenKhachHang;
            e.isDaiDien = model.isDaiDien;
            e.SoDienThoai = model.SoDienThoai;
            e.GhiChu = model.GhiChu;
            e.HopDongChuyenId = model.HopDongChuyenId;
            e.NamSinh = model.NamSinh;
           

        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult HopDongchuyenTao(HopDongChuyenModel model, bool continueEditing)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            if (ModelState.IsValid)
            {
                var _hopdong = new HopDongChuyen();
                 DateTime _thoigiandonkhach = Convert.ToDateTime( model.ThoiGianDonKhach);
                model.ThoiGianDonKhach =  model.ThoiGianDonKhach.Date.AddHours(model.GioDonKhach.Hour).AddMinutes(model.GioDonKhach.Minute);
                DateTime _thoigiantrakhach = Convert.ToDateTime(model.ThoiGianTraKhach);
                model.ThoiGianTraKhach = model.ThoiGianTraKhach.Date.AddHours(model.GioTraKhach.Hour).AddMinutes(model.GioTraKhach.Minute);
                HopDongChuyenToEntity(_hopdong, model);
                _hopdong.NgayTao = DateTime.Now;
                _hopdong.NguoiTaoId = _workContext.CurrentNhanVien.Id;
                _hopdong.TrangThaiId = (int)ENTrangThaiHopDongChuyen.MOI_DAT;
                _hopdong.NhaXeId = _workContext.NhaXeId;
                _hopdongchuyenService.InsertChuyenDiHopDong(_hopdong);
               
                SuccessNotification("Thêm mới hợp đồng chuyến thành công");
                return continueEditing ? RedirectToAction("HopDongChuyenSua", new { id = _hopdong.Id }) : RedirectToAction("ListHopDongChuyen");
            }
            return View(model);

        }
        public ActionResult HopDongChuyenSua(int id)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var hopdongchuyen = _hopdongchuyenService.GetChuyenDiHopDongById(id);
            if (hopdongchuyen == null || hopdongchuyen.TrangThai == ENTrangThaiHopDongChuyen.HUY)
                //No manufacturer found with the specified id
                return RedirectToAction("ListHopDongChuyen");
            var khachhangs = _hopdongchuyenService.GetAllKhachHangByHopDongId(id);
            var model = hopdongchuyen.toModel(_localizationService, khachhangs);
            model.HTThanhToans = this.GetCVEnumSelectList<ENHinhThucThanhToan>(_localizationService, model.HinhThucThanhToanId);
            return View(model);
        }
        public ActionResult HopDongChuyenChiTiet(int Id)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var hopdongchuyen = _hopdongchuyenService.GetChuyenDiHopDongById(Id);
            if (hopdongchuyen == null || hopdongchuyen.TrangThai == ENTrangThaiHopDongChuyen.HUY)
                //No manufacturer found with the specified id
                return RedirectToAction("ListHopDongChuyen");
            var khachhangs = _hopdongchuyenService.GetAllKhachHangByHopDongId(Id);
            var model = hopdongchuyen.toModel(_localizationService, khachhangs);
            model.HTThanhToans = this.GetCVEnumSelectList<ENHinhThucThanhToan>(_localizationService, model.HinhThucThanhToanId);
            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult HopDongChuyenSua(HopDongChuyenModel model, bool continueEditing)
        {



            var hopdongchuyen = _hopdongchuyenService.GetChuyenDiHopDongById(model.Id);
            if (hopdongchuyen == null || hopdongchuyen.TrangThai == ENTrangThaiHopDongChuyen.HUY)
                //No manufacturer found with the specified id
                return RedirectToAction("ListHopDongChuyen");
            if (ModelState.IsValid)
            {
                DateTime _thoigiandonkhach = Convert.ToDateTime(model.ThoiGianDonKhach);
                model.ThoiGianDonKhach = model.ThoiGianDonKhach.Date.AddHours(model.GioDonKhach.Hour).AddMinutes(model.GioDonKhach.Minute);
                DateTime _thoigiantrakhach = Convert.ToDateTime(model.ThoiGianTraKhach);
                model.ThoiGianTraKhach = model.ThoiGianTraKhach.Date.AddHours(model.GioTraKhach.Hour).AddMinutes(model.GioTraKhach.Minute);
                //update phieu gui hang
                HopDongChuyenToEntity(hopdongchuyen, model);

                _hopdongchuyenService.UpdateChuyenDiHopDong(hopdongchuyen);
                // update hang hoa
               

                if (continueEditing)
                {
                    return RedirectToAction("HopDongChuyenSua", new { id = hopdongchuyen.Id });
                }
                return RedirectToAction("ListHopDongChuyen");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult HopDongChuyenXoa(int id)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var hopdongchuyen = _hopdongchuyenService.GetChuyenDiHopDongById(id);
            if (hopdongchuyen == null || hopdongchuyen.TrangThai == ENTrangThaiHopDongChuyen.HUY)
                //No manufacturer found with the specified id
                return RedirectToAction("ListHopDongChuyen");

            _hopdongchuyenService.DeleteChuyenDiHopDong(hopdongchuyen);

            return RedirectToAction("ListHopDongChuyen");
        }
        [HttpPost]
        public ActionResult GetKhachHangInHopDongChuyen(DataSourceRequest command, int HopDongChuyenId)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var items = _hopdongchuyenService.GetAllKhachHangByHopDongId(HopDongChuyenId);
            var gridModel = new DataSourceResult
            {
                Data = items.Select(x =>
                {

                    return x.toModel();
                }),
                Total = items.Count()
            };

            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult KhachHangChuyenDelete(int id)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var khachhangchuyen = _hopdongchuyenService.GetKhachHangChuyenById(id);
            if (khachhangchuyen == null)
                throw new ArgumentException("No LichTrinhGiaVe mapping found with the specified id");
            _hopdongchuyenService.DeleteKhachHangChuyen(khachhangchuyen);
            return new NullJsonResult();
        }

        [HttpPost]
        public ActionResult KhachHangChuyenUpdate(KhachHangChuyenModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var khachhangchuyen = _hopdongchuyenService.GetKhachHangChuyenById(model.Id);
            if (khachhangchuyen == null)
                throw new ArgumentException("No LichTrinhGiaVe mapping found with the specified id");
            KhachHangChuyenToEntity(khachhangchuyen, model);
            _hopdongchuyenService.UpdateKhachHangChuyen(khachhangchuyen);
            return new NullJsonResult();

        }
        public ActionResult KhachHangChuyenInfo(int hopdongchuyenid)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            var model = new KhachHangChuyenModel();
            model.HopDongChuyenId = hopdongchuyenid;
           
            return View(model);
        }

        [HttpPost]
        public ActionResult KhachHangChuyenInfo(KhachHangChuyenModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            var khachhang = new KhachHangChuyen();
            if (ModelState.IsValid)
            {
                KhachHangChuyenToEntity(khachhang,model);
                _hopdongchuyenService.InsertKhachHangChuyen(khachhang);
                return Json("ok");

            }
            return Json("");

        }

        #endregion

        #endregion

    }
}