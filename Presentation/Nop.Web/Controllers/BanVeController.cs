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


namespace Nop.Web.Controllers
{
    public class BanVeController : BaseNhaXeController
    {
        #region Khoi Tao
        private readonly IStateProvinceService _stateProvinceService;
        private readonly INhaXeService _nhaxeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly IPhieuGuiHangService _phieuguihangService;
        private readonly IHangHoaService _hanghoaService;
        private readonly ICustomerService _customerService;
        private readonly IChonVeService _chonveService;
        private readonly IDiaChiService _diachiService;
        private readonly INhanVienService _nhanvienService;
        private readonly IPermissionService _permissionService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly CustomerSettings _customerSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreService _storeService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IXeInfoService _xeinfoService;
        private readonly IHanhTrinhService _hanhtrinhService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IBenXeService _benxeService;
        private readonly IVeXeService _vexeService;
        private readonly IPhoiVeService _phoiveService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAuthenticationService _authenticationService;
        private readonly INhaXeCustomerService _nhaxecustomerService;
        private readonly IGiaoDichKeVeXeService _giaodichkeveService;

        public BanVeController(
            IStateProvinceService stateProvinceService,
            INhaXeService nhaxeService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IPictureService pictureService,
            IPhieuGuiHangService phieuguihangService,
            IHangHoaService hanghoaService,
            ICustomerService customerService,
            IChonVeService chonveService,
            IDiaChiService diachiService,
            INhanVienService nhanvienService,
            IPermissionService permissionService,
            IDateTimeHelper dateTimeHelper,
            CustomerSettings customerSettings,
            DateTimeSettings dateTimeSettings,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerActivityService customerActivityService,
            IGenericAttributeService genericAttributeService,
            IStoreService storeService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IXeInfoService xeinfoService,
            IHanhTrinhService hanhtrinhService,
            IPriceFormatter priceFormatter,
            IBenXeService benxeService,
            IVeXeService vexeService,
            IPhoiVeService phoiveService,
            IShoppingCartService shoppingCartService,
            IAuthenticationService authenticationService,
            INhaXeCustomerService nhaxecustomerService,
            IGiaoDichKeVeXeService giaodichkeveService
            )
        {
            this._stateProvinceService = stateProvinceService;
            this._nhaxeService = nhaxeService;
            this._hanghoaService = hanghoaService;
            this._phieuguihangService = phieuguihangService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._pictureService = pictureService;
            this._customerService = customerService;
            this._chonveService = chonveService;
            this._diachiService = diachiService;
            this._nhanvienService = nhanvienService;
            this._permissionService = permissionService;
            this._dateTimeHelper = dateTimeHelper;
            this._customerSettings = customerSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._customerRegistrationService = customerRegistrationService;
            this._customerActivityService = customerActivityService;
            this._genericAttributeService = genericAttributeService;
            this._storeService = storeService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._xeinfoService = xeinfoService;
            this._hanhtrinhService = hanhtrinhService;
            this._priceFormatter = priceFormatter;
            this._benxeService = benxeService;
            this._vexeService = vexeService;
            this._phoiveService = phoiveService;
            this._shoppingCartService = shoppingCartService;
            this._authenticationService = authenticationService;
            this._nhaxecustomerService = nhaxecustomerService;
            this._giaodichkeveService = giaodichkeveService;

        }
        #endregion
        #region Cac ham chung

        #endregion
        #region ban ve
        List<SelectListItem> PrepareHanhTrinhList(bool isAll = true, bool isChonHanhTrinh = false)
        {
            List<HanhTrinh> hanhtrinhs = new List<HanhTrinh>();
            if (isAll)
                hanhtrinhs = _hanhtrinhService.GetAllHanhTrinhByNhaXeId(_workContext.NhaXeId);
            else
                hanhtrinhs = _hanhtrinhService.GetAllHanhTrinhByNhaXeId(_workContext.NhaXeId, _workContext.CurrentVanPhong.Id);
            var ddls = hanhtrinhs.Select(c =>
            {
                var item = new SelectListItem();
                item.Text = string.Format("{0} ({1})", c.MoTa, c.MaHanhTrinh);
                item.Value = c.Id.ToString();
                return item;
            }).ToList();

            if (isChonHanhTrinh)
                ddls.Insert(0, new SelectListItem { Text = GetLabel("LichTrinh.ChonHanhTrinh"), Value = "0" });
            return ddls;
        }
        protected virtual string GetLabel(string _name)
        {
            return _localizationService.GetResource(string.Format("ChonVe.NhaXe.{0}", _name));
        }
        protected virtual void ToHanhTrinhGiaVeModel(HanhTrinhGiaVe item, HanhTrinhGiaVeModel model)
        {
            model.Id = item.Id;
            model.HanhTrinhId = item.HanhTrinhId;
            model.DiemDonId = item.DiemDonId;
            model.TenDiemDon = item.DiemDon.TenDiemDon;
            model.DiemDenId = item.DiemDenId;
            model.TenDiemDen = item.DiemDen.TenDiemDon;
            model.NhaXeId = item.NhaXeId;
            model.GiaVe = item.GiaVe;
            model.GiaVeText = item.GiaVe.ToTien(_priceFormatter);
        }
        public ActionResult BanVeDauTuyen()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            
            var model = new BangGiaVeModel();
            model.NgayDi = DateTime.Now;
            model.ListHanhTrinh = PrepareHanhTrinhList(false);
          
