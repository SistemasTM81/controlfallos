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
    public partial class materiralTerminado : Form
    {
        validaciones v;
        int empresa, area, idUsuario, idEntrega;
        string folio = "";
        double existencia = 0.0;
        DataTable ds;

        public materiralTerminado(validaciones v, int empresa, int area, int IdUsuario)
        {
            this.v = v;
            InitializeComponent();
            this.empresa = empresa;
            this.area = area;
            this.idUsuario = IdUsuario;
        }
        public materiralTerminado()
        {
            InitializeComponent();
        }

        public void materialTerminado_Load(object sender, EventArgs e)
        {
            ConsultaGenerar(Convert.ToString("where t1.empresa = '" + empresa + "'"));
        }

        public void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Buscar(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFolio.Text))
            {
                ConsultaGenerar("WHERE t1.Folio = '" + txtFolio.Text + "' and t1.empresa = '" + empresa + "'");

            }
            else if (cbFecha.Checked ==true)
            {
                ConsultaGenerar("where  date_format(fechahora, ' %Y-%m-%d) between '" + dtpFechaDe.Value.ToString("yyyy/MM/dd") + "' and '" + dtpFechaA.Value.ToString("yyyy/MM/dd") + "' and t1.empresa = '" + empresa + "'");
            }
        }



        void ConsultaGenerar(string where)
        {
            ds = (DataTable)v.getData("SET lc_time_names = 'es_ES';Select  t2.codrefaccion, t2.nombreRefaccion, t1.cantidad, t1.fechahora, t2.CostoUni from materialproduccion as t1 inner join crefacciones as t2 on t1.refaccionfkcrefacciones = t2.idrefaccion " + where);
            for (int i = 0; i < ds.Rows.Count; i++)
                dgvMaterial.Rows.Add(ds.Rows[i].ItemArray);
            ds.Dispose();
            ds.EndInit();
            dgvMaterial.Columns["cbSelect"].DisplayIndex = 0;
            //dgvMaterial.DataSource = ds.Tables[0];
        }
       

    }
}
