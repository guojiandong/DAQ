using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ksat.AppPlugIn.UiCtrl;

namespace AirLeakTester.UiControl
{
    public partial class NumInputControl : AbstractUserControlBase
    {
        #region 初始值
        public float num = 0;
        //设置增量
        public float increment = 1;
        //设置小数点位数
        public int decimalPlaces = 0;
        //设置最大值
        public float maximum = 100;
        //设置最小值
        public float minimum = -100;

        //是否可编辑
        public bool ctrlEnabled
        {
            set {
                btnAdd.Enabled = value;
                btnReduce.Enabled = value;
                inputTxt.Enabled = value;
            }
        }

        //获取或设置文本框的值
        public string ctrlText
        {
            get {
                return inputTxt.Text;
            }
            set {
                if(!string.IsNullOrEmpty(value))
                inputTxt.Text  = Convert.ToDouble(value).ToString("f"+ decimalPlaces);              
            }
        }
        #endregion

        public NumInputControl()
        {
            InitializeComponent();
            inputTxt.Font = base.Font;
        }

        void initControl()
        {  
            if (!string.IsNullOrEmpty(ctrlText))
            {
                inputTxt.Text = ctrlText;
            }
            else {
                inputTxt.Text = num.ToString("f" + decimalPlaces);
            }
        }

        void relayoutControl() {
            int h = this.Height;
            btnAdd.SetBounds(0, 0, h, h);
            inputTxt.AutoSize = false;
            inputTxt.SetBounds(h, 0, this.Width - h * 2, h);
            btnReduce.SetBounds(this.Width - h, 0, h, h);

            //Font fo = new Font(inputTxt.Font.FontFamily, h * 0.62f);
            //inputTxt.Font = fo;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //numInput.UpButton();
            float.TryParse(inputTxt.Text, out num);
            if (Math.Round(num + increment, decimalPlaces) > maximum)
            {
                inputTxt.Text = maximum.ToString("f" + decimalPlaces);
            }
            else {
                inputTxt.Text = (num + increment).ToString("f" + decimalPlaces);
            }
        }

        private void btnReduce_Click(object sender, EventArgs e)
        {
            //numInput.DownButton();
            float.TryParse(inputTxt.Text, out num);
            if (Math.Round(num - increment, decimalPlaces) < minimum)
            {
                inputTxt.Text = minimum.ToString("f" + decimalPlaces);
            }
            else
            {
                inputTxt.Text = (num - increment).ToString("f" + decimalPlaces);
            }
        }

        private void NumInputControl_Load(object sender, EventArgs e)
        {
            initControl();
        }

        private void NumInputControl_Resize(object sender, EventArgs e)
        {
            relayoutControl();
        }

        private void NumInputControl_SizeChanged(object sender, EventArgs e)
        {
            relayoutControl();
        }

        private void inputTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            //48代表0，57代表9，8代表回退，46代表小数点，45代表负号
            if ((e.KeyChar < '0' || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 45) && (e.KeyChar != 46))
                e.Handled = true;

            //负号若有则为第一位
            if (e.KeyChar == 45 && inputTxt.SelectionStart != 0)
            {
                e.Handled = true;
            }

            //小数点不能为第一位
            if (e.KeyChar == 46)
            {
                //小数点不能为第一位
                if (inputTxt.Text.Length == 0)
                {
                    e.Handled = true;
                }
                //负数时小数点不能为第二位
                if (inputTxt.Text == "-")
                {
                    e.Handled = true;
                }
            }

            //只能有一个小数点
            if (inputTxt.Text.Contains(".") && e.KeyChar == 46)
            {
                e.Handled = true;
            }

            //验证小数点后面的位数
            if (decimalPlaces == 0)
            {
                if (e.KeyChar == 46) e.Handled = true;
            }
        }

        private void inputTxt_TextChanged(object sender, EventArgs e)
        {
            //验证小数位数
            if (decimalPlaces > 0)
            {
                var arr = inputTxt.Text.Split('.');
                if (arr != null && arr.Length == 2)
                {
                    if (arr[1].Length > decimalPlaces)
                    {
                        inputTxt.Text = inputTxt.Text.Substring(0, inputTxt.Text.Length - 1);
                        inputTxt.SelectionStart = inputTxt.Text.Length;
                    }
                }
            }

            //光标输入时数值范围验证
            float.TryParse(inputTxt.Text, out num);
            if (num > maximum)
            {
                inputTxt.Text = (maximum).ToString("f" + decimalPlaces);
            }
            else if (num < minimum)
            {
                inputTxt.Text = (minimum).ToString("f" + decimalPlaces);
            }

        }
    }
}
