using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ticket_management;

namespace CS311_3B_2023_Database
{
    public partial class frmGrade : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        
        private string username, studentNumber;
        Class1 grades = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmGrade(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmGrade_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
            {
                try
                {
                    DataTable dt = grades.GetData($"SELECT * FROM tbl_students " +
                        $"WHERE StudentNumber = '{txtStudentNumber.Text}'");
                    if (dt.Rows.Count > 0)
                    {
                        studentNumber = txtStudentNumber.Text;
                        txtName.Text = $"{dt.Rows[0].Field<string>("Lastname")}, " +
                            $"{dt.Rows[0].Field<string>("Firstname")} " +
                            $"{dt.Rows[0].Field<string>("Middlename")}";
                        txtCourse.Text = dt.Rows[0].Field<string>("Course");
                        txtYearLevel.Text = dt.Rows[0].Field<string>("Year");

                        dt = grades.GetData($"SELECT sub.SubjectCode, sub.SubjectDescription, " +
                            $"sub.Unit, grd.Grade FROM tbl_grades grd " +
                            $"JOIN tbl_subjects sub ON grd.SubjectCode = sub.SubjectCode " +
                            $"WHERE grd.StudentNumber = '{txtStudentNumber.Text}'" +
                            $"ORDER BY sub.SubjectCode");

                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("No data found", "Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtStudentNumber.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on search button",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Input a student number first", "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtStudentNumber.Focus();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtStudentNumber.Clear();
            txtName.Clear();
            txtCourse.Clear();
            txtYearLevel.Clear();
            txtStudentNumber.Focus();
            dataGridView1.DataSource = null; // Set the DataSource to null
            dataGridView1.Rows.Clear(); // Now clear the rows
            dataGridView1.Refresh();    // Refresh the datagridview
            frmGrade_Load(sender, e);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(studentNumber))
            {
                frmNewGrade newgrade = new frmNewGrade(studentNumber, txtName.Text, txtCourse.Text);
                if (newgrade.ShowDialog() == DialogResult.OK)
                {
                    txtStudentNumber.Text = studentNumber;
                    btnSearch_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Input a student number first", "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtStudentNumber.Focus();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(studentNumber) && (row >= 0 && row < dataGridView1.Rows.Count))
            {
                string SubjectCode = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string Description = dataGridView1.Rows[row].Cells[1].Value.ToString();
                string EditGrade = dataGridView1.Rows[row].Cells[3].Value.ToString();
                frmUpdateGrade updategrades = new frmUpdateGrade(studentNumber, txtName.Text, txtCourse.Text,
                    SubjectCode, Description, EditGrade, username);
                if (updategrades.ShowDialog() == DialogResult.OK)
                {
                    txtStudentNumber.Text = studentNumber;
                    btnSearch_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Select a row to edit", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Delete Grade?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                    grades.executeSQL("DELETE FROM tbl_grades WHERE StudentNumber = '" + studentNumber + "' AND SubjectCode = '" + selectedUser + "'");
                    if (grades.rowAffected > 0)
                    {
                        MessageBox.Show("Grade Deleted!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grades.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                        DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                        DateTime.Now.ToShortTimeString() +
                        "', 'Deleted a grade from Student  " + studentNumber + " with a subjectCode of " + selectedUser + "', '" +
                        username + "', 'Grade Management')");

                        txtStudentNumber.Text = studentNumber;
                        btnSearch_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private int row = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on datagrid cell click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmGrade_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtStudentNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }
    }
}