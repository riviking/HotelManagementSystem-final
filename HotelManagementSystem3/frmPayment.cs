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

namespace HotelManagementSystem3
{
    public partial class frmPayment : Form
    {
        public frmPayment()
        {
            InitializeComponent();
            ApplyOceanTheme();
        }

        private void ApplyOceanTheme()
        {
            // Apply form background
            OceanTheme.ApplyFormBackground(this);

            // Apply button styles
            OceanTheme.ApplyButtonStyle(btnPay);
            OceanTheme.ApplyButtonStyle(btnLoadBookings);
            OceanTheme.ApplyButtonStyle(btnRemovePayment);
            OceanTheme.ApplyButtonStyle(btnExit);

            // Apply textbox styles
            OceanTheme.ApplyTextBoxStyle(txtAmount);

            // Apply combobox styles
            OceanTheme.ApplyComboBoxStyle(cmbBooking2);


            // Apply datetimepicker styles
            OceanTheme.ApplyDateTimePickerStyle(dtpPaidDate);

            // Apply datagridview style
            OceanTheme.ApplyDataGridViewStyle(dgvPayments);
        }

        private void LoadBookings()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                string query = @"
        SELECT BookingID 
        FROM Bookings
        WHERE PaymentStatus = 'Unpaid'";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbBooking2.DisplayMember = "BookingID";
                cmbBooking2.ValueMember = "BookingID";
                cmbBooking2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool isLoaded = false;
        private void cmbBooking_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBooking2.SelectedValue == null)
                    return;

                if (!int.TryParse(cmbBooking2.SelectedValue.ToString(), out int bookingId))
                    return;

                SqlConnection con = DB.GetConnection();

                SqlCommand cmd = new SqlCommand(
                    "SELECT TotalAmount, DateOut FROM Bookings WHERE BookingID=@id", con);

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = bookingId;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Auto-fill Amount
                    txtAmount.Text = dr["TotalAmount"].ToString();

                    // 🎯 Auto-fill DateTimePicker with DateOut from Bookings
                    if (dr["DateOut"] != DBNull.Value)
                    {
                        dtpPaidDate.Value = Convert.ToDateTime(dr["DateOut"]);
                    }
                    else
                    {
                        dtpPaidDate.Value = DateTime.Now;
                    }
                }

                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadPayments()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                string query = @"
        SELECT 
            PaymentID,
            BookingID,
            TotalAmount,
            PaidDate
        FROM Payments";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPayments.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmPayments_Load(object sender, EventArgs e)
        {
            LoadBookings();
            LoadPayments();


            dtpPaidDate.Value = DateTime.Now;

            isLoaded = true;
        }

        private void cmbBooking2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            if (cmbBooking2.SelectedValue == null) return;

            SqlConnection con = DB.GetConnection();

            SqlCommand cmd = new SqlCommand(
                "SELECT TotalAmount, DateOut FROM Bookings WHERE BookingID=@id", con);

            cmd.Parameters.AddWithValue("@id", cmbBooking2.SelectedValue);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                // Auto-fill Amount
                txtAmount.Text = dr["TotalAmount"].ToString();

                // 🎯 Auto-fill DateTimePicker with DateOut from Bookings
                if (dr["DateOut"] != DBNull.Value)
                {
                    dtpPaidDate.Value = Convert.ToDateTime(dr["DateOut"]);
                }
                else
                {
                    dtpPaidDate.Value = DateTime.Now;
                }
            }

            dr.Close();
            con.Close();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBooking2.SelectedValue == null)
                {
                    MessageBox.Show("Select a booking!");
                    return;
                }

                int bookingId = Convert.ToInt32(cmbBooking2.SelectedValue);

                SqlConnection con = DB.GetConnection();

                //  STEP 1: INSERT PAYMENT
                string query = @"
        INSERT INTO Payments (BookingID, TotalAmount, PaidDate)
        VALUES (@b, @a, @d)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@b", SqlDbType.Int).Value = bookingId;
                cmd.Parameters.Add("@a", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAmount.Text);
                cmd.Parameters.Add("@d", SqlDbType.DateTime).Value = dtpPaidDate.Value;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                //  STEP 2: UPDATE BOOKING STATUS → PAID
                SqlCommand updateCmd = new SqlCommand(@"
        UPDATE Bookings 
        SET PaymentStatus='Paid'
        WHERE BookingID=@id", con);

                updateCmd.Parameters.AddWithValue("@id", bookingId);

                con.Open();
                updateCmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Payment Successful 💳");

                // 🔄 STEP 3: REFRESH DATA (AFTER all updates are done)
                LoadPayments();
                LoadBookings();

                // CLEAR FIELDS
                cmbBooking2.SelectedIndex = -1;
                txtAmount.Clear();
                dtpPaidDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadBookings_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT BookingID FROM Bookings  WHERE PaymentStatus = 'Unpaid'", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbBooking2.DisplayMember = "BookingID";
                cmbBooking2.ValueMember = "BookingID";
                cmbBooking2.DataSource = dt;

                if (dt.Rows.Count > 0)
                    MessageBox.Show($"✓ {dt.Rows.Count} Unpaid Booking(s) loaded");
                else
                    MessageBox.Show("No unpaid bookings found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRemovePayment_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a payment row is selected
                if (dgvPayments.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a payment record to delete!", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the PaymentID from the selected row
                int paymentId = Convert.ToInt32(dgvPayments.SelectedRows[0].Cells[0].Value);
                int bookingId = Convert.ToInt32(dgvPayments.SelectedRows[0].Cells[1].Value);
                decimal amount = Convert.ToDecimal(dgvPayments.SelectedRows[0].Cells[2].Value);

                // Confirmation Dialog
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete this payment?\n\n" +
                    $"Payment ID: {paymentId}\n" +
                    $"Booking ID: {bookingId}\n" +
                    $"Amount: Rs. {amount}",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // DELETE THE PAYMENT
                SqlConnection con = DB.GetConnection();

                SqlCommand cmdDeletePayment = new SqlCommand(
                    "DELETE FROM Payments WHERE PaymentID = @PaymentID", con);
                cmdDeletePayment.Parameters.AddWithValue("@PaymentID", paymentId);

                con.Open();
                int rowsAffected = cmdDeletePayment.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    // UPDATE BOOKING STATUS BACK TO UNPAID
                    SqlCommand cmdUpdateBooking = new SqlCommand(
                        "UPDATE Bookings SET PaymentStatus = 'Unpaid' WHERE BookingID = @BookingID", con);
                    cmdUpdateBooking.Parameters.AddWithValue("@BookingID", bookingId);

                    con.Open();
                    cmdUpdateBooking.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show($"Payment Deleted Successfully! ✓\n\nBooking {bookingId} status changed to Unpaid.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 🔄 REFRESH PAYMENTS GRID
                    LoadPayments();

                    // 🔄 REFRESH BOOKINGS DROPDOWN
                    LoadBookings();

                    // Clear the input fields
                    cmbBooking2.SelectedIndex = -1;
                    txtAmount.Clear();
                    dtpPaidDate.Value = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting payment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