            return View(model);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult _GetBangGia(int HanhTrinhId, string NgayDi)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedPartialView();
            if(_workContext.CurrentVanPhong.Id==CommonHelper.VanPhongKhoiTaoId)
            {
                return AccessDeniedPartialView();
            }
            var model = new BangVeInfoModel();
            model.NgayDi = Convert.ToDateTime(NgayDi);
            var hanhtrinh = _hanhtrinhService.GetHanhTrinhById(HanhTrinhId);
           var hanhtrinhdiemdon=_hanhtrinhService.GetAllHanhTrinhDiemDonByHanhTrinhId(HanhTrinhId).OrderBy(c=>c.ThuTu);
           var diemdonhientai=new DiemDon();
           if (hanhtrinhdiemdon.Count() > 0)
           {
               diemdonhientai = _hanhtrinhService.GetDiemDonByHanhTrinhDiemDonId(hanhtrinhdiemdon.First().Id);
           }
           else
               return null;
                
            
           
            model.TenDauTuyen = diemdonhientai.TenDiemDon;
            int TongVe = 0;
            decimal TongTien = 0;
            model.HanhTrinhGiaVes = _hanhtrinhService.GetallHanhTrinhGiaVe(HanhTrinhId, 0, diemdonhientai.Id).Select(c =>
                {
                    var item = new HanhTrinhGiaVeModel();
                    ToHanhTrinhGiaVeModel(c, item);                    
                    item.VeXes = _giaodichkeveService.GetVeXeBanQuayByDay(_workContext.NhaXeId, model.NgayDi,c.Id, 0, _workContext.CurrentNhanVien.Id, c.GiaVe, c.HanhTrinh.isTuyenDi);
                    
                    item.SoVeDaBan = item.VeXes.Count();
                    TongVe = TongVe + item.SoVeDaBan;
                    TongTien = TongTien +( item.SoVeDaBan * c.GiaVe);
                    for (int k = 0; k < item.SoVeDaBan;k++ )
                    {
                        if(k==0)
                        {
                            var vetruoc=_giaodichkeveService.GetVeXeItemById(item.VeXes[k].Id+1);
                            if (item.VeXes[k].NgayBan.Value.AddMinutes(5) >= DateTime.Now && vetruoc.TrangThai==ENVeXeItemTrangThai.DA_GIAO_HANG)
                            {
                                item.VeXes[k].CanDelete = true;
                            }
                        }
                        else
                        {
                            item.VeXes[k].CanDelete = false;
                        }
                       
                    }


                    var vechuanbiban = _giaodichkeveService.GetVeChuanBiBanTaiQuay(_workContext.NhaXeId, _workContext.CurrentVanPhong.Id, c.GiaVe, c.HanhTrinh.isTuyenDi);
                    if (vechuanbiban != null)
                    {
                        item.SoSeriHienTai = vechuanbiban.SoSeri;
                        if (vechuanbiban.quyenve != null)
                            item.QuyenDangBan = vechuanbiban.quyenve.ThongTin;
                        else
                            item.QuyenDangBan = "_________________";
                        
                    }
                   else
                    {
                        item.IsHetVe = true;
                        item.QuyenDangBan = "_________________";
                    }
                    item.SoVeConLai = _giaodichkeveService.CountVeConLaiTaiQuay(_workContext.NhaXeId, _workContext.CurrentVanPhong.Id, c.GiaVe, c.HanhTrinh.isTuyenDi);
                    return item;
                }).ToList();
            model.TongVeDaBan = TongVe;
            model.TongTien = TongTien.ToTien(_priceFormatter);
            model.SoDiemDon = model.HanhTrinhGiaVes.Count();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DatVe(int HanhTrinhGiaVeId, string NgayDi)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var hanhtrinhgiave = _hanhtrinhService.GetHanhTrinhGiaVeId(HanhTrinhGiaVeId);

            var isluotdi = false;
            if (hanhtrinhgiave.HanhTrinhId > 0)
            {
                isluotdi = hanhtrinhgiave.HanhTrinh.isTuyenDi;
            }
            var item = new PhoiVe();
            item.HanhTrinhGiaVeId = HanhTrinhGiaVeId;
            item.NgayDi = Convert.ToDateTime(NgayDi);
            item.TrangThaiId = (int)ENTrangThaiPhoiVe.DaGiaoHang;
            item.isChonVe = false;
            item.NguoiDatVeId = _workContext.CurrentNhanVien.Id;
            item.GiaVeHienTai = hanhtrinhgiave.GiaVe;            
            item.NgayTao = DateTime.Now;
            item.NgayUpd = DateTime.Now;
            _phoiveService.ThanhToanVeTaiQuay(item);
            var vexeitems = _giaodichkeveService.BanVeTaiQuay(_workContext.NhaXeId, _workContext.CurrentNhanVien.Id, HanhTrinhGiaVeId, hanhtrinhgiave.GiaVe, isluotdi,item.NgayDi,_workContext.CurrentVanPhong.Id);
            if (vexeitems != null)
            {
                item.VeXeItemId = vexeitems.Id;
                item.MaVe = vexeitems.SoSeri;
                _phoiveService.UpdatePhoiVe(item);
                return ThanhCong();
            }

