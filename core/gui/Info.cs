using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using Sarkui.core.details;
using Sarkui.core.gui;
//using Sarkui.core.plugins.interfaces;
using Sarkui.core.util;
//using Sarkui.core.tools;

namespace Sarkui.core.gui
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
            lblversion.Text = "version " + Application.ProductVersion.Split('.')[0] + "." + Application.ProductVersion.Split('.')[1] + "."+ Application.ProductVersion.Split('.')[2];
            this.MaximizeBox = false;
            this.MinimizeBox = false;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://ko-fi.com/sarkas");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/sarkinios/sarkui");
        }
    }
}
