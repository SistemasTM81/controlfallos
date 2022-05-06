using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace controlFallos
{
    public partial class MaterialProduccion : Form
    {
        validaciones v;
        int empresa, area, idUsuario, idEntrega;
        string folio = "";
        double existencia = 0.0;




        public MaterialProduccion(validaciones v, int empresa, int area, int IdUsuario)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = IdUsuario;
            cmbMecanico.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            dtFecha.MaxDate = DateTime.Now;
            CargarMecanico();
        }






        public MaterialProduccion()
        {
            InitializeComponent();
        }

        private void codigo_Validate(object sender, EventArgs e)
        {
            buscaref(txtcodigo.Text);
        }

        private void MaterialP_Load(object sender, EventArgs e)
        {
            obtener_folio();
        }

        private void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbDrawable(object sender, DrawItemEventArgs e)
        {
            v.combos_DrawItem(sender, e);
        }

        private void btn_Guardar(object sender, EventArgs e)
        {
            if (v.materialP(folio, txtcodigo.Text, Double.Parse(txtCantidad.Text), cmbMecanico.SelectedIndex, txtMotivo.Text))
            {
                guardar();
            }
           
        }
        private void nombrealmacen(object sender, EventArgs e)
        {
            obtenerNombre();
        }
        void obtener_folio()
        {
           string consecutivo = v.getFolioP(empresa).ToString();
            if (!string.IsNullOrWhiteSpace(consecutivo))
            {
                folio = "P0-" + (Convert.ToInt32(consecutivo)+1);
            }
            else
            {
                folio = "P0-1";
            }
        }

        public void CargarMecanico()
        {
            cmbMecanico.DataSource = null;
            DataTable dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';SELECT DISTINCT convert(t2.idPersona, char) id,convert(UPPER(CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,''))),char) AS Nombre FROM  cpersonal as t2  where t2.empresa='" + empresa + "' and t2.area = '1'  and t2.cargofkcargos != '2' and t2.status = '1' ORDER BY CONCAT(coalesce(t2.ApPaterno,''), ' ', coalesce(t2.ApMaterno,''), ' ', coalesce(t2.nombres,'')) asc;");
            DataRow nuevaFila = dt.NewRow();
            DataRow nuevaFila2 = dt.NewRow();
            nuevaFila["id"] = 0;
            nuevaFila["Nombre"] = "--SELECCIONE MECANICO--".ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            nuevaFila2["id"] = 8000000;
            nuevaFila2["Nombre"] = "OTRO".ToUpper();
            dt.Rows.InsertAt(nuevaFila2, dt.Rows.Count + 1);
            cmbMecanico.DisplayMember = "id";
            cmbMecanico.ValueMember = "Nombre";
            cmbMecanico.DataSource = dt;

        }
        public void buscaref(string codigo)
        {
            string cadenaR = "";

            cadenaR = v.ObtenerRef("SET lc_time_names = 'es_ES';select convert(nombreRefaccion,char), convert(t2.Simbolo, char), convert(t1.existencias, char) from crefacciones as t1 inner join cmarcas as t3 on t1.marcafkcmarcas = t3.idmarca inner join cfamilias as t4 on t3.descripcionfkcfamilias = t4.idfamilia inner join cunidadmedida as t2 on t4.umfkcunidadmedida = t2.idunidadmedida where t1.codrefaccion = '" + codigo + "' and t1.empresa  = '" + empresa + "' and t1.existencias != 0");
            if (cadenaR.ToString().Equals(""))
            {
                MessageBox.Show("No se encontro la refaccion".ToUpper(), "SIN REPORTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtcodigo.Text = "";
            }
            else
            {
                string[] seprar = cadenaR.Split(';');
                lblNomRef.Text = seprar[0].ToString();
                lblMedida.Text = seprar[1].ToString();
                existencia = Convert.ToDouble(seprar[2].ToString());
            }


        }

        void guardar()
        {
            v.Carroceros("insert into materialproduccion (Folio, refaccionfkcrefacciones, cantidad, fechahora, empresa, almacenfkcpersonal, motivo) values ('" + folio.ToString() + "',(select idrefaccion  from crefacciones where codrefaccion ='" + txtcodigo.Text + "' and empresa = '" + empresa + "'),'" + txtCantidad.Text + "', now(), '" + empresa + "', '" + idEntrega + "','"+ txtMotivo.Text + "')");
        }

        void obtenerNombre()
        {
            MySqlCommand sql = new MySqlCommand("SELECT CONCAT(coalesce(t1.ApPaterno,''),' ',coalesce(t1.ApMaterno,''),' ',coalesce(t1.nombres,'')) AS almacenista, t2.puesto,t1.idPersona as id,t2.idpuesto FROM cpersonal as t1 INNER JOIN puestos AS t2 ON t2.idpuesto=t1.cargofkcargos inner join datosistema as t3 on t3.usuariofkcpersonal =t1.idpersona WHERE t3.password='" + v.Encriptar(txtDispenso.Text) + "' AND  t1.status='1' AND t2.status='1' and t1.empresa='" + empresa + "' ;", v.c.dbconection());
            MySqlDataReader cmd = sql.ExecuteReader();
            v.c.dbconection().Close();
            if (!cmd.Read())
            {
                MessageBox.Show("La contraseña de almacenista ingresada es incorrecta", "CONTRASEÑA INCORRECTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDispenso.Focus();
                txtDispenso.Clear();
            }
            else
            {
                lblNomUsuario.Text = cmd.GetString("Almacenista").ToString();
                idEntrega = Convert.ToInt32(cmd.GetString("id").ToString());
            }
        }
    }
}
