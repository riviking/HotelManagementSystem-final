using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem3
{
    public partial class Login : Form
    {
        // ── colour palette ──────────────────────────────────────────────
        private readonly Color clrDarkBrown  = Color.FromArgb(25, 55, 109);     // Deep Blue (form bg)
        private readonly Color clrMedBrown   = Color.FromArgb(41, 128, 185);    // Primary Blue (panel bg)
        private readonly Color clrGold       = Color.FromArgb(251, 188, 5);     // Accent Yellow
        private readonly Color clrGoldLight  = Color.FromArgb(56, 142, 60);     // Green (hover)
        private readonly Color clrText       = Color.FromArgb(255, 255, 255);   // White (labels)
        private readonly Color clrInputBg    = Color.FromArgb(245, 245, 245);   // Light gray (textbox bg)
        private readonly Color clrInputBorder= Color.FromArgb(52, 152, 219);    // Bright Blue (textbox border)

        public Login()
        {
            InitializeComponent();
            // Subscribe to Paint so we can draw the gradient + decorative line
            this.Paint += Login_Paint;
        }

        // ── Form Load ───────────────────────────────────────────────────
        private void Login_Load(object sender, EventArgs e)
        {
            StyleForm();
            
            StyleTextBoxes();
            StyleButtons();
            AddHotelTitle();
        }

        // ── form / background ───────────────────────────────────────────
        private void StyleForm()
        {
            this.Text            = "🏨  Hotel Management System — Login";
            this.BackColor       = clrDarkBrown;
            this.ForeColor       = clrText;
            this.ClientSize      = new Size(800, 450);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterScreen;
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {
            // Soft vertical gradient overlay on the left half - Green to transparent
            using (var brush = new LinearGradientBrush(
                new Rectangle(0, 0, this.Width / 2, this.Height),
                Color.FromArgb(80, 56, 142, 60),
                Color.FromArgb(0,  0,   0,  0),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, 0, 0, this.Width / 2, this.Height);
            }

            // Yellow accent line under the title area
            using (var pen = new Pen(clrGold, 2))
            {
                e.Graphics.DrawLine(pen, 50, 95, this.Width - 50, 95);
            }
        }

        // ── hotel title label (added dynamically) ───────────────────────
        private void AddHotelTitle()
        {
            // Main title
            var lblTitle = new Label
            {
                Text      = "🏨  GRAND HOTEL",
                Font      = new Font("Georgia", 22, FontStyle.Bold),
                ForeColor = clrGold,
                BackColor = Color.Transparent,
                AutoSize  = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size      = new Size(700, 45),
                Location  = new Point(50, 20)
            };
            this.Controls.Add(lblTitle);
            lblTitle.BringToFront();

            // Sub-title
            var lblSub = new Label
            {
                Text      = "Management System",
                Font      = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = clrText,
                BackColor = Color.Transparent,
                AutoSize  = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size      = new Size(700, 22),
                Location  = new Point(50, 65)
            };
            this.Controls.Add(lblSub);
            lblSub.BringToFront();
        }

        // ── labels ───────────────────────────────────────────────────────
        private void StyleLabels()
        {
            var labelFont = new Font("Segoe UI", 10, FontStyle.Bold);

            label1.Font      = labelFont;
            label1.ForeColor = clrGold;
            label1.BackColor = Color.Transparent;
            label1.Text      = "👤  Username";
            label1.AutoSize  = true;
            label1.Location  = new Point(200, 145);

            label2.Font      = labelFont;
            label2.ForeColor = clrGold;
            label2.BackColor = Color.Transparent;
            label2.Text      = "🔒  Password";
            label2.AutoSize  = true;
            label2.Location  = new Point(200, 192);

            label3.Font      = new Font("Segoe UI", 9, FontStyle.Underline);
            label3.ForeColor = clrGoldLight;
            label3.BackColor = Color.Transparent;
            label3.Text      = "No account? Sign up here";
            label3.Cursor    = Cursors.Hand;
            label3.Location  = new Point(290, 308);
            label3.AutoSize  = true;
        }

        // ── text boxes ───────────────────────────────────────────────────
        private void StyleTextBoxes()
        {
            StyleTextBox(txtUsername);
            StyleTextBox(txtPassword);

            txtUsername.Location = new Point(330, 141);
            txtUsername.Size     = new Size(220, 28);

            txtPassword.Location = new Point(330, 188);
            txtPassword.Size     = new Size(220, 28);
        }

        private void StyleTextBox(TextBox tb)
        {
            tb.BackColor  = clrInputBg;
            tb.ForeColor  = clrText;
            tb.BorderStyle= BorderStyle.FixedSingle;
            tb.Font       = new Font("Segoe UI", 10);
        }

        // ── buttons ──────────────────────────────────────────────────────
        private void StyleButtons()
        {
            // Login button — yellow filled with dark blue text
            StyleButton(btnLogin,
                text     : "  Login",
                backColor: clrGold,
                foreColor: clrDarkBrown,
                location : new Point(230, 260),
                size     : new Size(120, 36));

            // Exit button — blue background with green text
            StyleButton(btnExit,
                text     : "  Exit",
                backColor: clrMedBrown,
                foreColor: clrGoldLight,
                location : new Point(390, 260),
                size     : new Size(120, 36));

            // Sign-up button — blue background with yellow text
            StyleButton(signupbtn,
                text     : "  Create Account",
                backColor: clrMedBrown,
                foreColor: clrGold,
                location : new Point(280, 345),
                size     : new Size(160, 36));

            // Hover effects
            AddHover(btnLogin,  hoverBack: clrGoldLight, hoverFore: clrDarkBrown,
                                normalBack: clrGold,     normalFore: clrDarkBrown);
            AddHover(btnExit,   hoverBack: Color.FromArgb(52, 152, 219), hoverFore: clrGoldLight,
                                normalBack: clrMedBrown, normalFore: clrGoldLight);
            AddHover(signupbtn, hoverBack: Color.FromArgb(52, 152, 219), hoverFore: clrGold,
                                normalBack: clrMedBrown, normalFore: clrGold);
        }

        private void StyleButton(Button btn, string text,
            Color backColor, Color foreColor,
            Point location, Size size)
        {
            btn.Text             = text;
            btn.BackColor        = backColor;
            btn.ForeColor        = foreColor;
            btn.FlatStyle        = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = clrGold;
            btn.FlatAppearance.BorderSize  = 1;
            btn.Font             = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor           = Cursors.Hand;
            btn.Location         = location;
            btn.Size             = size;
            btn.UseVisualStyleBackColor = false;
        }

        private void AddHover(Button btn,
            Color hoverBack,  Color hoverFore,
            Color normalBack, Color normalFore)
        {
            btn.MouseEnter += (s, e) => { btn.BackColor = hoverBack;  btn.ForeColor = hoverFore;  };
            btn.MouseLeave += (s, e) => { btn.BackColor = normalBack; btn.ForeColor = normalFore; };
        }

        // ── existing event handlers (unchanged logic) ────────────────────
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = DB.GetConnection();
                con.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Username=@u AND Password=@p";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", txtPassword.Text);

                int count = (int)cmd.ExecuteScalar();
                con.Close();

                if (count == 1)
                {
                    MessageBox.Show("Login Successful ✅");
                    this.Hide();
                    frmDashboard dash = new frmDashboard();
                    dash.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password ❌");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void signupbtn_Click(object sender, EventArgs e)
        {
            frmSignUp signUpForm = new frmSignUp();
            signUpForm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            frmSignUp signUpForm = new frmSignUp();
            signUpForm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
