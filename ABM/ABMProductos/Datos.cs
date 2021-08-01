using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace ABMProductos
{
    class Datos
    {
        OleDbConnection Conexion = new OleDbConnection();
        OleDbCommand Comando = new OleDbCommand();
        OleDbDataReader Lector;
        string CadenaConexion = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\usuario\Desktop\ABMProductos\DBFProducto.mdb";
        // para sql
        //string CadenaConexion "Data Source=138.99.7.66,1433;Initial Catalog=TUP_PII_2020;User ID=tup_2020;password=tup2020_!@NH";
        public Datos()
        {
            Conexion = new OleDbConnection();
            Conexion.ConnectionString = CadenaConexion;
            Comando = new OleDbCommand();

        }

        public Datos(string cadenaconexion)
        {
            Conexion = new OleDbConnection();
            Comando = new OleDbCommand();
        }
        public string pCadenaConexio { get => CadenaConexion; set => CadenaConexion = value; }
        public OleDbDataReader pLector { get => Lector; set => Lector = value; }

        public void conectar()
        {
            Conexion.Open();
            Comando.Connection = Conexion;
            Comando.CommandType = CommandType.Text;

        }
        public void desconectar()
        {
            Conexion.Close();
        }

        public DataTable consultartabla(string nombreTabla)
        {
            conectar();
            Comando.CommandText = "select  * from " + nombreTabla;
            DataTable tabla = new DataTable();
            tabla.Load(Comando.ExecuteReader());

            desconectar();
            return tabla;
        }
        public void leerTabla(string nombreTabla)
        {
            conectar();

            Comando.CommandText = "select  * from " + nombreTabla;
            Lector = Comando.ExecuteReader();

           

        }
        public void actualizar(string consultaSQL)
        {
            conectar();
            Comando.CommandText = consultaSQL;
            Comando.ExecuteNonQuery();
            desconectar();
        }

    }
}
