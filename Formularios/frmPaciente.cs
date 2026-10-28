


using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmPaciente : Form
    {
        cConexion conexion;//variable para llamar a la clase
        SqlCommand cmd;// para llamar los comandos
        SqlDataAdapter da; // para las consultas
        DataTable dt; // como se van a traer los datos
        int i, contador;// var,indice, contadir y boton
        cPaciente paciente = new cPaciente();



        public frmPaciente()
        {
            InitializeComponent();
            conexion = new cConexion();
            CargarEps();
            cargarPaciente();

        }
        void CargarEps()
        {
            string sql = "SELECT idEPS, Nombre FROM tblEPS";
            SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion.Abriconexion());
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            cmbEps.DataSource = tabla;
            cmbEps.DisplayMember = "Nombre";
            cmbEps.ValueMember = "idEPS";
            cmbEps.SelectedIndex = -1;

            conexion.CerrarConexion();
        }
        void llenar(DataTable dt, int i)
        {
            txtDocumento.Text = dt.Rows[i][0].ToString();
            txtNombre.Text = dt.Rows[i][1].ToString();
            txtDireccion.Text = dt.Rows[i][2].ToString();
            txtTelefono.Text = dt.Rows[i][3].ToString();
            mtxtFecha.Text = dt.Rows[i][4].ToString();
            cmbSexo.Text = dt.Rows[i][5].ToString();
            txtcontacto.Text = dt.Rows[i][6].ToString();
            txtTipo.Text = dt.Rows[i][7].ToString();
            cmbEps.SelectedValue = dt.Rows[i][8];
            bool estado = Convert.ToBoolean(dt.Rows[i]["Estado"]);
            if (estado)
            {
                rdbActivo.Checked = true;
            }
            else
            {
                rdbInactivo.Checked = false;
            }
            contador = dt.Rows.Count;
        }
        void cargarPaciente()
        {
            i = 0;
            cmd = new SqlCommand("SELECT * FROM tblPaciente", conexion.Abriconexion());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            conexion.CerrarConexion();
            if (dt.Rows.Count > 0)
            {
                llenar(dt, i);
            }
            else
            {
                Limpiar();
                contador = 0;
            }

        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show(
                    "Debe ingresar el documento del paciente."
                );

                txtDocumento.Focus();
                return;
            }

            paciente.Documento = txtDocumento.Text.Trim();
            paciente.Nombre = txtNombre.Text.Trim();
            paciente.Direccion = txtDireccion.Text.Trim();
            paciente.Telefono = txtTelefono.Text.Trim();
            paciente.FechaNacimiento = DateTime.ParseExact(mtxtFecha.Text, "dd/MM/yyyy",
    CultureInfo.InvariantCulture);
            paciente.SexoBiologico = cmbSexo.Text;
            paciente.Contacto = txtcontacto.Text.Trim();
            paciente.TipoSangre = txtTipo.Text.Trim();
            paciente.IdEPS = Convert.ToInt32(cmbEps.SelectedValue);

            if (rdbActivo.Checked)
            {
                paciente.Estado = true;
            }
            else
            {
                paciente.Estado = false;
            }
            if (paciente.Insertar())
            {
                MessageBox.Show("Paciente ingresado correctamente.", "Ingreso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cargarPaciente();
            }
            else
            {
                MessageBox.Show("No fue posible ingresar el paciente.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el documento del paciente.");
                txtDocumento.Focus();
                return;
            }
            paciente.Documento = txtDocumento.Text.Trim();
            if (paciente.Consultar())
            {
                txtDocumento.Text = paciente.Documento;
                txtNombre.Text = paciente.Nombre;
                txtDireccion.Text = paciente.Direccion;
                txtTelefono.Text = paciente.Telefono;
                mtxtFecha.Text = paciente.FechaNacimiento.ToString("dd/MM/yyyy");
                cmbSexo.Text = paciente.SexoBiologico;
                txtcontacto.Text = paciente.Contacto;
                txtTipo.Text = paciente.TipoSangre;
                cmbEps.SelectedValue = paciente.IdEPS;
                if (paciente.Estado == true)
                {
                    rdbActivo.Checked = true;
                }
                else
                {
                    rdbInactivo.Checked = true;
                }
            }
            else
            {
                MessageBox.Show("El documento no existe.");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el documento del paciente.");
                txtDocumento.Focus();
                return;
            }

            paciente.Documento = txtDocumento.Text.Trim();
            paciente.Nombre = txtNombre.Text.Trim();
            paciente.Direccion = txtDireccion.Text.Trim();
            paciente.Telefono = txtTelefono.Text.Trim();
            paciente.FechaNacimiento = Convert.ToDateTime(mtxtFecha.Text);
            paciente.SexoBiologico = cmbSexo.Text;
            paciente.Contacto = txtcontacto.Text.Trim();
            paciente.TipoSangre = txtTipo.Text.Trim();
            paciente.IdEPS = Convert.ToInt32(cmbEps.SelectedValue);
            if (rdbActivo.Checked)
            {
                paciente.Estado = true;
            }
            else
            {
                paciente.Estado = false;
            }

            if (paciente.Modificar())
            {
                MessageBox.Show("Paciente modificado correctamente.", "Modificación", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                cargarPaciente();
            }
            else
            {
                MessageBox.Show("No fue posible modificar el paciente.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnRetiro_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el documento del paciente.");
                txtDocumento.Focus();
                return;
            }

            DialogResult respuesta = MessageBox.Show("¿Está seguro de retirar este paciente?", "Confirmar retiro",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.No)
            {
                return;
            }
            paciente.Documento = txtDocumento.Text.Trim();
            if (paciente.Retirar())
            {
                MessageBox.Show("Paciente retirado correctamente.", "Retiro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarPaciente();
                Limpiar();
            }
            else
            {
                MessageBox.Show("No fue posible retirar el paciente.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void Limpiar()
        {
            txtDocumento.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            mtxtFecha.Clear();
            cmbSexo.SelectedIndex = -1;
            txtcontacto.Clear();
            txtTipo.Clear();
            cmbEps.SelectedIndex = -1;
            rdbActivo.Checked = false;
            rdbInactivo.Checked = false;
            txtDocumento.Focus();


        }

    }
}
