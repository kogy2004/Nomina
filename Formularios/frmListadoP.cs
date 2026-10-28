using Nomina.Clases;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmListadoP : Form
    {
        private const string MensajeBusqueda = "Nombre o Documento";
        cPaciente paciente = new cPaciente();
        public frmListadoP()
        {
            InitializeComponent();
        }
        void MostrarBusqueda()
        {
            txtBuscar.Text = MensajeBusqueda;
            txtBuscar.ForeColor = Color.Gray;

        }
        private void OcultarColumna(string nombreColumna)
        {
            if (dgvPacientes.Columns[nombreColumna] != null)
                dgvPacientes.Columns[nombreColumna].Visible = false;
        }
        private void RenombrarColumna(string nombreColumna, string nuevoTitulo)
        {
            if (dgvPacientes.Columns[nombreColumna] != null)
                dgvPacientes.Columns[nombreColumna].HeaderText = nuevoTitulo;
        }

        private void ConsultarPacientes()
        {
            bool? estado = null;
            if (cmbEstado.SelectedIndex == 1)
            {
                estado = true;
            }
            else if (cmbEstado.SelectedIndex == 2)
            {
                estado = false;
            }

            var textoBusqueda = txtBuscar.Text == MensajeBusqueda ? string.Empty : txtBuscar.Text;

            DataTable dt = paciente.Listar(estado, textoBusqueda);

            dgvPacientes.DataSource = dt;
            OcultarColumna("IdEPS");
            OcultarColumna("Direccion");
            OcultarColumna("Contacto");

            RenombrarColumna("Documento", "Documento");
            RenombrarColumna("Nombre", "Nombre");
            RenombrarColumna("Telefono", "Teléfono");
            RenombrarColumna("SexoBiologico", "Sexo");
            RenombrarColumna("TipoSangre", "Tipo de sangre");
            RenombrarColumna("Estado", "Activo");

            if (dgvPacientes.Columns["FechaNacimiento"] != null)
            {
                dgvPacientes.Columns["FechaNacimiento"].HeaderText = "Fecha nacimiento";
                dgvPacientes.Columns["FechaNacimiento"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            lblTotal.Text = $"Total: {dt.Rows.Count} paciente(s)";
        }

        private void frmListadoP_Load(object sender, EventArgs e)
        {
            cmbEstado.SelectedIndex = 0;
            MostrarBusqueda();
            ConsultarPacientes();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarPacientes();

        }

        private void txtBuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MostrarBusqueda();
            }
        }

        private void txtBuscar_Enter(object sender, EventArgs e)
        {
            if (txtBuscar.Text == MensajeBusqueda && txtBuscar.ForeColor == Color.Gray)
            {
                txtBuscar.Text = string.Empty;
                txtBuscar.ForeColor = SystemColors.WindowText;
            }
        }

    }

}
