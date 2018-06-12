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
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Nop.Plugin.NhaXeAPI.Controllers
{
    public class LApiController : BaseController
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
        private readonly ICacheManager _cacheManager;        
        #endregion

        #region Ctor

        public LApiController(ICacheManager cacheManager,
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
            this._cacheManager = cacheManager;
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
        private String DecodeParameter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return Server.UrlDecode(input);
        }
        private String CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        
        string getIPAddress()
        {
            string ip;
            try
            {
                ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.IndexOf(",") > 0)
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        ip = ipRange[le];
                    }
                }
                else
                {
                    ip = Request.UserHostAddress;
                }
            }
            catch { ip = null; }

            return ip; 
        }
        string isAuthentication(string codename)
        {
            if (string.IsNullOrEmpty(codename))
                return "CodeName rỗng";            
            if (!codename.Equals(_settings.CodeName, StringComparison.Ordinal))
                return "CodeName không đúng";
            string ip = getIPAddress();
            if (!ip.Equals(_settings.ClientIP, StringComparison.Ordinal))
                return "IP không được được phép truy cập";
            return String.Empty;
        }
        string isRightCheckSum(string checksum,params string[] inputs)
        {
            string _checksum=_settings.KeyPass;
            foreach(var input in inputs)
            {
                _checksum = _checksum+"," + input;
            }
            var _checksumhash=CreateMD5Hash(_checksum);
            if (!_checksumhash.Equals(checksum, StringComparison.Ordinal))
                return "CheckSum không đúng, bạn hãy kiểm tra lại KeyPass và cú pháp tạo checksum";
            return String.Empty;
        }
        NhanVien GetNhanVienCauHinh()
        {
            return _cacheManager.Get("SETTING_API_NHAN_VIEN_ID", () =>
            {
                return _nhanvienService.GetById(_settings.NhanVienId);                
            });
        }
        ChuyenDi GetChuyenDiHienTai(int ChuyenDiId)
        {
            string _chuyendicache_key=string.Format("SETTING_API_CHUYEN_DI_{0}",ChuyenDiId);
            return _cacheManager.Get(_chuyendicache_key, () =>
            {
                return _limousinebanveService.GetChuyenDiById(ChuyenDiId);
            });
        }
        #endregion
        #region Hanh trinh, chuyen di
        
        public ActionResult GetAllHanhTrinh(string codename, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var _nhanvien = GetNhanVienCauHinh();
            var vanphongids = _nhanvien.VanPhongs.Select(c => c.Id).ToArray();
            var hanhtrinhs = _hanhtrinhService.GetAllHanhTrinhByNhaXeId(_nhanvien.NhaXeID, vanphongids).Select(c=> new {
                Id=c.Id,
                MoTa=c.MoTa,
                MaHanhTrinh = c.MaHanhTrinh,
                TongKhoangCach = c.TongKhoangCach
            }).ToList();
            return Successful(hanhtrinhs);
        }
        public ActionResult GetAllChuyenDi(string codename,int HanhTrinhId,int KhungGioId, string NgayDi,string ThongTinKhachHang, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, HanhTrinhId.ToString(), KhungGioId.ToString(), NgayDi, ThongTinKhachHang);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            DateTime _ngaydi;
            if(!DateTime.TryParse(NgayDi,out _ngaydi))
            {
                return ErrorOccured("Ngày đi không đúng định dạng (yyyy-mm-dd)");
            }
            var _nhanvien = GetNhanVienCauHinh();
            var chuyendis = _limousinebanveService.GetAllChuyenDi(_nhanvien.NhaXeID, HanhTrinhId, 0, (ENKhungGio)KhungGioId, _ngaydi, "", ThongTinKhachHang).Select(c => { return c.toModel(_localizationService); }).ToList();
            return Successful(chuyendis);
        }
        public ActionResult GetChuyenDi(string codename, int ChuyenDiId, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, ChuyenDiId.ToString());
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var chuyendi = _limousinebanveService.GetChuyenDiById(ChuyenDiId);
            if (chuyendi == null)
            {
                return ErrorOccured("Chuyến đi không tồn tại");
            }
            return Successful(chuyendi.toModel(_localizationService));

        }

        public ActionResult GetSoDoGheXe(string codename, int ChuyenDiId, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, ChuyenDiId.ToString());
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var xexuatben = GetChuyenDiHienTai(ChuyenDiId);
            if (xexuatben == null)
            {
                return ErrorOccured("Chuyến đi không tồn tại");
            }
            var datves = _limousinebanveService.GetDatVeByChuyenDi(ChuyenDiId);
            //lay thong tin so do ghe xe
            var sodoghequytacs = _xeinfoService.GetAllSoDoGheXeQuyTac(xexuatben.lichtrinhloaixe.LoaiXeId);
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
                    phoive.GiaTien = _limousinebanveService.GetGiaVeTheoSoDoId(xexuatben.HanhTrinhId, xexuatben.lichtrinhloaixe.LoaiXeId, sodo.Id);
                    //lay gia ve theo cau hinh gia ve theo lich trinh
                    if (phoive.GiaTien == decimal.Zero)
                        phoive.GiaTien = xexuatben.lichtrinhloaixe.GiaVe;
                }
                item.Id = phoive.Id;
                item.TrangThaiId = phoive.TrangThaiId;
                item.ChuyenDiId = phoive.ChuyenDiId.GetValueOrDefault(0);
                item.NgayDi = phoive.NgayDi.toStringDate();
                item.GiaVe = Convert.ToInt32(phoive.GiaTien);
                item.CustomerId = phoive.KhachHangId.GetValueOrDefault();
                item.MaVe = phoive.Ma;
                item.ViTriLen = phoive.TenDiemDon;
                item.ViTriXuong = phoive.TenDiemDon;
                if (phoive.khachhang != null)
                {
                    item.TenKhachHang = phoive.khachhang.Ten;
                    item.SoDienThoai = phoive.khachhang.DienThoai;
                }
                phoives.Add(item);
            }

            return Successful(phoives);
        }
        #endregion
        #region Nghiep vu dat ve
        private DatVe _DatCho(ChuyenDi chuyendi, int SoDoGheId,string SessionId)
        {
            //trang thai khong hop le
            if (chuyendi.trangthai == ENTrangThaiXeXuatBen.KET_THUC || chuyendi.trangthai == ENTrangThaiXeXuatBen.HUY)
            {
                return null;
            }
            //kiem tra xem co ai dat chua
            var checkdatve = chuyendi.DatVes.Where(c => c.SoDoGheId == SoDoGheId && c.TrangThaiId != (int)ENTrangThaiDatVe.HUY).FirstOrDefault();
            if (checkdatve != null)
            {
                //neu co roi thi kiem tra thoi gian dat trc do neu chua qua 2 phut thi out
                if (checkdatve.NgayTao.AddSeconds(_settings.THOI_GIAN_GHE_DAT_CHO) > DateTime.Now)
                {
                    //trong truong hop dat ve, nhung paste thong tin thi bo qua cho dat nay, neu da dat roi
                    return null;
                }
                else
                {
                    //kiem tra trang thai ghe 
                    if (checkdatve.trangthai == ENTrangThaiDatVe.MOI)
                    {
                        //huy ghe nay di                        
                        _limousinebanveService.DeleteDatVe(checkdatve);

                    }
                    else
                        return null;

                }
            }
            //tao thong tin dat ve
            var datve = new DatVe();
            datve.NhaXeId = chuyendi.NhaXeId;
            datve.ChuyenDiId = chuyendi.Id;
            datve.SoDoGheId = SoDoGheId;
            datve.NguoiTaoId = _settings.NhanVienId;
            datve.NgayDi = chuyendi.NgayDi;
            datve.LichTrinhId = chuyendi.LichTrinhId;
            datve.HanhTrinhId = chuyendi.HanhTrinhId;
            datve.trangthai = ENTrangThaiDatVe.MOI;
            datve.SessionID = SessionId;           
            _limousinebanveService.InsertDatVe(datve);
            return _limousinebanveService.GetDatVeById(datve.Id);
        }
        public ActionResult DatCho(string codename, int ChuyenDiId, int SoDoGheId,string SessionId, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, ChuyenDiId.ToString(), SoDoGheId.ToString(), SessionId);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            var chuyendi = _limousinebanveService.GetChuyenDiById(ChuyenDiId);
            if (chuyendi == null)
            {
                return ErrorOccured("Chuyến đi không tồn tại");
            }
            //trang thai khong hop le
            if (chuyendi.trangthai == ENTrangThaiXeXuatBen.KET_THUC || chuyendi.trangthai == ENTrangThaiXeXuatBen.HUY)
            {
                return ErrorOccured("Chuyến đi đã kết thúc hoặc bị hủy");
            }
            //kiem tra xem co ai dat chua
            var checkdatve = _DatCho(chuyendi, SoDoGheId, SessionId);
            if (checkdatve == null)
            {
                return ErrorOccured("Vị trí đã có người đặt chỗ");
            }
            return Successful(checkdatve.toModel(_localizationService));

        }
        public ActionResult DatVe(string codename, int ChuyenDiId, string SessionId, int GiaTien, int isThanhToan, string TenKhach, string DienThoai, string TenDiemDon, string TenDiemTra, string GhiChu, string checksum)
        {
            
            
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, ChuyenDiId.ToString(), SessionId, GiaTien.ToString(), isThanhToan.ToString(), TenKhach, DienThoai, TenDiemDon, TenDiemTra, GhiChu);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);



            var chuyendi = _limousinebanveService.GetChuyenDiById(ChuyenDiId);
            if (chuyendi == null)
            {
                return ErrorOccured("Chuyến đi không tồn tại");
            }
            //trang thai khong hop le
            if (chuyendi.trangthai == ENTrangThaiXeXuatBen.KET_THUC || chuyendi.trangthai == ENTrangThaiXeXuatBen.HUY)
            {
                return ErrorOccured("Chuyến đi đã kết thúc hoặc bị hủy");
            }
            //kiem tra xem co ai dat chua
            var _datveitems = _limousinebanveService.GetDatVeBySession(chuyendi.NhaXeId, chuyendi.Id, SessionId);
            if(_datveitems.Count==0)
            {
                return ErrorOccured("Không tìm thấy thông tin đặt chỗ");
            }
            TenKhach = DecodeParameter(TenKhach);
            DienThoai = DecodeParameter(DienThoai);
            TenDiemDon = DecodeParameter(TenDiemDon);
            TenDiemTra = DecodeParameter(TenDiemTra);
            GhiChu = DecodeParameter(GhiChu);
            //cap nhat thong tin khach hang
            var _kh = _limousinebanveService.GetKhachHangByDienThoai(chuyendi.NhaXeId, DienThoai);
            if (_kh == null)
                _kh = new KhachHang();
            _kh.NhaXeId = chuyendi.NhaXeId;
            _kh.DienThoai = DienThoai;
            _kh.Ten = TenKhach;
            _kh.DiaChi = TenDiemDon;
            if (_kh.Id > 0)
                _limousinebanveService.UpdateKhachHang(_kh);
            else
                _kh = _limousinebanveService.InsertKhachHang(_kh);
            
            foreach (var _datveitem in _datveitems)
            {
                _datveitem.DiaChiNha = TenDiemDon;
                _datveitem.GhiChu = GhiChu;
                _datveitem.KhachHangId = _kh.Id;
                _datveitem.isThanhToan = isThanhToan == 1;
                _datveitem.trangthai = ENTrangThaiDatVe.DA_XEP_CHO;               
                _datveitem.TenDiemDon = TenDiemDon;
                _datveitem.TenDiemTra = TenDiemTra;
                _datveitem.GiaTien = Convert.ToDecimal(GiaTien);
                _limousinebanveService.UpdateDatVe(_datveitem);
            }
            var models = _limousinebanveService.GetDatVeBySession(chuyendi.NhaXeId, chuyendi.Id, SessionId).Select(c=> {
                return c.toModel(_localizationService);
            }).ToList();
            return Successful(models);

        }

        public ActionResult ChuyenVe(string codename, int DatVeId, int ChuyenDiId, int SoDoGheId, string GhiChu, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, DatVeId.ToString(), ChuyenDiId.ToString(), SoDoGheId.ToString(), GhiChu);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            if (!_settings.isChoPhepHuy)
                return ErrorOccured("Bạn bị cấm chuyển vé");

            var chuyendi = _limousinebanveService.GetChuyenDiById(ChuyenDiId);
            if (chuyendi == null)
            {
                return ErrorOccured("Chuyến đi không tồn tại");
            }
            //trang thai khong hop le
            if (chuyendi.trangthai == ENTrangThaiXeXuatBen.KET_THUC || chuyendi.trangthai == ENTrangThaiXeXuatBen.HUY || chuyendi.NgayDiThuc.AddMinutes(60) < DateTime.Now)
            {
                return ErrorOccured("Chuyến đi đã kết thúc hoặc bị hủy hoặc hết thời gian được phép thao tác");
            }

            var _datveitemold = _limousinebanveService.GetDatVeById(DatVeId);
            if (_datveitemold == null)
            {
                return ErrorOccured("Thông tin đặt vé không tồn tại");
            }
            //trang thai khong hop le
            if (_datveitemold.trangthai == ENTrangThaiDatVe.HUY)
            {
                return ErrorOccured("Vé đã bị hủy");
            }
            //kiem tra phai cung nguoi dat ve thi moi dc huy
            if (_datveitemold.NguoiTaoId != _settings.NhanVienId)
                return ErrorOccured("Bạn không thể chuyển vé này(không thuộc sở hữu)");

            var _datveitemnew = _DatCho(chuyendi, SoDoGheId,new Guid().ToString());
            if (_datveitemnew == null)
            {
                return ErrorOccured("Vị trí đã có người đặt chỗ");
            }
            GhiChu = DecodeParameter(GhiChu);

            _datveitemnew.isDonTaxi = _datveitemold.isDonTaxi;
            _datveitemnew.DiaChiNha = _datveitemold.DiaChiNha;
            _datveitemnew.GhiChu = GhiChu;
            _datveitemnew.DiemDonId = _datveitemold.DiemDonId;
            _datveitemnew.KhachHangId = _datveitemold.KhachHangId;
            _datveitemnew.isThanhToan = _datveitemold.isThanhToan;
            _datveitemnew.isNoiBai = _datveitemold.isNoiBai;
            _datveitemnew.trangthai = _datveitemold.trangthai;
            _datveitemnew.isLenhDonTaXi = _datveitemold.isLenhDonTaXi;
            _datveitemnew.MaTaXi = _datveitemold.MaTaXi;
            _datveitemnew.isDaXacNhan = _datveitemold.isDaXacNhan;
            _datveitemnew.TenDiemDon = _datveitemold.TenDiemDon;
            _datveitemnew.TenDiemTra = _datveitemold.TenDiemTra;
            _datveitemnew.VeChuyenDenId = _datveitemold.Id;
            _datveitemnew.TenKhachHangDiKem = _datveitemold.TenKhachHangDiKem;
            //add by lent, trong truong hop chuyen tu hanh trinh khac sang hanh trinh moi, thi van luu hanh trinh nhu cu
            _datveitemnew.GiaTien = _datveitemold.GiaTien;
            _datveitemnew.HanhTrinhId = _datveitemold.HanhTrinhId;
            _datveitemnew.LichTrinhId = _datveitemold.LichTrinhId;

            _limousinebanveService.UpdateDatVe(_datveitemnew);
            _datveitemold.trangthai = ENTrangThaiDatVe.HUY;
            _datveitemold.GhiChu = _datveitemold.GhiChu + string.Format("Lý do hủy: Chuyển sang chuyến đi mới (Id={0})", ChuyenDiId);
            _limousinebanveService.UpdateDatVe(_datveitemold);
            // luu lai nhat ky
            var note = "HD " + _datveitemold.Ma + " được chuyển sang " + _datveitemnew.chuyendi.Ma + " lúc " + _datveitemnew.chuyendi.NgayDiThuc + " bởi " + _datveitemnew.nguoitao.HoVaTen;
            _limousinebanveService.InsertDatVeNote(_datveitemold.Id, note);

            _datveitemold.trangthai = ENTrangThaiDatVe.HUY;
            _limousinebanveService.UpdateDatVe(_datveitemold);
            return SuccessfulSimple("OK");
        }
        public ActionResult HuyVe(string codename, int ChuyenDiId, int DatVeId, string GhiChu, string checksum)
        {
            //kiem tra ket noi setting
            string _checkauthentication = isAuthentication(codename);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            //kiem tra check sum
            _checkauthentication = isRightCheckSum(checksum, codename, ChuyenDiId.ToString(), DatVeId.ToString(), GhiChu);
            if (!String.IsNullOrEmpty(_checkauthentication))
                return ErrorOccured(_checkauthentication);
            if(!_settings.isChoPhepHuy)
                return ErrorOccured("Bạn bị cấm hủy vé");

            var chuyendi = _limousinebanveService.GetChuyenDiById(ChuyenDiId);
            if (chuyendi == null)
            {
                return ErrorOccured("Chuyến đi không tồn tại");
            }
            //trang thai khong hop le
            if (chuyendi.trangthai == ENTrangThaiXeXuatBen.KET_THUC || chuyendi.trangthai == ENTrangThaiXeXuatBen.HUY || chuyendi.NgayDiThuc.AddMinutes(60)<DateTime.Now)
            {
                return ErrorOccured("Chuyến đi đã kết thúc hoặc bị hủy hoặc hết thời gian được phép hủy");
            }
           
            var datve = _limousinebanveService.GetDatVeById(DatVeId);
            if (datve == null)
            {
                return ErrorOccured("Thông tin đặt vé không tồn tại");
            }
            //trang thai khong hop le
            if (datve.trangthai == ENTrangThaiDatVe.HUY)
            {
                return ErrorOccured("Vé đã bị hủy");
            }
            //kiem tra phai cung nguoi dat ve thi moi dc huy
            if (datve.NguoiTaoId != _settings.NhanVienId)
                return ErrorOccured("Bạn không thể hủy vé này(không thuộc sở hữu)");
            GhiChu = DecodeParameter(GhiChu);
            datve.GhiChu = datve.GhiChu+"->"+GhiChu;
            datve.trangthai = ENTrangThaiDatVe.HUY;
            _limousinebanveService.UpdateDatVe(datve);
            return SuccessfulSimple("OK");
        }
       
        #endregion


        #region Misc

       
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

        public ActionResult TestUrlDecode(string val)
        {
            val = DecodeParameter(val);
            return SuccessfulSimple(val);
        }
    }
}