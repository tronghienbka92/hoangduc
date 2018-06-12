using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;

namespace Nop.Data.Mapping.NhaXes
{
    public class LichTrinhLoaiXeMap : NopEntityTypeConfiguration<LichTrinhLoaiXe>
    {
        public LichTrinhLoaiXeMap()
        {
            this.ToTable("CV_LichTrinh_LoaiXe");
            this.HasKey(c => c.Id);
            this.Property(u => u.GiaVe).HasPrecision(18, 0);
            this.HasRequired(c => c.hanhtrinh)
               .WithMany()
               .HasForeignKey(c => c.HanhTrinhId);
            this.HasRequired(c => c.lichtrinh)
               .WithMany()
               .HasForeignKey(c => c.LichTrinhId);
            this.HasRequired(c => c.loaixe)
               .WithMany()
               .HasForeignKey(c => c.LoaiXeId);
        }
    }
}
