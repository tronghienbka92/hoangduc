
namespace Nop.Core.Domain.NhaXes
{
    public enum ENKieuVanPhong
    {
        /// <summary>
        /// Simple product
        /// </summary>
        TruSo = 1,
        /// <summary>
        /// Grouped product
        /// </summary>
        VanPhong = 2,
    }
    public enum ENTrangThaiXe
    {
        HoatDong = 1,
        BaoDuong = 2,
        DungHoatDong = 3,
        Huy = 4
    }
    public enum ENKieuXe
    {
        All = 0,
        GheNgoi = 1,
        GiuongNam = 2

    }
    public enum ENTrangThaiHopDongChuyen
    {

        MOI_DAT = 1,
        HUY = 2

    }
    public enum ENHinhThucThanhToan
    {
        CHUYEN_KHOAN = 1,
        TIEN_MAT = 2,
        CHUA_THANH_TOAN = 3
    }
    public enum ENLoaiDiemDon
    {
        BenXuatPhatKetThuc = 1,
        GiuaHanhTrinh = 2,
        DiemChot = 3
    }

    public enum ENKieuNhanVien
    {
        QuanLy = 1,
        LeTan = 2,
        KeToan = 3,
        LaiXe = 4,
        PhuXe = 5,
        Khac = 6,
        HanhChinh = 7
    }

    public enum ENGioiTinh
    {
        Nam = 1,
        Nu = 2
    }
    public enum ENTrangThaiNhanVien
    {
        DangLamViec = 1,
        Nghi = 2
    }

    public enum ENBaoCaoQuy
    {
        Quy1 = 1,
        Quy2 = 2,
        Quy3 = 3,
        Quy4 = 4

    }
    public enum ENLoaiHangHoa
    {
        /// <summary>
        /// hàng xốp, đồ ăn, hàng dễ vỡ
        /// </summary>
        XopDeVo = 1,
        /// <summary>
        /// hàng nặng: xe máy, xe đạp
        /// </summary>
        KimLoaiNang = 2,
        /// <summary>
        /// Linh kiện, thiet bị điện tử: điện thoại, laptop
        /// </summary>
        ThietBiDienTu = 3,
        Tien = 4,
        /// <summary>
        /// giấy tờ phong bì, tài liệu
        /// </summary>
        GiayTo = 5,
        LoaiKhac = 6

    }
    public enum ENTinhTrangVanChuyen
    {
        All = 0,
        ChuaVanChuyen = 1,
        DangVanChuyen = 2,
        NhanHang = 3,
        Huy = 4,
        KetThuc = 5


    }
    public enum ENBaoCaoLoaiThoiGian
    {
        TheoNgay = 1,
        TheoThang = 2,
        TheoQuy = 3,
        TheoNam = 4

    }
    public enum ENBaoCaoChuKyThoiGian
    {
        HangNgay = 1,
        HangThang = 2,
        HangNam = 3
    }

    /// <summary>
    /// 0: phoi ve, 1:dung cho chuyen ve, 2: in phoive
    /// </summary>
    public enum ENPhanLoaiPhoiVe
    {
        PHOI_VE = 0,
        CHUYEN_VE = 1,
        IN_PHOI_VE = 2
    }
    public enum ENKieuDuLieu
    {
        SO = 1,
        KY_TU = 2,
        NGAY_THANG = 3,
        PHAN_TRAM = 4,
    }
    public enum ENTrangThaiXeXuatBen
    {
        ALL = 0,
        CHO_XUAT_BEN = 1,
        DANG_DI = 2,
        KET_THUC = 3,
        HUY = 4,
        DU_KIEN = 6
    }
    public enum ENNhaXeCauHinh
    {
        //cau hinh chung

        //ve, phoi ve,
        VE_MAU_IN_PHOI = 200,
        VE_MAU_IN_PHOI_PAGES = 2001,
        VE_MAU_IN_PHOI_REPEATSTARTEND = 2002,

        VE_MAU_IN_CUONG_VE = 201,
        VE_MAU_IN_CUONG_VE_LIEN = 2010,

        //ky gui
        //DICH VU GIA TRI GIA TANG
        KY_GUI_DVGT_GIA_TRI = 300,
        KY_GUI_DVGT_CONG_KENH = 301,
        KY_GUI_DVGT_DIEN_TU_DE_VO = 302,
        KY_GUI_DVGT_NHE_CONG_KENH = 303,
        //xe xuat ben
        KY_GUI_MAU_HANG_HOA_XUAT_BEN = 310,
        KY_GUI_MAU_HANG_HOA_XUAT_BEN_PAGES = 3101,
        KY_GUI_MAU_HANG_HOA_XUAT_BEN_REPEATSTARTEND = 3102,

        //mau phieu gui hang hoa
        KY_GUI_PHIEU_GUI_HANG = 311,
        KY_GUI_PHIEU_GUI_HANG_LIEN = 3110,

        //in ke ve
        IN_KE_VE = 321,

        //in lenh don taxi
        LENH_DON_TAXI = 400,
        // lenh phu don khach
        LENH_PHU_DON_KHACH = 500,
        LENH_PHU_DON_KHACH_PAGES = 5001,
        LENH_PHU_DON_KHACH_REPEATSTARTEND = 5002,
        

        // lenh phu don khach
        MAU_DS_TO_TAXI = 600,
        MAU_DS_TO_TAXI_PAGES = 6001,
        MAU_DS_TO_TAXI_REPEATSTARTEND = 6002,

        //lenh hop dong tron goi
        LENH_HOP_DONG_TRON_GOI = 700,
        LENH_HOP_DONG_TRON_GOI_PAGES = 7001,
        LENH_HOP_DONG_TRON_GOI_REPEATSTARTEND = 7002,

        //lenh dieu dong xe
        LENH_DIEU_DONG_XE = 800,
        LENH_DIEU_DONG_XE_PAGES = 8001,
        LENH_DIEU_DONG_XE_REPEATSTARTEND = 8002,
    }
    public enum ENGiaoDichKeVeTrangThai
    {
        ALL = 0,
        MOI_TAO = 1,
        DANG_CHINH_SUA = 2,
        HOAN_THANH = 3,
        HUY = 4
    }
    public enum ENVeXeItemTrangThai
    {
        ALL = 0,
        MOI_TAO = 1,
        DA_GIAO_HANG = 2,
        DA_BAN = 3,
        DA_SU_DUNG = 4,
        HUY = 9
    }
    public enum ENGiaoDichKeVeMenhGiaAction
    {
        MOI = 1,
        SUA = 2,
        SUA_VA_XOA = 3
    }

    public enum ENGiaoDichKeVePhanLoai
    {
        KE_VE = 0,
        TRA_VE = 1
    }
    public enum ENLoaiVeXeItem
    {
        ALL = 0,
        VeQuay = 1,
        VeDat = 2,
        VeOnline = 3,
        VeVip = 4,
        VeThuong = 5

    }
    public enum ENTrangThaiDatVe
    {
        ALL = 0,
        MOI = 1,
        DA_XEP_CHO = 2,
        DA_DI = 3,
        HUY = 4,
        CON_TRONG = 5


    }

}
