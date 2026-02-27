using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Assistant
{
    /// <summary>
    /// Wi-Fi bağlantı durumunu temsil eden numaralandırma (enum).
    /// Durum göstergesi kontrolünde kullanılır.
    /// </summary>
    public enum Durum
    {
        /// <summary>Durum belirlenmemiş</summary>
        None = 0,
        /// <summary>Oturum açık / Bağlantı aktif</summary>
        Aktif = 1,
        /// <summary>Oturum kapalı / Bağlantı pasif</summary>
        Pasif = 2,
        /// <summary>İşlem devam ediyor / Yükleniyor</summary>
        Yukleniyor = 3
    }
}
