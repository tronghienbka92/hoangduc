using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Models.NhaXes;

namespace Nop.Web.Models.NhaXeBaoCao
{
    //[Validator(typeof(LoginValidator))]
    public  class DoanhThuNhanVienTheoXeModel : BaseNopModel
    {
        public DoanhThuNhanVienTheoXeModel()
        {
            
        }      
       
        public string BienSoXe { get; set; }
        public string TenLaiXe { get; set; }
        public int TongLuot { get; set; }
        public int SoKhach { get; set; }
        public decimal ThanhTien { get; set; }
        public string TienFomat { get; set; }
       
      
    }

    public  class ListDoanhthuNhanVienXeModel : BaseNopModel
    {
        public ListDoanhthuNhanVienXeModel()
        {
            doanhthus = new List<DoanhThuNhanVienTheoXe>();
        }

        [UIHint("DateNullable")]
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.TuNgay")]
        public DateTime TuNgay { get; set; }
        [UIHint("DateNullable")]
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.DenNgay")]
        public DateTime DenNgay { get; set; }
         public string SearchName { get; set; }
         public List<DoanhThuNhanVienTheoXe> doanhthus { get; set; }


    }
    public class TKLuotKhachHangModel : BaseNopModel
    {
        public TKLuotKhachHangModel()
        {
            doanhthus = new List<DatVe>();
        }

         [UIHint("DateNullable")]
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.TuNgay")]
        public DateTime TuNgay { get; set; }
         [UIHint("DateNullable")]
        [NopResourceDisplayName("ChonVe.NhaXe.BaoCaoNhaXe.DenNgay")]
        public DateTime DenNgay { get; set; }
        public int KhachHangId { get; set; }
        public string DienThoai { get; set; }
        public List<DatVe> doanhthus { get; set; }
        public List<HopDongChuyenModel> hopdongchuyens { get; set; }
        public List<ListDoanhThuTongHop> DoanhThuTongHops { get; set; }
        public int TongSL { get; set; }
        public decimal TongTien { get; set; }

    }
    
    public class ListDoanhThuChiTietModel : BaseNopModel
    {
        public ListDoanhThuChiTietModel()
        {
            ListXe = new List<XeDiTrongNgay>();
        }

       
        [UIHint("DateNullable")]
        [NopResourceDisplayName("Ngày đi")]
        public DateTime NgayDi { get; set; }
        [UIHint("DateNullable")]
        [NopResourceDisplayName("Từ ngày")]
        public DateTime TuNgay { get; set; }
        [UIHint("DateNullable")]
        [NopResourceDisplayName("Đến ngày")]
        public DateTime DenNgay { get; set; }
        public string SearchName { get; set; }
        public List<XeDiTrongNgay> ListXe { get; set; }
       


    }
   
  
}