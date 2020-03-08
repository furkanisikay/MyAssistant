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
    public partial class Status : UserControl
    {
        public Status()
        {
            InitializeComponent();
        }


        private Durum durum;

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

        public string Yazi { get => Delegates.Text.Get(lblText); set => Delegates.Text.Set(lblText, value); }

    }
}
