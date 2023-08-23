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

namespace Library_project
{
    public partial class IssueBooks : Form
    {
        public IssueBooks()
        {
            InitializeComponent();
        }

        private void IssueBooks_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source = OPREKIN-PC\\SQLEXPRESS ; database=library;integrated security=True";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();

            cmd = new SqlCommand ("select bName from newbook", con);
            SqlDataReader Sdr = cmd.ExecuteReader();

            while (Sdr.Read())
            {
                for (int i = 0; i < Sdr.FieldCount; i++)
                {
                    comboBoxBooks.Items.Add(Sdr.GetString(i)); 
                }
            }
            Sdr.Close();
            con.Close();
        }

        int count;
        private void btnsearch_Click(object sender, EventArgs e)
        {
            if (txtMembership.Text != "")
            {
                String mid = txtMembership.Text;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "data source = OPREKIN-PC\\SQLEXPRESS ; database=library;integrated security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "select * from newstudent where memNo = '"+mid+"'";
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);


                //...................................................................................
                //Code to Count how many book has been issued on this enrollment number
                cmd.CommandText = "select count(mem_no) from IRBook where mem_no  = '" + mid + "' and book_return_date is null";
                SqlDataAdapter DA1 = new SqlDataAdapter(cmd);
                DataSet DS1 = new DataSet();
                DA.Fill(DS1);

                count = int.Parse(DS1.Tables[0].Rows[0][0].ToString());


                //...................................................................................

                if (DS.Tables[0].Rows.Count != 0)
                {
                    txtName.Text = DS.Tables[0].Rows[0][1].ToString();
                    txtMembership.Text = DS.Tables[0].Rows[0][2].ToString();
                    txtAddress.Text = DS.Tables[0].Rows[0][3].ToString();
                    txtAge.Text = DS.Tables[0].Rows[0][4].ToString();
                    txtContact.Text = DS.Tables[0].Rows[0][5].ToString();
                    txtEmail.Text = DS.Tables[0].Rows[0][6].ToString();
                }
                else
                {
                    txtName.Clear();
                    txtMembership.Clear(); 
                    txtAddress.Clear(); 
                    txtAge.Clear(); 
                    txtContact.Clear();
                    txtEmail.Clear();
                    MessageBox.Show("Invalid Membership No", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnissue_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                if (comboBoxBooks.SelectedIndex != -1 && count <= 2)
                {
                    String no = txtMembership.Text;
                    String mname = txtName.Text;
                    String maddress = txtAddress.Text;
                    String Age = txtAge.Text;
                    Int64 contact = Int64.Parse(txtContact.Text);
                    String email = txtEmail.Text;
                    String bookname = comboBoxBooks.Text;
                    String bookissuedate = dateTimePicker1.Text;

                    String mid = txtMembership.Text;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "data source = OPREKIN-PC\\SQLEXPRESS ; database=library;integrated security=True";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = cmd.CommandText = "insert into IRBook (mem_no,mem_name,mem_address,mem_age,mem_contact,mem_email,book_name,book_issue_date) values ('" + no + "', '" + mname + "', '" + maddress + "', '" + Age + "', '" + contact + "', '" + email + "', '" + bookname + "', '" + bookissuedate + "')";
                    con.Close();

                    MessageBox.Show("Book Issued.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Select Book. OR Maximum number of Book Has been ISSUED", "No Book Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Enter Valid Enrollment Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMembership_TextChanged(object sender, EventArgs e)
        {
            if (txtMembership.Text == "")
            {
                txtName.Clear();
                txtAddress.Clear();
                txtAge.Clear();
                txtContact.Clear();
                txtEmail.Clear();
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            txtMembership.Clear();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Sure?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            this.Close();
        }
    }
}
