using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;

namespace Nop.Data.Mapping.NhaXes
{
    public class KhachHangChuyenMap : NopEntityTypeConfiguration<KhachHangChuyen>
    {
        public KhachHangChuyenMap()
        {
            this.ToTable("CV_KhachHangChuyen");
            this.HasKey(c => c.Id);
            this.HasRequired(c => c.HopDongInfo)
              .WithMany()
              .HasForeignKey(c => c.HopDongChuyenId);
         
        }
    }
}
