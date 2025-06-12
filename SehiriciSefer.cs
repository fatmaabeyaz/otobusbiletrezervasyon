using System;

namespace otobusbilet
{
    public class SehiriciSefer : Sefer, IRezervasyon
    {
        private decimal fiyat;

        public SehiriciSefer(string nereden, string nereye, DateTime tarih, Otobus otobus, decimal fiyat)
            : base(nereden, nereye, tarih, otobus)
        {
            this.fiyat = fiyat;
        }

        public override decimal Fiyat
        {
            get { return fiyat; }
            set { fiyat = value; }
        }

        public override void RezerveEt(Yolcu yolcu, Koltuk koltuk)
        {
            if (!koltuk.Rezerve)
            {
                koltuk.Rezerve = true;
                koltuk.Yolcu = yolcu;
            }
        }

        public void RezerveGoster()
        {
            foreach (var koltuk in Otobus.Koltuklar)
            {
                if (koltuk.Rezerve)
                {
                    Console.WriteLine($"Koltuk {koltuk.Numara} - {koltuk.Yolcu.Isim} {koltuk.Yolcu.Soyisim}");
                }
            }
        }
    }
}
