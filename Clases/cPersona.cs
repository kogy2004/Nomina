using System;

namespace Nomina.Clases
{
    public class cPersona
    {
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }


        public cPersona()
        {

        }

        public cPersona(string documento, string nombre, string direccion, string telefono, DateTime fechaNacimiento)
        {
            Documento = documento;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
        }

        public int Edad
        {
            get
            {
                int edad = DateTime.Now.Year - FechaNacimiento.Year;

                if (FechaNacimiento.Date > DateTime.Now.AddYears(-edad))
                {
                    edad--;
                }
                return edad;
            }
        }
    }
}
