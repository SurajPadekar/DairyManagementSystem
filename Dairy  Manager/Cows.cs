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

namespace Dairy__Manager
{
    public partial class Cows : Form
    {
        public Cows()
        {
            InitializeComponent();
            populate();
        }
        // SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-K0VE1E79\SQLEXPRESS04;Initial Catalog=DairyFarmDb;Persist Security Info=True;User ID=sa;Password=***********;Encrypt=True;Trust Server Certificate=True");
        //SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-K0VE1E79\SQLEXPRESS04;Initial Catalog=DairyFarmDb;Persist Security Info=True;User ID=sa;Password=***********;Encrypt=True;");

        SqlConnection Con = new SqlConnection(@"Data Source=LAPTOP-K0VE1E79\SQLEXPRESS04;Initial Catalog=DairyFarmDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        private void DFMS_Click(object sender, EventArgs e)
        {

        }

       

        

        private void gunaLineTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {
            MilkProduction Ob = new MilkProduction();
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

        private void AgeTb_TextChanged(object sender, EventArgs e)
        {

        }


        //private void populate()
        //{
        //    Con.Open();
        //    string query = "select * fron CowTbl";
        //    SqlDataAdapter sda =new SqlDataAdapter(query,Con);
        //    SqlCommandBuilder builder = new SqlCommandBuilder(sda);
        //    var ds = new DataSet();
        //    sda.Fill(ds);
        //    CowsDGV.DataSource = ds.Tables[0];
        //    Con.Close();
        //}

        private void populate()
        {
            try
            {
                Con.Open();
                string query = "select * from Cowtbl"; // Correct the typo here
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                CowsDGV.DataSource = ds.Tables[0];
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
            CowNameTb.Text = "";
            EarTagTb.Text = "";
            ColorTb.Text = "";
            BreedTb.Text = "";
            WeightTb.Text = "";
            AgeTb.Text = "";
            PastureTb.Text = "";
            key = 0;
        }

        int age = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            
                if (CowNameTb.Text == "" || EarTagTb.Text == "" || ColorTb.Text == "" || BreedTb.Text == "" || WeightTb.Text == "" || AgeTb.Text == "" || PastureTb.Text == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    try
                    {
                        Con.Open();
                        string Query = "INSERT INTO CowTbl VALUES(@CowName, @EarTag, @Color, @Breed, @Age, @Weight, @Pasture)";
                        SqlCommand cmd = new SqlCommand(Query, Con);
                        cmd.Parameters.AddWithValue("@CowName", CowNameTb.Text);
                        cmd.Parameters.AddWithValue("@EarTag", EarTagTb.Text);
                        cmd.Parameters.AddWithValue("@Color", ColorTb.Text);
                        cmd.Parameters.AddWithValue("@Breed", BreedTb.Text);
                        cmd.Parameters.AddWithValue("@Age", age);  // Assuming 'age' is calculated or assigned elsewhere
                        cmd.Parameters.AddWithValue("@Weight", WeightTb.Text);
                        cmd.Parameters.AddWithValue("@Pasture", PastureTb.Text);

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Cow information added successfully");
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


        private void Cows_Load(object sender, EventArgs e)
        {

        }

        private void DOBDate_ValueChanged(object sender, EventArgs e)
        {
            age = Convert.ToInt32((DateTime.Today.Date - DOBDate.Value.Date).Days) / 365;
            //MessageBox.Show("" + age);

        }

        private void DOBDate_MouseLeave(object sender, EventArgs e)
        {
            age = Convert.ToInt32((DateTime.Today.Date - DOBDate.Value.Date).Days) / 365;
            AgeTb.Text = "" +age;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        int key = 0;
        private void CowsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
                if (e.RowIndex >= 0 && e.RowIndex < CowsDGV.Rows.Count)
                {
                    DataGridViewRow row = CowsDGV.Rows[e.RowIndex];

                    CowNameTb.Text = row.Cells["CowName"].Value.ToString();
                    EarTagTb.Text = row.Cells["EarTag"].Value.ToString();
                    ColorTb.Text = row.Cells["Color"].Value.ToString();
                    BreedTb.Text = row.Cells["Breed"].Value.ToString();
                    WeightTb.Text = row.Cells["WeightAtBirth"].Value.ToString();
                    PastureTb.Text = row.Cells["Pasture"].Value.ToString();

                    if (string.IsNullOrEmpty(CowNameTb.Text))
                    {
                    key = 0;
                    age = 0;
                     }
                  else
                  {
                    key = Convert.ToInt32(row.Cells["CowId"].Value);
                    age = Convert.ToInt32(row.Cells["Age"].Value);
                  }
                }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the cow to be deleted");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "delete from CowTbl where CowId=" + key + ";";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cow Data Deleted Successfully");

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
            if (CowNameTb.Text == "" || EarTagTb.Text == "" || ColorTb.Text == "" || BreedTb.Text == "" || WeightTb.Text == "" || AgeTb.Text == "" || PastureTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "update CowTbl set CowName = '" + CowNameTb.Text + "' , EarTag = '" + EarTagTb.Text + "' , Color = '" + ColorTb.Text + "' , Breed = '" + BreedTb.Text + "', Age = '" + AgeTb.Text + "', WeightAtBirth = '" + WeightTb.Text + "', Pasture = '" + PastureTb.Text + "' where CowId = " + key + "; ";

                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cow Data Updated Successfully");
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

        private void CowNameTb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

