using System;
using System.Windows.Forms;

namespace Nomina.Formularios
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "admin" && txtContrasegna.Text == "1234")
            {
                MessageBox.Show("Bienvendio al Sistema!");
                frmMenu menu = new frmMenu();
                menu.Show();
                timer1.Stop();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o Constraseña incorrectos!");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            MessageBox.Show("Tiempo Excedido");
            this.Close();
        }
    }
}
