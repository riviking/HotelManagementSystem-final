using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static HotelManagementSystem3.OceanTheme;

namespace HotelManagementSystem3
{
    public partial class frmReport : Form
    {
        // Summary card panels
        private Panel pnlTotalIncome, pnlTotalBookings, pnlTotalRooms, pnlPaidBookings, pnlUnpaidBookings;

        public frmReport()
        {
            InitializeComponent();
            ApplyOceanTheme();
            CreateSummaryCardPanels();
        }

        private void CreateSummaryCardPanels()
        {
            // Create panels for each summary metric with styling
            pnlTotalIncome = CreateSummaryCard(lblTotalIncomee, OceanTheme.Colors.PrimaryBlue, "💰 Total Income");
            pnlTotalBookings = CreateSummaryCard(lblTotalBookings, OceanTheme.Colors.PrimaryGreen, "📅 Bookings");
            pnlTotalRooms = CreateSummaryCard(lblTotalRooms, OceanTheme.Colors.BrightBlue, "🏨 Rooms");
            pnlPaidBookings = CreateSummaryCard(lblPaidBookings, OceanTheme.Colors.DeepGreen, "💳 Paid");
            pnlUnpaidBookings = CreateSummaryCard(lblUnpaidBookings, OceanTheme.Colors.AccentYellow, "❌ Unpaid");
        }

        private Panel CreateSummaryCard(Label label, Color accentColor, string title)
        {
            Panel panel = new Panel
            {
                BackColor = Color.FromArgb(245, 248, 250),
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10),
                Width = 180,
                Height = 80
            };

