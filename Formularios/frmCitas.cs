using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmCitas : Form
    {
        cConexion cn;
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dtBusqueda, dtCitasPendientes;

        public frmCitas()
        {
            InitializeComponent();
            cn = new cConexion();
            ActualizarCitasVencidas();

        }

        private void frmCitas_Load(object sender, EventArgs e)
        {
            mntCalendario.MinDate = DateTime.Today;


        }
        bool CargarPaciente(string cedulaC)
        {
            cmd = new SqlCommand("Select nombre from tblPaciente where documento='" + cedulaC + "'", cn.Abriconexion());
            da = new SqlDataAdapter(cmd);
            dtBusqueda = new DataTable();
            da.Fill(dtBusqueda);
            if (dtBusqueda.Rows.Count > 0)
            {
                txtNombre.Text = dtBusqueda.Rows[0][0].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void txtCedula_Leave(object sender, EventArgs e)
        {
            if (!CargarPaciente(txtCedula.Text))
            {
                MessageBox.Show("El paciente no existe.Debe Registrarlo.");
                frmPaciente frm = new frmPaciente();
                frm.Show();
                return;
            }
        }

        private void lstHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstHorario.SelectedItems == null)
                return;

            string fecha = mntCalendario.SelectionStart.ToShortDateString();

            string hora = lstHorario.SelectedItem.ToString();
            CargarMedicosDisponibles(fecha, hora);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("INSERT INTO tblCita VALUES (@documento,@fechaCita,@horaCita,@documentoM,@estado)", cn.Abriconexion());
            cmd.Parameters.AddWithValue("@documento", txtCedula.Text);
            cmd.Parameters.AddWithValue("@fechaCita", mntCalendario.SelectionStart.ToShortDateString());
            cmd.Parameters.AddWithValue("@horaCita", lstHorario.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@documentoM", cmbMedico.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@estado", "Pendiente");
            cmd.ExecuteNonQuery();
            MessageBox.Show("Cita guardada correctamente");
        }
        private void ActualizarCitasVencidas()
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"UPDATE tblCita SET estado = 'Vencida' WHERE CONVERT(date,fechaCita,103)<CAST(GetDate()AS date) AND estado = 'Pendiente'", cn.Abriconexion());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar citas Vencidas"
                    + ex.Message);
            }
            finally
            {
                cn.CerrarConexion();
            }
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                MessageBox.Show("Ingrese la cédula del paciente.");
                txtCedula.Focus();
                return;
            }

            if (!CargarPaciente(txtCedula.Text))
            {
                MessageBox.Show("El paciente no existe.");
                return;
            }

            cmd = new SqlCommand(@"SELECT c.fechaCita, c.horaCita, e.Nombre AS NombreMedico FROM tblCita c INNER JOIN tblEmpleado e ON c.documentoM = e.documentoM WHERE c.documento = @documento AND c.estado = '" + "Pendiente" + "' ORDER BY c.fechaCita, c.horaCita", cn.Abriconexion());
            cmd.Parameters.AddWithValue("@documento", txtCedula.Text);

            da = new SqlDataAdapter(cmd);
            dtCitasPendientes = new DataTable();
            da.Fill(dtCitasPendientes);

            mntCalendario.RemoveAllBoldedDates();

            if (dtCitasPendientes.Rows.Count == 0)
            {
                MessageBox.Show("El paciente no tiene citas pendientes.", "Consulta de citas", MessageBoxButtons.OK, MessageBoxIcon.Information);

                mntCalendario.UpdateBoldedDates();
                lstHorario.Invalidate();
                return;

            }

            var detalleCitas = new StringBuilder();//permite crear cadenas de texo por partes
            detalleCitas.AppendLine($"El paciente tiene {dtCitasPendientes.Rows.Count} cita(s) pendiente(s):");

            foreach (DataRow fila in dtCitasPendientes.Rows)
            {
                //Es una fecha válida?
                if (DateTime.TryParse(fila["fechaCita"].ToString(), out DateTime fecha))
                {
                    //Coloca en negrita la fecha
                    mntCalendario.AddBoldedDate(fecha.Date);
                }
                detalleCitas.AppendLine($"- {fecha:dd/MM/yyyy} {fila["horaCita"]} con {fila["NombreMedico"]}");
            }

            mntCalendario.UpdateBoldedDates(); //Actualiza todas la fechas en negrita

            //Se ubica en la primera fecha de la cita
            //TryParse transforma el texto en una fecha real
            if (DateTime.TryParse(dtCitasPendientes.Rows[0]["fechaCita"].ToString(), out DateTime primeraFecha))
            {
                mntCalendario.SetDate(primeraFecha.Date);
            }

            ColorearHorariosPendientes();

            MessageBox.Show(detalleCitas.ToString());
        }
        private void ColorearHorariosPendientes()
        {
            lstHorario.Invalidate();
        }

        //Este método, se ejecuta automáticamente cada vez que lstHorario necesita dibujar uno de sus ítems.
        private void lstHorario_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            string hora = lstHorario.Items[e.Index].ToString();
            bool pendiente = false;
            if (dtCitasPendientes != null)
            {
                foreach (DataRow fila in dtCitasPendientes.Rows)
                {
                    DateTime fechaCita;
                    if (DateTime.TryParse(fila["fechaCita"].ToString(), out fechaCita))
                    {
                        DateTime fechaSeleccionada = mntCalendario.SelectionStart.Date;
                        string horaCita = fila["horaCita"].ToString();
                        if (fechaCita.Date == fechaSeleccionada && horaCita == hora)
                        {
                            pendiente = true;
                            break;
                        }
                    }
                }
            }

            if (pendiente)
            {
                e.Graphics.FillRectangle(Brushes.LightCoral, e.Bounds);
                e.Graphics.DrawString(hora, e.Font, Brushes.DarkRed, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                e.Graphics.DrawString(hora, e.Font, Brushes.Black, e.Bounds);
            }
            e.DrawFocusRectangle();
        }

        private void mntCalendario_DateChanged(object sender, DateRangeEventArgs e)
        {
            lstHorario.Invalidate();
        }

        private void CargarMedicosDisponibles(string fecha, string hora)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT documentoM, Nombre FROM tblEmpleado WHERE documentoM NOT IN (SELECT documentoM FROM tblCita WHERE fechaCita = @Fecha AND horaCita = @Hora)", cn.Abriconexion());

            cmd.Parameters.AddWithValue("@Fecha", fecha);
            cmd.Parameters.AddWithValue("@Hora", hora);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cmbMedico.DataSource = dt;
                cmbMedico.DisplayMember = "Nombre";
                cmbMedico.ValueMember = "documentoM";
            }
            else
            {
                MessageBox.Show("No quedan médicos disponibles");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCedula.Text) || lstHorario.SelectedItem == null)
            {
                MessageBox.Show("Seleccione el paciente y la hora de la cita que desea cancelar.", "Cancelar cita", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show("¿Está seguro de cancelar esta cita?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
                return;
            string fecha = mntCalendario.SelectionStart.ToString("dd/MM/yyyy");
            string hora = lstHorario.SelectedItem.ToString();
            SqlCommand cmd = new SqlCommand(@"UPDATE tblCita SET estado = 'Cancelada' WHERE documento = @documento AND fechaCita = @fecha AND horaCita  = @hora AND estado <> 'Cancelada'", cn.Abriconexion());

            try
            {
                cmd.Parameters.AddWithValue("@documento", txtCedula.Text.Trim());
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@hora", hora);
                int filas = cmd.ExecuteNonQuery();
                if (filas > 0)
                    MessageBox.Show("La cita fue cancelada.", "Cancelar cita", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    MessageBox.Show("No se encontró una cita activa con esos datos.", "Cancelar cita", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cancelar la cita: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtCedula.Clear();
            txtNombre.Clear();

            mntCalendario.RemoveAllBoldedDates();
            mntCalendario.UpdateBoldedDates();
            dtCitasPendientes = null;

            lstHorario.Invalidate();
            mntCalendario.SetDate(DateTime.Today);
            lstHorario.ClearSelected();
            cmbMedico.DataSource = null;

            txtCedula.Focus();
        }

        private int ObtenerIdCitaSeleccionada()
        {
            if (lstHorario.SelectedItem == null)
                return 0;
            string fechaSeleccionada = mntCalendario.SelectionStart.ToShortDateString();

            string horaSeleccionada = lstHorario.SelectedItem.ToString();

            if (dtCitasPendientes == null)
                return 0;

            foreach (DataRow fila in dtCitasPendientes.Rows)
            {
                if (DateTime.TryParse(fila["fechaCita"].ToString(), out DateTime fechaCita))
                {
                    string horaCita = fila["horaCita"].ToString();
                    if (fechaCita.Date ==
                        mntCalendario.SelectionStart.Date
                        && horaCita == horaSeleccionada)
                    {
                        return Convert.ToInt32(fila["IdCita"]);
                    }
                }
            }
            return 0;
        }

    }
}
