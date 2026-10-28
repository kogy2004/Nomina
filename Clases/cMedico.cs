using Nomina.Clases;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Nomina
{
    public class cMedico : cPersona
    {

        public double SalarioBase { get; set; }
        public int HorasTrabajadas { get; set; }
        public double HorasExtras { get; private set; }
        public double Subsidio { get; private set; }
        public double Eps { get; private set; }
        public double Devengado { get; private set; }
        public double Deducciones { get; private set; }
        public double Neto { get; private set; }

        public string Cargo { get; set; }
        public cMedico()
        {

        }

        public cMedico(string documento, string nombre, string direccion, string telefono, string cargo, DateTime fechaNacimiento) :
            base(documento, nombre, direccion, telefono, fechaNacimiento)
        {
            Cargo = cargo;
        }

        cConexion conexion = new cConexion();



        //Metodos para calcular nomina
        public void CalcularNeto()
        {
            double SalarioBruto;
            HorasExtras = 0;
            if (HorasTrabajadas > 48)
            {
                HorasExtras = ((HorasTrabajadas - 48) * SalarioBase) * 1.35;
            }
            SalarioBruto = SalarioBase * 48;
            if (SalarioBruto < 500000)
            {
                Subsidio = 65000;
            }
            else
            {
                Subsidio = 0;
            }
            Eps = SalarioBruto * 0.04;
            Devengado = SalarioBruto + HorasExtras + Subsidio;
            Deducciones = Eps;
            Neto = Devengado - Deducciones;
        }

        //Metodo para insertar un empleado
        public bool insertar()
        {
            string sql = @"INSERT INTO tblEmpleado VALUES (@Documento, @Nombre, @Direccion, @Telefono , @Cargo)";

            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());
                comando.Parameters.AddWithValue("@Documento", Documento);
                comando.Parameters.AddWithValue("@Nombre", Nombre);
                comando.Parameters.AddWithValue("@Direccion", Direccion);
                comando.Parameters.AddWithValue("@Telefono", Telefono);
                comando.Parameters.AddWithValue("@Cargo", Cargo);

                int filas = comando.ExecuteNonQuery();
                conexion.CerrarConexion();
                return filas > 0;
            }
            catch (Exception ex)
            {
                conexion.CerrarConexion();
                MessageBox.Show("ERROR AL INGRESAR: \n\n" + ex.Message);
                return false;
            }
        }
        public bool Consultar()
        {
            string sql = @"SELECT * FROM tblEmpleado WHERE Documento = @Documento";

            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());
                comando.Parameters.AddWithValue("@Documento", Documento);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexion.CerrarConexion();

                if (dt.Rows.Count > 0)
                {
                    Documento = dt.Rows[0]["Documento"].ToString();
                    Nombre = dt.Rows[0]["Nombre"].ToString();
                    Direccion = dt.Rows[0]["Direccion"].ToString();
                    Telefono = dt.Rows[0]["Telefono"].ToString();
                    Cargo = dt.Rows[0]["Cargo"].ToString();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                conexion.CerrarConexion();
                return false;
            }
        }
        public bool Modificar()
        {

            string sql = @"UPDATE tblEmpleado SET Nombre = @Nombre,Direccion = @Direccion, Telefono = @Telefono,Cargo = @Cargo WHERE Documento = @Documento";
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());
                comando.Parameters.AddWithValue("@Documento", Documento);
                comando.Parameters.AddWithValue("@Nombre", Nombre);
                comando.Parameters.AddWithValue("@Direccion", Direccion);
                comando.Parameters.AddWithValue("@Telefono", Telefono);
                comando.Parameters.AddWithValue("@Cargo", Cargo);
                int filas = comando.ExecuteNonQuery();
                conexion.CerrarConexion();
                return filas > 0;
            }
            catch (Exception ex)
            {
                conexion.CerrarConexion();
                MessageBox.Show("Error al modificar: " + ex.Message);
                return false;
            }
        }
        public bool Retirar()
        {
            cConexion conexion = new cConexion();
            string sql = @"DELETE FROM tblEmpleado WHERE Documento = @Documento";
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());
                comando.Parameters.AddWithValue("Documento", Documento);
                int filas = comando.ExecuteNonQuery();
                conexion.CerrarConexion();
                return filas > 0;
            }
            catch (Exception)
            {
                conexion.CerrarConexion();
                return false;
            }
        }
        public DataTable Listar(string cargo = null, string textoBusqueda = null)
        {
            string sql = @"SELECT * FROM tblEmpleado WHERE (@Cargo IS NULL OR Cargo = @Cargo) AND (@Texto IS NULL OR Nombre LIKE '%' + @Texto + '%' OR DocumentoM LIKE '%' + @Texto + '%') ORDER BY Nombre";

            DataTable dt = new DataTable();
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());

                SqlParameter pCargo = comando.Parameters.Add("@Cargo", SqlDbType.NVarChar, 50);
                pCargo.Value = (object)cargo ?? DBNull.Value;

                SqlParameter pTexto = comando.Parameters.Add("@Texto", SqlDbType.NVarChar, 50);
                pTexto.Value = string.IsNullOrWhiteSpace(textoBusqueda) ? (object)DBNull.Value : textoBusqueda.Trim();

                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                conexion.CerrarConexion();
                MessageBox.Show("ERROR AL CONSULTAR EL LISTADO:\n\n" + ex.Message);
            }
            return dt;
        }
    }
}
