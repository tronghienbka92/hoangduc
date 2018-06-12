using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;

namespace Nop.Data.Mapping.NhaXes
{
    public class XeVanChuyenMap : NopEntityTypeConfiguration<XeVanChuyen>
    {
        public XeVanChuyenMap()
        {
            this.ToTable("CV_XeVanChuyen");
            this.HasKey(c => c.Id);
            this.Property(u => u.TenXe).HasMaxLength(200);
            this.Property(u => u.BienSo).HasMaxLength(50);
            this.Property(u => u.DienThoai).HasMaxLength(100);
            this.Property(p => p.Latitude).HasPrecision(18, 9);
            this.Property(p => p.Longitude).HasPrecision(18, 9);
            this.HasRequired(c => c.loaixe)
             .WithMany()
             .HasForeignKey(c => c.LoaiXeId);
            this.HasOptional(c => c.laixe)
            .WithMany()
            .HasForeignKey(c => c.LaiXeId);
            this.Ignore(u => u.TrangThaiXe);
        }
    }
}
