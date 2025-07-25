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
    public partial class frmUpdateSubject : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End
        private string editSubjectCode, editSubjectDescription, editUnit, editCourse, 
            editPreRequisite1, editPreRequisite2,
            editPreRequisite3, editPreRequisite4, username;

        Class1 updatesubject = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmUpdateSubject(string editSubjectCode, string editSubjectDescription,
                                string editUnit, string editCourse, 
                                string editPreRequisite1, string editPreRequisite2,
                                string editPreRequisite3, string editPreRequisite4, 
                                string username)
        {
            InitializeComponent();
            this.editSubjectCode = editSubjectCode;
            this.editSubjectDescription = editSubjectDescription;
            this.editUnit = editUnit;
            this.editCourse = editCourse;
            this.editPreRequisite1 = editPreRequisite1;
            this.editPreRequisite2 = editPreRequisite2;
            this.editPreRequisite3 = editPreRequisite3;
            this.editPreRequisite4 = editPreRequisite4;
            this.username = username;
        }

        private void frmUpdateSubject_Load(object sender, EventArgs e)
        {
            txtSubjectCode.Text = editSubjectCode;
            txtSubjectDescription.Text = editSubjectDescription;
            try
            {
                DataTable dt = updatesubject.GetData("SELECT * FROM tbl_course ORDER BY courseDescription");
                if (dt.Rows.Count > 0)
                {
                    cmbCourse.DataSource = dt;
                    cmbCourse.DisplayMember = "courseDescription";
                    cmbCourse.ValueMember = "courseID";
                }
                DataTable dtSubject = updatesubject.GetData("SELECT SubjectCode, SubjectDescription FROM tbl_subjects ORDER BY SubjectDescription");
                DataRow optionals = dtSubject.NewRow();
                optionals["SubjectCode"] = "";
                optionals["SubjectDescription"] = "Select Pre Requisite";
                dtSubject.Rows.InsertAt(optionals, 0);
                if (dtSubject.Rows.Count > 0)
                {
                    bindingSource1.DataSource = dtSubject;
                    bindingSource2.DataSource = dtSubject;
                    bindingSource3.DataSource = dtSubject;
                    bindingSource4.DataSource = dtSubject;
                    
                    cmbPreRequisite1.DataSource = bindingSource1;
                    cmbPreRequisite2.DataSource = bindingSource2;
                    cmbPreRequisite3.DataSource = bindingSource3;
                    cmbPreRequisite4.DataSource = bindingSource4;
                    
                    cmbPreRequisite1.DisplayMember = "SubjectDescription";
                    cmbPreRequisite1.ValueMember = "SubjectCode";
                    cmbPreRequisite2.DisplayMember = "SubjectDescription";
                    cmbPreRequisite2.ValueMember = "SubjectCode";
                    cmbPreRequisite3.DisplayMember = "SubjectDescription";
                    cmbPreRequisite3.ValueMember = "SubjectCode";
                    cmbPreRequisite4.DisplayMember = "SubjectDescription";
                    cmbPreRequisite4.ValueMember = "SubjectCode";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on selecting courseID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (cmbUnit.Items.Contains(editUnit))
            {
                cmbUnit.SelectedItem = editUnit;
            }
            cmbCourse.SelectedValue = editCourse;
            cmbPreRequisite1.SelectedValue = editPreRequisite1;
            cmbPreRequisite2.SelectedValue = editPreRequisite2;
            cmbPreRequisite3.SelectedValue = editPreRequisite3;
            cmbPreRequisite4.SelectedValue = editPreRequisite4;
            txtSubjectCode.ReadOnly = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Update Subject?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    updatesubject.executeSQL("UPDATE tbl_subjects SET SubjectDescription = '" +
                        txtSubjectDescription.Text + "', " +
                        "Unit = '" + cmbUnit.Text + "', " +
                        "Course = '" + cmbCourse.SelectedValue.ToString() + "', " +
                        "PreRequisite1 = '" + cmbPreRequisite1.SelectedValue.ToString() + "', " +
                        "PreRequisite2 = '" + cmbPreRequisite2.SelectedValue.ToString() + "', " +
                        "PreRequisite3 = '" + cmbPreRequisite3.SelectedValue.ToString() + "', " +
                        "PreRequisite4 = '" + cmbPreRequisite4.SelectedValue.ToString() + "' " +
                        "WHERE SubjectCode = '" + txtSubjectCode.Text + "'");
                    if (updatesubject.rowAffected > 0)
                    {
                        MessageBox.Show("Subject Updated!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updatesubject.executeSQL("INSERT INTO tbl_logs VALUES ('" +
                            DateTime.Now.ToString("yyyy-MM-dd") + "', '" +
                            DateTime.Now.ToShortTimeString() +
                            "', 'Updated a student with subjectcode of " + txtSubjectCode.Text + "', '" +
                            username + "', 'Subject Management')");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void frmUpdateSubject_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
