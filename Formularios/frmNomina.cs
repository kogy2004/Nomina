using System;
using System.Windows.Forms;

namespace Nomina
{
    public partial class frmNomina : Form
    {
        public frmNomina()
        {
            InitializeComponent();
        }


        private void btnCalcular_Click(object sender, EventArgs e)
        {
            //crear el objeto Empleado
            cMedico empleado = new cMedico();

            empleado.Nombre = txtNombre.Text;
            empleado.SalarioBase = Convert.ToDouble(txtSalarioBase.Text);
            empleado.HorasTrabajadas = Convert.ToInt32(txtHoras.Text);
            empleado.CalcularNeto();

            // llevar los atributos al formulario
            txtExtras.Text = empleado.HorasExtras.ToString("N0");
            txtEps.Text = empleado.Eps.ToString("N0");
            txtSubsidio.Text = empleado.Subsidio.ToString("N0");
            txtDevengado.Text = empleado.Devengado.ToString("N0");
            txtDeducciones.Text = empleado.Deducciones.ToString("N0");
            txtNeto.Text = empleado.Neto.ToString("N0");


        }
    }
}
