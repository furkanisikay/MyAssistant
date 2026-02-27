using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NHTools;

namespace My_Assistant
{
    /// <summary>
    /// Bağlantı durumunu görsel olarak gösteren özel kullanıcı kontrolü.
    /// Aktif, Pasif ve Yükleniyor durumlarını simge ile temsil eder.
    /// </summary>
    public partial class Status : UserControl
    {
        public Status()
        {
            InitializeComponent();
        }

        // Mevcut bağlantı durumu
        private Durum durum;

        /// <summary>
        /// Bağlantı durumunu günceller ve ilgili durum simgesini gösterir.
        /// </summary>
        /// <param name="value">Yeni durum değeri (None, Aktif, Pasif, Yukleniyor)</param>
        public void SetDurum(Durum value)
        {
            durum = value;
            Optimizasyon.Delagate(pBox, () =>
            {
                switch (value)
                {
                    case Durum.None:
                        pBox.Image = null;
                        break;
                    case Durum.Aktif:
                        pBox.Image = Properties.Resources.tick_box_24px;
                        break;
                    case Durum.Pasif:
                        pBox.Image = Properties.Resources.close_window_24px;
                        break;
                    case Durum.Yukleniyor:
                        pBox.Image = Properties.Resources.loading;
                        break;
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// Durum etiketinin metin değeri. UI thread güvenli erişim sağlar.
        /// </summary>
        public string Yazi { get => Delegates.Text.Get(lblText); set => Delegates.Text.Set(lblText, value); }

    }
}
