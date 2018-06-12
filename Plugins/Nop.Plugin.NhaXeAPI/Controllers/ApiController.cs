using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.NhaXes;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Plugin.NhaXeAPI.Models;
using Nop.Services.Authentication;
using Nop.Services.Catalog;
using Nop.Services.Chonves;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.NhaXes;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Vendors;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nop.Plugin.NhaXeAPI.Controllers
{
    public class ApiController : BaseController
    {
        #region Fields
        private readonly RestServiceSettings _settings;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly INhaXeService _nhaxeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IPhieuGuiHangService _phieuguihangService;
        private readonly IHangHoaService _hanghoaService;
        private readonly ICustomerService _customerService;
        private readonly IChonVeService _chonveService;
        private readonly IDiaChiService _diachiService;
        private readonly INhanVienService _nhanvienService;
        private readonly IPermissionService _permissionService;
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IXeInfoService _xeinfoService;
        private readonly IHanhTrinhService _hanhtrinhService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IBenXeService _benxeService;
        private readonly IVeXeService _vexeService;
        private readonly IPhoiVeService _phoiveService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAuthenticationService _authenticationService;
        private readonly INhaXeCustomerService _nhaxecustomerService;
        private readonly ILimousineBanVeService _limousinebanveService;
        private Customer _customer;
        private NhanVien _nhanvien;
        private ChuyenDi xexuatben;
        int _customerId = 0;
        #endregion

        #region Ctor

        public ApiController(
            RestServiceSettings settings,
            IStateProvinceService stateProvinceService,
            INhaXeService nhaxeService,
            ILocalizationService localizationService,
            IWorkContext workContext,
             IPhieuGuiHangService phieuguihangService,
            IHangHoaService hanghoaService,
            ICustomerService customerService,
            IChonVeService chonveService,
            IDiaChiService diachiService,
            INhanVienService nhanvienService,
            IPermissionService permissionService,
            CustomerSettings customerSettings,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerActivityService customerActivityService,
            IGenericAttributeService genericAttributeService,
            IXeInfoService xeinfoService,
            IHanhTrinhService hanhtrinhService,
            IPriceFormatter priceFormatter,
            IBenXeService benxeService,
             IVeXeService vexeService,
            IPhoiVeService phoiveService,
            IShoppingCartService shoppingCartService,
            IAuthenticationService authenticationService,
            INhaXeCustomerService nhaxecustomerService,
            ILimousineBanVeService limousinebanveService
            )
        {
            this._limousinebanveService = limousinebanveService;
            this._settings = settings;
            this._stateProvinceService = stateProvinceService;
            this._nhaxeService = nhaxeService;
            this._hanghoaService = hanghoaService;
            this._phieuguihangService = phieuguihangService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._customerService = customerService;
            this._chonveService = chonveService;
            this._diachiService = diachiService;
            this._nhanvienService = nhanvienService;
            this._permissionService = permissionService;
            this._customerSettings = customerSettings;
            this._customerRegistrationService = customerRegistrationService;
            this._customerActivityService = customerActivityService;
            this._genericAttributeService = genericAttributeService;
            this._xeinfoService = xeinfoService;
            this._hanhtrinhService = hanhtrinhService;
            this._priceFormatter = priceFormatter;
            this._benxeService = benxeService;
            this._vexeService = vexeService;
            this._phoiveService = phoiveService;
            this._shoppingCartService = shoppingCartService;
            this._authenticationService = authenticationService;
            this._nhaxecustomerService = nhaxecustomerService;

        }

        #endregion
        #region Common
        Customer currentCustomer
        {
            get
            {
                if (_customer != null)
                    return _customer;
                _customer = _customerService.GetCustomerById(_customerId);
                return _customer;
            }
        }
        NhanVien currentNhanVien
        {
            get
            {
                if (_nhanvien != null)
                    return _nhanvien;
                _nhanvien = _nhanvienService.GetByCustomerId(_customerId);
                return _nhanvien;
            }
        }

        private bool isRightAccess()
        {
            if (_permissionService.Authorize(StandardPermissionProvider.CVHoatDongBanVeKyGuiTrenTuyen, currentCustomer))
                return true;
            return false;
        }

        string isAuthentication(int NhaXeId, int CustomerId, string apiToken)
        {
            if (!IsApiTokenValid(apiToken))
                return "Không đúng apiToken";
            _customerId = CustomerId;
            //kiem tra co quyen ko 
            if (!isRightAccess())
            {
                return "Bạn không có quyền vào chức năng này !";
            }
            //kiem tra thong tin nhan vien
            _nhanvien = _nhanvienService.GetByCustomerId(CustomerId);
            if (_nhanvien == null)
                return "Nhân viên không tồn tại";
            //xem nhan vien co thuoc nha xe ko 
            if (_nhanvien.NhaXeID != NhaXeId)
                return "Nhân viên không thuộc nhà xe";
            return String.Empty;
        }
        string isAuthentication(int NhaXeId, int CustomerId, string apiToken, int XeXuatBenId)
        {
            string _isauthen = isAuthentication(NhaXeId, CustomerId, apiToken);
            if (!String.IsNullOrEmpty(_isauthen))
                return _isauthen;
            xexuatben = _limousinebanveService.GetChuyenDiById(XeXuatBenId);
            if (xexuatben == null)
                return "Không tồn tại thông tin xe xuất bến";
            if (xexuatben.NhaXeId != NhaXeId)
                return "Xe xuất bến không thuộc nhà xe";
            return String.Empty;
        }
        #endregion
        #region Customers

        public ActionResult Login(string Email, string Password)
        {
            //BienSoXe = BienSoXe + "@chonve.vn";
            var loginresult = _customerRegistrationService.ValidateCustomer(Email, Password);
            if (loginresult != CustomerLoginResults.Successful)
            {
                return ErrorOccured("Tài khoản hoặc mật khẩu không đúng !");
            }
            _customer = _customerService.GetCustomerByEmail(Email);

            //kiem tra co quyen ko 
            if (!isRightAccess())
            {
                return ErrorOccured("Bạn không có quyền vào chức năng này !");
            }
            //kiem tra cac thong tin nha xe
            NhaXe currentNhaXe = _nhaxeService.GetNhaXeByCustommerId(currentCustomer.Id);
            if (currentNhaXe == null)
            {
                return ErrorOccured("Không xác định nhà xe !");
            }
            //luu nhat ky
            _customerActivityService.InsertActivity("PublicStore.Login", "Tài khoản nhà xe đăng nhập", currentCustomer);
            //OK, lay thong tin va truyen xuong client
            var loginInfo = new
            {
                Id = currentCustomer.Id,
                NhaXeId = currentNhaXe.Id,
                GuidId = _settings.ApiToken,
                FullName = currentCustomer.GetFullName(),
                TenNhaXe = currentNhaXe.TenNhaXe
            };
            return Successful(loginInfo);
        }
        /// <summary>
        /// Lay id xe tu bien so xe
        /// </summary>
        /// <param name="NhaXeId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="BienSoXe"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public ActionResult GetXeInfo(int NhaXeId, int CustomerId, string BienSoXe, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            //lay thong tin xe van chuyen tu bien so xe
            var xevanchuyen = _xeinfoService.GetXeInfoByBienSo(NhaXeId, BienSoXe);
            if (xevanchuyen == null)
                return ErrorOccured("Không tìm thấy xe có biển số này trong nhà xe");
            return SuccessfulSimple(xevanchuyen.Id.ToString());
        }
        /// <summary>
        /// Lay thong ti chuyen di hien tai theo bien so xe
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="NhaXeId"></param>
        /// <param name="BienSoXe"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public ActionResult GetChuyenDiHienTai(int NhaXeId, int CustomerId, int XeId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            //lay thong tin xe van chuyen tu bien so xe
            var xevanchuyen = _xeinfoService.GetXeInfoById(XeId);
            if (xevanchuyen == null)
                return ErrorOccured("Xe không tồn tại");

            var chuyendis = _limousinebanveService.GetAllChuyenDiTheoXe(xevanchuyen.Id);
            if (chuyendis.Count == 0)
                return ErrorOccured("Không có chuyến đi nào tại thời điểm này");

            //Lấy thông tin cac chuyen di
            var chuyendijson = chuyendis.Select(c => new
            {
                Id = c.Id,
                NgayDi = c.NgayDi.toStringDate(),
                GioXuatBen = c.NgayDi.ToString("HH:mm"),
                LaiXe = c.laixe.HoVaTen,
                ThongTin = c.hanhtrinh.toText(),
                SoHangGhe = xevanchuyen.loaixe.sodoghe.SoHang,
                SoCotGhe = xevanchuyen.loaixe.sodoghe.SoCot,
                SoTang = (int)xevanchuyen.loaixe.KieuXe,
                TrangThaiId = c.TrangThaiId,
                HanhTrinhId = c.HanhTrinhId
            }).ToList();
            return Successful(chuyendijson);
        }
        public ActionResult GetSoDoGheXe(int NhaXeId, int CustomerId, int XeXuatBenId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var datves = _limousinebanveService.GetDatVeByChuyenDi(XeXuatBenId);
            //lay thong tin so do ghe xe
            var sodoghequytacs = _xeinfoService.GetAllSoDoGheXeQuyTac(xexuatben.xevanchuyen.LoaiXeId);
            var phoives = new List<PhoiVeMobileModel>();

            //kiem tra trang thai tung ghe tren so do
            foreach (var sodo in sodoghequytacs)
            {
                var item = new PhoiVeMobileModel();
                //thong tin ghe/vi tri tri
                item.Val = sodo.Val;
                item.x = sodo.x;
                item.y = sodo.y;
                item.Tang = sodo.Tang;
                item.SoDoGheXeQuyTacId = sodo.Id;
                //thong tin trang thai vi tri
                var phoive = datves.Where(c => c.SoDoGheId == sodo.Id).FirstOrDefault();
                if (phoive == null)
                {
                    phoive = new DatVe();
                    phoive.trangthai = ENTrangThaiDatVe.CON_TRONG;
                    phoive.ChuyenDiId = xexuatben.Id;
                    phoive.NgayDi = xexuatben.NgayDi;
                    phoive.GiaTien = xexuatben.lichtrinh.GiaVeToanTuyen;
                }
                item.Id = phoive.Id;
                item.TrangThaiId = phoive.TrangThaiId;
                item.ChuyenDiId = phoive.ChuyenDiId.GetValueOrDefault(0);
                item.NgayDi = phoive.NgayDi.toStringDate();
                item.GiaVe = Convert.ToInt32(phoive.GiaTien);
                item.CustomerId = phoive.KhachHangId.GetValueOrDefault();
                item.MaVe = phoive.Ma;
                if (phoive.diemdon != null)
                    item.ViTriXuong = phoive.diemdon.TenDiemDon;
                if (phoive.khachhang != null)
                {
                    item.TenKhachHang = phoive.khachhang.Ten;
                    item.SoDienThoai = phoive.khachhang.DienThoai;
                }
                phoives.Add(item);
            }

            return Successful(phoives);
        }
        public ActionResult GetDatVeMoi(int NhaXeId, int CustomerId, int XeXuatBenId, string DatVeIds, string apiToken)
        {
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            int[] arrdatveid = Array.ConvertAll(DatVeIds.Split(','), s => int.Parse(s));
            var soluongmoi = _limousinebanveService.GetDatVeByChuyenDi(XeXuatBenId).Where(c => !arrdatveid.Contains(c.Id)).Count();
            return SuccessfulSimple(soluongmoi.ToString());
        }
        public ActionResult LenXe(int NhaXeId, int CustomerId, int XeXuatBenId, int SoDoGheXeQuyTacId, int GiaTien, string DiemLen, string DiemXuong, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);


            var sodo = _xeinfoService.GetSoDoGheXeQuyTacById(SoDoGheXeQuyTacId);
            if (sodo == null)
                return ErrorOccured("Dữ liệu không hợp lệ");

            var phoive = _phoiveService.GetPhoiVe(xexuatben.Id, sodo, xexuatben.NgayDi, true);
            phoive.GiaVeHienTai = Convert.ToDecimal(GiaTien);
            phoive.ViTriLenXe = DiemLen;
            phoive.ViTriXuongXe = DiemXuong;
            phoive.NguoiDatVeId = currentNhanVien.Id;
            phoive.CustomerId = CommonHelper.KhachVangLaiId;//khach vang lai
            if (_phoiveService.DatVe(phoive, ENTrangThaiPhoiVe.DaGiaoHang))
            {
                return SuccessfulSimple(phoive.Id.ToString());
            }
            return ErrorOccured("Không đặt được ở vị trí này");
        }
        /// <summary>
        /// Co 2 truong hop luc xuong la: xuong that, hoac huy ve
        /// </summary>
        /// <param name="NhaXeId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="XeXuatBenId"></param>
        /// <param name="PhoiVeId"></param>
        /// <param name="ViTriXuong"></param>
        /// <param name="isXuongXe"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public ActionResult XuongXe(int NhaXeId, int CustomerId, int XeXuatBenId, int PhoiVeId, string ViTriXuong, int isXuongXe, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            var phoive = _phoiveService.GetPhoiVeById(PhoiVeId);
            if (isXuongXe == 1)
                phoive.TrangThai = ENTrangThaiPhoiVe.KetThuc;
            else
            {
                //truong hop la huy ve
                //kiem tra phai cung nguoi dat ve thi moi dc huy
                if (phoive.NguoiDatVeId != currentNhanVien.Id)
                    return ErrorOccured("Bạn không thể hủy vé này");
                //kiem tra thoi gian, qua thoi gian 30p thi ko dc huy
                if (phoive.NgayTao.AddMinutes(30) < DateTime.Now)
                    return ErrorOccured("Quá thời hạn hủy 30 phút, bạn không thể hủy vé này");
                phoive.TrangThai = ENTrangThaiPhoiVe.Huy;
            }

            phoive.ViTriXuongXe = ViTriXuong;
            _phoiveService.UpdatePhoiVe(phoive);
            return SuccessfulSimple("OK");
        }
        public ActionResult XacNhanLenXe(int NhaXeId, int CustomerId, int XeXuatBenId, int DatVeId, int isLenXe, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            var phoive = xexuatben.DatVeHopLes().Where(c => c.Id == DatVeId).FirstOrDefault();
            if (phoive == null)
                ErrorOccured("Vị trí không hợp lệ");
            if (isLenXe == 1)
            {
                //khach xac nha da di
                phoive.trangthai = ENTrangThaiDatVe.DA_DI;
                phoive.isThanhToan = true;
            }
            else
            {
                //khach huy
                phoive.trangthai = ENTrangThaiDatVe.HUY;
                phoive.isKhachHuy = true;
            }


            _limousinebanveService.UpdateDatVe(phoive);
            return SuccessfulSimple(phoive.Id.ToString());
        }
        public ActionResult ChuyenCho(int NhaXeId, int CustomerId, int XeXuatBenId, int PhoiVeId, int SoDoGheXeQuyTacId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var phoive = _phoiveService.GetPhoiVeById(PhoiVeId);
            phoive.SoDoGheXeQuyTacId = SoDoGheXeQuyTacId;
            _phoiveService.UpdatePhoiVe(phoive);
            return SuccessfulSimple("OK");
        }
        /// <summary>
        /// cap nhat vi tri dinh vi cua xe, ko can xac thuc
        /// Dinh ky chay tam 2p update 1 lan
        /// </summary>
        /// <param name="XeId"></param>
        /// <param name="Latitude">dang chuoi so /1000.000.000</param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public ActionResult CapNhatViTri(int XeId, string Latitude, string Longitude)
        {
            if (Latitude == "0" || Longitude == "0")
                return ErrorOccured("Vị trí ko hợp lệ");
            //lay thong tin xe van chuyen tu bien so xe
            var xevanchuyen = _xeinfoService.GetXeInfoById(XeId);
            if (xevanchuyen == null)
                return ErrorOccured("Xe không tồn tại");

            xevanchuyen.Latitude = Convert.ToDecimal(Latitude) / 1000000000m;
            xevanchuyen.Longitude = Convert.ToDecimal(Longitude) / 1000000000m;
            xevanchuyen.NgayGPS = DateTime.Now;
            if (xevanchuyen.Latitude == Decimal.Zero || xevanchuyen.Longitude == Decimal.Zero)
                return ErrorOccured("Vị trí ko hợp lệ");
            _xeinfoService.UpdateXeInfo(xevanchuyen);
            return SuccessfulSimple("OK");


        }
        public ActionResult XeKhoiHanh(int NhaXeId, int CustomerId, int XeXuatBenId, string apiToken)
        {
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            _nhaxeService.UpdateHistoryXeXuatBenTrangThai(XeXuatBenId, currentNhanVien.Id, ENTrangThaiXeXuatBen.DANG_DI);
            return SuccessfulSimple("OK");
        }
        public ActionResult XeVeBen(int NhaXeId, int CustomerId, int XeXuatBenId, string apiToken)
        {
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            _nhaxeService.UpdateHistoryXeXuatBenTrangThai(XeXuatBenId, currentNhanVien.Id, ENTrangThaiXeXuatBen.KET_THUC);
            return SuccessfulSimple("OK");
        }
        #endregion

        #region "Hang hoa"
        public ActionResult GetHangHoaTrenXe(int NhaXeId, int CustomerId, int XeXuatBenId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            //lay thong tin hang hoa tren xe
            var phieuguihangs = _phieuguihangService.GetAll(NhaXeId, XeXuatBenId, 0, 0, ENTinhTrangVanChuyen.DangVanChuyen);
            var arrpgh = phieuguihangs.Select(pgh =>
            {
                var item = new PhieuGuiHangMobileModel();
                item.Id = pgh.Id;
                item.MaPhieu = pgh.MaPhieu;
                item.NguoiGuiTen = pgh.NguoiGui.HoTen;
                item.NguoiGuiDienThoai = pgh.NguoiGui.DienThoai;
                item.DiemGui = pgh.DiemGui;
                item.NguoiNhanTen = pgh.NguoiNhan.HoTen;
                item.NguoiNhanDienThoai = pgh.NguoiNhan.DienThoai;
                item.DiemTra = pgh.DiemTra;
                item.TenHang = pgh.ThongTinHanHoa();
                item.SoTien = Convert.ToInt32(pgh.HangHoas.Sum(c => c.GiaCuoc * c.SoLuong));
                return item;
            }).ToList();

            return Successful(arrpgh);
        }

        /// <summary>
        /// So tien <0 co nghia la chua thu cuoc
        /// </summary>
        /// <param name="NhaXeId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="XeXuatBenId"></param>
        /// <param name="GuiTen"></param>
        /// <param name="GuiSDT"></param>
        /// <param name="NhanTen"></param>
        /// <param name="NhanSDT"></param>
        /// <param name="DiemGui"></param>
        /// <param name="DiemTra"></param>
        /// <param name="TenHang"></param>
        /// <param name="SoTien"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public ActionResult NhanHang(int NhaXeId, int CustomerId, int XeXuatBenId
            , string GuiTen
            , string GuiSDT
            , string NhanTen
            , string NhanSDT
            , string DiemGui
            , string DiemTra
            , string TenHang
            , int SoTien
            , string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            var phieugui = new PhieuGuiHang();
            //them nguoi gui
            var nguoigui = _nhaxecustomerService.CreateNew(NhaXeId, GuiTen, GuiSDT, DiemGui);
            //them nguoi nhan
            var nguoinhan = _nhaxecustomerService.CreateNew(NhaXeId, NhanTen, NhanSDT, DiemTra);
            //them phieu gui hàng               
            phieugui.NhaXeId = NhaXeId;
            phieugui.XeXuatBenId = XeXuatBenId;
            phieugui.NguoiGuiId = nguoigui.Id;
            phieugui.NguoiNhanId = nguoinhan.Id;
            phieugui.VanPhongGuiId = currentNhanVien.VanPhongID.GetValueOrDefault();
            phieugui.VanPhongNhanId = currentNhanVien.VanPhongID.GetValueOrDefault();
            phieugui.NguoiTaoId = currentNhanVien.Id;
            phieugui.NguoiKiemTraHangId = currentNhanVien.Id;
            phieugui.TinhTrangVanChuyen = ENTinhTrangVanChuyen.DangVanChuyen;
            phieugui.NgayTao = DateTime.Now;
            phieugui.NgayUpdate = DateTime.Now;
            phieugui.GhiChu = "Hàng hóa được nhận trên quá trình di chuyển";
            phieugui.DiemGui = DiemGui;
            phieugui.DiemTra = DiemTra;
            if (SoTien > 0)
            {
                phieugui.DaThuCuoc = true;
                phieugui.NgayThanhToan = DateTime.Now;
                phieugui.NhanVienThuTienId = currentNhanVien.Id;
            }
            else
                phieugui.DaThuCuoc = false;

            _phieuguihangService.InsertPhieuGuiHang(phieugui);
            //them hàng hóa
            var hanghoa = new HangHoa();
            hanghoa.TenHangHoa = TenHang;
            hanghoa.LoaiHangHoa = ENLoaiHangHoa.LoaiKhac;
            hanghoa.GiaCuoc = Math.Abs(SoTien);
            hanghoa.SoLuong = 1;
            hanghoa.PhieuGuiHangId = phieugui.Id;
            _hanghoaService.InsertHangHoa(hanghoa);

            return SuccessfulSimple(phieugui.Id.ToString());
        }
        /// <summary>
        /// Co 2 truong hop luc xuong la: tra that, hoac huy hang
        /// </summary>
        /// <param name="NhaXeId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="XeXuatBenId"></param>
        /// <param name="PhieuGuiHangId"></param>
        /// <param name="isTranhang"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public ActionResult TraHang(int NhaXeId, int CustomerId, int XeXuatBenId, int PhieuGuiHangId, string DiemTra, int isTraHang, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);


            var phieuguihang = _phieuguihangService.GetPhieuGuiById(PhieuGuiHangId);
            if (phieuguihang == null)
                return ErrorOccured("Phiếu gửi hàng không hợp lệ");

            if (isTraHang == 1)
            {
                phieuguihang.DiemTra = DiemTra;
                if (!phieuguihang.DaThuCuoc)
                {
                    phieuguihang.DaThuCuoc = true;
                    phieuguihang.NgayThanhToan = DateTime.Now;
                    phieuguihang.NhanVienThuTienId = currentNhanVien.Id;

                }
                phieuguihang.TinhTrangVanChuyen = ENTinhTrangVanChuyen.KetThuc;
            }
            else
            {
                //truong hop la huy ve
                //kiem tra phai cung nguoi dat ve thi moi dc huy
                if (phieuguihang.NguoiTaoId != currentNhanVien.Id)
                    return ErrorOccured("Bạn không thể hủy phiếu này");
                //kiem tra thoi gian, qua thoi gian 30p thi ko dc huy
                if (phieuguihang.NgayTao.AddMinutes(30) < DateTime.Now)
                    return ErrorOccured("Quá thời hạn hủy 30 phút, bạn không thể hủy phiếu này");
                phieuguihang.TinhTrangVanChuyen = ENTinhTrangVanChuyen.Huy;
            }
            _phieuguihangService.UpdatePhieuGuiHang(phieuguihang);
            return SuccessfulSimple("OK");
        }
        #endregion
        #region Nhat ky
        public ActionResult GetNhatKy(int NhaXeId, int CustomerId, int XeXuatBenId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var nhatkys = xexuatben.NhatKys.OrderByDescending(c => c.Id).Select(c => new
            {
                Id = c.Id,
                NgayTao = c.NgayTao.toStringDateTime(),
                GhiChu = c.GhiChu
            }).ToList();
            return Successful(nhatkys);
        }
        HistoryXeXuatBenLog CreateHistoryXeXuatBenLog(ENTrangThaiXeXuatBen trangthai, String ghichu, int XeXuatBenId, int NguoiTaoId)
        {
            var item = new HistoryXeXuatBenLog();
            item.TrangThai = trangthai;
            item.GhiChu = ghichu;
            item.XeXuatBenId = XeXuatBenId;
            item.NguoiTaoId = NguoiTaoId;
            _nhaxeService.InsertHistoryXeXuatBenLog(item);
            return item;
        }
        public ActionResult ChuyenTrangThaiXe(int NhaXeId, int CustomerId, int XeXuatBenId, int TrangThaiId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            xexuatben.trangthai = (ENTrangThaiXeXuatBen)TrangThaiId;
            _limousinebanveService.UpdateChuyenDi(xexuatben);
            switch (xexuatben.trangthai)
            {
                case ENTrangThaiXeXuatBen.DANG_DI:
                    {
                        CreateHistoryXeXuatBenLog(xexuatben.trangthai, "Xe xuất bến, bắt đầu hành trình", XeXuatBenId, currentNhanVien.Id);
                        break;
                    }
                case ENTrangThaiXeXuatBen.KET_THUC:
                    {
                        CreateHistoryXeXuatBenLog(xexuatben.trangthai, "Xe vào bến, kết thúc hành trình", XeXuatBenId, currentNhanVien.Id);
                        break;
                    }
            }
            return SuccessfulSimple("OK");

        }
        public ActionResult TaoNhatKy(int NhaXeId, int CustomerId, int XeXuatBenId
            , string GhiChu
            , string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);

            var item = CreateHistoryXeXuatBenLog(xexuatben.trangthai, GhiChu, XeXuatBenId, currentNhanVien.Id);
            return SuccessfulSimple(item.Id.ToString());
        }
        /// <summary>
        /// Co 2 truong hop luc xuong la: tra that, hoac huy hang
        /// </summary>
        /// <param name="NhaXeId"></param>
        /// <param name="CustomerId"></param>
        /// <param name="XeXuatBenId"></param>
        /// <param name="PhieuGuiHangId"></param>
        /// <param name="isTranhang"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public ActionResult SuaNhatKy(int NhaXeId, int CustomerId, int XeXuatBenId, int NhatKyId, string GhiChu, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var item = _nhaxeService.GetHistoryXeXuatBenLogById(NhatKyId);
            if (item.NguoiTaoId != currentCustomer.Id)
                return ErrorOccured("Nhật ký không được phép sửa");
            item.GhiChu = GhiChu;
            _nhaxeService.UpdateHistoryXeXuatBenLog(item);
            return SuccessfulSimple("OK");
        }
        public ActionResult XoaNhatKy(int NhaXeId, int CustomerId, int XeXuatBenId, int NhatKyId, string apiToken)
        {
            //kiem tra xac thuc
            string _checkauthentication = isAuthentication(NhaXeId, CustomerId, apiToken, XeXuatBenId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var item = _nhaxeService.GetHistoryXeXuatBenLogById(NhatKyId);
            if (item.NguoiTaoId != currentCustomer.Id)
                return ErrorOccured("Nhật ký không được phép xóa");
            _nhaxeService.DeleteHistoryXeXuatBenLog(item);
            return SuccessfulSimple("OK");
        }
        #endregion
        #region Chot khach
        public ActionResult ChotKhach(int NhaXeId, int XeXuatBenId, string Email, string Password, int slpm, int sltt, string Latitude, string Longitude, string VitriChot, string apiToken)
        {
            var loginresult = _customerRegistrationService.ValidateCustomer(Email, Password);
            if (loginresult != CustomerLoginResults.Successful)
            {
                return ErrorOccured("Tài khoản hoặc mật khẩu không đúng !");
            }
            var tkchot = _customerService.GetCustomerByEmail(Email);

            //luu nhat ky
            _customerActivityService.InsertActivity("PublicStore.Login", "Tài khoản nhà xe đăng nhập để chốt khách", tkchot);
            //lay thong tin van phong
            var nhanvienchot = _nhanvienService.GetByCustomerId(tkchot.Id);
            if (nhanvienchot == null)
            {
                return ErrorOccured("Thông tin người chốt không hợp lệ");
            }
            if (!nhanvienchot.DiemDonId.HasValue)
            {
                return ErrorOccured("Thông tin người chốt không hợp lệ");
            }
            xexuatben = _limousinebanveService.GetChuyenDiById(XeXuatBenId);
            if (xexuatben == null)
                return ErrorOccured("Không tồn tại thông tin xe xuất bến");
            if (xexuatben.NhaXeId != NhaXeId)
                return ErrorOccured("Xe xuất bến không thuộc nhà xe");
            //thuc hien thong tin chot khach
            //kiem tra co thong tin chot trc chua
            var lschotkhach = _nhaxeService.GetChotKhachs(NhaXeId, HistoryXeXuatBenId: XeXuatBenId);
            var itemck = new ChotKhach();
            if (lschotkhach.Count > 0)
                itemck = lschotkhach.FirstOrDefault();
            itemck.SoLuongThucTe = sltt;
            itemck.SoLuongPhanMem = slpm;
            itemck.NguoiChotId = nhanvienchot.Id;
            itemck.DiemDonId = nhanvienchot.DiemDonId.Value;
            itemck.NgayChot = DateTime.Now;
            itemck.NhaXeId = NhaXeId;
            itemck.HistoryXeXuatBenId = xexuatben.Id;
            itemck.Latitude = Convert.ToDecimal(Latitude) / 1000000000m;
            itemck.Longitude = Convert.ToDecimal(Longitude) / 1000000000m;
            itemck.ViTriChot = VitriChot;
            if (itemck.Id > 0)
                _nhaxeService.UpdateChotKhach(itemck);
            else
                _nhaxeService.InsertChotKhach(itemck);
            itemck = _nhaxeService.GetChotKhachById(itemck.Id);

            string ghichu = string.Format("Chốt khách tại {0} : SL khách thực tế/phần mềm: {1}/{2}; người chốt: {3}", itemck.diemchot.TenDiemDon, sltt, slpm, nhanvienchot.HoVaTen);
            var item = CreateHistoryXeXuatBenLog(xexuatben.trangthai, ghichu, XeXuatBenId, nhanvienchot.Id);
            var nhatkyinfo = new
            {
                Id = item.Id,
                NgayTao = item.NgayTao.toStringDateTime(),
                GhiChu = item.GhiChu
            };
            return Successful(nhatkyinfo);
        }

        #endregion

        #region Misc

        public ActionResult InvalidApiToken(string apiToken)
        {
            var errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(apiToken))
                errorMessage = "No API token supplied.";
            else
                errorMessage = string.Format("Invalid API token: {0}", apiToken);

            return ErrorOccured(errorMessage);
        }

        public ActionResult ErrorOccured(string errorMessage)
        {
            return Json(new
            {
                success = false,
                data = errorMessage
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Successful(object data)
        {
            return Json(new
            {
                success = true,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SuccessfulSimple(string val)
        {
            var _data = new
            {
                RetMsg = val
            };
            return Successful(_data);
        }
        #endregion

        #region Helper methods

        private bool IsApiTokenValid(string apiToken)
        {
            if (string.IsNullOrWhiteSpace(apiToken) ||
                string.IsNullOrWhiteSpace(_settings.ApiToken))
                return false;

            return _settings.ApiToken.Trim().Equals(apiToken.Trim(),
                StringComparison.InvariantCultureIgnoreCase);
        }

        private object GetCustomerJson(Customer customer)
        {
            var customerJson = new
            {
                Id = customer.Id,
                Email = customer.Email,
                FullName = customer.GetFullName(),
                Phone = customer.GetPhone()
            };

            return customerJson;
        }




        #endregion
    }
}