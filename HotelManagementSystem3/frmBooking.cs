using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelManagementSystem3
{
  public partial class frmBooking : Form
  {
    // Guard flag to prevent calculation events from firing during form setup data-binding
    private bool isFormLoaded = false;

    public frmBooking()
    {
      InitializeComponent();
      ApplyOceanTheme();
    }

    private void ApplyOceanTheme()
    {
      // Apply form background
      OceanTheme.ApplyFormBackground(this);

      // Apply button styles
      OceanTheme.ApplyButtonStyle(btnBook);
      OceanTheme.ApplyButtonStyle(btnDelete);
      OceanTheme.ApplyButtonStyle(btnClear);
      OceanTheme.ApplyButtonStyle(btnExit);
      OceanTheme.ApplyButtonStyle(btnShowAll);

      // Apply search button styles
      OceanTheme.ApplySearchBarStyle(txtCustomerSearch, btnSearchCustomer);
      OceanTheme.ApplySearchBarStyle(txtRoomSearch, btnSearchRoom);
      OceanTheme.ApplySearchBarStyle(null, btnSearchDate);

      // Apply textbox styles
      OceanTheme.ApplyTextBoxStyle(txtTotal);

      // Apply combobox styles
      OceanTheme.ApplyComboBoxStyle(cmbCustomer);
      OceanTheme.ApplyComboBoxStyle(cmbRoom);

      // Apply label styles
      OceanTheme.ApplyLabelStyle(label1, true);
      OceanTheme.ApplyLabelStyle(labal2);
      OceanTheme.ApplyLabelStyle(label4);
      OceanTheme.ApplyLabelStyle(label3);
      OceanTheme.ApplyLabelStyle(label2);
      OceanTheme.ApplyLabelStyle(label5);
      OceanTheme.ApplyLabelStyle(label6);
      OceanTheme.ApplyLabelStyle(label7);
      OceanTheme.ApplyLabelStyle(label8);
      OceanTheme.ApplyLabelStyle(label9);

      // Apply datetimepicker styles
      OceanTheme.ApplyDateTimePickerStyle(dtpIn);
      OceanTheme.ApplyDateTimePickerStyle(dtpOut);
      OceanTheme.ApplyDateTimePickerStyle(dtpFrom);
      OceanTheme.ApplyDateTimePickerStyle(dtpTo);

      // Apply datagridview style
      OceanTheme.ApplyDataGridViewStyle(dgvBookings);
    }

    private void frmBooking_Load(object sender, EventArgs e)
    {
      // 1. Lock down calculating events completely during initial bind sequence
      isFormLoaded = false;

      dtpIn.Value = DateTime.Today;
      dtpOut.Value = DateTime.Today.AddDays(1);

      // 2. Load data tables into memory
      LoadRooms();
      LoadCustomers();
      LoadBookings();

      // 3. Setup complete, open form channels for dynamic UI interactions
      isFormLoaded = true;
      CalculateTotal();
    }

    private void LoadBookings()
    {
      try
      {
        SqlConnection con = DB.GetConnection();
        string query = "SELECT BookingID, CustomerID, RoomID, DateIn, DateOut, TotalAmount, PaymentStatus FROM Bookings";

        SqlDataAdapter da = new SqlDataAdapter(query, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        dgvBookings.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show("LoadBookings Error: " + ex.Message);
      }
    }

    private void LoadCustomers()
    {
      try
      {
        SqlDataAdapter da = new SqlDataAdapter("SELECT CustomerID, Name FROM Customers", DB.GetConnection());
        DataTable dt = new DataTable();
        da.Fill(dt);

        // Force layout state property reset to discard residual designer configurations
        cmbCustomer.DataSource = null;
        cmbCustomer.Text = string.Empty;

        // Bind mappings securely
        cmbCustomer.DisplayMember = "Name";
        cmbCustomer.ValueMember = "CustomerID";
        cmbCustomer.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show("LoadCustomers Error: " + ex.Message);
      }
    }

    private void LoadRooms()
    {
      try
      {
        // 🎯 IMPROVED: Load only rooms available for the selected date range
        string query = @"
          SELECT DISTINCT r.RoomID, r.RoomType
          FROM Rooms r
          WHERE r.Status = 'Available'
          AND r.RoomID NOT IN (
            SELECT b.RoomID
            FROM Bookings b
            WHERE (b.DateIn < @DateOut AND b.DateOut > @DateIn)      
          )
          ORDER BY r.RoomID";

        SqlDataAdapter da = new SqlDataAdapter(query, DB.GetConnection());

        // Add date parameters
        da.SelectCommand.Parameters.AddWithValue("@DateIn", dtpIn.Value.Date);
        da.SelectCommand.Parameters.AddWithValue("@DateOut", dtpOut.Value.Date);

        DataTable dt = new DataTable();
        da.Fill(dt);

        // 🎯 NEW: Add display column showing "RoomType (RoomID)" format
        // This makes it easier to identify specific rooms in the combo box
        dt.Columns.Add("DisplayValue", typeof(string), "RoomType + ' (' + RoomID + ')'");

        // Force layout state property reset to discard residual designer configurations
        cmbRoom.DataSource = null;
        cmbRoom.Text = string.Empty;

        // Bind mappings securely
        // Display: "Single (1002)", "Double (1003)", etc.
        // Value: RoomID for database operations
        cmbRoom.DisplayMember = "DisplayValue";
        cmbRoom.ValueMember = "RoomID";
        cmbRoom.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show("LoadRooms Error: " + ex.Message);
      }
    }

    private void CalculateTotal()
    {
      // Do not fire if form initialization steps are active
      if (!isFormLoaded) return;
      if (cmbRoom.SelectedValue == null || !int.TryParse(cmbRoom.SelectedValue.ToString(), out int roomId)) return;

      TimeSpan days = dtpOut.Value.Date - dtpIn.Value.Date;
      int totalDays = days.Days;

      if (totalDays <= 0)
      {
        txtTotal.Text = "0.00";
        return;
      }

      try
      {
        using (SqlConnection con = DB.GetConnection())
        using (SqlCommand cmd = new SqlCommand("SELECT PricePerNight FROM Rooms WHERE RoomID=@id", con))
        {
          cmd.Parameters.Add("@id", SqlDbType.Int).Value = roomId;
          con.Open();
          object result = cmd.ExecuteScalar();
          double price = (result == null || result == DBNull.Value) ? 0.0 : Convert.ToDouble(result);
          txtTotal.Text = (price * totalDays).ToString("F2");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Calculation Error: " + ex.Message);
      }
    }

    // Added event state tracking block checks to handle premature WinForms load lifecycle cycles
    private void dtpOut_ValueChanged(object sender, EventArgs e)
    {
      if (isFormLoaded)
      {
        // 🎯 RELOAD rooms when checkout date changes (to check availability for new date range)
        LoadRooms();
        CalculateTotal();
      }
    }

    private void dtpIn_ValueChanged(object sender, EventArgs e)
    {
      if (isFormLoaded)
      {
        // 🎯 AUTO-FILL: Set checkout date to the day after check-in
        dtpOut.Value = dtpIn.Value.AddDays(1);

        // 🎯 RELOAD rooms when check-in date changes (to check availability for new date range)
        LoadRooms();
        CalculateTotal();
      }
    }

    private void cmbRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (isFormLoaded) CalculateTotal();
    }

    private void btnBook_Click(object sender, EventArgs e)
    {
      try
      {
        // 1. UI Input Parameter Validations
        if (cmbCustomer.SelectedValue == null || !int.TryParse(cmbCustomer.SelectedValue.ToString(), out int customerId))
        {
          MessageBox.Show("Please select a valid customer.");
          return;
        }
        if (cmbRoom.SelectedValue == null || !int.TryParse(cmbRoom.SelectedValue.ToString(), out int roomId))
        {
          MessageBox.Show("Please select a valid room.");
          return;
        }
        if (string.IsNullOrWhiteSpace(txtTotal.Text) || txtTotal.Text == "0.00")
        {
          MessageBox.Show("Invalid booking range or pricing total calculated.");
          return;
        }

        SqlConnection con = DB.GetConnection();

        // STEP 1: DATE OVERLAP DOUBLE-BOOKING CHECK
        string checkQuery = @"
                    SELECT COUNT(*) FROM Bookings 
                    WHERE RoomID = @room 
                    AND ((DateIn <= @out AND DateOut >= @in))";

        SqlCommand check = new SqlCommand(checkQuery, con);
        check.Parameters.Add("@room", SqlDbType.Int).Value = roomId;
        check.Parameters.Add("@in", SqlDbType.DateTime).Value = dtpIn.Value.Date;
        check.Parameters.Add("@out", SqlDbType.DateTime).Value = dtpOut.Value.Date;

        con.Open();
        int existingBookingsCount = (int)check.ExecuteScalar();
        con.Close();

        if (existingBookingsCount > 0)
        {
          MessageBox.Show("This room is already reserved within the selected date range!");
          return;
        }

        // STEP 2: INSERT TRANSACTION RECORD BOOKING
        string insertQuery = @"
                    INSERT INTO Bookings (CustomerID, RoomID, DateIn, DateOut, TotalAmount, PaymentStatus)
                    VALUES (@c, @r, @in, @out, @t, 'Unpaid')";

        SqlCommand cmd = new SqlCommand(insertQuery, con);
        cmd.Parameters.Add("@c", SqlDbType.Int).Value = customerId;
        cmd.Parameters.Add("@r", SqlDbType.Int).Value = roomId;
        cmd.Parameters.Add("@in", SqlDbType.DateTime).Value = dtpIn.Value.Date;
        cmd.Parameters.Add("@out", SqlDbType.DateTime).Value = dtpOut.Value.Date;
        cmd.Parameters.Add("@t", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotal.Text);

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        // STEP 3: UPDATE ROOM AVAILABILITY CONFIGURATION
        SqlCommand cmd2 = new SqlCommand("UPDATE Rooms SET Status='Booked' WHERE RoomID=@id", con);
        cmd2.Parameters.Add("@id", SqlDbType.Int).Value = roomId;

        con.Open();
        cmd2.ExecuteNonQuery();
        con.Close();

        MessageBox.Show("Room Booked Successfully 🎉");

        LoadBookings();
        LoadRooms();
      }
      catch (Exception ex)
      {
        MessageBox.Show("Booking Operational Error: " + ex.Message);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      try
      {
        if (dgvBookings.SelectedRows.Count == 0)
        {
          MessageBox.Show("Select a complete row via the left sidebar marker first!");
          return;
        }

        int bookingId = Convert.ToInt32(dgvBookings.SelectedRows[0].Cells["BookingID"].Value);
        int roomId = Convert.ToInt32(dgvBookings.SelectedRows[0].Cells["RoomID"].Value);

        SqlConnection con = DB.GetConnection();

        SqlCommand cmd = new SqlCommand("DELETE FROM Bookings WHERE BookingID=@id", con);
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = bookingId;

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        SqlCommand cmd2 = new SqlCommand("UPDATE Rooms SET Status='Available' WHERE RoomID=@room", con);
        cmd2.Parameters.Add("@room", SqlDbType.Int).Value = roomId;

        con.Open();
        cmd2.ExecuteNonQuery();
        con.Close();

        MessageBox.Show("Booking dropped and room availability released.");

        LoadBookings();
        LoadRooms();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void dgvBookings_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0) return;
      try
      {
        DataGridViewRow row = dgvBookings.Rows[e.RowIndex];

        cmbCustomer.SelectedValue = row.Cells["CustomerID"].Value;
        cmbRoom.SelectedValue = row.Cells["RoomID"].Value;
        dtpIn.Value = Convert.ToDateTime(row.Cells["DateIn"].Value);
        dtpOut.Value = Convert.ToDateTime(row.Cells["DateOut"].Value);
        txtTotal.Text = Convert.ToDecimal(row.Cells["TotalAmount"].Value).ToString("F2");
      }
      catch (Exception ex)
      {
        MessageBox.Show("Cell Selection Mapping Error: " + ex.Message);
      }
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      cmbCustomer.SelectedIndex = -1;
      cmbRoom.SelectedIndex = -1;
      dtpIn.Value = DateTime.Today;
      dtpOut.Value = DateTime.Today.AddDays(1);
      txtTotal.Text = "0.00";
      dgvBookings.ClearSelection();
    }

    private void btnSearchCustomer_Click(object sender, EventArgs e)
    {
      try
      {
        SqlConnection con = DB.GetConnection();
        string query = @"
                    SELECT B.* FROM Bookings B
                    INNER JOIN Customers C ON B.CustomerID = C.CustomerID
                    WHERE C.Name LIKE @name";

        SqlDataAdapter da = new SqlDataAdapter(query, con);
        da.SelectCommand.Parameters.AddWithValue("@name", "%" + txtCustomerSearch.Text + "%");

        DataTable dt = new DataTable();
        da.Fill(dt);
        dgvBookings.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnSearchRoom_Click(object sender, EventArgs e)
    {
      try
      {
        SqlConnection con = DB.GetConnection();
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Bookings WHERE RoomID = @room", con);
        da.SelectCommand.Parameters.Add("@room", SqlDbType.Int).Value = Convert.ToInt32(txtRoomSearch.Text);

        DataTable dt = new DataTable();
        da.Fill(dt);
        dgvBookings.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnSearchDate_Click(object sender, EventArgs e)
    {
      try
      {
        SqlConnection con = DB.GetConnection();
        string query = "SELECT * FROM Bookings WHERE DateIn >= @from AND DateIn < DATEADD(day, 1, @to)";

        SqlDataAdapter da = new SqlDataAdapter(query, con);
        da.SelectCommand.Parameters.Add("@from", SqlDbType.DateTime).Value = dtpFrom.Value.Date;
        da.SelectCommand.Parameters.Add("@to", SqlDbType.DateTime).Value = dtpTo.Value.Date;

        DataTable dt = new DataTable();
        da.Fill(dt);
        dgvBookings.DataSource = dt;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void btnShowAll_Click(object sender, EventArgs e) => LoadBookings();
    private void btnExit_Click(object sender, EventArgs e) => this.Close();

    // Target method kept to resolve lingering automated designer compilation links
    private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
    {
    }
  }
}