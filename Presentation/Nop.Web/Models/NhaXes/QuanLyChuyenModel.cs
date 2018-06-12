using FluentValidation.Attributes;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Models.NhaXes
{
    public class QuanLyChuyenModel
    {
        public int NhanVienHientTaiId { get; set; }
        [UIHint("Date")]
        public DateTime NgayDi { get; set; }
        public int HanhTrinhId { get; set; }
        public List<SelectListItem> HanhTrinhs { get; set; }
        public int LichTrinhId { get; set; }
        public List<LichTrinh> LichTrinhs { get; set; }
        /// <summary>
        /// dung de tinh toan viec focus lich trinh hien tai tren trang
        /// </summary>
        public int LichTrinhStepId { get; set; }
        public int KhungGioId { get; set; }
        public ENKhungGio khunggio
        {
            get
            {
                return (ENKhungGio)KhungGioId;
            }
        }
        public IList<SelectListItem> khunggios { get; set; }
        public int KhuVucId { get; set; }
        public ENKhuVuc KhuVuc
        {
            get
            {
                return (ENKhuVuc)KhuVucId;
            }
        }
        public IList<SelectListItem> KhuVucs { get; set; }
        public int ChuyenDiId { get; set; }
        public ChuyenDiModel chuyendihientai { get; set; }
        public List<ChuyenDiModel> chuyendis { get; set; }
        public SoDoGheXeModel sodoghe { get; set; }

        /// <summary>
        /// Su dung cho viec chuyen ve
        /// </summary>
        public int ChuyenDiIdChuyenVe { get; set; }
        public List<SelectListItem> ChuyenDiChuyenVes { get; set; }
        public int DatVeIdChuyenVe { get; set; }
        public bool isCheckChuyenVe { get; set; }
        public string ThongTinKhachHang { get; set; }
        public List<DatVeCopyModel> arrDatVeCopy { get; set; }
        public bool isTaoChuyen { get; set; }
        public bool isQuanTri { get; set; }
        public class SoDoGheXeModel : BaseNopEntityModel
        {
            public ENPhanLoaiPhoiVe PhanLoai { get; set; }
            public string TenSoDo { get; set; }
            public string UrlImage { get; set; }
            public int SoLuongGhe { get; set; }
            public int KieuXeId { get; set; }
            public int SoCot { get; set; }
            public int SoHang { get; set; }
            /// <summary>
            /// Thong tin vi tri tren ma tran so do ghe co gia tri la 0, 1
            /// </summary>
            public int[,] MaTran { get; set; }
            //so tang 
            /// <summary>
            /// Thong tin ma tran phoi ve tang 1
            /// </summary>
            public DatVeModel[,] DatVes { get; set; }
            
        }

    }
    public class PhanVungModel
    {
        public int VungId { get; set; }
        public IList<SelectListItem> Vungs { get; set; }
       
    }
}