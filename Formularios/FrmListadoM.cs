using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class FrmListadoM : Form
    {
        cConexion conexion;//variable para llamar a la clase
        SqlCommand cmd;// para llamar los comandos
        SqlDataAdapter da; // para las consultas
        DataTable dt; // como se van a traer los datos
        int i, contador;// var,indice, contadir y boton
        private const string MensajeBusqueda = "Nombre o Documento";
        cMedico medico = new cMedico();
        cConexion cn = new cConexion();
        public FrmListadoM()
        {
            InitializeComponent();
            ConsultarMedicos();
            cargarEspecialidad();

        }
        void cargarEspecialidad()
        {
            string sql = "SELECT DISTINCT cargo FROM tblEmpleado";
            SqlDataAdapter adaptador = new SqlDataAdapter(sql, cn.Abriconexion());
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            cmbCargo.DataSource = tabla;
            cmbCargo.ValueMember = "cargo";
            cmbCargo.DisplayMember = "cargo";
            cmbCargo.SelectedIndex = -1;

            cn.CerrarConexion();
        }
        void MostrarBusqueda()
        {
            txtBuscar.Text = MensajeBusqueda;
            txtBuscar.ForeColor = Color.Gray;
        }

        private void FrmListadoM_Load(object sender, EventArgs e)
        {
            cmbCargo.SelectedIndex = 0;
            MostrarBusqueda();
            ConsultarMedicos();
        }
        private void RenombrarColumna(string nombreColumna, string nuevoTitulo)
        {
            if (dgvMedicos.Columns[nombreColumna] != null)
                dgvMedicos.Columns[nombreColumna].HeaderText = nuevoTitulo;

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarMedicos();
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


        private void ConsultarMedicos()
        {
            var textoBusqueda = txtBuscar.Text == MensajeBusqueda ? string.Empty : txtBuscar.Text;

            DataTable dt = medico.Listar(cmbCargo.Text, textoBusqueda);
            dgvMedicos.DataSource = dt;

            RenombrarColumna("Documento", "DocumentoM");
            RenombrarColumna("Nombre", "Nombre");
            RenombrarColumna("Telefono", "Teléfono");
            RenombrarColumna("Cargo", "Cargo");

            lblTotal.Text = $"Total: {dt.Rows.Count} Medico(s)";
        }
    }
}
