using Nop.Core.Domain.NhaXes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.NhaXes
{
    public class HangMucChiPhiMap : NopEntityTypeConfiguration<HangMucChiPhi>
    {
        public HangMucChiPhiMap()
        {
            this.ToTable("CV_HangMucChiPhi");
            this.HasKey(c => c.Id);
            this.Property(u => u.Ten).HasMaxLength(500);
        }
    }
}
