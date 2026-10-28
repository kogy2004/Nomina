
using System.Data;
using System.Data.SqlClient;


namespace Nomina.Clases
{
    internal class cConexion
    {
        //se define la ruta de la base de datos
        static private string CadenaConexion = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\salas\Downloads\Clases (5)\Clases\Nomina\dbClinica.mdf"";Integrated Security=True;Connect Timeout=30";

        //Definir una variable para cargar la base de datos
        private SqlConnection Conexion = new SqlConnection(CadenaConexion);

        // metodo para abrir la base de dato 
        public SqlConnection Abriconexion()
        {
            if (Conexion.State == ConnectionState.Closed)
                Conexion.Open();
            return Conexion;
        }

        //Metodo para cerrar la base de datos

        public SqlConnection CerrarConexion()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();
            return Conexion;

        }
    }
}
