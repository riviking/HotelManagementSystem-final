using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HotelManagementSystem3
{
    public partial class frmSignUp : Form
    {
        // ────── Vibrant Color Palette (Blue, Green, Yellow) ──────────────────────────────
        private readonly Color clrDarkBg     = Color.FromArgb(25, 55, 109);      // Deep Blue (form bg)
        private readonly Color clrPanelBg    = Color.FromArgb(41, 128, 185);     // Primary Blue (panel bg)
        private readonly Color clrAccent     = Color.FromArgb(251, 188, 5);      // Accent Yellow
        private readonly Color clrAccentHov  = Color.FromArgb(56, 142, 60);      // Green (hover)
        private readonly Color clrText       = Color.FromArgb(255, 255, 255);    // White (labels)
        private readonly Color clrInputBg    = Color.FromArgb(245, 245, 245);    // Light bg (textbox)
        private readonly Color clrInputBorder= Color.FromArgb(52, 152, 219);     // Bright Blue (border)

        public frmSignUp()
        {

            InitializeComponent();
        }

        private void frmSignUp_Load(object sender, EventArgs e)
        {
            ApplyVibrantStyle();
        }

        private void ApplyVibrantStyle()
        {
            // Form styling
            this.BackColor = clrDarkBg;
            this.ForeColor = clrText;
            this.Font = new Font("Segoe UI", 10);

            // Style all labels
            foreach (Control control in this.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = clrAccent;
                    label.BackColor = Color.Transparent;
                    label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = clrInputBg;
                    textBox.ForeColor = Color.FromArgb(0, 0, 0);
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.Font = new Font("Segoe UI", 10);
                }
                else if (control is Button button)
                {
                    button.BackColor = clrPanelBg;
                    button.ForeColor = clrAccent;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = clrAccent;
                    button.FlatAppearance.BorderSize = 1;
                    button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    button.Cursor = Cursors.Hand;

                    // Add hover effects
                    button.MouseEnter += (s, e) =>
                    {
                        button.BackColor = clrAccentHov;
                        button.ForeColor = clrText;
                    };
                    button.MouseLeave += (s, e) =>
                    {
                        button.BackColor = clrPanelBg;
                        button.ForeColor = clrAccent;
                    };
                }
                else if (control is LinkLabel linkLabel)
                {
                    linkLabel.LinkColor = clrAccent;
                    linkLabel.ActiveLinkColor = clrAccentHov;
                    linkLabel.VisitedLinkColor = clrAccent;
                    linkLabel.Font = new Font("Segoe UI", 9, FontStyle.Underline);
                }
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please fill all the fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Use the centralized connection provided by DB.cs
                using (SqlConnection connection = DB.GetConnection())
                {
                    if (connection == null)
                    {
                        MessageBox.Show("Database connection is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkCmd = new SqlCommand(checkUserQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (userCount > 0)
                        {
                            MessageBox.Show("Username already exists! Please choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    const string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Account created successfully! Please login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}