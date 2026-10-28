using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmConsulta : Form
    {
        cConexion cn;

        public frmConsulta()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void frmConsulta_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Today;
            cmbCita.DropDownStyle = ComboBoxStyle.DropDownList;
            cargarCitas();
        }
        void cargarCitas()
        {
            string sql = @"SELECT c.IdCita,c.horaCita + ' - ' + p.nombre AS Descripcion, p.nombre AS Paciente, e.nombre AS Medico, e.cargo FROM tblCita c INNER JOIN tblPaciente p ON c.documento = p.documento INNER JOIN tblEmpleado e ON c.documentoM = e.documentoM WHERE TRY_CONVERT(date, c.fechaCita, 103) = @Fecha AND c.estado = 'Pendiente' ORDER BY TRY_CONVERT(time, c.horaCita)";
            try
            {
                SqlCommand comando = new SqlCommand(sql, cn.Abriconexion());
                comando.Parameters.Add("@Fecha", SqlDbType.Date).Value = dtpFecha.Value.Date;

                SqlDataAdapter da = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Primero Display/ValueMember y luego DataSource
                cmbCita.DisplayMember = "Descripcion";
                cmbCita.ValueMember = "IdCita";
                cmbCita.DataSource = dt;
                cmbCita.SelectedIndex = -1;

                limpiarDatosCita();

                if (dt.Rows.Count == 0)
                    lblPaciente.Text = "No hay citas pendientes";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las citas: " + ex.Message);
            }
            finally
            {
                cn.CerrarConexion();
            }
        }
        void limpiarCampos()
        {
            txtMotivo.Clear();
            txtSintomas.Clear();
            txtDiagnostico.Clear();
            txtTratamiento.Clear();
            txtObservaciones.Clear();
            txtPresion.Clear();
            nudPeso.Value = 0;
            nudEstatura.Value = 0;
            nudTemperatura.Value = 0;
            cmbCita.Focus();
        }
        void limpiarDatosCita()
        {
            lblPaciente.Text = "-";
            lblMedico.Text = "-";
            lblEspecialidad.Text = "-";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbCita.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la cita que se va a atender.", "Consulta",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCita.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Escriba el motivo de la consulta.", "Consulta",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
            {
                MessageBox.Show("Escriba el diagnóstico.", "Consulta",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiagnostico.Focus();
                return;

            }
            int idCita = Convert.ToInt32(cmbCita.SelectedValue);

            string sqlInsert = @"INSERT INTO tblConsulta (IdCita, motivo, sintomas, diagnostico, tratamiento,observaciones, peso, estatura, presionArterial, temperatura) VALUES (@IdCita, @motivo, @sintomas, @diagnostico, @tratamiento, @observaciones, @peso, @estatura, @presion, @temperatura)";

            string sqlUpdate = "UPDATE tblCita SET estado = 'Atendida' WHERE IdCita = @IdCita";

            SqlConnection conexion = cn.Abriconexion();
            SqlTransaction transaccion = conexion.BeginTransaction();

            try
            {
                // Guardar la consulta
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, conexion, transaccion);
                cmdInsert.Parameters.Add("@IdCita", SqlDbType.Int).Value = idCita;
                cmdInsert.Parameters.Add("@motivo", SqlDbType.VarChar, 200).Value = txtMotivo.Text.Trim();
                cmdInsert.Parameters.Add("@sintomas", SqlDbType.VarChar, 500).Value = textoONulo(txtSintomas.Text);
                cmdInsert.Parameters.Add("@diagnostico", SqlDbType.VarChar, 300).Value = txtDiagnostico.Text.Trim();
                cmdInsert.Parameters.Add("@tratamiento", SqlDbType.VarChar, 500).Value = textoONulo(txtTratamiento.Text);
                cmdInsert.Parameters.Add("@observaciones", SqlDbType.VarChar, 500).Value = textoONulo(txtObservaciones.Text);
                cmdInsert.Parameters.Add("@peso", SqlDbType.Decimal).Value = numeroONulo(nudPeso.Value);
                cmdInsert.Parameters.Add("@estatura", SqlDbType.Decimal).Value = numeroONulo(nudEstatura.Value);
                cmdInsert.Parameters.Add("@presion", SqlDbType.VarChar, 10).Value = textoONulo(txtPresion.Text);
                cmdInsert.Parameters.Add("@temperatura", SqlDbType.Decimal).Value = numeroONulo(nudTemperatura.Value);
                cmdInsert.ExecuteNonQuery();

                // Actualizar la cita para que quede como atendida
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conexion, transaccion);
                cmdUpdate.Parameters.Add("@IdCita", SqlDbType.Int).Value = idCita;
                cmdUpdate.ExecuteNonQuery();

                // Si las dos instrucciones salieron bien, se confirman
                transaccion.Commit();

                MessageBox.Show("La consulta del paciente quedó registrada.", "Consulta",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Si algo falló, se deshace todo igual que lo hacíamos con la base de datos
                transaccion.Rollback();
                MessageBox.Show("Error al guardar la consulta: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                cn.CerrarConexion();
            }

            limpiarCampos();
            cargarCitas(); // la cita atendida ya no aparece en el combo
        }
        object textoONulo(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? (object)DBNull.Value : texto.Trim();
        }

        // Si el valor es 0 (no se tomó el dato) se guarda NULL
        object numeroONulo(decimal valor)
        {
            return valor == 0 ? (object)DBNull.Value : valor;
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            cargarCitas();
        }

        private void cmbCita_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView fila = cmbCita.SelectedItem as DataRowView;
            if (fila == null)
            {
                limpiarDatosCita();
                return;
            }
            lblPaciente.Text = fila["Paciente"].ToString();
            lblMedico.Text = fila["Medico"].ToString();
            lblEspecialidad.Text = fila["Cargo"].ToString();
        }
    }
}
