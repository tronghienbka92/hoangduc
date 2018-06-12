using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;

namespace Nop.Data.Mapping.NhaXes
{
    public class HanhTrinhKhuVucMap : NopEntityTypeConfiguration<HanhTrinhKhuVuc>
    {
        public HanhTrinhKhuVucMap()
        {
            this.ToTable("CV_HanhTrinh_KhuVuc");
            this.HasKey(c => c.Id);
           
            this.HasRequired(c => c.HanhTrinh)
            .WithMany()
            .HasForeignKey(c => c.HanhTrinhId);
          

        }
    }
}
