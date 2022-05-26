/*

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Data;


/**PRUEBA EXPORTAR EXCEL**/
/*
namespace controlFallos

{

    class exportar

    {

        StreamWriter w;

        string ruta;

        public string xpath

        {

            get { return ruta; }

            set { value = ruta; }

        }



        public exportar(string path)

        {

            ruta = @path;

        }



        public void Export(ArrayList titulos, DataTable datos)

        {

            try

            {

                FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.ReadWrite);

                w = new StreamWriter(fs);

                string comillas = char.ConvertFromUtf32(34);

                StringBuilder html = new StringBuilder();



                html.Append(@"<html>");

                html.Append(@"<head>");

                html.Append(@"<meta http-equiv=" + comillas + "Content-Type" + comillas + "content=" + comillas + "text/html; charset=utf-8" + comillas + "/>");

                html.Append(@"<title>Actividades</title>");

                html.Append(@"</head>");

                html.Append(@"<body>");

                html.Append(@"<table border = 1>");

                html.Append(@"<tr> <b>");

                foreach (object item in titulos)

                {

                    html.Append(@"<th>" + item.ToString() + "</th>");

                }

                html.Append(@"</b> </tr>");

                for (int i = 0; i < datos.Rows.Count; i++)

                {

                    html.Append(@"<tr>");

                    for (int j = 0; j < datos.Columns.Count; j++)

                    {

                        switch (datos.Rows[i][j].ToString())

                        {

                            case "Detenido por el cliente":

                                html.Append(@"<td bgcolor = Red><font color = White>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "Libre p/ejecutar":

                                html.Append(@"<td bgcolor = Blue><font color = White>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "Pendiente por ROWAN":

                                html.Append(@"<td bgcolor = Black><font color = White>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "Sin comprar":

                                html.Append(@"<td bgcolor = Orange><font color = Black>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "Sin llegar":

                                html.Append(@"<td bgcolor = Yellow><font color = Black>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "Terminado":

                                html.Append(@"<td bgcolor = Green><font color = White>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "En Espera":

                                html.Append(@"<td bgcolor = 8000FF><font color = White>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            case "En proceso":

                                html.Append(@"<td bgcolor = 00FFFF><font color = Black>" + datos.Rows[i][j].ToString() + "</Font></td>");

                                break;

                            default:

                                html.Append(@"<td>" + datos.Rows[i][j].ToString() + "</td>");

                                break;

                        }

                    }

                    html.Append(@"</tr>");

                }

                html.Append(@"</body>");

                html.Append(@"</html>");

                w.Write(html.ToString());

                w.Close();

            }

            catch (Exception ex)

            {

                throw ex;

            }

        }

    }

}
*/