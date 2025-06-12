using System;

namespace otobusbilet
{
    public abstract class Sefer : IRezervasyon
    {
        public string Nereden { get; set; }
        public string Nereye { get; set; }
        public DateTime Tarih { get; set; }
        public Otobus Otobus { get; set; }

        // Abstract üyeler (zorunlu implementasyon için)
        public abstract decimal Fiyat { get; set; }
        public abstract void RezerveEt(Yolcu yolcu, Koltuk koltuk);

        public Sefer(string nereden, string nereye, DateTime tarih, Otobus otobus)
        {
            Nereden = nereden;
            Nereye = nereye;
            Tarih = tarih;
            Otobus = otobus;
        }
    }
}
