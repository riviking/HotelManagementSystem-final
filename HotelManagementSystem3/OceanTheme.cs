using System;
using System.Drawing;
using System.Windows.Forms;

namespace HotelManagementSystem3
{
    /// <summary>
    /// Modern Vibrant Theme Styling Helper
    /// Provides vibrant blue, green, and yellow color palette for hotel management system
    /// </summary>
    public static class OceanTheme
    {
        // Modern Vibrant Color Palette - Blue, Green, Yellow Mix
        public static class Colors
        {
            // Blue Tones
            public static Color DeepBlue = Color.FromArgb(25, 55, 109);       // #193D6D
            public static Color PrimaryBlue = Color.FromArgb(41, 128, 185);   // #2980B9
            public static Color BrightBlue = Color.FromArgb(52, 152, 219);    // #3498DB

            // Green Tones
            public static Color DeepGreen = Color.FromArgb(27, 94, 32);       // #1B5E20
            public static Color PrimaryGreen = Color.FromArgb(56, 142, 60);   // #388E3C
            public static Color BrightGreen = Color.FromArgb(76, 175, 80);    // #4CAF50
            public static Color LightGreen = Color.FromArgb(200, 230, 201);   // #C8E6C9

            // Yellow/Gold Tones
            public static Color AccentYellow = Color.FromArgb(251, 188, 5);   // #FBBC05
            public static Color LightYellow = Color.FromArgb(255, 243, 181);  // #FFF3B5

            // Neutral Tones
            public static Color DarkText = Color.FromArgb(33, 33, 33);        // #212121
            public static Color LightText = Color.White;
            public static Color PaleBackground = Color.FromArgb(245, 245, 245); // #F5F5F5
            public static Color WhiteSnow = Color.FromArgb(255, 255, 255);    // #FFFFFF
        }

        // Apply Modern Theme to Button with Hover Effects
        public static void ApplyButtonStyle(Button btn)
        {
            btn.BackColor = Colors.DeepBlue;
            btn.ForeColor = Colors.LightText;
            btn.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.BorderColor = Colors.PrimaryBlue;
            btn.Cursor = Cursors.Hand;

            // Add hover effects - blend with yellow/blue
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Colors.BrightBlue;
                btn.FlatAppearance.MouseOverBackColor = Colors.BrightBlue;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Colors.DeepBlue;
            };
        }

        // Apply Modern Theme to TextBox
        public static void ApplyTextBoxStyle(TextBox txt)
        {
            txt.BackColor = Colors.WhiteSnow;
            txt.ForeColor = Colors.DarkText;
            txt.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        // Apply Ocean Theme to ComboBox
        public static void ApplyComboBoxStyle(ComboBox cmb)
        {
            cmb.BackColor = Colors.WhiteSnow;
            cmb.ForeColor = Colors.DarkText;
            cmb.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
            cmb.FlatStyle = FlatStyle.Flat;
        }

        // Apply Ocean Theme to Label
        public static void ApplyLabelStyle(Label lbl, bool isTitle = false)
        {
            lbl.ForeColor = Colors.WhiteSnow;
            if (isTitle)
            {
                lbl.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            }
            else
            {
                lbl.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            }
        }

        // Apply Modern Theme to DataGridView
        public static void ApplyDataGridViewStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Colors.PaleBackground;
            dgv.ForeColor = Colors.DarkText;
            dgv.GridColor = Color.FromArgb(220, 220, 220);
            dgv.EnableHeadersVisualStyles = false;

            // Header styling - Deep Green with Gold accent
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Colors.DeepGreen;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Colors.LightText;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colors.AccentYellow;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Colors.DarkText;

            // Row styling - White with Light green alternating
            dgv.DefaultCellStyle.BackColor = Colors.WhiteSnow;
            dgv.DefaultCellStyle.ForeColor = Colors.DarkText;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Colors.AccentYellow;
            dgv.DefaultCellStyle.SelectionForeColor = Colors.DarkText;

            // Alternating row colors - Light green tint
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Colors.LightGreen;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Colors.DarkText;
        }

        // Apply Ocean Theme to DateTimePicker
        public static void ApplyDateTimePickerStyle(DateTimePicker dtp)
        {
            dtp.BackColor = Colors.WhiteSnow;
            dtp.ForeColor = Colors.DarkText;
            dtp.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
        }

        // Apply Search Bar Styling with Yellow Accent
        public static void ApplySearchBarStyle(TextBox searchBox, Button searchBtn)
        {
            if (searchBox != null)
            {
                searchBox.BackColor = Colors.PaleBackground;
                searchBox.ForeColor = Colors.DarkText;
                searchBox.Font = new Font("Segoe UI", 16F, FontStyle.Regular);
                searchBox.BorderStyle = BorderStyle.FixedSingle;
            }

            if (searchBtn != null)
            {
                searchBtn.BackColor = Colors.AccentYellow;
                searchBtn.ForeColor = Colors.DarkText;
                searchBtn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                searchBtn.FlatStyle = FlatStyle.Flat;
                searchBtn.FlatAppearance.BorderSize = 0;
                searchBtn.Cursor = Cursors.Hand;

                searchBtn.MouseEnter += (s, e) =>
                {
                    searchBtn.BackColor = Colors.PrimaryBlue;
                    searchBtn.ForeColor = Colors.LightText;
                };

                searchBtn.MouseLeave += (s, e) =>
                {
                    searchBtn.BackColor = Colors.AccentYellow;
                    searchBtn.ForeColor = Colors.DarkText;
                };
            }
        }

        // Apply Form Background
        public static void ApplyFormBackground(Form form)
        {
            form.BackColor = Colors.WhiteSnow;
        }
    }
}
