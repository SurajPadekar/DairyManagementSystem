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

namespace Dairy__Manager
{
    public partial class MilkProduction : Form
    {
        public MilkProduction()
        {
            InitializeComponent();
            FillCowId();
            populate();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            Cows Ob = new Cows();
            Ob.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            CowHealth Ob = new CowHealth();
            Ob.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Breed Ob = new Breed();
            Ob.Show();
            this.Hide();
        }

        private void label18_Click(object sender, EventArgs e)
        {
            MilkSales Ob = new MilkSales();
            Ob.Show();
            this.Hide();
        }

        private void label20_Click(object sender, EventArgs e)
        {
            Finance Ob = new Finance();
            Ob.Show();
            this.Hide();
        }

        private void label22_Click(object sender, EventArgs e)
        {
            Dashboard Ob = new Dashboard();
            Ob.Show();
            this.Hide();
        }

        private void CowIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MilkProduction_Load(object sender, EventArgs e)
        {

        }
        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-K0VE1E79\SQLEXPRESS04;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
       private void FillCowId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select CowId from CowTbl",Con);
            SqlDataReader Rdr;
            Rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CowId",typeof(int));
            dt.Load(Rdr);
            CowIdCb.ValueMember = "CowId";
            CowIdCb.DataSource = dt;
            Con.Close();
        }

        private void populate()
        {
            try
            {
                Con.Open();
                string query = "select * from MilkTable"; // Correct the typo here
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                MilkDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
            }
        }

        private void Clear()
        {
            Cownmametb.Text = "";
            Amtb.Text = "";
            noonTb.Text = "";
            PmTb.Text = "";
            TotalTb.Text = "";
        }
        private void GetCowName()
        {
            Con.Open();
            string query = "select * from CowTbl where CowId = " + CowIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Cownmametb.Text = dr["CowName"].ToString();

            }
            Con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CowIdCb.SelectedIndex == -1  || Cownmametb.Text == "" || Amtb.Text == "" || noonTb.Text == "" || PmTb.Text == "" || TotalTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO MilkTable VALUES(@CowId, @CowName, @AmMilk, @NoonMilk, @PmMilk, @TotalMilk, @Date)";
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@CowId", CowIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@CowName", Cownmametb.Text);
                    cmd.Parameters.AddWithValue("@AmMilk", Amtb.Text);
                    cmd.Parameters.AddWithValue("@NoonMilk", noonTb.Text);
                    cmd.Parameters.AddWithValue("@PmMilk", PmTb.Text);  
                    cmd.Parameters.AddWithValue("@TotalMilk", TotalTb.Text);
                    cmd.Parameters.AddWithValue("@Date", Date.Value.Date);

                    // Execute the query
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Milk information added successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // Make sure to close the connection after use
                    Con.Close();
                    populate();
                    Clear();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void CowIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();
        }

        private void PmTb_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void PmTb_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(Amtb.Text, out int amtValue) &&
             int.TryParse(noonTb.Text, out int noonValue) &&
             int.TryParse(PmTb.Text, out int pmValue))
            {
                // Perform the calculation and update TotalTb.
                int total = amtValue + noonValue + pmValue;
                TotalTb.Text = total.ToString();
            }
            else
            {
                // Handle parsing errors, e.g., display a message or set a default value.
                TotalTb.Text = "Invalid input";
            }

        }
        int key = 0;
        private void MilkDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < MilkDGV.Rows.Count)
            {
                DataGridViewRow row = MilkDGV.Rows[e.RowIndex];

                CowIdCb.SelectedValue = row.Cells["CowId"].Value.ToString();
                Cownmametb.Text = row.Cells["CowName"].Value.ToString();
                Amtb.Text = row.Cells["AmMilk"].Value.ToString();
                noonTb.Text = row.Cells["NoonMilk"].Value.ToString();
                PmTb.Text = row.Cells["PmMilk"].Value.ToString();
                TotalTb.Text = row.Cells["TotalMilk"].Value.ToString();
                Date.Text = row.Cells["DateProd"].Value.ToString();

                if (string.IsNullOrEmpty(Cownmametb.Text))
                {
                    key = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells["CowId"].Value);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the milk production to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from MilkTable where CowId=" + key + ";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Milk Production Data Deleted Successfully");

                    Con.Close();
                    populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CowIdCb.Text == "" || Cownmametb.Text == "" || Amtb.Text == "" || noonTb.Text == "" || PmTb.Text == "" || TotalTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update MilkTable set CowId = '" + CowIdCb.Text + "' , CowName = '" + Cownmametb.Text + "' , AmMilk = '" + Amtb.Text + "' , NoonMilk = '" + noonTb.Text + "', PmMilk = '" + PmTb.Text + "', TotalMilk = '" + TotalTb.Text + "', DateProd = '" + Date.Text + "' where CowId = " + key + "; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Milk Production Data Updated Successfully");

                    Con.Close();
                    populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }


    }

}
        

