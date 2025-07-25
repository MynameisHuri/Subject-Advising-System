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
    public partial class frmNewSubject : Form
    {
        /// To move the Form
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();
        /// End

        private int errorCount;
        Class1 newsubject = new Class1("DESKTOP-ENQ54KM", "CS311-3B-2023-Database", "huri", "olorosisimo");
        public frmNewSubject()
        {
            InitializeComponent();
        }
        public void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(txtSubjectCode.Text))
            {
                errorProvider1.SetError(txtSubjectCode, "Subject Code is empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtSubjectDescription.Text))
            {
                errorProvider1.SetError(txtSubjectDescription, "Subject Description is empty");
                errorCount++;
            }
            if (cmbUnit.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbUnit, "Select Unit");
                errorCount++;
            }
            if (cmbCourse.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbCourse, "Select Course");
                errorCount++;
            }
            if (cmbPreRequisite1.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbPreRequisite1, "Select PreRequisite1");
            }
            if (cmbPreRequisite2.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbPreRequisite2, "Select PreRequisite2");
            }
            if (cmbPreRequisite3.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbPreRequisite3, "Select PreRequisite3");
            }
            if (cmbPreRequisite4.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbPreRequisite4, "Select PreRequisite4");
            }
            try
            {
                DataTable dt = newsubject.GetData("SELECT * FROM tbl_subjects WHERE SubjectCode = '" + txtSubjectCode.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtSubjectCode, "Subject Code is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validate exist SubjectCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmNewSubject_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtcourse = newsubject.GetData("SELECT * FROM tbl_course ORDER BY courseDescription");
                if (dtcourse.Rows.Count > 0)
                {
                    cmbCourse.DataSource = dtcourse;
                    cmbCourse.DisplayMember = "courseDescription";
                    cmbCourse.ValueMember = "courseID";
                }
                DataTable dtSubject = newsubject.GetData("SELECT SubjectCode, SubjectDescription FROM tbl_subjects ORDER BY SubjectDescription");
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                try
                {
                    newsubject.executeSQL("INSERT INTO tbl_subjects VALUES ('" + txtSubjectCode.Text + "', '" + txtSubjectDescription.Text + "', " +
                        "'" + cmbUnit.Text + "', '" + cmbCourse.SelectedValue.ToString() + "', " +
                        "'" + cmbPreRequisite1.SelectedValue.ToString() + "', " + 
                        "'" + cmbPreRequisite2.SelectedValue.ToString() + "', " +
                        "'" + cmbPreRequisite3.SelectedValue.ToString() + "', " +
                        "'" + cmbPreRequisite4.SelectedValue.ToString() + "')");
                    if (newsubject.rowAffected > 0)
                    {
                        MessageBox.Show("New subject added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on save button", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSubjectCode.Clear();
            txtSubjectDescription.Clear();
            cmbUnit.SelectedIndex = -1;
            cmbCourse.SelectedIndex = -1;
            cmbPreRequisite1.SelectedIndex = 0;
            cmbPreRequisite2.SelectedIndex = 0;
            cmbPreRequisite3.SelectedIndex = 0;
            cmbPreRequisite4.SelectedIndex = 0;
            txtSubjectCode.Focus();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmNewSubject_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
