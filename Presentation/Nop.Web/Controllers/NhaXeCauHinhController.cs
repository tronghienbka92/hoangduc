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

namespace Nop.Web.Controllers
{
    public class NhaXeCauHinhController : BaseNhaXeController
    {
        const string _ITEMS = "[ITEMS]";
        const string _ITEM1S = "[ITEM1S]";
        #region Khoi Tao
        private readonly IStateProvinceService _stateProvinceService;
        private readonly INhaXeService _nhaxeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly IPhieuGuiHangService _phieuguihangService;
        private readonly IHangHoaService _hanghoaService;
        private readonly ICustomerService _customerService;
        private readonly IDiaChiService _diachiService;
        private readonly INhanVienService _nhanvienService;
        private readonly IPermissionService _permissionService;
        private readonly IXeInfoService _xeinfoService;
        private readonly IHanhTrinhService _hanhtrinhService;
        private readonly IBenXeService _benxeService;
        private readonly INhaXeCustomerService _nhaxecustomerService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IPhoiVeService _phoiveService;
        private readonly IGiaoDichKeVeXeService _giaodichkeveService;
        private readonly ILimousineBanVeService _limousinebanveService;
        private readonly IHopDongChuyenService _hopdongchuyenService;
        public NhaXeCauHinhController(IStateProvinceService stateProvinceService,
            INhaXeService nhaxeService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IPictureService pictureService,
             IPhieuGuiHangService phieuguihangService,
            IHangHoaService hanghoaService,
            ICustomerService customerService,
            IDiaChiService diachiService,
            INhanVienService nhanvienService,
            IPermissionService permissionService,
            IXeInfoService xeinfoService,
            IHanhTrinhService hanhtrinhService,
            IBenXeService benxeService,
            INhaXeCustomerService nhaxecustomerService,
            IPriceFormatter priceFormatter,
            IPhoiVeService phoiveService,
            IGiaoDichKeVeXeService giaodichkeveService,
            ILimousineBanVeService limousinebanveService,
            IHopDongChuyenService hopdongchuyenService
            )
        {
            this._limousinebanveService = limousinebanveService;
            this._phoiveService = phoiveService;
            this._priceFormatter = priceFormatter;
            this._stateProvinceService = stateProvinceService;
            this._nhaxeService = nhaxeService;
            this._hanghoaService = hanghoaService;
            this._phieuguihangService = phieuguihangService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._pictureService = pictureService;
            this._customerService = customerService;
            this._diachiService = diachiService;
            this._nhanvienService = nhanvienService;
            this._permissionService = permissionService;
            this._xeinfoService = xeinfoService;
            this._hanhtrinhService = hanhtrinhService;
            this._benxeService = benxeService;
            this._nhaxecustomerService = nhaxecustomerService;
            this._giaodichkeveService = giaodichkeveService;
            this._hopdongchuyenService = hopdongchuyenService;
        }
        #endregion
        NhaXeCauHinhModel fromEntity(NhaXeCauHinh item)
        {
            var model = new NhaXeCauHinhModel();
            model.Id = item.Id;
            model.kieudulieu = item.kieudulieu;
            model.MaCauHinh = item.MaCauHinh;
            model.Ten = item.Ten;
            model.GiaTri = item.GiaTri;
            return model;
        }
        NhaXeCauHinh fromModel(NhaXeCauHinhModel model)
        {
            var item = new NhaXeCauHinh();
            item.NhaXeId = _workContext.NhaXeId;
            item.MaCauHinh = model.MaCauHinh;
            item.kieudulieu = model.kieudulieu;
            item.Ten = model.Ten;
            item.GiaTri = model.GiaTri;
            return item;
        }

