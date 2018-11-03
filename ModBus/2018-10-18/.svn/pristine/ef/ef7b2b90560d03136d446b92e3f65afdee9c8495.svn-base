using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugInUiLibrary.UiCtrl
{
    public partial class FrmLoadingCircle : Form
    {
        public FrmLoadingCircle()
        {
            InitializeComponent();
        }

        private void FrmLoadingCircle_Load(object sender, EventArgs e)
        {
            loadingCircle1.Width = this.Width;
            loadingCircle1.Height = this.Height;

            loadingCircle1.Message = "江苏立讯机器人有限责任公司";
            loadingCircle1.FontSzie = 16;
            loadingCircle1.frmWidth = this.Width;

            loadingCircle1.Switch();
        }

        private void FrmLoadingCircle_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadingCircle1.Stop();
        }
    }
}
