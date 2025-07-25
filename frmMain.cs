using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311_3B_2023_Database
{
    public partial class frmMain : Form
    {
        private string username, usertype;
        public frmMain(String username, String usertype)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;

        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts accounts = new frmAccounts(username);
            accounts.MdiParent = this;
            accounts.Show();
        }
        private void courseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCourse courses = new frmCourse(username);
            courses.MdiParent = this;
            courses.Show();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudent students = new frmStudent(username);
            students.MdiParent = this;
            students.Show();
        }

        private void subjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSubject subjects = new frmSubject(username);
            subjects.MdiParent = this;
            subjects.Show();
        }

        private void advisingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdvisingSubject advisesubject = new frmAdvisingSubject();
            advisesubject.MdiParent = this;
            advisesubject.Show();
        }
        private void gradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGrade grades = new frmGrade(username);
            grades.MdiParent = this;
            grades.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            username = usertype = string.Empty;
            frmLogin login = new frmLogin();
            this.Close();
            login.Show();
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUsername.Text = "Username: " + username;
            lblUsertype.Text = "Usertype: " + usertype;
            if (usertype == "SECRETARY")
            {
                accountsToolStripMenuItem.Visible = false;
                courseToolStripMenuItem.Visible = false;
                gradesToolStripMenuItem.Visible = true;
                studentsToolStripMenuItem.Visible = false;
                subjectsToolStripMenuItem.Visible = false;
            }
            else if (usertype == "REGISTRAR")
            {
                accountsToolStripMenuItem.Visible = false;
                courseToolStripMenuItem.Visible = true;
                gradesToolStripMenuItem.Visible = false;
                studentsToolStripMenuItem.Visible = true;
                subjectsToolStripMenuItem.Visible = true;
            }
            else
            {
                 accountsToolStripMenuItem.Visible = true;
                 courseToolStripMenuItem.Visible = true;
                 gradesToolStripMenuItem.Visible = true;
                 studentsToolStripMenuItem.Visible = true;
                 subjectsToolStripMenuItem.Visible = true;
            }
        }
        ///FORM DESIGN
    }
}
