using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
namespace controlFallos
{

    class databaseLocalClone
    {
        //validaciones v;
        //MySqlConnection localConnection;
        //public databaseLocalClone(validaciones v)
        //{
        //    this.v = v;
        //    localConnection = new MySqlConnection("Server = "+v.c.hostLocal+"; user="+v.c.userLocal+"; password = "+v.c.passwordLocal+" ; database = sistrefaccmant ;port="+v.c.portLocal);
        //    if (File.Exists(Application.StartupPath + @"\updates.srf") && v.c.conexionOriginal())
        //        v.c.insertarGlobal();
        //    CopyTable(new string[] { "bloqueologin", "canaqueles", "careas", "catcategorias", "catincidencias", "cattipos", "ccharolas", "cdescfallo", "cempresas", "cestaciones", "cfallosesp", "cfallosgrales", "cfamilias", "cgiros", "civa", "cladas", "cmarcas", "cmedidas", "cmodelos", "cnfamilias", "cniveles", "comparativas", "cpasillos", "cpersonal", "cproveedores", "crefacciones", "cservicios", "cunidades", "cunidadmedida", "datosistema", "detallesordencompra", "encabezadoreportes", "estatusvalidado", "huellasupervision", "incidenciapersonal", "ladanac", "modificaciones_sistema", "nombresoc", "ordencompra", "pedidosrefaccion", "privilegios", "proveedorescomparativa", "puestos", "refaccionescomparativa", "relacservicioestacion", "reportemantenimiento", "reportepercance", "reportepersonal", "reportesupervicion", "reportetri", "vigencias_supervision", "refacciones_standby" });
        //}
        //void CopyTable(string[] tables)
        //{
        //    foreach (string table in tables)
        //    {
        //        if (table.ToString().Trim() == "datosistema")
        //        {

        //        }
        //        DataTable dt;
        //        if (table.Equals("huellasupervision"))
        //            dt = (DataTable)v.getData("SELECT idHuella, PersonafkCpersonal, convert(template using utf8) AS template, calidad FROM " + table);
        //        else if (table.Equals("reportepercance"))
        //            dt = (DataTable)v.getData("SELECT idreportePercance, consecutivo, ecofkcunidades, conductorfkcpersonal, fechaHoraAccidente, servicioenlaborfkcservicios, lugaraccidente, direccion, estacion1fkcestaciones, estacion2fkcestaciones, estacion3fkcestaciones, estacion4fkcestaciones, ecorecuperacionfkcunidades, estacionfkcestaciones, sintesisocurrido, coordenadasimagenes, descripcion, marcavehiculotercero, yearvehiculotercero, placasvehiculotercero, nombreconductortercero, telefonoconductortercero, domicilioconductortercero, numreporteseguro, horaotorgamiento, horallegadaseguro, nombreajustador, solucion, numacta, supervisorkcpersonal, unidadmedica, perteneceunidad, nombreResponsableunidad, encasolesionados, CONVERT( dibujo USING utf8), comentarios, firmaconductorfkcpersonal, firmasupervisorfkcpersonal, finalizado, usuarioFinalizofkcpersonal, dibujoExportado, fechaHoraInsercion, usuarioinsertofkcpersonal FROM reportepercance");
        //        else
        //            dt = (DataTable)v.getData("Set names 'utf8';SELECT * FROM " + table);
        //        insertarLocal("TRUNCATE TABLE " + table);

        //        string res = "";
        //        foreach (DataRow Rows in dt.Rows)
        //        {
        //            string temp = "";
        //            if (!string.IsNullOrWhiteSpace(res))
        //                res += ",";

        //            foreach (object elements in Rows.ItemArray)
        //            {
        //                if (!string.IsNullOrWhiteSpace(temp)) temp += ",";

        //                if (elements.GetType() == typeof(DateTime))
        //                {
        //                    DateTime dataTime = DateTime.Parse(elements.ToString());
        //                    temp += "'" + dataTime.ToString("yyyy-MM-dd HH:mm") + "'";
        //                }
        //                else
        //                    temp += !string.IsNullOrWhiteSpace(elements.ToString()) ? "'" + elements + "'" : "NULL";
        //            }
        //            temp = "(" + temp + ")";
        //            res += temp;
        //        }
        //        if (!string.IsNullOrWhiteSpace(res)) { res += ";"; insertarLocal("INSERT INTO " + table + " VALUES " + res); }
        //        System.Threading.Thread.Sleep(500);
            
        //    }
        //}
        //bool insertarLocal(string sql)
        //{
        //    try
        //    {
        //        if (localConnection.State != ConnectionState.Open) localConnection.Open();
        //        MySqlCommand cmd = new MySqlCommand(sql, localConnection);
        //        int i = cmd.ExecuteNonQuery();
        //        if (i > 0) return true;
        //        else return false;
        //    }
        //    catch { return false; }
        //}
     
    }
}