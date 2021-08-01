using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Drawing;
using System.Drawing.Text; 
using System.Linq;
using System.Text;
using System.Windows.Forms; 

namespace ABMProductos
{
    public partial class frmProducto : Form
    {
        
        
        Daatos oBD = new Daatos(@"Data Source=DESKTOP-518QF43;Initial Catalog=producto;Integrated Security=True"); // creamos oBD como objeto de la clase datos
        // al pasarle la cadena de conexion acá, estamos usando el constructor con parametros de la clase datos, y asi ingresamos la cadena
        
        List<Producto> lP = new List<Producto>();//lista dinamica

        enum Accion // esto es una "enumeracion" para saber si al hacer click en nuevo tiene q hacer insert o update
        { // la usamos tipo bandera
            nuevo,
            editado
        }
        

        Accion miAccion = Accion.editado; // definimos una instancia de accion y lo ponemos en editado. sólo en boton nuevo cambiamos "miAccion" a nuevo
        public frmProducto()
        {
            InitializeComponent();
            
            
        }
        private void frmProducto_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenido, usted debe elegir entre las opciones NUEVO - EDITAR - BORRAR. De acuerdo a sus preferencias","BIENVENIDO...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Habilitar(false);

            CargarCombo(cboMarca, "marca"); // nombre de la tabla como aparece en la Base de datos "marca"
            CargarLista("producto");
        }
        //--
        private void CargarLista(string nomTabla)
        {
            
            lP.Clear();            
            oBD.LeerTabla(nomTabla);
            while (oBD.Dr.Read()) // read retorna true o false, entonces mientras lea va a dar true y entrar al ciclo
            {
                Producto p = new Producto();
                if (!oBD.Dr.IsDBNull(0))    // valida q no traiga nulos // usamos Dr que es la propiedad del datareader
                    p.Codigo = oBD.Dr.GetInt32(0); // Codigo es la propiedad pCodigo

                if (!oBD.Dr.IsDBNull(1))
                    p.Detalle = oBD.Dr.GetString(1); //1 es el numero de columna segun la tabla en la BD

                if (!oBD.Dr.IsDBNull(2))
                    p.Tipo = oBD.Dr.GetInt32(2);

                if (!oBD.Dr.IsDBNull(3))
                    p.Marca = oBD.Dr.GetInt32(3);

                if (!oBD.Dr.IsDBNull(4))
                    p.Precio = oBD.Dr.GetDouble(4);

                if (!oBD.Dr.IsDBNull(5))
                    p.Fecha = oBD.Dr.GetDateTime(5); // el get varia segun el tipo de dato que hay en la BD
                                
                lP.Add(p);  //a mi lista prod le agrego un producto p ya cargado en cada vuelta del while


            }

            oBD.Dr.Close();
            oBD.Desconectar();
            lstProducto.Items.Clear();

            for (int i = 0; i < lP.Count; i++)  
            {
                lstProducto.Items.Add(lP[i].ShowProducto()); // showproducto es el toString o mostrar de la clase Producto
            }

            lstProducto.SelectedIndex = 0; // para que quede siempre parado en el primer item
        }
        //--
        public void Habilitar(bool j)
        {
            txtCodigo.Enabled = j;
            txtDetalle.Enabled = j;
            cboMarca.Enabled = j;
            rbtNetBook.Enabled = j;
            rbtNoteBook.Enabled = j;
            txtPrecio.Enabled = j;
            dtpFecha.Enabled = j;
            lstProducto.Enabled = j;
            btnGrabar.Enabled = j;
            btnCancelar.Enabled = j;
            btnNuevo.Enabled = !j;
            btnEditar.Enabled = !j;
            btnBorrar.Enabled = !j;
            btnSalir.Enabled = !j;
            btnEliminar.Enabled = j;
        }
        //--
        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtDetalle.Clear();
            cboMarca.SelectedIndex = -1;
            rbtNetBook.Checked = false;
            rbtNoteBook.Checked = false;
            txtPrecio.Clear();
            dtpFecha.Checked = false;
        }
        
        private void CargarCombo(ComboBox combo, string nomTabla)
        {

            DataTable dt = new DataTable();
            //consultarTabla es metodo (que return un datatable) de la clase DATOS. en el evento LOAD llamamos al metodo cargarCombo
            dt = oBD.ConsultarTabla(nomTabla); //  consultarTabla es dnd hacemo el select * // le pasamos el nombre de la tabla por parametro
            combo.DataSource = dt;
            combo.ValueMember = dt.Columns[0].ColumnName; //la primer columna tiene el identity y evitamos errores de llenar mal el nombre de los campos de la tabla de la base de datos
            combo.DisplayMember = dt.Columns[1].ColumnName; // esta tiene la descripcion

            combo.DropDownStyle = ComboBoxStyle.DropDownList; // para q no se pueda escribir o NO EDITABLE

        }
        //--
        private bool ValidarDatos()
        {
            if (txtCodigo.Text == "")
            {
                MessageBox.Show("Debe ingresar Codigo...");
                txtCodigo.Focus();                
                return false;
            }
            if (txtDetalle.Text == "")
            {
                MessageBox.Show("Recuerde ingresar Detalle del produto...");
                txtDetalle.Focus();
                return false;
            }
            if (cboMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una Marca de la lista...");
                cboMarca.Focus();
                return false;
            }
            if (!rbtNetBook.Checked && !rbtNoteBook.Checked)
            {
                MessageBox.Show("Debe Seleccionar Tipo...");
                return false;
            }
            if (txtPrecio.Text == "")
            {
                MessageBox.Show("Recuerde ingresar el Precio...");
                txtPrecio.Focus();
                return false;
            }
            if (!dtpFecha.Checked)
            {
                MessageBox.Show("Debe Seleccionar una Fecha...");
                dtpFecha.Focus();
                return false;
            }
            return true;
        }
        //--
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Proceda a cargar los datos del producto y luego pulse GRABAR o CANCELAR para elegir otra opcion diferente", "Nuevo Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Habilitar(true);
            btnEliminar.Enabled = false;
            LimpiarCampos();
            txtCodigo.Text = Convert.ToString(0); // le colocamos un 0 para q no de error de validacion, pero es autogenerado el codigo del producto
            txtCodigo.Enabled = false; // desactivamos el campo para evitar confusion con la opcion "EDITAR" por parte del usuario
            txtDetalle.Focus();
            lstProducto.Enabled = false;
            miAccion = Accion.nuevo;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Proceda a modificar los datos del producto seleccionandolo en la lista y luego pulse GRABAR o CANCELAR para elegir otra opcion diferente", "Editar Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Habilitar(true);
            btnEliminar.Enabled = false;
            txtCodigo.Enabled = false;
            txtDetalle.Focus();
            miAccion = Accion.editado;

        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            
            Habilitar(false);
            lstProducto.Enabled = true;
            btnEliminar.Enabled = true;
            btnCancelar.Enabled = true;
            btnNuevo.Enabled = false;
            btnEditar.Enabled = false;
            if (lstProducto.SelectedIndex==0)
            {
              
                      
            
                LimpiarCampos();
                MessageBox.Show("Seleccione en la lista el producto que desea eliminar", "Borrar Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                lstProducto.Focus();
                if (lstProducto.SelectedIndex != -1)
                {
                    //
                    
                }
            }
            
            
            //if (MessageBox.Show("Esta seguro de querer borrar este producto de forma permanente?", "BORRANDO...",
            //                                            MessageBoxButtons.YesNo,
            //                                            MessageBoxIcon.Warning,
            //                                            MessageBoxDefaultButton.Button2)
            //                                            == DialogResult.Yes)
            //{
            //    string consultaSQL = $"delete from producto" +
            //                         $" where codigo = {lP[lstProducto.SelectedIndex].Codigo}";
                                
            //    CargarLista("producto");
            //    LimpiarCampos();
            //}
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            string consultaSQL = ""; // declaro esta variable y luego la uso abajo


            if (ValidarDatos()) // si todo esta bien validado ahi creo el producto p 
            {

                Producto p = new Producto(); // dsp de crear el producto p asigno todo a sus propiedades
                p.Codigo = int.Parse(txtCodigo.Text);
                p.Detalle = txtDetalle.Text;
                p.Marca = (int)cboMarca.SelectedValue; // devueve el numero de la posicion
                if (rbtNoteBook.Checked) // si esta seleccionado "notebook" el ptipo es 1
                {
                    p.Tipo = 1;
                }
                else { p.Tipo = 2; } // si no el ptipo es 2
                p.Precio = Convert.ToDouble(txtPrecio.Text);
                p.Fecha = dtpFecha.Value; /// valor del datetimepicker. No se castea porque datetimepicker devuelve un " datetime" y la propiedad fecha es un datetime 

                if (miAccion == Accion.nuevo) // accion funcionando como bandera
                {  // tener en cuenta que si el campo es identity no lo podemos asignar en la consulta, no agregarlo como campo ni valor
                    consultaSQL = $"insert into producto (detalle, tipo, idmarca, precio, fecha)" + // al ser el codigo identity no lo agregamos
                                      $"VALUES ('{p.Detalle}'," + // lleva comilla simple por ser string
                                              $" {p.Tipo}, " +
                                               $"{p.Marca}, " +
                                               $"{p.Precio}, " +
                                              $"'{p.Fecha}')"; // fecha lleva comilla simple

                    oBD.Actualizar(consultaSQL); // la consulta le va por parametro al metodo actualizar de la clase datos
                    MessageBox.Show("El Producto fue añadido con éxito! ", "Nuevo Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarLista("producto"); // nombre de la tabla que va a estar visible mediante un "select" en el listbox
                    MessageBox.Show("Si desea agregar otro producto elija la opción NUEVO nuevamente", "Nuevo Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Habilitar(false);
                    btnEliminar.Enabled = false;
                    btnGrabar.Enabled = false;
                }
                else
                {

                    consultaSQL = $"update producto set detalle = '{p.Detalle}'," + // tener en cuenta q un update la PK no se actualiza asi que no la agrego
                                                      $"tipo = {p.Tipo}," +
                                                      $"idmarca = {p.Marca}," +
                                                      $"precio = {p.Precio}," +
                                                      $"fecha = '{p.Fecha}'" +

                                                      $" where id = {p.Codigo}"; // siempre va where :)

                    oBD.Actualizar(consultaSQL); // se actualiza la BD por la consulta q enviamos por parametro
                    MessageBox.Show("El Producto fue Editado y actualizado con éxito! ", "Editar Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarLista("producto");   // se actualza la lista en el form
                    MessageBox.Show("Si desea editar otro producto elija la opción EDITAR nuevamente", "Editar Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Habilitar(false);
                    btnEliminar.Enabled = false;
                    btnGrabar.Enabled = false;
                }
            }


            //Habilitar(true);
            lstProducto.Enabled = false;

        }


        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Habilitar(false);
            LimpiarCampos();
            miAccion = Accion.editado;
            //lstProducto.SelectedIndex = -1;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            Close();
        }

        private void frmProducto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Esta seguro de querer abandonar el programa?", "SALIENDO",
                                                              MessageBoxButtons.YesNo,
                                                              MessageBoxIcon.Question,
                                                              MessageBoxDefaultButton.Button1) ==
                                                                DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void lstProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCampos(lstProducto.SelectedIndex); // al seleccionar uno de la lista el metodo CargarCampos va a cargar cada campo,
                                                     //con los datos que correspondan al producto
        }
        
        private void CargarCampos(int posicion)
        {
            txtCodigo.Text = lP[posicion].Codigo.ToString();
            txtDetalle.Text = lP[posicion].Detalle;
            cboMarca.SelectedValue = lP[posicion].Marca;
            if (lP[posicion].Tipo == 1)
            {
                rbtNoteBook.Checked = true;
            }
            if (lP[posicion].Tipo == 2)
            {
                rbtNetBook.Checked = true;
            }
            txtPrecio.Text = lP[posicion].Precio.ToString();
            dtpFecha.Value = lP[posicion].Fecha;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string consultaSQL = "";
            if (MessageBox.Show("Esta seguro de querer borrar este producto de forma permanente?", "BORRANDO...",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning,
                                                        MessageBoxDefaultButton.Button2)
                                                        == DialogResult.Yes)
            {
                consultaSQL = $"delete from producto" +
                                    $" where id = {lP[lstProducto.SelectedIndex].Codigo}";

                oBD.Actualizar(consultaSQL); // la consulta le va por parametro al metodo actualizar de la clase datos
                MessageBox.Show("El Producto fue borrado con éxito! ", "borrar Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarLista("producto"); // nombre de la tabla que va a estar visible mediante un "select" en el listbox
                MessageBox.Show("Si desea borrar otro producto elija la opción borrar nuevamente", "Borrar Producto...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Habilitar(false);
            }
        }
    }
}
     

