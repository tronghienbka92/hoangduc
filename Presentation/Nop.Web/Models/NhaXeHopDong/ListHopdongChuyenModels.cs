using FluentValidation.Attributes;
using Nop.Core.Domain.NhaXes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.NhaXes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nop.Web.Models.NhaXes
{
   
    public class ListHopdongChuyenModels : BaseNopEntityModel
    {
        public ListHopdongChuyenModels()
         {
            
             
         }
          [NopResourceDisplayName("Biển số xe")]
        public string BienSoXe { get; set; }
          [NopResourceDisplayName("Số hợp đồng")]
        public string SoHopDong { get; set; }
    }
}