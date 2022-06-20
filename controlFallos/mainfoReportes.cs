using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace controlFallos
{
    public class mainfoReportes
    {
        validaciones v = new validaciones();
        public DataTable obtener_eportes(string buscar)
        {
            DataTable dt;
            dt = (DataTable)v.getData("SET lc_time_names = 'es_ES';select(select concat(t4.identificador, LPAD(consecutivo, 4, '0'))) AS 'ECO', upper(Date_format(t1.FechaReporte, '%W %d de %M del %Y')) as 'FECHA DEL REPORTE', t1.KmEntrada as 'KILOMETRAJE DE REPORTE', UPPER(t1.DescFalloNoCod) as 'DESCRIPCIÓN DE FALLO NO CODIFICADO' from reportesupervicion as t1 inner join cunidades as t2 on t1.UnidadfkCUnidades = t2.idunidad INNER JOIN careas AS t4 on t4.idarea = t2.areafkcareas inner join cempresas as T5 on T5.idempresa = T4.empresafkcempresas " + buscar);
            return dt;
        }
    }
}
