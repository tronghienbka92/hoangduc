using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System;
using Nop.Core.Domain.Chonves;
using Nop.Core.Domain.NhaXes;

namespace Nop.Web.Models.NhaXeBanVe
{
    //[Validator(typeof(LoginValidator))]
    public partial class BangGiaVeModel : BaseNopModel
    {
        public BangGiaVeModel()
        {
            ListHanhTrinh = new List<SelectListItem>();
           
        }
        [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.HanhTrinhId")]
        public int HanhTrinhId { get; set; }
        public IList<SelectListItem> ListHanhTrinh { get; set; }
        [NopResourceDisplayName("ChonVe.NhaXe.QuanLiPhoiVe.NgayDi")]
        [UIHint("DatePhoiVe")]
        public DateTime NgayDi { get; set; }
    }
    public partial class BangVeInfoModel : BaseNopModel
    {
        public BangVeInfoModel()
        {
            HanhTrinhGiaVes = new List<HanhTrinhGiaVeModel>();
           
        }
          public List<HanhTrinhGiaVeModel> HanhTrinhGiaVes { get; set; }
          public int SoDiemDon { get; set; }
          public string TenDauTuyen { get; set; }
          public int TongVeDaBan { get; set; }
          public string TongTien { get; set; }
          public DateTime NgayDi { get; set; }
    }

    public partial class HanhTrinhGiaVeModel : BaseNopEntityModel
    {        
        public int HanhTrinhId { get; set; }
        public int NhaXeId { get; set; }
        public int DiemDonId { get; set; }
        public string TenDiemDon { get; set; }
        public int DiemDenId { get; set; }
        public string TenDiemDen { get; set; }        
        public Decimal GiaVe { get; set; }
        public bool IsHetVe { get; set; }
        public string GiaVeText { get; set; }
        public string SoSeriHienTai { get; set; }
        public int SoVeConLai { get; set; }      
        public int SoVeDaBan { get; set; }     
        public List<VeXeItem> VeXes { get; set; }
        public string QuyenDangBan { get; set; }        
    }
}