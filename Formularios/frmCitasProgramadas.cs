using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmCitasProgramadas : Form
    {

        cConexion cn;
        public frmCitasProgramadas()
        {
            InitializeComponent();
            cn = new cConexion();
        }

        void consultaCitas()
        {
            string sql = @"SELECT c.fechaCita, c.horaCita, p.documento, p.nombre AS Paciente,e.nombre AS Medico, e.cargo FROM tblCita c INNER JOIN tblPaciente p ON c.documento = p.documento INNER JOIN tblEmpleado e ON c.documentoM = e.documentoM WHERE TRY_CONVERT(date, c.fechaCita, 103) = @Fecha AND c.estado = 'Pendiente' ORDER BY TRY_CONVERT(time, c.horaCita)";
            try
            {
                SqlCommand comando = new SqlCommand(sql, cn.Abriconexion());
                comando.Parameters.Add("@Fecha", SqlDbType.Date).Value = mntFecha.SelectionStart.Date;

                SqlDataAdapter da = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //Operador ternario: variable = condición ? valorSiEsVerdadero : valorSiEsFalso;
                lblMensaje.Text = dt.Rows.Count == 0 ? "No hay citas pendientes\npara esta fecha" : dt.Rows.Count + "\ncita(s) pendiente(s)";
                //if (dt.Rows.Count == 0)
                //    lblMensaje.Text = "No hay citas pendientes\npara esta fecha";
                //else
                //    lblMensaje.Text = dt.Rows.Count + "\ncita(s) pendiente(s)";

                dgvCitasProgramadas.DataSource = dt;
                formatoColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar las citas: " + ex.Message);
            }
            finally
            {
                cn.CerrarConexion();
            }
        }

        private void frmCitasProgramadas_Load(object sender, EventArgs e)
        {
            consultaCitas();
            configurarGrid();
        }

        void configurarGrid()
        {
            dgvCitasProgramadas.DefaultCellStyle.Font = new Font("Comic Sans MS", 8);
            dgvCitasProgramadas.EnableHeadersVisualStyles = false;
            dgvCitasProgramadas.ColumnHeadersDefaultCellStyle.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);
            dgvCitasProgramadas.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvCitasProgramadas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCitasProgramadas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCitasProgramadas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCitasProgramadas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgvCitasProgramadas.ReadOnly = true;
            dgvCitasProgramadas.AllowUserToAddRows = false;
            dgvCitasProgramadas.RowHeadersVisible = false;
            dgvCitasProgramadas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCitasProgramadas.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }
        void formatoColumnas()
        {
            dgvCitasProgramadas.Columns["fechaCita"].HeaderText = "Fecha";
            dgvCitasProgramadas.Columns["fechaCita"].Width = 80;
            dgvCitasProgramadas.Columns["fechaCita"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCitasProgramadas.Columns["horaCita"].HeaderText = "Hora";
            dgvCitasProgramadas.Columns["horaCita"].Width = 50;
            dgvCitasProgramadas.Columns["horaCita"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCitasProgramadas.Columns["documento"].HeaderText = "Documento";
            dgvCitasProgramadas.Columns["documento"].Width = 80;
            dgvCitasProgramadas.Columns["Paciente"].Width = 150;
            dgvCitasProgramadas.Columns["Medico"].HeaderText = "Médico";
            dgvCitasProgramadas.Columns["Medico"].Width = 150;
            dgvCitasProgramadas.Columns["cargo"].HeaderText = "cargo";
            dgvCitasProgramadas.Columns["cargo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void mntFecha_DateSelected(object sender, DateRangeEventArgs e)
        {
            consultaCitas();
        }
    }
}
