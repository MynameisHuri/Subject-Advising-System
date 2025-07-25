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
    public partial class frmAdvisingSubject : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        Class1 advisesubject = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        private string studentNumber;
        public frmAdvisingSubject()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
            {
                try
                {
                    DataTable dt = advisesubject.GetData("SELECT * FROM tbl_students " +
                        $"WHERE studentNumber = '{txtStudentNumber.Text}'");
                    if (dt.Rows.Count > 0)
                    {
                        studentNumber = txtStudentNumber.Text;
                        txtLastname.Text = dt.Rows[0].Field<string>("Lastname");
                        txtFirstname.Text = dt.Rows[0].Field<string>("Firstname");
                        txtCourse.Text = dt.Rows[0].Field<string>("Course");
                        txtYearLevel.Text = dt.Rows[0].Field<string>("Year");

                        string query = "SELECT DISTINCT s.SubjectCode, s.SubjectDescription, s.Unit FROM tbl_subjects s " +
                        $"LEFT JOIN tbl_grades g ON s.SubjectCode = g.SubjectCode AND g.StudentNumber = '{studentNumber}' " +
                         "WHERE g.StudentNumber IS NULL AND s.SubjectCode NOT IN (SELECT p.SubjectCode FROM tbl_subjects p WHERE ";
                        for (int i = 1; i <= 4; i++)
                        {
                            query += $"(p.prerequisite{i} <> '' AND NOT EXISTS (SELECT 1 FROM tbl_grades g2 " +
                                $"WHERE g2.StudentNumber = '{studentNumber}' AND g2.SubjectCode = p.Prerequisite{i} " +
                                 "AND g2.Grade NOT IN ('INC', 'DRP')))";

                            if (i < 4)
                                query += " OR ";
                            else
                                query += ");";
                        }
                        dt = advisesubject.GetData(query);

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

        private void frmAdvisingSubject_MouseDown(object sender, MouseEventArgs e)
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
        private void frmAdvisingSubject_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblTime.Text = DateTime.Now.ToLongTimeString();
            lblDate.Text = DateTime.Now.ToLongDateString();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }
    }
}
