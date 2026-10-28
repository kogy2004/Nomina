using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace Nomina.Clases
{
    public class cPaciente : cPersona
    {
        public string SexoBiologico { get; set; }
        public string Contacto { get; set; }
        public string TipoSangre { get; set; }
        public int IdEPS { get; set; }
        public bool Estado { get; set; }
        public cPaciente()
        {
        }
        public cPaciente(string documento, string nombre, string direccion, string telefono, DateTime fechaNacimiento, string sexoBiologico, string contacto, string tipoSangre, int idEPS, bool estado)
            : base(documento, nombre, direccion, telefono, fechaNacimiento)
        {
            SexoBiologico = sexoBiologico;
            Contacto = contacto;
            TipoSangre = tipoSangre;
            IdEPS = idEPS;
            Estado = estado;
        }
        cConexion conexion = new cConexion();


        // Insertar
        public bool Insertar()
        {
            string sql = @"INSERT INTO tblPaciente VALUES (@Documento, @Nombre, @Direccion, @Telefono,@FechaNacimiento, @SexoBiologico, @Contacto,@TipoSangre, @IdEPS, @Estado)";

            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());

                comando.Parameters.AddWithValue("@Documento", Documento);
                comando.Parameters.AddWithValue("@Nombre", Nombre);
                comando.Parameters.AddWithValue("@Direccion", Direccion);
                comando.Parameters.AddWithValue("@Telefono", Telefono);
                comando.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento);
                comando.Parameters.AddWithValue("@SexoBiologico", SexoBiologico);
                comando.Parameters.AddWithValue("@Contacto", Contacto);
                comando.Parameters.AddWithValue("@TipoSangre", TipoSangre);
                comando.Parameters.AddWithValue("@IdEPS", IdEPS);
                comando.Parameters.AddWithValue("@Estado", Estado);


                int filas = comando.ExecuteNonQuery();

                conexion.CerrarConexion();

                return filas > 0;
            }
            catch (Exception ex)
            {
                conexion.CerrarConexion();

                MessageBox.Show("ERROR AL INGRESAR:\n\n" + ex.Message);

                return false;
            }
        }


        // Método para consultar un paciente
        public bool Consultar()
        {
            string sql = @"SELECT * FROM tblPaciente WHERE Documento = @Documento";

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
                    FechaNacimiento = Convert.ToDateTime(dt.Rows[0]["FechaNacimiento"].ToString());
                    SexoBiologico = dt.Rows[0]["SexoBiologico"].ToString();
                    Contacto = dt.Rows[0]["Contacto"].ToString();
                    TipoSangre = dt.Rows[0]["TipoSangre"].ToString();
                    IdEPS = Convert.ToInt32(dt.Rows[0]["IdEPS"].ToString());
                    if (Convert.ToBoolean(dt.Rows[0]["Estado"]) == true)
                    {
                        Estado = true;
                    }
                    else
                    {
                        Estado = false;
                    }

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                conexion.CerrarConexion();
                return false;
            }
        }

        public bool Modificar()
        {
            string sql = @"UPDATE tblPaciente SET Nombre = @Nombre,Direccion = @Direccion,Telefono = @Telefono,FechaNacimiento=@FechaNacimiento,SexoBiologico=@SexoBiologico,Contacto=@Contacto,TipoSangre=@TipoSangre,IdEPS = @IdEPS, Estado= @Estado WHERE Documento = @Documento";
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());

                comando.Parameters.AddWithValue("@Documento", Documento);
                comando.Parameters.AddWithValue("@Nombre", Nombre);
                comando.Parameters.AddWithValue("@Direccion", Direccion);
                comando.Parameters.AddWithValue("@Telefono", Telefono);
                comando.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento);
                comando.Parameters.AddWithValue("@SexoBiologico", SexoBiologico);
                comando.Parameters.AddWithValue("@Contacto", Contacto);
                comando.Parameters.AddWithValue("@TipoSangre", TipoSangre);
                comando.Parameters.AddWithValue("@IdEPS", IdEPS);
                comando.Parameters.AddWithValue("@Estado", Estado);
                int filas = comando.ExecuteNonQuery();
                conexion.CerrarConexion();
                return filas > 0;
            }
            catch (Exception ex)
            {
                conexion.CerrarConexion();
                MessageBox.Show("ERROR AL MODIFICAR:\n\n" + ex.Message);
                return false;
            }
        }

        public bool Retirar()
        {
            string sql = @"DELETE FROM tblPaciente WHERE Documento = @Documento";
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());
                comando.Parameters.AddWithValue("@Documento", Documento);
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
        public DataTable Listar(bool? estado = null, string textoBusqueda = null)
        {
            string sql = @"SELECT * FROM tblPaciente WHERE (@Estado IS NULL OR Estado = @Estado)                       AND (@Texto IS NULL OR Nombre LIKE '%' + @Texto + '%' OR Documento LIKE '%' + @Texto + '%') ORDER BY Nombre";

            DataTable dt = new DataTable();
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexion.Abriconexion());

                SqlParameter pEstado = comando.Parameters.Add("@Estado", SqlDbType.Bit);
                pEstado.Value = (object)estado ?? DBNull.Value;

                SqlParameter pTexto = comando.Parameters.Add("@Texto", SqlDbType.NVarChar, 100);
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
