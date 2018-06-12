using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;

namespace Nop.Data.Mapping.NhaXes
{
    public class HanhTrinhLoaiXeGiaVeMap : NopEntityTypeConfiguration<HanhTrinhLoaiXeGiaVe>
    {
        public HanhTrinhLoaiXeGiaVeMap()
        {
            this.ToTable("CV_HanhTrinh_LoaiXe_GiaVe");
            this.HasKey(c => c.Id);
            this.Property(u => u.GiaVe).HasPrecision(18, 0);
            
        }
    }
}
