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
    public partial class masInfoES : Form
    {
        string proviene;
        validaciones v;
        int idUsuario, empresa, area, idRefaccion;
        DataTable dt;

        public masInfoES(validaciones v, int idUsuario, int empresa, int area, int idRefaccion, string proviene)
        {
            this.v = v;
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.idRefaccion = idRefaccion;
            this.proviene = proviene;
            InitializeComponent();
        }


        public masInfoES()
        {
            InitializeComponent();
        }

        public void Cerrar(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Load_(object sender, EventArgs e)
        {
            if (proviene.ToString().Equals("Entradas"))
            {
                Consulta("where t2.refaccionfkCRefacciones = '" + idRefaccion + "'  and date_format(t2.FechaHora, '%Y') = '2022' and t1.empresa = '" + empresa + "'");
            }
            else
            {

            }
            
        }
        public void Consulta(string cadena)
        {
            if (proviene.ToString().Equals("Entradas"))
            {
                dt = (DataTable)v.getData("select t1.codrefaccion, t1.nombreRefaccion, t2.CantidadIngresa, t5.Simbolo, t2.Costo, ROUND((t2.Costo * t2.CantidadIngresa),2) as SubTotal, ROUND((t2.Costo * t2.CantidadIngresa) * 0.16,2) as 'IVA',ROUND((t2.Costo * t2.CantidadIngresa) + ((t2.Costo * t2.CantidadIngresa) * 0.16) ,2) as 'Total', t2.proveedor as'PROVEEDOR', t2.FechaHora FROM crefacciones as t1 inner join centradasm as t2 on t1.idrefaccion = t2.refaccionfkCRefacciones left join cmarcas as t3 on t3.idmarca = t1.marcafkcmarcas inner join cfamilias as t4 on t3.descripcionfkcfamilias =t4.idfamilia inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + cadena);
            }
        }

    }
}
