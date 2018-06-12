using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.NhaXes
{
    public class BangGiaVeMap : NopEntityTypeConfiguration<BangGiaVe>
    {
        public BangGiaVeMap()
        {
            this.ToTable("CV_BangGiaVe");
            this.HasKey(c => c.Id);
            this.Property(u => u.ThongTin).HasMaxLength(500);
            this.Property(p => p.GiaVe).HasPrecision(18, 0);
        }
    }
}
