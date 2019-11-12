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
    public partial class mostrarFolio : Form
    {
        string folio;
        validaciones v = new validaciones();
        object empresa, area;
        public mostrarFolio(string folio,object empresa, object area)
        {
            InitializeComponent();
            this.folio = folio;
            this.empresa = empresa;
            this.area = area;
        }
        void paralabelsImpares(string[] arreglo)
        {

            int x = 40;
            int y = 0, c = 0;

            for (int i = 0; i < arreglo.Length; i++)
            {
               
                Label l = new Label();
                l.UseMnemonic = false;
                if (arreglo[i].Length > 80)
                {
              
                    string temp2="";
                    string temp = "";
                    string[] temp4 = arreglo[i].Split(' ');
                    for (int j = 0; j < temp4.Length; j++)
                    {
                        if ((temp + " " + temp4[j]).Length < 80)
                        {
                            temp += " " + temp4[j];
                        }
                        else
                        {
                            temp2 += temp + Environment.NewLine;
                            temp = "";
                            j--;
                            c += 18;
                        }
                    }
                    if (temp!= null)
                    {
                        temp2 += temp;
                        temp = null;
                    }
                    l.Text = v.mayusculas(temp2);
                
                }else
                {
                    l.Text = v.mayusculas(arreglo[i]);
                }

                    l.AutoSize = true;
                    l.Location = new Point(x, y);
                    l.Font = new Font(this.Font, FontStyle.Bold);
                    l.Name = "lbl" + i;
                    l.TabIndex = i;

                    gbadd.Controls.Add(l);

                    l.Left = (this.gbadd.Width - l.Size.Width) / 2;
                y += 40+c;
                c = 0;
            }

            
        }

        private void mostrarFolio_Load(object sender, EventArgs e)
        {
            lbltitle.Text = "Información de Reporte Con Folio: "+folio;
            lbltitle.Left = (this.panel1.Width - lbltitle.Size.Width) / 2;
            lbltitle.Top = (panel1.Height - lbltitle.Size.Height) / 2;
            object res = null;
            if (Convert.ToInt32(empresa)==1)
            {
                res = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('Folio: ',t2.Folio,';',(SELECT concat('ECO: ',ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t2.UnidadfkCUnidades),'; Fecha / Hora: ', CONCAT(DATE_FORMAT(t2.FechaReporte, '%W, %d de %M del %Y'),' / ',t2.HoraEntrada),';Supervisor: ', (SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t2.SupervisorfkCPersonal),';Conductor: ',(SELECT CONCAT(credencial,': ',nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona=t2.CredencialConductorfkCPersonal),';Servicio: ',(select concat(nombre,': ',descripcion) FROM cservicios WHERE idservicio=t2.Serviciofkcservicios),';Hora de Entrada: ',t2.HoraEntrada,';Kilometraje: ',t2.KmEntrada,';Tipo de Fallo:',t2.TipoFallo,if((SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo) is null, Concat(';Fallo No Codificado: ',DescFalloNoCod), CONCAT(';Descripción de Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo),';Fallo: ',(select concat(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp))),';Observaciones: ',t2.ObservacionesSupervision,';Tipo: Exportación de Datos A Excel')) FROM reportesupervicion as t2   WHERE t2.folio='" + folio + "';").ToString().Split(';');
            }else
            {
                if ((int)area==1)
                {
                    res = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('folio: ',t2.Folio,';Unidad: ',(SELECT concat(ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t2.UnidadfkCUnidades),';Fecha de Reporte: ',DATE_FORMAT(t2.FechaReporte,'%W, %d de %M del %Y'),';Estatus Del Mantenimiento: ',t1.Estatus,';Código de Fallo: ',(SELECT CONCAT(codfallo,' - ',falloesp) FROM cfallosesp WHERE idfalloesp=t2.CodFallofkcfallosesp),';Fecha de Reporte de Mantenimiento: ',DATE_FORMAT(t1.FechaReporteM,'%W, %d de %M del %Y'),';Mecánico: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.MecanicofkPersonal),';Mecánico de Apoyo: ',if(t1.MecanicoApoyofkPersonal is null,'\"Sin Mecánico de Apoyo\"',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t1.MecanicofkPersonal)),';Supervisor: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.SupervisorfkCPersonal),';Hora de Entrada: ',t2.HoraEntrada,';Tipo de Fallo: ',t2.TipoFallo,if(t2.DescrFallofkcdescfallo is null,CONCAT(';Descripción de Fallo No Codificado:',t2.DescFalloNoCod),CONCAT('DeSCRIPCIÓN DE Fallo: ',(SELECT descfallo FROM cdescfallo WHERE iddescfallo=t2.DescrFallofkcdescfallo))),';Observaciones de Supervisión: ',t2.ObservacionesSupervision,';')) FROM sistrefaccmant.reportemantenimiento as t1 INNER JOIN reportesupervicion as t2 ON t1.FoliofkSupervicion = t2.idreportesupervicion WHERE t2.Folio='"+folio+"'").ToString().Split(';');
                }else
                {
                    res = v.getaData("SET lc_time_names='es_ES';SELECT UPPER(CONCAT('fOLIO:',t3.Folio,(SELECT concat(';ECONÓMICO: ',ta2.identificador,LPAD(ta1.consecutivo,4,'0')) FROM cunidades as ta1 INNER JOIN careas as ta2 ON ta1.areafkcareas=ta2.idarea WHERE ta1.idunidad=t3.UnidadfkCUnidades),';Fecha de Solicitud de Refacción: ',t4.FechaReporteM,'; Mecánico Que SOlicita La Refacción: ',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t4.mecanicofkpersonal),';Folio de Factura: ',t2.FolioFactura,';Fecha de Entrega de Refacción: ',DATE_FORMAT(t2.fechaEntrega,'%W, %d de %M del %Y'),';Persona Que Entregó La Refacción:',(SELECT CONCAT(nombres,' ',apPaterno,' ',apMaterno) FROM cpersonal WHERE idpersona = t2.PersonaEntregafkcPersonal),';Observaciones: ',if(t2.ObservacionesTrans='','\"Sin Refacciones\"',t2.ObservacionesTrans))) FROM modificaciones_sistema as t1 INNER JOIN reportetri as t2 ON t1.idregistro=t2.idReporteTransinsumos INNER JOIN reportesupervicion as t3 ON t2.idreportemfkreportemantenimiento=t3.idReporteSupervicion INNER JOIN reportemantenimiento as t4 ON t3.idReporteSupervicion = t4.FoliofkSupervicion WHERE t3.folio='" + folio+"'").ToString().Split(';');
                }
            }
            paralabelsImpares((string[])res);
        }
    }
}
