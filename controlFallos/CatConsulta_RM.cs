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
    public partial class CatConsulta_RM : Form
    {
        validaciones v;
        string empresas, area, IdUsuario;
        DataSet ds;

        public CatConsulta_RM(validaciones v, string empresa, string area, string IdUsuario)
        {
            this.v = v;
            this.empresas = empresa;
            this.area = area;
            this.IdUsuario = IdUsuario;
            InitializeComponent();
        }
        
        public CatConsulta_RM()
        {
            InitializeComponent();
        }

        public void Loas_C(object sender, EventArgs e)
        {
            Consulta_Reportes();
        }


        void Consulta_Reportes()
        {
            ds = (DataSet)v.getData("select t1.Folio as 'FOLIO',(select concat(t4.identificador,LPAD(consecutivo,4,'0'))) AS 'ECONOMICO', upper(date_format(t1.fechareporte,'%W %d de %M del %Y')) as 'FECHA DE REPORTE',t1.KmEntrada as 'KILOMETRAJE DE UNIDAD',if(t1.tipofallo='1','CORRECTIVO',(if(t1.tipofallo='2','PREVENTIVO',(if(t1.tipofallo='3','REITERATIVO',(if(t1.tipofallo='4','REPROGRAMADO','SEGUIMIENTO'))))))) as 'TIPO DE FALLO', t1.HoraEntrada as 'HORA DEREPORTE',t2.FechaHoraI,'TIEMPO MANTENIMIENTO', t2.FechaHoraT,coalesce(t1.DescFalloNoCod,'') as 'FALLO NO CODIFICADO',(select upper(concat(coalesce(x3.appaterno,''),' ',coalesce(x3.apmaterno,''),' ',coalesce(x3.nombres,''))) from cpersonal as x3 where x3.idpersona=t2.MecanicofkPersonal) as 'MÉCANICO',(select upper(x4.nombreFalloGral) from cfallosgrales as x4 where x4.idFalloGral=t2.FalloGralfkFallosGenerales) as 'GRUPO DE FALLO',if(t2.estatus is null,'',(if(t2.Estatus=1,'EN PROCESO',(if(t2.estatus=2,'REPROGRAMADA','LIBERADA'))))) as 'ESTATUS',coalesce(upper(t2.TrabajoRealizado),'') as 'TRABAJO REALIZADO' from reportesupervicion as t1 left join reportemantenimiento as t2 on t1.idReporteSupervicion=t2.FoliofkSupervicion inner join cunidades as t3 on t1.UnidadfkCUnidades=t3.idunidad INNER JOIN careas AS t4 on t4.idarea=t3.areafkcareas inner join cempresas as T5 on T5.idempresa=T4.empresafkcempresas inner join cmodelos as t6 on t3.modelofkcmodelos = t6.idmodelo  and (t6.empresaMantenimiento = '3' or t6.empresaMantenimiento = '1')  and t1.FechaReporte between left(subdate(now(), interval 1 day),10) and left(now(),10)  order by t1.idReporteSupervicion desc ");
            dgvreportes.DataSource = ds.Tables[0];




        }
    }
}