            return Loi();
        }
        /// <summary>
        /// DatVeTheoSoLuong
        /// </summary>
        /// <param name="HanhTrinhGiaVeId"></param>
        /// <param name="NgayDi"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult DatVeSoLuong(int HanhTrinhGiaVeId,string soLuong, string NgayDi)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();

            var hanhtrinhgiave = _hanhtrinhService.GetHanhTrinhGiaVeId(HanhTrinhGiaVeId);

            var isluotdi = false;
            if (hanhtrinhgiave.HanhTrinhId > 0)
            {
                isluotdi = hanhtrinhgiave.HanhTrinh.isTuyenDi;
            }
            var _sl = Convert.ToInt32(soLuong);
            for (int i = 0; i < _sl; i++)
             {
                 var item = new PhoiVe();
                 item.HanhTrinhGiaVeId = HanhTrinhGiaVeId;
                 item.NgayDi = Convert.ToDateTime(NgayDi);
                 item.TrangThaiId = (int)ENTrangThaiPhoiVe.DaGiaoHang;
                 item.isChonVe = false;
                 item.NguoiDatVeId = _workContext.CurrentNhanVien.Id;
                 item.GiaVeHienTai = hanhtrinhgiave.GiaVe;
                 item.NgayTao = DateTime.Now;
                 item.NgayUpd = DateTime.Now;
                 _phoiveService.ThanhToanVeTaiQuay(item);
                 var vexeitems = _giaodichkeveService.BanVeTaiQuay(_workContext.NhaXeId, _workContext.CurrentNhanVien.Id, HanhTrinhGiaVeId, hanhtrinhgiave.GiaVe, isluotdi, item.NgayDi,_workContext.CurrentVanPhong.Id);
                 if (vexeitems != null)
                 {
                     item.VeXeItemId = vexeitems.Id;
                     item.MaVe = vexeitems.SoSeri;
                     _phoiveService.UpdatePhoiVe(item);
                    
                 }
             }


             return ThanhCong();
        }
        /// <summary>
        /// huy dat ve phai huy ngaydi, ngayban, trangthai trong vexeitem
        /// </summary>
        /// <param name="SoSeri"></param>
        /// <param name="MauVe"></param>
        /// <param name="KyHieu"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HuyDatVe(string SoSeri, string MauVe, string KyHieu)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            if (string.IsNullOrEmpty(SoSeri))
                return Loi();
           if( _phoiveService.DeletePhoiVeTaiQuay(SoSeri))
           {
               var vexeitem = _giaodichkeveService.GetVeXeItem(_workContext.NhaXeId, SoSeri, MauVe, KyHieu);
               vexeitem.NgayBan = null;
               vexeitem.TrangThaiId = (int)ENVeXeItemTrangThai.DA_GIAO_HANG;
               vexeitem.NgayDi = null;
               _giaodichkeveService.UpdateVeXeItem(vexeitem);
               return ThanhCong();
           }
            
            return Loi();
        }
        #endregion
        #region Dieu chinh quyen ve
        /// <summary>
        /// Dieu chinh thong tin thu tu ban ve
        /// </summary>
        /// <param name="HanhTrinhGiaVeId"></param>
        /// <returns></returns>
        public ActionResult _DieuChinhQuyenVe(int HanhTrinhGiaVeId)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            var model=new VeXeQuyenListModel();
            model.HanhTrinhGiaVeId = HanhTrinhGiaVeId;
            var htgiave=_hanhtrinhService.GetHanhTrinhGiaVeId(HanhTrinhGiaVeId);
            var menhgia=_giaodichkeveService.GetMenhGiaVeByGia(htgiave.GiaVe);
            //lay thong tin quyen ve
            var quyenves = _giaodichkeveService.GetTonVeXeQuyen(_workContext.NhaXeId, menhgia.Id, _workContext.CurrentVanPhong.Id, _workContext.CurrentNhanVien.Id);
            model.vexequyens = quyenves.Select(c => new SelectListItem
            {
                Text=c.ThongTin,
                Value=c.Id.ToString()
            }).ToList();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult DieuChinhThuTuQuyenVe(string quyenids)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVHoatDongBanVe))
                return AccessDeniedView();
            if (string.IsNullOrEmpty(quyenids))
                return Loi();
            string[] _quyenids = quyenids.Split(',');
            for (int i = 0; i < _quyenids.Length;i++ )
            {
                _giaodichkeveService.UpdateQuyenVeThuTuBan(Convert.ToInt32(_quyenids[i]), i + 1);
            }
            return ThanhCong();
        }
        #endregion
    }
}