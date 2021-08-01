using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // agregada para poder trabajar con ADO.net
using System.Data;  //agregada tmb

namespace ABMProductos
{
    class Daatos
    {
        SqlConnection conexion;
        SqlCommand comando;
        SqlDataReader dr; 
        string cadenaConexion;

        public SqlDataReader Dr { get => dr; set => dr = value; } // propiedad
        public string CadenaConexion { get => cadenaConexion; set => cadenaConexion = value; } // propiedad

        public Daatos() // constructor para instanciar y poner null 
        {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.dr = null;
            this.cadenaConexion = null; // minuscula, es el atributo cadenaConexion
        }

        public Daatos(string cadenaConexion) // constructor para pasar la cad conexion. Este es el que elegimos usar en el form
        {
            this.conexion = new SqlConnection();
            this.comando = new SqlCommand();
            this.dr = null;
            this.cadenaConexion = cadenaConexion;
        }
        public void Conectar()
        {
            conexion.ConnectionString = cadenaConexion;
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }
        public void Desconectar()
        {
            conexion.Close();
            conexion.Dispose();  // libera la memoria 
        }

        public DataTable ConsultarTabla(string nomTabla) // metodo q devuelve tipo datatable
        {
            DataTable dt = new DataTable(); // siempre es mejor instanciar el datatable acá
            Conectar(); // le ordenamos que conecte
            comando.CommandText = $"select * from {nomTabla}"; // usa el comandtext que le pasamos aca
            dt.Load(comando.ExecuteReader()); // se usa executeReader xq es un select 
            Desconectar(); // nos desconectamos porque DataTable guarda los datos y nos permite cerrar la conexion ;)
            return dt;
        }

        public void LeerTabla(string nomTabla) // lo hacemos que no devuelva datatable para probar el datareader
        {
            Conectar();
            comando.CommandText = $"select * from {nomTabla}";
            dr = comando.ExecuteReader();  // no cerramos conexion x q necesita q esté abierta el datareader
        }

        public void Actualizar(string strSQL) // recibe un parametro con la consulta sql
        {
            Conectar();
            comando.CommandText = strSQL;
            comando.ExecuteNonQuery(); // ejecute la sentencia sql
            Desconectar();
        }
    }
}
