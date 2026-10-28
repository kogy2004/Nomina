using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmEmpleado : Form
    {
        cConexion cn;//variable para llamar a la clase
        SqlCommand cmd;// para llamar los comandos
        SqlDataAdapter da; // para las consultas
        DataTable dt; // como se van a traer los datos
        int i, contador;// var,indice, contadir y boton
        cMedico empleado = new cMedico();

        void llenar(DataTable dt, int i)
        {
            txtDocumento.Text = dt.Rows[i][0].ToString();
            txtNombre.Text = dt.Rows[i][1].ToString();
            txtDireccion.Text = dt.Rows[i][2].ToString();
            txtTelefono.Text = dt.Rows[i][3].ToString();
            txtCargo.Text = dt.Rows[i][4].ToString();
            contador = dt.Rows.Count;
        }
        private void Limpiar()
        {
            txtDocumento.Clear();
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtCargo.Clear();

            txtDocumento.Focus();

        }
        void cargarEmpleado()
        {
            i = 0;
            cn = new cConexion();
            cmd = new SqlCommand("select * from tblEmpleado", cn.Abriconexion());
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt); // llena el dt con los datos de la consulta
            cn.CerrarConexion();
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

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            i = 0;
            llenar(dt, i);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            i--;
            if (i == -1)
            {
                i = 0;
            }
            llenar(dt, i);
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            i++;
            if (i == contador)
            {
                i--;
            }
            llenar(dt, i);
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            i = contador - 1;
            llenar(dt, i);
        }

        private void btnIngreso_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Debe de ingresar el documento del empleado");
                txtDocumento.Focus();
                return;
            }
            empleado.Documento = txtDocumento.Text.Trim();
            empleado.Nombre = txtNombre.Text.Trim();
            empleado.Direccion = txtDireccion.Text.Trim();
            empleado.Telefono = txtTelefono.Text.Trim();
            empleado.Cargo = txtCargo.Text.Trim();

            if (empleado.insertar())
            {
                MessageBox.Show("Empleado ingresado correctamente.", "Ingreso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarEmpleado();

            }
            else
            {
                MessageBox.Show("No fue posible ingresar el empleado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el Documento del Empleado");
                txtDocumento.Focus();
                return;
            }
            empleado.Documento = txtDocumento.Text.Trim();
            if (empleado.Consultar())
            {
                txtDocumento.Text = empleado.Documento;
                txtNombre.Text = empleado.Nombre;
                txtDireccion.Text = empleado.Direccion;
                txtTelefono.Text = empleado.Telefono;
                txtCargo.Text = empleado.Cargo;
            }
            else
            {
                MessageBox.Show("El documento no existe");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el documento del empleado");
                txtDocumento.Focus();
                return;
            }
            empleado.Documento = txtDocumento.Text.Trim();
            empleado.Nombre = txtNombre.Text.Trim();
            empleado.Direccion = txtDireccion.Text.Trim();
            empleado.Telefono = txtTelefono.Text.Trim();
            empleado.Cargo = txtCargo.Text.Trim();
            if (empleado.Modificar())
            {
                MessageBox.Show("Empleado modificado correctamente.", "Modificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se pudo Modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRetiro_Click(object sender, EventArgs e)
        {
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el documento del empleado");
                txtDocumento.Focus();
                return;
            }
            DialogResult respuesta = MessageBox.Show("¿Esta seguro de retirar este empleado?", "Confirmar retiro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.No)
            {
                return;
            }
            empleado.Documento = txtDocumento.Text.Trim();

            if (empleado.Retirar())
            {
                MessageBox.Show("Empleado retirado correctamente", "Retiro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarEmpleado();
                Limpiar();
            }
            else
            {
                MessageBox.Show("No se posible retirar el empleado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public frmEmpleado()
        {
            InitializeComponent();
            cargarEmpleado();
        }
    }
}
