using Nop.Core.Domain.Chonves;
using System;
using System.Collections.Generic;


namespace Nop.Core.Domain.NhaXes
{
    public class HanhTrinhKhuVuc : BaseEntity
    {
        public int HanhTrinhId { get; set; }
        public virtual HanhTrinh HanhTrinh { get; set; }   
       
        public int KhuVucId { get; set; }
       
       
        
        
    }
}
