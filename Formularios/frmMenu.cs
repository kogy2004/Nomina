using System;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmMenu : Form

    {
        private Form activeForm = null;
        public frmMenu()
        {
            InitializeComponent();
            disenoPersonal();
        }
        void disenoPersonal()
        {
            pnlPaciente.Visible = false;
            pnlMedico.Visible = false;
            pnlCitas.Visible = false;

        }
        void ocultarSubmenu()
        {
            if (pnlPaciente.Visible)
                pnlPaciente.Visible = false;
            if (pnlMedico.Visible)
                pnlMedico.Visible = false;
            if (pnlCitas.Visible)
                pnlCitas.Visible = false;
        }
        void mostrarSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                ocultarSubmenu();
                submenu.Visible = true;
            }
        }
        private void AbrirenPanel(Form frmHijo)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = frmHijo;
            frmHijo.TopLevel = false;
            frmHijo.FormBorderStyle = FormBorderStyle.None;
            frmHijo.Dock = DockStyle.Fill;
            pnlCentral.Controls.Add(frmHijo);
            pnlCentral.Tag = frmHijo;
            frmHijo.BringToFront();
            frmHijo.Show();
        }

        private void btnPaciente_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlPaciente);
        }

        private void btnIngresP_Click(object sender, EventArgs e)
        {
            AbrirenPanel(new frmPaciente());
            ocultarSubmenu();
        }

        private void btnListadoP_Click(object sender, EventArgs e)
        {
            AbrirenPanel(new frmListadoP());
            ocultarSubmenu();
        }

        private void btnCita_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlCitas);
        }

        private void btnMedico_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(pnlMedico);
        }

        private void btnListadoM_Click(object sender, EventArgs e)
        {
            AbrirenPanel(new FrmListadoM());
            ocultarSubmenu();
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            AbrirenPanel(new frmHistorial());
            ocultarSubmenu();
        }

        private void btnIngresoM_Click(object sender, EventArgs e)
        {
            AbrirenPanel(new frmEmpleado());
            ocultarSubmenu();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirenPanel(new frmConsulta());
            ocultarSubmenu();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
