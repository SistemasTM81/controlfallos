using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class materiralTerminado : Form
    {
        validaciones v;
        int empresa, area, idUsuario, idEntrega;
        string folio = "";
        double existencia = 0.0;
        DataSet ds;

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
            ConsultaGenerar(Convert.ToString(empresa));
        }

        public void txtCodigo_Validated(object sender, EventArgs e)
        {

        }



        void ConsultaGenerar(string where)
        {
            ds = (DataSet)v.getaData("SET lc_time_names = 'es_ES'Select t1.Folio, t2.codrefaccion, t2.nombreRefaccion, t1.cantidad, t1.fechahora, t2.CostoUni from materialproduccion as t1 inner join crefacciones as t2 on t1.refaccionfkcrefacciones = t2.idrefaccion where t1.empresa = '" + empresa + "'");
            dgvMaterial.DataSource = ds.Tables[0];
        }
        
    }
}
