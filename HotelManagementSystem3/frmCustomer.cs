using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace HotelManagementSystem3
{
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();
            ApplyOceanTheme();
        }

        private void ApplyOceanTheme()
        {
            // Apply form background
            OceanTheme.ApplyFormBackground(this);

            // Apply button styles
            OceanTheme.ApplyButtonStyle(btnAdd);
            OceanTheme.ApplyButtonStyle(btnUpdate);
            OceanTheme.ApplyButtonStyle(button3);
            OceanTheme.ApplyButtonStyle(btnClear);
            OceanTheme.ApplyButtonStyle(btnExit);
            OceanTheme.ApplyButtonStyle(btnSearch);

            // Apply textbox styles
            OceanTheme.ApplyTextBoxStyle(txtName);
            OceanTheme.ApplyTextBoxStyle(txtPhone);
            OceanTheme.ApplyTextBoxStyle(txtNIC);
            OceanTheme.ApplySearchBarStyle(txtSearch, btnSearch);

            // Apply label styles
            OceanTheme.ApplyLabelStyle(label1, true);
            OceanTheme.ApplyLabelStyle(label2);
            OceanTheme.ApplyLabelStyle(label3);
            OceanTheme.ApplyLabelStyle(label4);

            // Apply datagridview style
            OceanTheme.ApplyDataGridViewStyle(dgvCustomers);
        }

        private void LoadCustomers()
        {
            try
            {
                SqlConnection con = DB.GetConnection();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvCustomers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Enter Name");
                return;
            }

            try
            {
                SqlConnection con = DB.GetConnection();

                string query = "INSERT INTO Customers (Name, Phone, NIC) VALUES (@n,@p,@nic)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@p", txtPhone.Text);
                cmd.Parameters.AddWithValue("@nic", txtNIC.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer Added ✅");

                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];

                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtNIC.Text = row.Cells["NIC"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);

                SqlConnection con = DB.GetConnection();

                string query = "UPDATE Customers SET Name=@n, Phone=@p, NIC=@nic WHERE CustomerID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@n", txtName.Text);
                cmd.Parameters.AddWithValue("@p", txtPhone.Text);
                cmd.Parameters.AddWithValue("@nic", txtNIC.Text);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer Updated ✏️");

                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);

                SqlConnection con = DB.GetConnection();

                string query = "DELETE FROM Customers WHERE CustomerID=@id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Customer Deleted ❌");

                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtPhone.Clear();
            txtNIC.Clear();
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
                SqlConnection con = DB.GetConnection();

                string query = "SELECT * FROM Customers WHERE Name LIKE @search OR Phone LIKE @search OR NIC LIKE @search";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvCustomers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
