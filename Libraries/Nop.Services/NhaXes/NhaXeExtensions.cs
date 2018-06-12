using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.NhaXes
{
    public static class NhaXeExtensions
    {
        #region Cac ham dung chung
        //Định nghĩa dãy phát âm các ký số
        static readonly string[] chuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        //Định nghĩa dãy phát âm các đơn vị tỷ, ngàn, triệu
        static readonly string[] chuCach = { "", "tỷ", "ngàn", "triệu" };

        private static string SoTram2Chu(uint soCanDoi, bool xet0tram)
        {
            uint soHangChuc, soHangTram, soHangDonVi;
            string str = "";
            //Xác định 3 ký số miêu tả hàng trăm, hàng chục và đơn vị
            soHangTram = soCanDoi / 100;
            soCanDoi = soCanDoi % 100;
            soHangChuc = soCanDoi / 10;
            soHangDonVi = soCanDoi % 10;
            if ((xet0tram || (soHangTram != 0)) && ((soHangTram > 0) || (soHangChuc + soHangDonVi > 0)))
                str = chuSo[soHangTram] + " trăm";
            if (soHangChuc >= 2)
                str = str + " " + chuSo[soHangChuc] + " mươi";
            if (soHangChuc == 1)
                str = str + " mười";
            if (soHangDonVi == 0) return str;
            if (soHangChuc == 0)
            {
                if ((soHangTram != 0) || (str != null))
                    str = str + " lẻ " + chuSo[soHangDonVi];
                else str = str + chuSo[soHangDonVi];
                return str;
            }
            if (soHangChuc == 1)
            {
                if (soHangDonVi != 5)
                    str = str + " " + chuSo[soHangDonVi];
                else
                    str = str + " lăm";
                return str;
            }
            if (soHangDonVi == 1)
                str = str + " mốt";
            else if (soHangDonVi == 5) str = str + " lăm";
            else str = str + " " + chuSo[soHangDonVi];
            return str;

        }
        static string ChuHoaDauTien(string input)
        {
            string temp = input.Substring(0, 1);
            return temp.ToUpper() + input.Remove(0, 1);
        }
        /// <summary>
        /// Chuyen so sang chu
        /// </summary>
        /// <param name="soCanDoi"></param>
        /// <returns></returns>
        public static string ToTienBangChu(this Decimal SoTien)
        {
            int idx = 0;//Vị trí dấu chấm phân cách từng 3 ký số
            long baKySo = 0;
            string str = null;
            string tram = null;
            string tuNganCach = null;
            long soCanDoi = Convert.ToInt64(SoTien);
            if (soCanDoi == 0)
            {
                str = "không";
            }
            else
            {
                while (soCanDoi != 0)
                {
                    baKySo = soCanDoi % 1000;
                    soCanDoi = soCanDoi / 1000;
                    if (soCanDoi != 0)
                        tram = SoTram2Chu(Convert.ToUInt32(baKySo), true);
                    else tram = SoTram2Chu(Convert.ToUInt32(baKySo), false);
                    if (idx == 0)
                    {//vị trí đơn vị
                        if (soCanDoi == 0)
                            str = tram;
                        else str = " " + tram;
                    }
                    else if (tram.Length != 0)
                    {//vị trí ngàn, triệu, tỷ
                        tuNganCach = chuCach[(idx % 3) + 1];
                        str = tram + " " + tuNganCach + " " + str;
                    }
                    else if ((idx % 3) == 0)
                        str = " tỷ" + str;
                    idx = idx + 1;
                }
            }
            //upper ky tu dau tien
            str = str.Trim();
            if (str.Substring(0, 2) == "lẻ")
            {
                str = str.Substring(3).Trim();
            }
            str = ChuHoaDauTien(str.Trim());
            return str;
        }

        static readonly string[] VietNamChar = 
    { 
        "aAeEoOuUiIdDyY", 
        "áàạảãâấầậẩẫăắằặẳẵ", 
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", 
        "éèẹẻẽêếềệểễ", 
        "ÉÈẸẺẼÊẾỀỆỂỄ", 
        "óòọỏõôốồộổỗơớờợởỡ", 
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", 
        "úùụủũưứừựửữ", 
        "ÚÙỤỦŨƯỨỪỰỬỮ", 
        "íìịỉĩ", 
        "ÍÌỊỈĨ", 
        "đ", 
        "Đ", 
        "ýỳỵỷỹ", 
        "ÝỲỴỶỸ" 
    };
        public static string LoaiBoDauTiengViet(this string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }
        #endregion
        public static string ToTien(this Decimal val, IPriceFormatter priceFormatter)
        {
            return priceFormatter.FormatPrice(val, true, false);
        }
        public static string ToThapPhan(this Decimal val)
        {
            return val.ToString("###,###,##0.#0");
        }
        public static string ToSoNguyen(this Object val)
        {
            return Convert.ToDecimal(val).ToString("###,###,##0");
        }
        public static string ToNgayThang(this DateTime dt)
        {
            return string.Format("Ngày {0} tháng {1} năm {2}", dt.ToString("dd"), dt.ToString("MM"), dt.ToString("yyyy"));
        }
        public static string ToText(this DiaChi item)
        {
            if (item == null)
                return "";
            string _diachi = item.DiaChi1;
            if (!string.IsNullOrEmpty(item.DiaChi2))
                _diachi = _diachi + " " + item.DiaChi2;
            if (item.Quanhuyen != null)
                return string.Format("{0} - {1} - {2}", _diachi, item.Quanhuyen.Ten, item.Province.Name);
            if (item.Province != null)
                return string.Format("{0} - {1}", _diachi, item.Province.Name);
            return _diachi;
        }
        public static string GetDiemDon(this NguonVeXe item)
        {
            return "";

        }
        public static string GetHanhTrinh(this NguonVeXe item)
        {
            if (item == null)
                return "";
            return string.Format("{0} - {1}", item.TenDiemDon, item.TenDiemDen);
        }
        public static string GetDiemDen(this NguonVeXe item)
        {
            return "";
        }
        public static string ThongTin(this NhanVien item)
        {
            if (item == null)
                return "";
            return string.Format("{0} ({1})", item.HoVaTen, item.DienThoai);
        }
        /// <summary>
        /// Format theo dinh dang yyyy-MM-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string toStringDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// Format theo dinh dang yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string toStringDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToMoTa(this NguonVeXe item)
        {
            if (item == null)
                return "NULL";
            return string.Format("{0} - {1} ({2} đ)", item.TenDiemDon, item.TenDiemDen, item.GiaVeHienTai.ToSoNguyen());
        }

        public static string ThongTinHanHoa(this PhieuGuiHang item)
        {
            string strthongtin = "";
            foreach (var hh in item.HangHoas)
            {
                if (string.IsNullOrEmpty(strthongtin))
                    strthongtin = hh.TenHangHoa;
                else
                    strthongtin = strthongtin + " - " + hh.TenHangHoa;

            }
            return strthongtin;
        }
        public static string ThongTin(this NhanVien item, bool isCMTorDienThoai = true)
        {
            if (item == null)
                return "";
            if (isCMTorDienThoai)
                return string.Format("{0} ({1})", item.HoVaTen, item.CMT_Id);
            return string.Format("{0} ({1})", item.HoVaTen, item.DienThoai);

        }
        public static string ThongTinLaiPhuXes(this HistoryXeXuatBen item, string ngancach = "; ")
        {
            if (item == null)
                return "";
            var ret = "";
            foreach (var nv in item.LaiPhuXes)
            {
                if (string.IsNullOrEmpty(ret))
                    ret = nv.nhanvien.ThongTin(false);
                else
                    ret = ret + ngancach + nv.nhanvien.ThongTin(false);
            }
            return ret;
        }
        public static string ThongTinLaiPhuXe(this HistoryXeXuatBen item, int So = 0)
        {
            if (item == null)
                return "";
            if (item.LaiPhuXes.Count > So)
                return item.LaiPhuXes.ElementAt(So).nhanvien.ThongTin(false);
            return "";
        }
        public static string ToText(this HistoryXeXuatBen item)
        {
            if (item == null)
                return "";

            return string.Format("{0}:Hành trình: {1} - {2}, Thời gian đi: {3}", item.Id, item.NguonVeInfo.TenDiemDon, item.NguonVeInfo.TenDiemDen, item.NgayDi.ToString("yyyy-MM-dd HH:mm"));

        }
        public static string toText(this KhachHang item)
        {
            if (item == null)
                return "";
            return string.Format("{0}({1})", item.Ten, item.DienThoai);
        }
        public static string toText(this LichTrinh item, bool withMa = true)
        {
            if (item == null)
                return "";
            if (withMa)
                return string.Format("{0} - {1} : {2}", item.ThoiGianDi.ToString("HH:mm"), item.ThoiGianDi.AddMinutes(item.KhungThoiGian).ToString("HH:mm"), item.MaLichTrinh);

            return string.Format("{0} - {1}", item.ThoiGianDi.ToString("HH:mm"), item.ThoiGianDi.AddMinutes(item.KhungThoiGian).ToString("HH:mm"));
        }
        public static string toText(this HanhTrinh item)
        {
            if (item == null)
                return "";
           
            return string.Format("{0} : {1}", item.MaHanhTrinh, item.MoTa);
        }
        public static string toText(this ChuyenDi item)
        {
            if (item == null)
                return "";
            if (item.xevanchuyen != null)
                return string.Format("{0}({1})", item.Ma, item.xevanchuyen.BienSo);
            return item.Ma;
        }
        public static string ChuyenDitoText(this ChuyenDi item,bool IsMa)
        {
            if (item == null)
                return "";
            if (IsMa)
                return string.Format("{0}({1})", item.Ma, item.NgayDiThuc.ToString("HH:mm"));
            else
                return item.NgayDiThuc.ToString("HH:mm");
           
        }
    }

}
