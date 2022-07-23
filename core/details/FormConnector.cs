using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarkui.core.details
{




    class FormConnector
    {
        private Main mMainForm;

        private List<Main> mConnectedForms = new List<Main>();

        private Point mMainLocation;

        public FormConnector(Main mainForm)
        {
            this.mMainForm = mainForm;
            this.mMainLocation = new Point(this.mMainForm.Location.X, this.mMainForm.Location.Y);
            this.mMainForm.LocationChanged += new EventHandler(MainForm_LocationChanged);
        }

        public void ConnectForm(Main form)
        {
            if (!this.mConnectedForms.Contains(form))
            {
                this.mConnectedForms.Add(form);
            }
        }

        void MainForm_LocationChanged(object sender, EventArgs e)
        {
            Point relativeChange = new Point(this.mMainForm.Location.X - this.mMainLocation.X, this.mMainForm.Location.Y - this.mMainLocation.Y);
            foreach (Main form in this.mConnectedForms)
            {
                form.Location = new Point(form.Location.X + relativeChange.X, form.Location.Y + relativeChange.Y);
            }

            this.mMainLocation = new Point(this.mMainForm.Location.X, this.mMainForm.Location.Y);
        }
    }
}