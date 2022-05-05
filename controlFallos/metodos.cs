using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace controlFallos
{
    public class metodos
    {
        DataSet Ds = new DataSet();
        conexion con;
        public MySqlDataAdapter obtener_informacion(string sql)
        {
           
            MySqlCommand comando = new MySqlCommand(sql, con.dbconection());
            MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
           
            return adaptador;
        }

        public void Exportar(DataGridView ds)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add(); 
                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                //Recorremos el DataGridView rellenando la hoja de trabajo
                for ( int i = 0; i < ds.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < ds.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 1, j + 1] = ds.Rows[i].Cells[j].Value.ToString();
                    }
                }
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
            }
        }

        public DataTable obtener_cmb(string consulta)
        {
            DataTable dt = new DataTable();
            MySqlCommand comando = new MySqlCommand(consulta, con.dbconection());
            MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
            adaptador.Fill(dt);
            return dt;
        }
    }
}