        #region Giao dich ke ve menh gia model
        GiaoDichKeVeMenhGiaModel toModel(GiaoDichKeVeMenhGia entity, int STT)
        {
            var model = new GiaoDichKeVeMenhGiaModel();
            model.Id = entity.Id;
            model.STT = STT;
            model.MenhGiaId = entity.MenhGiaId;
            model.MenhGia = entity.menhgia.MenhGia;
            model.SoLuong = entity.SoLuong;
            model.SeriFrom = entity.SeriFrom;
            if (model.SoLuong > 0)
                model.SeriTo = string.Format("{0}", entity.SeriNumFrom + entity.SoLuong - 1);
            model.isVeMoi = entity.isVeMoi;
            model.GhiChu = entity.GhiChu;
            model.ActionType = entity.ActionType;
            return model;
        }
        #endregion
        void setGiaTriSubItem(NhaXeCauHinhModel model, bool isSet = true)
        {
            try
            {
                bool isCheck = false;
                switch (model.MaCauHinh)
                {
                    case ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN:
                        {
                            var _ItemPerPage = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN_PAGES);
                            if (_ItemPerPage != null)
                                model.ItemPerPage = Convert.ToInt32(_ItemPerPage.GiaTri);
                            var _startendrepeat = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN_REPEATSTARTEND);
                            if (_startendrepeat != null)
                            {
                                string[] arrstartend = _startendrepeat.GiaTri.Split('|');
                                if (arrstartend.Length == 2)
                                {
                                    model.KyTuRepeatStart = arrstartend[0];
                                    model.KyTuRepeatEnd = arrstartend[1];
                                    isCheck = true;
                                }
                            }


                            break;
                        }
                    case ENNhaXeCauHinh.VE_MAU_IN_PHOI:
                        {
                            var _ItemPerPage = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.VE_MAU_IN_PHOI_PAGES);
                            if (_ItemPerPage != null)
                                model.ItemPerPage = Convert.ToInt32(_ItemPerPage.GiaTri);
                            var _startendrepeat = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.VE_MAU_IN_PHOI_REPEATSTARTEND);
                            if (_startendrepeat != null)
                            {
                                string[] arrstartend = _startendrepeat.GiaTri.Split('|');
                                if (arrstartend.Length == 2)
                                {
                                    model.KyTuRepeatStart = arrstartend[0];
                                    model.KyTuRepeatEnd = arrstartend[1];
                                    isCheck = true;
                                }
                            }
                            break;
                        }
                    case ENNhaXeCauHinh.LENH_PHU_DON_KHACH:
                        {
                            var _ItemPerPage = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_PHU_DON_KHACH_PAGES);
                            if (_ItemPerPage != null)
                                model.ItemPerPage = Convert.ToInt32(_ItemPerPage.GiaTri);
                            var _startendrepeat = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_PHU_DON_KHACH_REPEATSTARTEND);
                            if (_startendrepeat != null)
                            {
                                string[] arrstartend = _startendrepeat.GiaTri.Split('|');
                                if (arrstartend.Length == 2)
                                {
                                    model.KyTuRepeatStart = arrstartend[0];
                                    model.KyTuRepeatEnd = arrstartend[1];
                                    isCheck = true;
                                }
                            }
                            break;
                        }
                    case ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI:
                        {
                            var _ItemPerPage = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI_PAGES);
                            if (_ItemPerPage != null)
                                model.ItemPerPage = Convert.ToInt32(_ItemPerPage.GiaTri);
                            var _startendrepeat = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI_REPEATSTARTEND);
                            if(_startendrepeat !=null)
                            {
                                string[] arrstartend = _startendrepeat.GiaTri.Split('|');
                                if(arrstartend.Length == 2)
                                {
                                    model.KyTuRepeatStart = arrstartend[0];
                                    model.KyTuRepeatEnd = arrstartend[1];
                                    isCheck = true;
                                }
                            }
                            break;
                        }
                    case ENNhaXeCauHinh.LENH_DIEU_DONG_XE:
                        {
                            var _ItemPerLage = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_DIEU_DONG_XE_PAGES);
                            if (_ItemPerLage != null)
                                model.ItemPerPage = Convert.ToInt32(_ItemPerLage.GiaTri);
                            var _startendrepeat = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_DIEU_DONG_XE_REPEATSTARTEND);
                            if(_startendrepeat !=null)
                            {
                                string[] arrstartend = _startendrepeat.GiaTri.Split('|');
                                if(arrstartend.Length==2)
                                {
                                    model.KyTuRepeatStart = arrstartend[0];
                                    model.KyTuRepeatEnd = arrstartend[1];
                                }
                            }
                            break;
                        }
                    case ENNhaXeCauHinh.MAU_DS_TO_TAXI:
                        {
                            var _ItemPerPage = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.MAU_DS_TO_TAXI_PAGES);
                            if (_ItemPerPage != null)
                                model.ItemPerPage = Convert.ToInt32(_ItemPerPage.GiaTri);
                            var _startendrepeat = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.MAU_DS_TO_TAXI_REPEATSTARTEND);
                            if (_startendrepeat != null)
                            {
                                string[] arrstartend = _startendrepeat.GiaTri.Split('|');
                                if (arrstartend.Length == 2)
                                {
                                    model.KyTuRepeatStart = arrstartend[0];
                                    model.KyTuRepeatEnd = arrstartend[1];
                                    isCheck = true;
                                }
                            }
                            break;
                        }
                    case ENNhaXeCauHinh.VE_MAU_IN_CUONG_VE:
                        {
                            var _solien = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.VE_MAU_IN_CUONG_VE_LIEN);
                            if (_solien != null)
                                model.SoLien = Convert.ToInt32(_solien.GiaTri);
                            break;
                        }
                    case ENNhaXeCauHinh.KY_GUI_PHIEU_GUI_HANG:
                        {
                            var _solien = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.KY_GUI_PHIEU_GUI_HANG_LIEN);
                            if (_solien != null)
                                model.SoLien = Convert.ToInt32(_solien.GiaTri);
                            break;
                        }
                }
                if (!isCheck || !isSet)
                    return;

                int _posrepeat = model.GiaTri.IndexOf(model.KyTuRepeatStart);
                if (_posrepeat > 0)
                {
                    string _part1 = model.GiaTri.Substring(0, _posrepeat);
                    string _part2 = model.GiaTri.Substring(_posrepeat);
                    int _posendrepeat = _part2.IndexOf(model.KyTuRepeatEnd);
                    string _part3 = _part2.Substring(_posendrepeat + model.KyTuRepeatEnd.Length);
                    _part2 = _part2.Substring(0, _posendrepeat + model.KyTuRepeatEnd.Length);
                    model.GiaTri = _part1 + _ITEMS + _part3;
                    model.GiaTriItem = _part2;
                    //kiem tra tiep co vong lap ko
                    _posrepeat = model.GiaTri.IndexOf(model.KyTuRepeatStart);
                    if (_posrepeat > 0)
                    {
                        _part1 = model.GiaTri.Substring(0, _posrepeat);
                        _part2 = model.GiaTri.Substring(_posrepeat);
                        _posendrepeat = _part2.IndexOf(model.KyTuRepeatEnd);
                        _part3 = _part2.Substring(_posendrepeat + model.KyTuRepeatEnd.Length);
                        _part2 = _part2.Substring(0, _posendrepeat + model.KyTuRepeatEnd.Length);
                        model.GiaTri = _part1 + _ITEM1S + _part3;
                        model.GiaTriItem1 = _part2;
                    }

                }
            }
            catch
            { }

        }
        string getGiaTri(string _item, string code, string val)
        {
            return _item.Replace("[" + code + "]", val);
        }
        void setGiaTri(NhaXeCauHinhModel model, string code, string val, bool isEmpty = false)
        {
            if (isEmpty)
                model.GiaTri = model.GiaTri.Replace(code, val);
            else
                model.GiaTri = model.GiaTri.Replace("[" + code + "]", val);
        }
        void setGiaTri(NhaXeCauHinhModel model, string code, int val)
        {
            model.GiaTri = model.GiaTri.Replace("[" + code + "]", val.ToSoNguyen());
        }
        void setGiaTri(NhaXeCauHinhModel model, string code, Decimal val)
        {
            model.GiaTri = model.GiaTri.Replace("[" + code + "]", val.ToTien(_priceFormatter));
        }
        void setGiaTriNgayThang(NhaXeCauHinhModel model, DateTime dt)
        {
            setGiaTri(model, "NGAY", dt.ToString("dd"));
            setGiaTri(model, "THANG", dt.ToString("MM"));
            setGiaTri(model, "NAM", dt.ToString("yyyy"));
        }

        void setGiaTriModel(NhaXeCauHinhModel model, int Id)
        {
           // setGiaTriNgayThang(model, DateTime.Now);
            switch (model.MaCauHinh)
            {
                case ENNhaXeCauHinh.LENH_DON_TAXI:
                    {
                        //lay thong tin phoi ve
                        var _datve = _limousinebanveService.GetDatVeById(Id);
                        if (_datve != null && _datve.trangthai != ENTrangThaiDatVe.HUY)
                        {
                            var datve = _datve.toModel(_localizationService);
                            setGiaTri(model, "MA", datve.Ma);
                            setGiaTri(model, "THONG_TIN_DAT_VE",string.Format("{0} - {1}", datve.TenChuyenDi,datve.TenHanhTrinh));
                            setGiaTri(model, "TEN_KHACH", datve.TenKhachHang);
                            setGiaTri(model, "DIEN_THOAI", datve.DienThoai);
                            setGiaTri(model, "DIA_CHI", datve.DiaChiNha);
                            setGiaTri(model, "DIEM_DON", datve.TenDiemDon);
                            setGiaTri(model, "MA_TAXI", datve.MaTaXi);
                            setGiaTri(model, "NGAY_DI", datve.NgayDi.ToString("dd/MM/yyyy HH:mm"));

                        }
                        else
                        {
                            model.GiaTri = "Thông tin đặt vé không hợp lệ";
                        }
                        break;
                    }
               
                case ENNhaXeCauHinh.VE_MAU_IN_CUONG_VE:
                    {
                        //lay thong tin phoi ve
                       var _datve = _limousinebanveService.GetDatVeById(Id);
                        if (_datve != null && _datve.trangthai != ENTrangThaiDatVe.HUY)
                        {
                              var datve = _datve.toModel(_localizationService);
                            setGiaTri(model, "TENDAYDU", datve.TenKhachHang);
                            setGiaTri(model, "DIACHITHANHVIEN", datve.DiaChiNha);
                            setGiaTri(model, "LYDONOP", "");
                            setGiaTri(model, "SOTIEN", datve.GiaTien.ToTien(_priceFormatter));
                            setGiaTri(model, "SOTIENBANGCHU", datve.GiaTien.ToTienBangChu());
                            setGiaTriNgayThang(model, DateTime.Now);
                            string contents = "";
                            for (int l = 1; l <= model.SoLien; l++)
                            {
                                contents = contents + model.GiaTri.Replace("[LIEN_NUM]", l.ToString());
                            }
                            model.GiaTri = contents;
                        }
                        else
                        {
                            model.GiaTri = "Thông tin vé không hợp lệ";
                        }
                        break;
                    }
                case ENNhaXeCauHinh.KY_GUI_PHIEU_GUI_HANG:
                    {
                        //lay thong tin phieu gui hang
                        var item = _phieuguihangService.GetPhieuGuiById(Id);
                        if (item != null && item.NhaXeId == _workContext.NhaXeId)
                        {
                            setGiaTri(model, "MA", item.MaPhieu);
                            setGiaTri(model, "VPGUI_TEN", item.VanPhongGui.TenVanPhong);
                            setGiaTri(model, "VPGUI_MA", item.VanPhongGui.Ma);
                            setGiaTri(model, "VPGUI_DIENTHOAI", item.VanPhongGui.DienThoaiDatVe);


                            if (item.VanPhongGui.diachiinfo != null)
                            {
                                setGiaTri(model, "VPGUI_DIACHI", item.VanPhongGui.diachiinfo.ToText());
                                setGiaTri(model, "VPGUI_FAX", item.VanPhongGui.diachiinfo.Fax);
                            }
                            else
                            {
                                setGiaTri(model, "VPGUI_DIACHI", "");
                                setGiaTri(model, "VPGUI_FAX", "");
                            }

                            setGiaTri(model, "VPNHAN_TEN", item.VanPhongNhan.TenVanPhong);
                            setGiaTri(model, "VPNHAN_MA", item.VanPhongNhan.Ma);
                            setGiaTri(model, "VPNHAN_DIENTHOAI", item.VanPhongNhan.DienThoaiDatVe);
                            if (item.VanPhongNhan.diachiinfo != null)
                            {
                                setGiaTri(model, "VPNHAN_DIACHI", item.VanPhongNhan.diachiinfo.ToText());
                                setGiaTri(model, "VPNHAN_FAX", item.VanPhongNhan.diachiinfo.Fax);
                            }
                            else
                            {
                                setGiaTri(model, "VPNHAN_DIACHI", "");
                                setGiaTri(model, "VPNHAN_FAX", "");
                            }

                            setGiaTri(model, "NGUOIGUI_TEN", item.NguoiGui.HoTen);
                            setGiaTri(model, "NGUOIGUI_DIACHI", item.NguoiGui.DiaChiLienHe);
                            setGiaTri(model, "NGUOIGUI_DIENTHOAI", item.NguoiGui.DienThoai);
                            setGiaTri(model, "NGUOINHAN_TEN", item.NguoiNhan.HoTen);
                            setGiaTri(model, "NGUOINHAN_DIACHI", item.NguoiNhan.DiaChiLienHe);
                            setGiaTri(model, "NGUOINHAN_DIENTHOAI", item.NguoiNhan.DienThoai);
                            setGiaTri(model, "THONG_TIN_HANG_HOA", item.ThongTinHanHoa());
                            setGiaTri(model, "GHI_CHU", item.GhiChu);
                            setGiaTri(model, "KHOI_LUONG", item.TongKhoiLuong.ToSoNguyen());
                            setGiaTri(model, "KICH_THUOC", item.TongSoKien.ToSoNguyen());
                            setGiaTri(model, "TIEN_CUOC", item.TongTienCuoc);
                            if (item.DaThuCuoc)
                                setGiaTri(model, "THANH_TOAN", "Đã thanh toán");
                            else
                                setGiaTri(model, "THANH_TOAN", "Chưa thanh toán");
                            string tienchu = item.TongTienCuoc.ToTienBangChu();

                            setGiaTri(model, "TIEN_CUOC_CHU", tienchu);
                            string contents = "";
                            for (int l = 1; l <= model.SoLien; l++)
                            {
                                contents = contents + model.GiaTri.Replace("[LIEN_NUM]", l.ToString());
                            }
                            model.GiaTri = contents;
                        }
                        break;
                    }
                case ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN:
                    {
                        var _historyxexuatben = _nhaxeService.GetHistoryXeXuatBenId(Id);
                        var phieuguihangs = _phieuguihangService.GetAll(_workContext.NhaXeId, XeXuatBenId: Id, TinhTrangVanChuyenId: ENTinhTrangVanChuyen.DangVanChuyen);
                        if (_historyxexuatben != null && _historyxexuatben.NguonVeInfo.NhaXeId == _workContext.NhaXeId)
                        {
                            PhieuGuiHang phieuinfo = null;
                            if (phieuguihangs.Count > 0)
                                phieuinfo = phieuguihangs[0];

                            setGiaTri(model, "MA", "HH" + Id.ToString());
                            if (phieuinfo != null)
                            {
                                setGiaTri(model, "VPGUI_TEN", phieuinfo.VanPhongGui.TenVanPhong);
                                setGiaTri(model, "VPNHAN_TEN", phieuinfo.VanPhongNhan.TenVanPhong);
                            }
                            else
                            {
                                setGiaTri(model, "VPGUI_TEN", "");
                                setGiaTri(model, "VPNHAN_TEN", "");
                            }

                            setGiaTri(model, "HANH_TRINH", string.Format("{0} - {1}", _historyxexuatben.NguonVeInfo.TenDiemDon, _historyxexuatben.NguonVeInfo.TenDiemDen));
                            setGiaTri(model, "LICH_TRINH", _historyxexuatben.NguonVeInfo.LichTrinhInfo.ThoiGianDi.ToString("HH:mm"));
                            setGiaTri(model, "SO_XE", _historyxexuatben.xevanchuyen.BienSo);

                            string _itemcontents = "";
                            int i = 1;
                            decimal tongkl = decimal.Zero, tongcuoc = decimal.Zero;
                            int tongkien = 0;
                            foreach (var p in phieuguihangs)
                            {
                                p.TongKhoiLuong = p.HangHoas.Sum(c => c.CanNang * c.SoLuong);
                                p.TongTienCuoc = p.HangHoas.Sum(c => c.GiaCuoc * c.SoLuong);
                                p.TongSoKien = p.HangHoas.Sum(c => c.SoLuong);
                                string _itemcontent = model.GiaTriItem;
                                _itemcontent = getGiaTri(_itemcontent, "STT", i.ToString());
                                _itemcontent = getGiaTri(_itemcontent, "MA_PHIEU", p.MaPhieu);
                                _itemcontent = getGiaTri(_itemcontent, "NGUOIGUI_TEN", p.NguoiGui.HoTen);
                                _itemcontent = getGiaTri(_itemcontent, "NGUOIGUI_DIENTHOAI", p.NguoiGui.DienThoai);
                                _itemcontent = getGiaTri(_itemcontent, "NGUOINHAN_TEN", p.NguoiNhan.HoTen);
                                _itemcontent = getGiaTri(_itemcontent, "NGUOINHAN_DIENTHOAI", p.NguoiNhan.DienThoai);
                                _itemcontent = getGiaTri(_itemcontent, "SO_LUONG", p.HangHoas.Count.ToString());
                                _itemcontent = getGiaTri(_itemcontent, "KHOI_LUONG", p.TongKhoiLuong.ToSoNguyen());
                                _itemcontent = getGiaTri(_itemcontent, "THANH_TIEN", Convert.ToInt32(p.TongTienCuoc / 1000).ToSoNguyen());
                                _itemcontent = getGiaTri(_itemcontent, "GHI_CHU", p.GhiChu);
                                _itemcontents = _itemcontents + _itemcontent;
                                tongkl += p.TongKhoiLuong;
                                tongcuoc += p.TongTienCuoc / 1000;
                                tongkien = p.HangHoas.Count;
                                i++;
                            }
                            if (i < model.ItemPerPage)
                            {
                                for (int j = i; j <= model.ItemPerPage; j++)
                                {
                                    string _itemcontent = model.GiaTriItem;
                                    _itemcontent = getGiaTri(_itemcontent, "STT", j.ToString());
                                    _itemcontent = getGiaTri(_itemcontent, "MA_PHIEU", "");
                                    _itemcontent = getGiaTri(_itemcontent, "NGUOIGUI_TEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "NGUOIGUI_DIENTHOAI", "");
                                    _itemcontent = getGiaTri(_itemcontent, "NGUOINHAN_TEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "NGUOINHAN_DIENTHOAI", "");
                                    _itemcontent = getGiaTri(_itemcontent, "SO_LUONG", "");
                                    _itemcontent = getGiaTri(_itemcontent, "KHOI_LUONG", "");
                                    _itemcontent = getGiaTri(_itemcontent, "THANH_TIEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "GHI_CHU", "");
                                    _itemcontents = _itemcontents + _itemcontent;
                                }
                            }
                            setGiaTri(model, _ITEMS, _itemcontents, true);
                            setGiaTri(model, "TONG_SO_LUONG", tongkien.ToString());
                            setGiaTri(model, "TONG_KHOI_LUONG", tongkl.ToThapPhan());
                            setGiaTri(model, "TONG_TIEN_CUOC", tongcuoc.ToSoNguyen());

                            setGiaTri(model, "LIEN_NUM", 1);//tu dien vao so lien
                        }
                        break;
                    }
                
                case ENNhaXeCauHinh.VE_MAU_IN_PHOI:
                    {
                        var _historyxexuatben = _limousinebanveService.GetChuyenDiById(Id);
                        if (_historyxexuatben != null)
                        {

                            //lay thong ti khach di xe
                            var hanhkhachs = _limousinebanveService.GetDatVeByChuyenDi(Id);
                            string _itemcontents = "";
                            int STT = 1;                           

                            foreach (var hk in hanhkhachs)
                            {
                               string _itemcontent = model.GiaTriItem;
                               _itemcontent = getGiaTri(_itemcontent, "STT", STT.ToString());
                               string tenkh = string.IsNullOrEmpty(hk.TenKhachHangDiKem) ? hk.khachhang.Ten : hk.TenKhachHangDiKem;
                               _itemcontent = getGiaTri(_itemcontent, "HOVATEN", tenkh);                               
                               _itemcontent = getGiaTri(_itemcontent, "DIEN_THOAI", hk.khachhang.DienThoai);                                
                               _itemcontents = _itemcontents + _itemcontent;
                               STT++;
                            }
                            //neu chua 2 row thi phai them trong vao
                            if(STT<model.ItemPerPage)
                            {
                                for(int i=STT;i<=model.ItemPerPage;i++)
                                {
                                    string _itemcontent = model.GiaTriItem;
                                    _itemcontent = getGiaTri(_itemcontent, "STT", i.ToString());
                                    _itemcontent = getGiaTri(_itemcontent, "HOVATEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "DIEN_THOAI", "");
                                    _itemcontents = _itemcontents + _itemcontent;
                                }
                            }
                            setGiaTri(model, _ITEMS, _itemcontents, true);
                            setGiaTri(model, "TONG_SO_KHACH", hanhkhachs.Count.ToString());
                            setGiaTriNgayThang(model, _historyxexuatben.NgayDi);
                            if (_historyxexuatben.STTChuyen > 0)
                                setGiaTri(model, "SO_HD", _historyxexuatben.STTChuyen.ToString());
                            else
                                setGiaTri(model, "SO_HD", "______");

                        }
                        break;
                    }
                case ENNhaXeCauHinh.LENH_PHU_DON_KHACH:
                    {
                        var _historyxexuatben = _limousinebanveService.GetChuyenDiById(Id);
                        if (_historyxexuatben != null)
                        {

                            //lay thong ti khach di xe
                            var hanhkhachs = _limousinebanveService.GetDatVeByChuyenDi(Id).Select(c =>
                                {
                                    var _datve = new DatVe();
                                    _datve = c;
                                    var htdd = _hanhtrinhService.GetHTDDByHanhTrinhVaDiemDon(_datve.HanhTrinhId,_datve.DiemDonId.GetValueOrDefault(0));
                                    if(htdd!=null)
                                    {
                                        _datve.ThuTuDon = htdd.ThuTu;

                                    }
                                    return _datve;
                                }).OrderBy(c=>c.ThuTuDon);

                            string _itemcontents = "";
                            int STT = 1;
                            var GioXuatBen = _historyxexuatben.ChuyenDitoText(false);
                            foreach (var hk in hanhkhachs)
                            {
                                string _itemcontent = model.GiaTriItem;
                                _itemcontent = getGiaTri(_itemcontent, "STT", STT.ToString());
                              
                                _itemcontent = getGiaTri(_itemcontent, "DIACHIDON", hk.TenDiemDon);
                                _itemcontent = getGiaTri(_itemcontent, "DIEMDEN", hk.TenDiemTra);
                                
                                string tenkh = string.IsNullOrEmpty(hk.TenKhachHangDiKem) ? hk.khachhang.Ten : hk.TenKhachHangDiKem;
                                _itemcontent = getGiaTri(_itemcontent, "HOVATEN", tenkh);
                                string _dienthoai = hk.khachhang.DienThoai;
                                if (!string.IsNullOrEmpty(hk.GhiChu))
                                    _dienthoai = _dienthoai + "(" + hk.GhiChu + ")";
                                _itemcontent = getGiaTri(_itemcontent, "DIEN_THOAI", _dienthoai);
                                _itemcontents = _itemcontents + _itemcontent;
                                STT++;
                            }
                            //neu chua 2 row thi phai them trong vao
                            if (STT < model.ItemPerPage)
                            {
                                for (int i = STT; i <= model.ItemPerPage; i++)
                                {
                                    string _itemcontent = model.GiaTriItem;
                                    _itemcontent = getGiaTri(_itemcontent, "STT", i.ToString());
                                    _itemcontent = getGiaTri(_itemcontent, "DIACHIDON", "");
                                    _itemcontent = getGiaTri(_itemcontent, "DIEMDEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "HOVATEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "DIEN_THOAI", "");
                                    _itemcontents = _itemcontents + _itemcontent;
                                }
                            }
                            setGiaTri(model, _ITEMS, _itemcontents, true);
                            setGiaTri(model, "TONG_SO_KHACH", hanhkhachs.Count().ToString());
                            setGiaTriNgayThang(model, _historyxexuatben.NgayDi);
                            setGiaTri(model, "GIO_XUAT_BEN", GioXuatBen);
                            setGiaTri(model, "HANH_TRINH", _historyxexuatben.hanhtrinh.MoTa);
                            string bienso = "";
                            string laixe = "";
                            if (_historyxexuatben.xevanchuyen != null)
                            {
                               
                                bienso = _historyxexuatben.xevanchuyen.BienSo;
                            }
                            if (_historyxexuatben.laixe != null)
                                laixe = _historyxexuatben.laixe.HoVaTen;
                            setGiaTri(model, "BIEN_SO_XE", bienso);
                            setGiaTri(model, "TEN_LAI_XE", laixe);
                            if (_historyxexuatben.STTChuyen > 0)
                                setGiaTri(model, "SO_HD", _historyxexuatben.STTChuyen.ToString());
                            else
                                setGiaTri(model, "SO_HD", "______");

                        }
                        break;
                    }
                case ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI :
                    {
                        var _hopdongchuyendi = _hopdongchuyenService.GetChuyenDiHopDongById(Id);
                        var hanhkhachs = _hopdongchuyenService.GetAllKhachHangByHopDongId(Id);
                        string _itemcontents = "";
                        int STT = 1;
                        foreach(var hk in hanhkhachs)
                        {
                            string _itemcontent = model.GiaTriItem;
                            _itemcontent = getGiaTri(_itemcontent, "STT", STT.ToString());

                            _itemcontent = getGiaTri(_itemcontent, "HOVATEN", hk.TenKhachHang);
                            _itemcontent = getGiaTri(_itemcontent, "NAMSINH", hk.NamSinh.ToString());
                            _itemcontent = getGiaTri(_itemcontent, "SODIENTHOAI", hk.SoDienThoai);
                            _itemcontent = getGiaTri(_itemcontent, "DIACHIDON", _hopdongchuyendi.DiemDonKhach);
                            _itemcontent = getGiaTri(_itemcontent, "DIEMDEN", _hopdongchuyendi.DiemTraKhach);
                            _itemcontent = getGiaTri(_itemcontent, "GHICHU", hk.GhiChu);
                            _itemcontents = _itemcontents + _itemcontent;
                            STT++;
                        }
                        if (STT < model.ItemPerPage)
                        {
                            for (int i = STT; i <= model.ItemPerPage; i++)
                            {
                                string _itemcontent = model.GiaTriItem;
                                _itemcontent = getGiaTri(_itemcontent, "STT", i.ToString());
                                _itemcontent = getGiaTri(_itemcontent, "DIACHIDON", "");
                                _itemcontent = getGiaTri(_itemcontent, "DIEMDEN", "");
                                _itemcontent = getGiaTri(_itemcontent, "HOVATEN", "");
                                _itemcontent = getGiaTri(_itemcontent, "NAMSINH", "");
                                _itemcontent = getGiaTri(_itemcontent, "SODIENTHOAI", "");

                                _itemcontent = getGiaTri(_itemcontent, "GHICHU", "");
                                _itemcontents = _itemcontents + _itemcontent;
                            }
                        }
                        setGiaTri(model, _ITEMS, _itemcontents, true);
                        setGiaTri(model, "TONG_SO_KHACH", hanhkhachs.Count().ToString());
                        setGiaTriNgayThang(model, _hopdongchuyendi.NgayTao);
                        string GioXuatBen = _hopdongchuyendi.ThoiGianDonKhach.Value.ToString("HH:ss");
                        setGiaTri(model, "GIO_XUAT_BEN", GioXuatBen);
                        setGiaTri(model, "HANH_TRINH", _hopdongchuyendi.LoTrinh);
                        string bienso = "";
                        if (_hopdongchuyendi.XeInfo !=null)
                            bienso=_hopdongchuyendi.XeInfo.BienSo;
                        setGiaTri(model, "BIEN_SO_XE", bienso);
                        string laixe = "";
                        if (_hopdongchuyendi.laixe != null)
                            laixe = _hopdongchuyendi.laixe.HoVaTen;
                        setGiaTri(model, "TEN_LAI_XE", laixe);
                        setGiaTri(model, "SO_HD", _hopdongchuyendi.SoHopDong);
                        break;
                    }
                case ENNhaXeCauHinh.LENH_DIEU_DONG_XE:
                    {
                        var _historyxexuatben = _limousinebanveService.GetChuyenDiById(Id);
                        if (_historyxexuatben != null)
                        {
                            var GioXuatBen = _historyxexuatben.ChuyenDitoText(false);
                            setGiaTri(model, "THOI_GIAN", GioXuatBen);
                            setGiaTri(model, "TU_NGAY", _historyxexuatben.NgayDi.ToString("dd-MM-yyyy"));
                            setGiaTri(model, "DEN_NGAY", _historyxexuatben.NgayDi.ToString("dd-MM-yyyy"));
                            setGiaTri(model, "LOAI_XE", _historyxexuatben.lichtrinhloaixe.loaixe.TenLoaiXe);
                            setGiaTri(model, "TUYEN_DUONG", _historyxexuatben.lichtrinhloaixe.hanhtrinh.MoTa);
                            string laixe = "";
                            if (_historyxexuatben.laixe != null)
                                laixe = _historyxexuatben.laixe.HoVaTen;
                            setGiaTri(model, "LAI_XE", laixe);

                        }
                        break;
                    }
                case ENNhaXeCauHinh.MAU_DS_TO_TAXI:
                    {
                        var _historyxexuatben = _limousinebanveService.GetChuyenDiById(Id);
                        if (_historyxexuatben != null)
                        {
                           
                            //lay thong ti khach di xe
                            var hanhkhachs = _limousinebanveService.GetDatVeByChuyenDi(Id).Where(c=>c.isDonTaxi).GroupBy(c => new { c.KhachHangId })
                                .Select(g => new MauChuyenTaxi
                                {
                                    KhachHangId = g.Key.KhachHangId,
                                    SoLuongKhach = g.Count(),
                                    DiaChiNha = g.First().DiaChiNha,
                                    khachhang = g.First().khachhang,
                                    DiemTraTaxi = g.First().diemdon.TenDiemDon,
                                    GhiChu=g.First().GhiChu
                                   

                                }).ToList();

                            string _itemcontents = "";
                            int STT = 1;
                            var GioXuatBen = _historyxexuatben.ChuyenDitoText(false);
                            foreach (var hk in hanhkhachs)
                            {

                                string _itemcontent = model.GiaTriItem;
                                _itemcontent = getGiaTri(_itemcontent, "STT", STT.ToString());
                                _itemcontent = getGiaTri(_itemcontent, "HOVATEN", hk.khachhang.Ten);
                                string _dienthoai = hk.khachhang.DienThoai;
                                if (!string.IsNullOrEmpty(hk.GhiChu))
                                    _dienthoai = _dienthoai + "(" + hk.GhiChu + ")";
                                _itemcontent = getGiaTri(_itemcontent, "DIEN_THOAI", _dienthoai);
                                _itemcontent = getGiaTri(_itemcontent, "DIEMDON", hk.DiaChiNha);
                                _itemcontent = getGiaTri(_itemcontent, "DIEMTRA", hk.DiemTraTaxi);                                
                                _itemcontent = getGiaTri(_itemcontent, "SOLUONGKHACH",hk.SoLuongKhach.ToString());                               
                                _itemcontents = _itemcontents + _itemcontent;
                                STT++;
                            }
                            //neu chua 2 row thi phai them trong vao
                            if (STT < model.ItemPerPage)
                            {
                                for (int i = STT; i <= model.ItemPerPage; i++)
                                {
                                    string _itemcontent = model.GiaTriItem;
                                    _itemcontent = getGiaTri(_itemcontent, "STT", i.ToString());
                                    _itemcontent = getGiaTri(_itemcontent, "HOVATEN", "");
                                    _itemcontent = getGiaTri(_itemcontent, "DIEN_THOAI", "");
                                    _itemcontent = getGiaTri(_itemcontent, "DIEMDON", "");
                                    _itemcontent = getGiaTri(_itemcontent, "DIEMTRA", "");
                                    _itemcontent = getGiaTri(_itemcontent, "SOLUONGKHACH", "");                                   
                                    _itemcontents = _itemcontents + _itemcontent;
                                }
                            }
                            setGiaTri(model, _ITEMS, _itemcontents, true);
                            setGiaTri(model, "TONG_SO_KHACH", hanhkhachs.Sum(c=>c.SoLuongKhach).ToString());
                            setGiaTriNgayThang(model, _historyxexuatben.NgayDi);
                            setGiaTri(model, "GIO_XUAT_BEN", GioXuatBen);
                            if (_historyxexuatben.STTChuyen > 0)
                                setGiaTri(model, "SO_HD", _historyxexuatben.STTChuyen.ToString());
                            else
                                setGiaTri(model, "SO_HD", "______");

                        }
                        break;
                    }

            }
        }
        public ActionResult InPhieu(int MaID, int Id)
        {
            var model = new NhaXeCauHinhModel();
            //lay thong tin cau hinh truoc do
            ENNhaXeCauHinh cauhinh = (ENNhaXeCauHinh)MaID;
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, cauhinh);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model);
                setGiaTriModel(model, Id);
            }
            else
            {
                model.kieudulieu = getKieuDuLieu(cauhinh);
                model.MaCauHinh = cauhinh;
                model.GiaTri = "Bạn chưa tạo mẫu";
            }
            return View(model);
        }


        public ActionResult Index(string Code)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            //lay thong tin cau hinh truoc do
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, Code);
            if (item != null)
            {
                model = fromEntity(item);
            }
            else
            {
                model.kieudulieu = getKieuDuLieu(Code);
                model.MaCauHinh = (ENNhaXeCauHinh)Enum.Parse(typeof(ENNhaXeCauHinh), Code);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);
            }
            return View(model);
        }
        public ActionResult CuongVe()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.VE_MAU_IN_CUONG_VE);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model);
            }
            else
            {
                model.Ten = "Mẫu cuống vé";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.VE_MAU_IN_CUONG_VE;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CuongVe(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);

                var _solien = new NhaXeCauHinh();
                _solien.kieudulieu = ENKieuDuLieu.SO;
                _solien.MaCauHinh = ENNhaXeCauHinh.VE_MAU_IN_CUONG_VE_LIEN;
                _solien.GiaTri = model.SoLien.ToString();
                _solien.NhaXeId = _workContext.NhaXeId;
                _solien.Ten = "Mẫu cuống vé (số liên)";
                _nhaxeService.Insert(_solien);

                SuccessNotification("Cập nhật thành công!");
            }
            return View(model);
        }

        public ActionResult PhieuGuiHangHoa()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.KY_GUI_PHIEU_GUI_HANG);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model);
            }
            else
            {
                model.Ten = "Mẫu phiếu gửi hàng hóa";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.KY_GUI_PHIEU_GUI_HANG;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult PhieuGuiHangHoa(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);
                var _solien = new NhaXeCauHinh();
                _solien.kieudulieu = ENKieuDuLieu.SO;
                _solien.MaCauHinh = ENNhaXeCauHinh.KY_GUI_PHIEU_GUI_HANG_LIEN;
                _solien.GiaTri = model.SoLien.ToString();
                _solien.NhaXeId = _workContext.NhaXeId;
                _solien.Ten = "Mẫu phiếu gửi hàng hóa (số liên)";
                _nhaxeService.Insert(_solien);

                SuccessNotification("Cập nhật thành công!");
            }
            return View(model);
        }
        public ActionResult LenhChuyenHangHoa()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model, false);

            }
            else
            {
                model.Ten = "Mẫu lệnh chuyển hàng hóa";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LenhChuyenHangHoa(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);
                //cap nhat subitem
                var _pagesize = new NhaXeCauHinh();
                _pagesize.kieudulieu = ENKieuDuLieu.SO;
                _pagesize.MaCauHinh = ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN_PAGES;
                _pagesize.GiaTri = model.ItemPerPage.ToString();
                _pagesize.NhaXeId = _workContext.NhaXeId;
                _pagesize.Ten = "Mẫu lệnh chuyển hàng hóa (pagesize)";
                _nhaxeService.Insert(_pagesize);
                var _startend = new NhaXeCauHinh();
                _startend.kieudulieu = ENKieuDuLieu.KY_TU;
                _startend.MaCauHinh = ENNhaXeCauHinh.KY_GUI_MAU_HANG_HOA_XUAT_BEN_REPEATSTARTEND;
                _startend.GiaTri = string.Format("{0}|{1}", model.KyTuRepeatStart, model.KyTuRepeatEnd);
                _startend.NhaXeId = _workContext.NhaXeId;
                _startend.Ten = "Mẫu lệnh chuyển hàng hóa (startend)";
                _nhaxeService.Insert(_startend);
                SuccessNotification("Cập nhật thành công!");
            }
            return View(model);
        }
        /// <summary>
        /// danh sach hanh khach (phoi di duong)
        /// </summary>
        /// <returns></returns>
        public ActionResult LenhChuyenKhach()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.VE_MAU_IN_PHOI);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model, false);
            }
            else
            {
                model.Ten = "Mẫu lệnh chuyển hành khách";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.VE_MAU_IN_PHOI;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LenhChuyenKhach(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);

                //cap nhat subitem
                var _pagesize = new NhaXeCauHinh();
                _pagesize.kieudulieu = ENKieuDuLieu.SO;
                _pagesize.MaCauHinh = ENNhaXeCauHinh.VE_MAU_IN_PHOI_PAGES;
                _pagesize.GiaTri = model.ItemPerPage.ToString();
                _pagesize.NhaXeId = _workContext.NhaXeId;
                _pagesize.Ten = "Mẫu lệnh chuyển hành khách (pagesize)";
                _nhaxeService.Insert(_pagesize);
                var _startend = new NhaXeCauHinh();
                _startend.kieudulieu = ENKieuDuLieu.KY_TU;
                _startend.MaCauHinh = ENNhaXeCauHinh.VE_MAU_IN_PHOI_REPEATSTARTEND;
                _startend.GiaTri = string.Format("{0}|{1}", model.KyTuRepeatStart, model.KyTuRepeatEnd);
                _startend.NhaXeId = _workContext.NhaXeId;
                _startend.Ten = "Mẫu lệnh chuyển hành khách (startend)";
                _nhaxeService.Insert(_startend);
                SuccessNotification("Cập nhật thành công!");

            }
            return View(model);
        }
        /// <summary>
        /// Danh sach hanh khach(lenh phu)
        /// </summary>
        /// <returns></returns>
        public ActionResult LenhPhuDiDuong()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_PHU_DON_KHACH);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model, false);
              
            }
            else
            {
                model.Ten = "Mẫu lệnh phụ đi đường";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.LENH_PHU_DON_KHACH;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LenhPhuDiDuong(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);

                //cap nhat subitem
                var _pagesize = new NhaXeCauHinh();
                _pagesize.kieudulieu = ENKieuDuLieu.SO;
                _pagesize.MaCauHinh = ENNhaXeCauHinh.LENH_PHU_DON_KHACH_PAGES;
                _pagesize.GiaTri = model.ItemPerPage.ToString();
                _pagesize.NhaXeId = _workContext.NhaXeId;
                _pagesize.Ten = "Mẫu lệnh phụ chuyển hành khách (pagesize)";
                _nhaxeService.Insert(_pagesize);
                var _startend = new NhaXeCauHinh();
                _startend.kieudulieu = ENKieuDuLieu.KY_TU;
                _startend.MaCauHinh = ENNhaXeCauHinh.LENH_PHU_DON_KHACH_REPEATSTARTEND;
                _startend.GiaTri = string.Format("{0}|{1}", model.KyTuRepeatStart, model.KyTuRepeatEnd);
                _startend.NhaXeId = _workContext.NhaXeId;
                _startend.Ten = "Mẫu lệnh phụ chuyển hành khách (startend)";
                _nhaxeService.Insert(_startend);
                SuccessNotification("Cập nhật thành công!");

            }
            return View(model);
        }
        /// <summary>
        /// phieu don khach chuyen cho ben taxi
        /// </summary>
        /// <returns></returns>
        public ActionResult KhachHangToTaxi()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.MAU_DS_TO_TAXI);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model, false);
            }
            else
            {
                model.Ten = "Mẫu lệnh chuyển hành khách";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.MAU_DS_TO_TAXI;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult KhachHangToTaxi(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);

                //cap nhat subitem
                var _pagesize = new NhaXeCauHinh();
                _pagesize.kieudulieu = ENKieuDuLieu.SO;
                _pagesize.MaCauHinh = ENNhaXeCauHinh.MAU_DS_TO_TAXI_PAGES;
                _pagesize.GiaTri = model.ItemPerPage.ToString();
                _pagesize.NhaXeId = _workContext.NhaXeId;
                _pagesize.Ten = "Mẫu Gửi danh sách KH cho taxi (pagesize)";
                _nhaxeService.Insert(_pagesize);
                var _startend = new NhaXeCauHinh();
                _startend.kieudulieu = ENKieuDuLieu.KY_TU;
                _startend.MaCauHinh = ENNhaXeCauHinh.MAU_DS_TO_TAXI_REPEATSTARTEND;
                _startend.GiaTri = string.Format("{0}|{1}", model.KyTuRepeatStart, model.KyTuRepeatEnd);
                _startend.NhaXeId = _workContext.NhaXeId;
                _startend.Ten = "Mẫu Gửi danh sách KH cho taxi  (startend)";
                _nhaxeService.Insert(_startend);
                SuccessNotification("Cập nhật thành công!");

            }
            return View(model);
        }
        
        public ActionResult LenhDonKhachTaxi()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_DON_TAXI);
            if (item != null)
            {
                model = fromEntity(item);
            }
            else
            {
                model.Ten = "Mẫu lệnh chuyển hành khách bằng taxi";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.LENH_DON_TAXI;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LenhDonKhachTaxi(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);
                SuccessNotification("Cập nhật thành công!");

            }
            return View(model);
        }
        /// <summary>
        /// mau giay hop dong tron goi
        /// </summary>
        /// <returns></returns>
        public ActionResult LenhHopDongTronGoi()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model, false);
            }
            else
            {
                model.Ten = "Mẫu lệnh hợp đồng trọn gói";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LenhHopDongTronGoi(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);

                //cap nhat subitem
                var _pagesize = new NhaXeCauHinh();
                _pagesize.kieudulieu = ENKieuDuLieu.SO;
                _pagesize.MaCauHinh = ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI_PAGES;
                _pagesize.GiaTri = model.ItemPerPage.ToString();
                _pagesize.NhaXeId = _workContext.NhaXeId;
                _pagesize.Ten = "Mẫu lệnh hợp đồng trọn gói (pagesize)";
                _nhaxeService.Insert(_pagesize);
                var _startend = new NhaXeCauHinh();
                _startend.kieudulieu = ENKieuDuLieu.KY_TU;
                _startend.MaCauHinh = ENNhaXeCauHinh.LENH_HOP_DONG_TRON_GOI_REPEATSTARTEND;
                _startend.GiaTri = string.Format("{0}|{1}", model.KyTuRepeatStart, model.KyTuRepeatEnd);
                _startend.NhaXeId = _workContext.NhaXeId;
                _startend.Ten = "Mẫu lệnh hợp đồng trọn gói (startend)";
                _nhaxeService.Insert(_startend);
                SuccessNotification("Cập nhật thành công!");

            }
            return View(model);
        }
        public ActionResult LenhDieuDongXe()
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            var model = new NhaXeCauHinhModel();
            var item = _nhaxeService.GetNhaXeCauHinhByCode(_workContext.NhaXeId, ENNhaXeCauHinh.LENH_DIEU_DONG_XE);
            if (item != null)
            {
                model = fromEntity(item);
                setGiaTriSubItem(model, false);
            }
            else
            {
                model.Ten = "Mẫu lệnh điều động xe";
                model.kieudulieu = ENKieuDuLieu.KY_TU;
                model.MaCauHinh = ENNhaXeCauHinh.LENH_DIEU_DONG_XE;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LenhDieuDongXe(NhaXeCauHinhModel model)
        {
            if (this.CheckNoAccessIntoNhaXe(_workContext, _permissionService, StandardPermissionProvider.CVThongTinNhaXe))
                return this.AccessDeniedView();
            if (ModelState.IsValid)
            {
                var item = fromModel(model);
                _nhaxeService.Insert(item);

                //cap nhat subitem
                var _pagesize = new NhaXeCauHinh();
                _pagesize.kieudulieu = ENKieuDuLieu.SO;
                _pagesize.MaCauHinh = ENNhaXeCauHinh.LENH_DIEU_DONG_XE_PAGES;
                _pagesize.GiaTri = model.ItemPerPage.ToString();
                _pagesize.NhaXeId = _workContext.NhaXeId;
                _pagesize.Ten = "Mẫu lệnh điều động xe (pagesize)";
                _nhaxeService.Insert(_pagesize);
                var _startend = new NhaXeCauHinh();
                _startend.kieudulieu = ENKieuDuLieu.KY_TU;
                _startend.MaCauHinh = ENNhaXeCauHinh.LENH_DIEU_DONG_XE_REPEATSTARTEND;
                _startend.GiaTri = string.Format("{0}|{1}", model.KyTuRepeatStart, model.KyTuRepeatEnd);
                _startend.NhaXeId = _workContext.NhaXeId;
                _startend.Ten = "Mẫu lệnh điều động xe (startend)";
                _nhaxeService.Insert(_startend);
                SuccessNotification("Cập nhật thành công!");

            }
            return View(model);
        }
    }


}