using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagementSystem3
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
            ApplyVibrantTheme();
        }

        private void ApplyVibrantTheme()
        {
            // Apply form background
            OceanTheme.ApplyFormBackground(this);

            // Apply button styles
            if (Controls.OfType<Button>().Any())
            {
                foreach (Button btn in Controls.OfType<Button>())
                {
                    OceanTheme.ApplyButtonStyle(btn);
                }
            }

            // Apply label styles
            foreach (Label lbl in Controls.OfType<Label>())
            {
                if (lbl.Font.Size > 12)
                    OceanTheme.ApplyLabelStyle(lbl, true);
                else
                    OceanTheme.ApplyLabelStyle(lbl);
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            frmCustomer c = new frmCustomer();
            c.Show();
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            frmRoom r = new frmRoom();
            r.Show();
        }

        private void btnBooking_Click(object sender, EventArgs e)
        {
            frmBooking b = new frmBooking();
            b.Show();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            frmPayment p = new frmPayment();
            p.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            frmReport r = new frmReport();
            r.Show();
        }
    }
}
