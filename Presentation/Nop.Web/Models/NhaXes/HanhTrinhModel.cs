using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Models.NhaXes
{
    [Validator(typeof(HanhTrinhValidator))]
    public class HanhTrinhModel : BaseNopEntityModel
    {
        
        public HanhTrinhModel()
        {
            DiemDons = new List<HanhTrinhDiemDonModel>();
            AddHanhTrinhDiemDonModel = new HanhTrinhDiemDonModel();
            AvailableDiemDons = new List<SelectListItem>();
            AllVanPhongs = new List<VanPhongModel>();
        }

        [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.MaHanhTrinh")]
        public string MaHanhTrinh { get; set; }
        [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.MoTa")]
        public string MoTa { get; set; }
        [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.TongKhoangCach")]
        public int TongKhoangCach { get; set; }
        public string GiaVeResult { get; set; }
        public int SoDiemDon { get; set; }
         [NopResourceDisplayName("Tỉnh thành")]
        public int VungId { get; set; }
        public IList<SelectListItem> Vungs { get; set; }
        public HanhTrinhDiemDonModel AddHanhTrinhDiemDonModel { get; set; }
        public List<DiemDonModel> AllDiemDonStartEnds { get; set; }
        public List<DiemDonModel> AllDiemDonMids { get; set; }
        public HanhTrinhGiaVeModelToArray[,] HanhTrinhGiaVes { get; set; }
        public List<HanhTrinhDiemDonModel> DiemDons { get; set; }
        public IList<SelectListItem> AvailableDiemDons { get; set; }
        public class HanhTrinhGiaVeModelToArray : BaseNopEntityModel
        {

            public int FromDiemDonId { get; set; }
            public int ToDiemDonId { get; set; }
            public decimal GiaNguonVe { get; set; }
            public int NguonVeXeId { get; set; }
            public int HanhTrinhGiaVeId { get; set; }
            public string TenFromDiemDon { get; set; }
            public string TenToDiemDon { get; set; }


        }

        public class HanhTrinhDiemDonModel : BaseNopEntityModel
        {
           
            public int HanhTrinhId { get; set; }

            [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.DiemDon.DiemDonId")]
            public int DiemDonId { get; set; }
            public string DiemDonText { get; set; }            
            [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.DiemDon.ThuTu")]
            public int ThuTu { get; set; }
            [NopResourceDisplayName("ChonVe.NhaXe.HanhTrinh.DiemDon.KhoangCach")]
            public int KhoangCach { get; set; }
           
        }
        public List<VanPhongModel> AllVanPhongs { get; set; }
        public int[] SelectedVanPhongIds { get; set; }
        public int[] SelectedKhuVucIds { get; set; }
        public int WIDTH_OF_LINE { get; set; }

    }
}