            // Add a colored top border for accent
            Color localAccentColor = accentColor;
            panel.Paint += (s, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(localAccentColor), 0, 0, panel.Width, 4);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200), 1), 0, 0, panel.Width - 1, panel.Height - 1);
            };

            // Add title label
            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(5, 8)
            };

            // Style the value label
            label.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label.ForeColor = OceanTheme.Colors.DarkText;
            label.AutoSize = true;
            label.Location = new Point(5, 28);

            panel.Controls.Add(titleLabel);

            return panel;
        }

        private void ApplyOceanTheme()
        {
            // Apply form background - gradient-like pale background
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Style summary panel
            tableLayoutPanel2.BackColor = Color.FromArgb(255, 255, 255);
            tableLayoutPanel2.Padding = new Padding(15);

            // Apply label styles - summary card labels
            OceanTheme.ApplyLabelStyle(lblTotalIncomee);
            OceanTheme.ApplyLabelStyle(lblTotalBookings);
            OceanTheme.ApplyLabelStyle(lblTotalRooms);
            OceanTheme.ApplyLabelStyle(lblPaidBookings);
            OceanTheme.ApplyLabelStyle(lblUnpaidBookings);

            // Style chart containers with border and background
            StyleChartPanel(chart1, "📊 Daily Income Report");
            StyleChartPanel(chart2, "🏨 Room Status");
            StyleChartPanel(chart3, "💳 Payment Status");
            StyleChartPanel(chartMonthly, "📈 Monthly Income Trend");

            // Apply button styles
            if (Controls.OfType<Button>().Any())
            {
                foreach (Button btn in Controls.OfType<Button>())
                {
                    OceanTheme.ApplyButtonStyle(btn);
                }
            }

            // Apply label styles for titles
            foreach (Label lbl in Controls.OfType<Label>())
            {
                if (lbl.Name.StartsWith("label"))
                {
                    lbl.ForeColor = OceanTheme.Colors.DarkText;
                    lbl.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                }
            }

            // Apply datagridview style if any
            foreach (DataGridView dgv in Controls.OfType<DataGridView>())
            {
                OceanTheme.ApplyDataGridViewStyle(dgv);
            }

            // Style DateTimePicker
            OceanTheme.ApplyDateTimePickerStyle(dateTimePicker1);

            // Style the tableLayoutPanel1 background
            tableLayoutPanel1.BackColor = Color.FromArgb(248, 249, 250);

            // Style tableLayoutPanel3 (month selector)
            tableLayoutPanel3.BackColor = Color.FromArgb(255, 255, 255);
            tableLayoutPanel3.Padding = new Padding(10);
            label6.ForeColor = OceanTheme.Colors.DarkText;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        private void StyleChartPanel(Chart chart, string title)
        {
            chart.BackColor = Color.FromArgb(255, 255, 255);
            chart.BorderlineColor = Color.FromArgb(220, 220, 220);
            chart.BorderlineWidth = 1;
            chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;

            // Add title to chart
            if (chart.Titles.Count == 0)
            {
                var chartTitle = new System.Windows.Forms.DataVisualization.Charting.Title
                {
                    Text = title,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = OceanTheme.Colors.DeepBlue
                };
                chart.Titles.Add(chartTitle);
            }
            else
            {
                chart.Titles[0].Text = title;
                chart.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                chart.Titles[0].ForeColor = OceanTheme.Colors.DeepBlue;
            }

            // Style chart area
            foreach (var area in chart.ChartAreas)
            {
                area.BackColor = Color.FromArgb(252, 252, 253);
                area.BorderColor = Color.FromArgb(200, 200, 200);
                area.AxisX.LineColor = Color.FromArgb(200, 200, 200);
                area.AxisY.LineColor = Color.FromArgb(200, 200, 200);
                area.AxisX.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
                area.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

                // Style axis labels
                area.AxisX.LabelStyle.ForeColor = OceanTheme.Colors.DarkText;
                area.AxisY.LabelStyle.ForeColor = OceanTheme.Colors.DarkText;
                area.AxisX.TitleForeColor = OceanTheme.Colors.DeepBlue;
                area.AxisY.TitleForeColor = OceanTheme.Colors.DeepBlue;
            }

            // Style legend
            foreach (var legend in chart.Legends)
            {
                legend.BackColor = Color.FromArgb(255, 255, 255);
                legend.ForeColor = OceanTheme.Colors.DarkText;
                legend.BorderColor = Color.FromArgb(200, 200, 200);
            }
        }

        private void frmReport_Load(object sender, EventArgs e)
        {

            lblTotalIncomee.Text = "0";
            lblTotalBookings.Text = "0";
            lblTotalRooms.Text = "0";
            lblPaidBookings.Text = "0";
            lblUnpaidBookings.Text = "0";

            LoadSummaryCards();
        }

        private void LoadSummaryCards()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                con.Open();

                // 💰 Total Income
                SqlCommand cmd1 = new SqlCommand("SELECT ISNULL(SUM(TotalAmount),0) FROM Payments", con);
                decimal totalIncome = Convert.ToDecimal(cmd1.ExecuteScalar());
                lblTotalIncomee.Text = "Rs. " + totalIncome;

                // 📅 Total Bookings
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Bookings", con);
                lblTotalBookings.Text = cmd2.ExecuteScalar().ToString();

                // 🏨 Total Rooms
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Rooms", con);
                lblTotalRooms.Text = cmd3.ExecuteScalar().ToString();

                // 💳 Paid Bookings
                SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM Bookings WHERE PaymentStatus='Paid'", con);
                lblPaidBookings.Text = cmd4.ExecuteScalar().ToString();

                // ❌ Unpaid Bookings
                SqlCommand cmd5 = new SqlCommand("SELECT COUNT(*) FROM Bookings WHERE PaymentStatus='Unpaid'", con);
                lblUnpaidBookings.Text = cmd5.ExecuteScalar().ToString();

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadIncomeChart()
        {
            SqlConnection con = DB.GetConnection();

            string query = @"
    SELECT 
        CAST(PaidDate AS DATE) AS PayDate,
        SUM(TotalAmount) AS Total
    FROM Payments
    GROUP BY CAST(PaidDate AS DATE)";

            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.Titles.Add("📊 Daily Income Report");
            chart1.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            chart1.Titles[0].ForeColor = OceanTheme.Colors.DeepBlue;

            Series s = new Series();
            s.ChartType = SeriesChartType.Column;
            s.Color = OceanTheme.Colors.PrimaryBlue;

            foreach (DataRow row in dt.Rows)
            {
                s.Points.AddXY(row["PayDate"], row["Total"]);
            }

            chart1.Series.Add(s);

            s.IsValueShownAsLabel = true;
            s.LabelForeColor = OceanTheme.Colors.DeepBlue;

            chart1.ChartAreas[0].BackColor = Color.FromArgb(252, 252, 253);
            chart1.ChartAreas[0].AxisX.Title = "Date";
            chart1.ChartAreas[0].AxisY.Title = "Amount (Rs.)";
            chart1.ChartAreas[0].AxisX.TitleForeColor = OceanTheme.Colors.DeepBlue;
            chart1.ChartAreas[0].AxisY.TitleForeColor = OceanTheme.Colors.DeepBlue;
            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = OceanTheme.Colors.DarkText;
            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = OceanTheme.Colors.DarkText;

            chart1.BackColor = Color.White;
            chart1.BorderlineColor = Color.FromArgb(220, 220, 220);
            chart1.BorderlineWidth = 1;
        }

        private void btnIncomeChart_Click(object sender, EventArgs e)
        {
            LoadIncomeChart();
        }

        private void LoadRoomChart()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                SqlCommand cmd = new SqlCommand(@"
        SELECT Status, COUNT(*) AS Total
        FROM Rooms
        GROUP BY Status", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                chart2.Series.Clear();
                chart2.ChartAreas.Clear();
                chart2.Titles.Clear();

                chart2.ChartAreas.Add("ChartArea1");

                Series s = new Series();
                s.ChartType = SeriesChartType.Pie;
                s.ChartArea = "ChartArea1";

                // Color array for pie slices
                Color[] colors = new Color[]
                {
                    OceanTheme.Colors.BrightGreen,
                    OceanTheme.Colors.AccentYellow,
                    OceanTheme.Colors.PrimaryBlue,
                    OceanTheme.Colors.DeepGreen
                };

                int colorIndex = 0;
                while (dr.Read())
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(
                        dr["Status"].ToString(),
                        Convert.ToInt32(dr["Total"])
                    );
                    point.Color = colors[colorIndex % colors.Length];
                    s.Points.Add(point);
                    colorIndex++;
                }

                chart2.Series.Add(s);

                chart2.Titles.Add("🏨 Room Status");
                chart2.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                chart2.Titles[0].ForeColor = OceanTheme.Colors.DeepBlue;

                s["PieLabelStyle"] = "Outside";
                chart2.BackColor = Color.White;
                chart2.ChartAreas[0].BackColor = Color.FromArgb(252, 252, 253);

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRoomChart_Click(object sender, EventArgs e)
        {
            LoadRoomChart();
        }

        private void LoadPaymentChart()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                SqlCommand cmd = new SqlCommand(@"
        SELECT PaymentStatus, COUNT(*) AS Total
        FROM Bookings
        GROUP BY PaymentStatus", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                chart3.Series.Clear();
                chart3.ChartAreas.Clear();
                chart3.Titles.Clear();

                chart3.ChartAreas.Add("ChartArea1");

                Series s = new Series();
                s.ChartType = SeriesChartType.Pie;
                s.ChartArea = "ChartArea1";

                // Color array for payment status
                Color[] statusColors = new Color[]
                {
                    OceanTheme.Colors.DeepGreen,    // Paid
                    OceanTheme.Colors.AccentYellow  // Unpaid
                };

                int colorIdx = 0;
                while (dr.Read())
                {
                    DataPoint point = new DataPoint();
                    point.SetValueXY(
                        dr["PaymentStatus"].ToString(),
                        Convert.ToInt32(dr["Total"])
                    );
                    point.Color = statusColors[colorIdx % statusColors.Length];
                    s.Points.Add(point);
                    colorIdx++;
                }

                chart3.Series.Add(s);

                chart3.Titles.Add("💳 Payment Status");
                chart3.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                chart3.Titles[0].ForeColor = OceanTheme.Colors.DeepBlue;

                chart3.BackColor = Color.White;
                chart3.ChartAreas[0].BackColor = Color.FromArgb(252, 252, 253);

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPaymentChart_Click(object sender, EventArgs e)
        {
            LoadPaymentChart();
        }

        private void LoadMonthlyIncomeChart()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                string query = @"
        SELECT 
            FORMAT(PaidDate, 'yyyy-MM') AS Month,
            SUM(TotalAmount) AS Total
        FROM Payments
        GROUP BY FORMAT(PaidDate, 'yyyy-MM')";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                chartMonthly.Series.Clear();
                chartMonthly.ChartAreas.Clear();
                chartMonthly.Titles.Clear();

                chartMonthly.ChartAreas.Add("Area1");

                Series s = new Series();
                s.ChartType = SeriesChartType.Column;
                s.ChartArea = "Area1";
                s.Color = OceanTheme.Colors.BrightBlue;

                foreach (DataRow row in dt.Rows)
                {
                    s.Points.AddXY(row["Month"], row["Total"]);
                }

                s.IsValueShownAsLabel = true;
                s.LabelForeColor = OceanTheme.Colors.DeepBlue;

                chartMonthly.Series.Add(s);
                chartMonthly.Titles.Add("📈 Monthly Income Trend");
                chartMonthly.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                chartMonthly.Titles[0].ForeColor = OceanTheme.Colors.DeepBlue;

                chartMonthly.ChartAreas[0].BackColor = Color.FromArgb(252, 252, 253);
                chartMonthly.ChartAreas[0].AxisX.Title = "Month";
                chartMonthly.ChartAreas[0].AxisY.Title = "Amount (Rs.)";
                chartMonthly.ChartAreas[0].AxisX.TitleForeColor = OceanTheme.Colors.DeepBlue;
                chartMonthly.ChartAreas[0].AxisY.TitleForeColor = OceanTheme.Colors.DeepBlue;
                chartMonthly.ChartAreas[0].AxisX.LabelStyle.ForeColor = OceanTheme.Colors.DarkText;
                chartMonthly.ChartAreas[0].AxisY.LabelStyle.ForeColor = OceanTheme.Colors.DarkText;

                chartMonthly.BackColor = Color.White;
                chartMonthly.BorderlineColor = Color.FromArgb(220, 220, 220);
                chartMonthly.BorderlineWidth = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMonthlyReport_Click(object sender, EventArgs e)
        {
            LoadMonthlyIncomeChart();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadMonthlyIncomeChart();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
