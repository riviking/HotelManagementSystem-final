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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HotelManagementSystem3
{
    public partial class frmRoom : Form
    {
        // Timer for search delay (avoid searching on every keystroke)
        private Timer searchTimer;

        public frmRoom()
        {
            InitializeComponent();
            ApplyOceanTheme();
            InitializeSearchTimer();
        }

        private void InitializeSearchTimer()
        {
            searchTimer = new Timer();
            searchTimer.Interval = 300; // Wait 300ms after user stops typing
            searchTimer.Tick += SearchTimer_Tick;
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            btnSearch.PerformClick(); // Execute search
        }

        private void ApplyOceanTheme()
        {
            // Apply form background
            OceanTheme.ApplyFormBackground(this);

            // Apply button styles
            OceanTheme.ApplyButtonStyle(btnAdd);
            OceanTheme.ApplyButtonStyle(btnUpdate);
            OceanTheme.ApplyButtonStyle(btnDelete);
            OceanTheme.ApplyButtonStyle(btnClear);
            OceanTheme.ApplyButtonStyle(btnExit);
            OceanTheme.ApplyButtonStyle(btnSearch);

            // Apply textbox styles
            OceanTheme.ApplyTextBoxStyle(txtPrice);
            OceanTheme.ApplySearchBarStyle(txtSearch, btnSearch);

            // Apply combobox styles
            OceanTheme.ApplyComboBoxStyle(cmbType);
            OceanTheme.ApplyComboBoxStyle(cmbStatus);

            

            // Apply datagridview style
            OceanTheme.ApplyDataGridViewStyle(dgvRooms);
        }

        private void LoadRooms()
        {
            try
            {
                using (SqlConnection con = DB.GetConnection())
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT RoomID, RoomType, PricePerNight, Status FROM Rooms", con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvRooms.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
        }

        private void frmRoom_Load(object sender, EventArgs e)
        {
            LoadRooms();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(cmbType.Text))
                {
                    MessageBox.Show("Please select a Room Type");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Please enter a Price");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Price must be a valid number");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbStatus.Text))
                {
                    MessageBox.Show("Please select a Status");
                    return;
                }

                using (SqlConnection con = DB.GetConnection())
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Rooms (RoomType, PricePerNight, Status) VALUES (@RoomType, @PricePerNight, @Status)", con))
                {
                    cmd.Parameters.AddWithValue("@RoomType", cmbType.Text);
                    cmd.Parameters.AddWithValue("@PricePerNight", price);
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Room Added 🛏️");
                LoadRooms();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding room: " + ex.Message);
            }
        }

        private void dgvRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dgvRooms.Rows[e.RowIndex];

                cmbType.Text = row.Cells["RoomType"].Value?.ToString() ?? "";
                txtPrice.Text = row.Cells["PricePerNight"].Value?.ToString() ?? "";
                cmbStatus.Text = row.Cells["Status"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading row data: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRooms.CurrentRow == null)
                {
                    MessageBox.Show("Select a room to update.");
                    return;
                }

                if (!int.TryParse(dgvRooms.CurrentRow.Cells["RoomID"].Value?.ToString(), out int id))
                {
                    MessageBox.Show("Invalid RoomID.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbType.Text))
                {
                    MessageBox.Show("Please select a Room Type");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Please enter a valid Price");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbStatus.Text))
                {
                    MessageBox.Show("Please select a Status");
                    return;
                }

                using (SqlConnection con = DB.GetConnection())
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Rooms SET RoomType=@RoomType, PricePerNight=@PricePerNight, Status=@Status WHERE RoomID=@RoomID";
                    cmd.Parameters.AddWithValue("@RoomType", cmbType.Text);
                    cmd.Parameters.AddWithValue("@PricePerNight", price);
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@RoomID", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Room Updated ✏️");
                LoadRooms();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRooms.CurrentRow == null)
                {
                    MessageBox.Show("Please select a room to delete.");
                    return;
                }

                if (!int.TryParse(dgvRooms.CurrentRow.Cells["RoomID"].Value?.ToString(), out int id))
                {
                    MessageBox.Show("Invalid RoomID.");
                    return;
                }

                using (SqlConnection con = DB.GetConnection())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Rooms WHERE RoomID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Room Deleted ❌");
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting room: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPrice.Clear();
            cmbType.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            txtSearch.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // If search box is empty, load all rooms
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    LoadRooms();
                    return;
                }

                using (SqlConnection con = DB.GetConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT RoomID, RoomType, PricePerNight, Status FROM Rooms WHERE RoomType LIKE @search OR Status LIKE @search OR CAST(RoomID AS NVARCHAR(10)) LIKE @search", con))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvRooms.DataSource = dt;

                    // Show result count
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show($"No rooms found matching '{txtSearch.Text}'", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Stop the existing timer
            searchTimer.Stop();

            // If search box is empty, immediately load all rooms
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadRooms();
                return;
            }

            // Restart timer - will search after user stops typing
            searchTimer.Start();
        }
    }
}
