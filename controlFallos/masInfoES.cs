using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
                ConsultaEntradas("where t2.refaccionfkCRefacciones = '" + idRefaccion + "'  and date_format(t2.FechaHora, '%Y') = '2022' and t1.empresa = '" + empresa + "'");
            }
            else
            {
                ConsultaSalidas("where t1.idrefaccion = '" + idRefaccion + "' and date_format(t2.fechaHoraPedido, '%Y') = '2022' and t1.empresa = '" + empresa + "'","where t1.idrefaccion = '" + idRefaccion + "' and date_format(t2.FechaHora, '%Y') = '2022' and t1.empresa = '" + empresa + "'"); 
            }
            
        }
        void ConsultaEntradas(string cadena)
        {
            dt = (DataTable)v.getData("select t1.codrefaccion, t1.nombreRefaccion, t2.CantidadIngresa, t5.Simbolo, concat('$ ', Round(t2.Costo,2)) as 'Costo Unitario', concat('$ ', ROUND((t2.Costo * t2.CantidadIngresa),2)) as SubTotal, concat('$ ', ROUND((t2.Costo * t2.CantidadIngresa) * 0.16,2)) as 'IVA',concat('$ ',ROUND((t2.Costo * t2.CantidadIngresa) + ((t2.Costo * t2.CantidadIngresa) * 0.16) ,2)) as 'Total', t2.proveedor as'PROVEEDOR', t2.FechaHora FROM crefacciones as t1 inner join centradasm as t2 on t1.idrefaccion = t2.refaccionfkCRefacciones left join cmarcas as t3 on t3.idmarca = t1.marcafkcmarcas inner join cfamilias as t4 on t3.descripcionfkcfamilias =t4.idfamilia inner join cunidadmedida as t5 on t5.idunidadmedida = t4.umfkcunidadmedida " + cadena);
            int numFila = dt.Rows.Count;
            dgvEntrada.Visible = true;
                if (numFila > 0)
                {
                    for (int i = 0; i < numFila; i++)
                    {
                        dgvEntrada.Rows.Add(dt.Rows[i].ItemArray);
                    }
                sumar(dgvEntrada);
                }
        }
        void ConsultaSalidas(string cadena1, string cadena2)
        {
            dt = (DataTable)v.getData("SELECT consecutivo ,codrefaccion , nombreRefaccion, CantidadEntregada, CostoUni, ROUND((CostoUni) * (5 /100) + (CostoUni) ,2) as 'Costo Venta',ROUND(((CostoUni ) * (5 /100) + (CostoUni))* CantidadEntregada,2) as 'Total'  FROM (select t4.consecutivo, t1.codrefaccion, t1.nombreRefaccion, t2.CantidadEntregada, t1.CostoUni,ROUND((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) ,2) as 'Costo Venta',ROUND(((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)))* t2.CantidadEntregada,2) as 'Total' from crefacciones as t1 inner join pedidosrefaccion as t2 on t1.idrefaccion = t2.RefaccionfkCRefaccion inner join reportesupervicion as t3 on t3.idReporteSupervicion = t2.FolioPedfkSupervicion inner join cunidades as t4 on t4.idunidad = t3.UnidadfkCUnidades " + cadena1 + " union all select t4.consecutivo, t1.codrefaccion, t1.nombreRefaccion, t2.CantidadEntregada, t1.CostoUni,ROUND((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) ,2) as 'CostoVenta',ROUND(((t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)) * (5 /100) + (t1.CostoUni * (select Costo from ctipocambio where idtipoCambio = t1.tipoMonedafkCTipoCambio)))* t2.CantidadEntregada,2) as 'Total' from crefacciones as t1 inner join ccarrocero as t2 on t1.idrefaccion = t2.refaccionfkCRefacciones inner join cunidades as t4 on t4.idunidad = t2.unidadfkCUnidades " + cadena2 + ") t");
            int numFila = dt.Rows.Count;
            if (numFila > 0)
            {
                dgvSalidas.Visible = true;
                for (int i = 0; i < numFila; i++)
                {
                    dgvSalidas.Rows.Add(dt.Rows[i].ItemArray);
                }
                sumarSalidas(dgvSalidas);
            }
        }

        void sumar(DataGridView data)
        {
            double suma = 0.0;
            foreach (DataGridViewRow row in data.Rows)
            {
                string[] sin_simbolo = row.Cells["toal"].Value.ToString().Split('$');
                if (row.Cells["toal"].Value != null)
                    suma += Convert.ToDouble(sin_simbolo[1].ToString());
            }
            lblCostoTotal.Text = lblCostoTotal.Text + " $ " + suma.ToString("n");
        }
        void sumarSalidas(DataGridView data)
        {
            double suma = 0.0;
            foreach (DataGridViewRow row in data.Rows)
            {
                
                if (row.Cells["toals"].Value != null)
                    suma += Convert.ToDouble(row.Cells["toals"].Value);
            }
            lblCostoTotal.Text = lblCostoTotal.Text + " $ " + suma.ToString("n");
        }

    }
}
