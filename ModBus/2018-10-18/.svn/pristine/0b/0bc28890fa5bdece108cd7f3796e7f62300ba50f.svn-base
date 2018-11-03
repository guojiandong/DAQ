using Ksat.AppPlugIn.UiCtrl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksat.AppPlugInUiLibrary.UiCtrl
{
    public partial class AboutDialog : AbstractChildForm
    {
        public AboutDialog() : this(Assembly.GetExecutingAssembly())
        {

        }

        private readonly Assembly mAssembly;

        public AboutDialog(Assembly assembly)
        {
            if (assembly == null)
                mAssembly = Assembly.LoadFrom(Process.GetCurrentProcess().MainModule.FileName);
            else
                mAssembly = assembly;

            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        public void setIcon()
        {
            //this.logoPictureBox.Image = 
        }

        public static AboutDialog ShowForm(Form owner, Assembly assembly)
        {
            AboutDialog form = new AboutDialog(assembly);
            form.Owner = owner;
            form.Show();

            return form;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                //Assembly.Load(Process.GetCurrentProcess().MainModule.FileName)
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                //object[] attributes = Assembly.LoadFrom(Process.GetCurrentProcess().MainModule.FileName).GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                object[] attributes = mAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(mAssembly.CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                object[] attributes = mAssembly.GetCustomAttributes(typeof(System.Reflection.AssemblyFileVersionAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyFileVersionAttribute customAttribute = (AssemblyFileVersionAttribute)attributes[0];
                    if (customAttribute != null)
                    {
                        return customAttribute.Version;
                    }
                }

                return mAssembly.GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = mAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = mAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = mAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = mAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
#endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
