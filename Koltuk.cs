using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace otobusbilet
{
    public class Koltuk
    {
        public int Numara { get; set; }
        public bool Rezerve { get; set; } = false;
        public Yolcu Yolcu { get; set; } = null;
    }
}
