using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class TblKhachHang
    {
        public TblKhachHang()
        {
            TblHoadons = new HashSet<TblHoadon>();
        }

        public string MaKh { get; set; } = null!;
        public string? TenKh { get; set; }
        public bool? Gt { get; set; }
        public string? Diachi { get; set; }
        public DateTime NgaySinh { get; set; }

        public virtual ICollection<TblHoadon> TblHoadons { get; set; }
    }
}
