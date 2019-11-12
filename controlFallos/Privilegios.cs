using MySql.Data.MySqlClient;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class Privilegios : Form
    {
        conexion c = new conexion();
        validaciones v = new validaciones();
        int idUsuario;
        int empresa,area;
        int total = 0;
        bool editar = false, res;

        public Privilegios(int idUsuario, int empresa,int area)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;

            rellenar();
            yaExiste();
            buscarNombre();
        }
        void buscarNombre()
        {
            string sql = "SELECT CONCAT(apPaterno,' ',apMaterno,' ',nombres) as Nombre FROM cpersonal WHERE idPersona ='"+idUsuario+"'";
            MySqlCommand cmd = new MySqlCommand(sql,c.dbconection());
            lbltitle.Text = "Asignar Privilegios a: "+cmd.ExecuteScalar();
            c.dbconection().Close();
       
        }
        private void button1_Click(object sender, EventArgs e)
        {

            Dispose();
            Close();

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender,e,this);
        }

        public void rellenar()
        {
            if (this.empresa == 1)
            {
                tbprivilegios.Rows.Add("1", "Catálogo de Areas", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catAreas");
                tbprivilegios.Rows.Add("2", "Catálogo de Personal", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPersonal");
                tbprivilegios.Rows.Add("3", "Catálogo de Empresas", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catEmpresas");
                tbprivilegios.Rows.Add("4", "Catálogo de Puestos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPuestos");
                tbprivilegios.Rows.Add("5", "Catálogo de Servicios", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catServicios");
                tbprivilegios.Rows.Add("6", "Catálogo de Unidades", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catUnidades");
                tbprivilegios.Rows.Add("7", "Reporte Supervisión", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "Form1");
                total = tbprivilegios.Rows.Count;
                
            }
            else if (this.empresa == 2)
            {
                if (area==1)
                {
                    tbprivilegios.Rows.Add("1", "Catálogo de Fallos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catfallosGrales");
                    tbprivilegios.Rows.Add("2", "Catálogo de Personal", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPersonal");
                    tbprivilegios.Rows.Add("3", "Catálogo de Puestos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPuestos");
                    tbprivilegios.Rows.Add("4", "Catálogo de Unidades", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catUnidades");
                   tbprivilegios.Rows.Add("5", "Reporte Mantenimiento", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "Mantenimiento");
                    total = tbprivilegios.Rows.Count;
                }
                else if (area==2)
                {
                    tbprivilegios.Rows.Add("1", "Catálogo de Personal", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPersonal");
                    tbprivilegios.Rows.Add("2", "Catálogo de Proveedores", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catProveedores");
                    tbprivilegios.Rows.Add("3", "Catálogo de Puestos", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catPuestos");
                    tbprivilegios.Rows.Add("4", "Catálogo de Refacciones", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "catRefacciones");
                    tbprivilegios.Rows.Add("5", "Generación de Requisiciones", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "ordencompra");
                    tbprivilegios.Rows.Add("6", "Reporte Almacén", true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "Almacen");
                    total = tbprivilegios.Rows.Count;
                }

               
                
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < total; x++)
            {

                for (int y = 2; y < 7; y++)
                {

                    tbprivilegios.Rows[x].Cells[y].Value = false.ToString();

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                insertar();
            }
            else
            {
                editarprivilegios();
            }
            tbprivilegios.ClearSelection();
            yaExiste();
          
            this.Close();
        }
        public void insertar()
        {
           
            string[,] privilegios = new string[total, 8];
            for (int x = 0; x < total; x++)
            {
                for (int y = 0; y < tbprivilegios.Rows[0].Cells.Count; y++)
                {

                    if (y == 2)
                    {
                        bool res = this.res = Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value);
                        if (!res)
                        {
                            for (int y1 = 3; y1 < 7; y1++)
                            {

                                tbprivilegios.Rows[x].Cells[y1].Value = false.ToString();

                            }
                        }

                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[3].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[4].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[5].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[6].Value))
                        {

                            tbprivilegios.Rows[x].Cells[y].Value = false;
                        }
                    }
                    if (y == 4)
                    {
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value))
                        {
                            tbprivilegios.Rows[x].Cells[y + 1].Value = false.ToString();
                            tbprivilegios.Rows[x].Cells[y + 2].Value = false.ToString();
                        }
                    }
                    privilegios[x, y] = tbprivilegios.Rows[x].Cells[y].Value.ToString();                   
                }

            }
            if (this.res)
           {

                try
                {
                    for (int y = 0; y < total; y++)
                    {
                  
                        string _ver = privilegios[y, 2].ToLower();
                        string _insertar = privilegios[y, 3].ToLower();
                        string _consultar = privilegios[y, 4].ToLower();
                        string _editar = privilegios[y, 5].ToLower();
                        string _desactivar = privilegios[y, 6].ToLower();
                        string _pClave = privilegios[y, 7];
                        String sql = @"INSERT INTO privilegios (usuariofkcpersonal,namform,ver,insertar,consultar,editar,desactivar) VALUES('";
                        sql += this.idUsuario + "','";
                        sql += _pClave + "','";
                        sql += v.getIntFrombool(Convert.ToBoolean(_ver)) + "',";
                        sql += "'" + v.getIntFrombool(Convert.ToBoolean(_insertar)) + "','";
                        sql += v.getIntFrombool(Convert.ToBoolean(_consultar)) + "','";
                        sql += v.getIntFrombool(Convert.ToBoolean(_editar));
                        sql += "','" + v.getIntFrombool(Convert.ToBoolean(_desactivar)) + "')";

                        bool res = v.c.insertar(sql);

                    }
                    MessageBox.Show("Se han Asignado los privilegios Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           else
            {
               MessageBox.Show("No Se Asignaron Privilegios Vacíos", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

               Close();
           }
        }

        public void editarprivilegios()
        {
            this.res = false;
            string[,] privilegios = new string[total, 8];
            for (int x = 0; x < total; x++)
            {

                for (int y = 0; y < 8; y++)
                {
                    if (y == 2)
                    {
                        this.res =Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value);
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value))
                        {
                            for (int y1 = 3; y1 < 7; y1++)
                            {

                                tbprivilegios.Rows[x].Cells[y1].Value = false.ToString();

                            }
                        }
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[3].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[4].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[5].Value) && !Convert.ToBoolean(tbprivilegios.Rows[x].Cells[6].Value))
                        {
                            tbprivilegios.Rows[x].Cells[y].Value = false;
                        }
                    }
                    if (y == 4)
                    {
                        if (!Convert.ToBoolean(tbprivilegios.Rows[x].Cells[y].Value))
                        {
                            tbprivilegios.Rows[x].Cells[y + 1].Value = false.ToString();
                            tbprivilegios.Rows[x].Cells[y + 2].Value = false.ToString();
                        }
                    }

                    privilegios[x, y] = tbprivilegios.Rows[x].Cells[y].Value.ToString();


                }
            }
            if (this.res)
            {


                try
                {
                    for (int y = 0; y < total; y++)
                    {
                        conexion c = new conexion();
                        String sql = @"UPDATE privilegios SET ver ='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 2])) + "',insertar ='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 3])) +
                                     "',consultar='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 4])) + "',editar='" + v.getIntFrombool(Convert.ToBoolean(privilegios[y, 5])) + "',desactivar ='" +
                                     v.getIntFrombool(Convert.ToBoolean(privilegios[y, 6])) + "' WHERE idprivilegio = " + privilegios[y, 0] + " and usuariofkcpersonal = " + this.idUsuario;
                        v.c.insertar(sql);

                    }

                    MessageBox.Show("Se han Actualizado los Privilegios Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }else
            {
                MessageBox.Show("No se Actualizaron Los Privilegios", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }

        }
        private void tbprivilegios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int row = e.RowIndex;
            var res = tbprivilegios.Rows[row].Cells[2].Value;
            if (e.ColumnIndex == 2)
            {

                tbprivilegios.Rows[row].Cells[6].Value = tbprivilegios.Rows[row].Cells[5].Value = tbprivilegios.Rows[row].Cells[4].Value = tbprivilegios.Rows[row].Cells[3].Value = tbprivilegios.Rows[row].Cells[2].Value;

              
            }
        }

        private void Privilegios_Load(object sender, EventArgs e)
        {
            if (yaExistePrivilegiosUsuario())
            {
                label1.Text = "Actualizar Privilegios";
            }else
            {
                label1.Text = "Asignar Privilegios";
            }
        }

        public void yaExiste()
        {
            if (yaExistePrivilegiosUsuario()) { 
                editar = true;
                int contadorIndice = 0;
                string con = "SELECT idprivilegio,ver,insertar,consultar,editar,desactivar FROM sistrefaccmant.privilegios WHERE usuariofkcpersonal='" + idUsuario + "' ORDER BY idprivilegio ASC";
                MySqlCommand mc = new MySqlCommand(con, c.dbconection());
                MySqlDataReader mdr = mc.ExecuteReader();
                while (mdr.Read())
                {
                    tbprivilegios.Rows[contadorIndice].Cells[0].Value = mdr.GetInt32("idprivilegio").ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[2].Value = v.getBoolFromInt(mdr.GetInt32("ver")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[3].Value = v.getBoolFromInt(mdr.GetInt32("insertar")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[4].Value = v.getBoolFromInt(mdr.GetInt32("consultar")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[5].Value = v.getBoolFromInt(mdr.GetInt32("editar")).ToString();
                    tbprivilegios.Rows[contadorIndice].Cells[6].Value = v.getBoolFromInt(mdr.GetInt32("desactivar")).ToString();
                    contadorIndice++;
                    }
                mdr.Close();
                v.c.dbcon.Close();
            }
            else
            {
                v.c.dbcon.Close();
            }
        }
        bool yaExistePrivilegiosUsuario()
        {
            String sql = "SELECT count(idprivilegio) as cuenta FROM privilegios WHERE usuariofkcpersonal = '" + this.idUsuario + "'";
            MySqlCommand cm = new MySqlCommand(sql, c.dbconection());
           bool i = Convert.ToInt32(cm.ExecuteScalar()) > 0;
            c.dbconection().Close();
            return i ;
        }
    }
}
