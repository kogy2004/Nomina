using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmHistorial : Form
    {

        cConexion cn;
        public frmHistorial()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        private void frmHistorial_Load(object sender, EventArgs e)
        {
            // Opciones disponibles para buscar
            cmbBuscarPor.Items.Clear();
            cmbBuscarPor.Items.Add("Paciente");
            cmbBuscarPor.Items.Add("Médico");
            cmbBuscarPor.SelectedIndex = 0;
            cmbBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;

            // Configuración de la tabla
            dgvHistorial.ReadOnly = true;
            dgvHistorial.AllowUserToAddRows = false;
            dgvHistorial.AllowUserToDeleteRows = false;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorial.RowHeadersVisible = false;

            // Estado inicial
            txtBusqueda.Clear();
            txtDetalle.Clear();
            txtDetalle.ReadOnly = true;
            lblTotal.Text = "Total: 0 consulta(s)";
        }
        void cargarHistorial()
        {
            string busqueda = txtBusqueda.Text.Trim();

            string sql = @"
        SELECT
            co.IdConsulta,
            co.IdCita,
            co.fechaConsulta AS Fecha,
            p.documento AS DocumentoPaciente,
            p.nombre AS Paciente,
            e.documentoM AS DocumentoMedico,
            e.nombre AS Medico,
            e.cargo AS Especialidad,
            co.motivo AS Motivo,
            co.diagnostico AS Diagnostico,
            co.sintomas AS Sintomas,
            co.tratamiento AS Tratamiento,
            co.observaciones AS Observaciones,
            co.peso AS Peso,
            co.estatura AS Estatura,
            co.presionArterial AS Presion,
            co.temperatura AS Temperatura
        FROM tblConsulta co
        INNER JOIN tblCita c
            ON co.IdCita = c.IdCita
        INNER JOIN tblPaciente p
            ON c.documento = p.documento
        INNER JOIN tblEmpleado e
            ON c.documentoM = e.documentoM
        WHERE
            (
                @TipoBusqueda = 'Paciente'
                AND
                (
                    p.documento LIKE @Busqueda
                    OR p.nombre LIKE @Busqueda
                )
            )
            OR
            (
                @TipoBusqueda = 'Médico'
                AND
                (
                    e.documentoM LIKE @Busqueda
                    OR e.nombre LIKE @Busqueda
                )
            )
        ORDER BY co.fechaConsulta DESC, co.IdConsulta DESC";

            try
            {
                SqlCommand comando = new SqlCommand(sql, cn.Abriconexion());

                comando.Parameters.Add(
                    "@TipoBusqueda",
                    SqlDbType.VarChar,
                    20
                ).Value = cmbBuscarPor.Text;

                comando.Parameters.Add(
                    "@Busqueda",
                    SqlDbType.VarChar,
                    150
                ).Value = "%" + busqueda + "%";

                SqlDataAdapter da = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvHistorial.DataSource = dt;

                lblTotal.Text = "Total: " + dt.Rows.Count + " consulta(s)";

                organizarColumnas();

                if (dt.Rows.Count > 0)
                {
                    dgvHistorial.ClearSelection();
                    dgvHistorial.Rows[0].Selected = true;
                    dgvHistorial.CurrentCell = dgvHistorial.Rows[0].Cells["Fecha"];

                    mostrarDetalle(dgvHistorial.Rows[0]);
                }
                else
                {
                    txtDetalle.Clear();

                    MessageBox.Show(
                        "No se encontraron consultas con los datos ingresados.",
                        "Historial",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al consultar el historial: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                cn.CerrarConexion();
            }
        }
        void organizarColumnas()
        {
            string[] columnasOcultas =
            {
        "IdConsulta",
        "IdCita",
        "DocumentoPaciente",
        "DocumentoMedico",
        "Sintomas",
        "Tratamiento",
        "Observaciones",
        "Peso",
        "Estatura",
        "Presion",
        "Temperatura"
    };

            foreach (string nombreColumna in columnasOcultas)
            {
                if (dgvHistorial.Columns.Contains(nombreColumna))
                {
                    dgvHistorial.Columns[nombreColumna].Visible = false;
                }
            }

            if (dgvHistorial.Columns.Contains("Fecha"))
                dgvHistorial.Columns["Fecha"].HeaderText = "Fecha";

            if (dgvHistorial.Columns.Contains("Paciente"))
                dgvHistorial.Columns["Paciente"].HeaderText = "Paciente";

            if (dgvHistorial.Columns.Contains("Medico"))
                dgvHistorial.Columns["Medico"].HeaderText = "Médico";

            if (dgvHistorial.Columns.Contains("Especialidad"))
                dgvHistorial.Columns["Especialidad"].HeaderText = "Especialidad";

            if (dgvHistorial.Columns.Contains("Motivo"))
                dgvHistorial.Columns["Motivo"].HeaderText = "Motivo";

            if (dgvHistorial.Columns.Contains("Diagnostico"))
                dgvHistorial.Columns["Diagnostico"].HeaderText = "Diagnóstico";

            if (dgvHistorial.Columns.Contains("Fecha"))
            {
                dgvHistorial.Columns["Fecha"].DefaultCellStyle.Format =
                    "dd/MM/yyyy hh:mm tt";
            }
        }
        void mostrarDetalle(DataGridViewRow fila)
        {
            string fecha = valorCelda(fila, "Fecha");

            // Si la fecha viene como DateTime, se muestra de forma más clara
            if (fila.Cells["Fecha"].Value != null &&
                fila.Cells["Fecha"].Value != DBNull.Value)
            {
                DateTime fechaConsulta;

                if (DateTime.TryParse(
                    fila.Cells["Fecha"].Value.ToString(),
                    out fechaConsulta))
                {
                    fecha = fechaConsulta.ToString("dd/MM/yyyy hh:mm tt");
                }
            }

            txtDetalle.Text =
                "DATOS DE LA CONSULTA" +
                Environment.NewLine +
                Environment.NewLine +

                "Fecha: " + fecha +
                Environment.NewLine +

                "Paciente: " +
                valorCelda(fila, "DocumentoPaciente") +
                " - " +
                valorCelda(fila, "Paciente") +
                Environment.NewLine +

                "Médico: " +
                valorCelda(fila, "DocumentoMedico") +
                " - " +
                valorCelda(fila, "Medico") +
                Environment.NewLine +

                "Especialidad: " +
                valorCelda(fila, "Especialidad") +
                Environment.NewLine +
                Environment.NewLine +

                "SIGNOS VITALES" +
                Environment.NewLine +

                "Peso: " +
                valorCelda(fila, "Peso") +
                " kg" +
                Environment.NewLine +

                "Estatura: " +
                valorCelda(fila, "Estatura") +
                " m" +
                Environment.NewLine +

                "Presión arterial: " +
                valorCelda(fila, "Presion") +
                Environment.NewLine +

                "Temperatura: " +
                valorCelda(fila, "Temperatura") +
                " °C" +
                Environment.NewLine +
                Environment.NewLine +

                "Motivo:" +
                Environment.NewLine +
                valorCelda(fila, "Motivo") +
                Environment.NewLine +
                Environment.NewLine +

                "Síntomas:" +
                Environment.NewLine +
                valorCelda(fila, "Sintomas") +
                Environment.NewLine +
                Environment.NewLine +

                "Diagnóstico:" +
                Environment.NewLine +
                valorCelda(fila, "Diagnostico") +
                Environment.NewLine +
                Environment.NewLine +

                "Tratamiento:" +
                Environment.NewLine +
                valorCelda(fila, "Tratamiento") +

                Environment.NewLine +

                "Observaciones:" +
                Environment.NewLine +
                valorCelda(fila, "Observaciones");
        }

        string valorCelda(DataGridViewRow fila, string columna)
        {
            object valor = fila.Cells[columna].Value;

            if (valor == null ||
                valor == DBNull.Value ||
                string.IsNullOrWhiteSpace(valor.ToString()))
            {
                return "No registrado";
            }

            return valor.ToString();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            cargarHistorial();
        }

        private void dgvHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                mostrarDetalle(dgvHistorial.Rows[e.RowIndex]);
            }
        }
    }
}
