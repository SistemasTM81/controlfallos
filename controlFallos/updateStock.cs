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
    public partial class updateStock : Form
    {
        public object idUsuario;
        public double stockaNT;
        public int idRefaccion, empresa, area;
        validaciones v;
        public updateStock(int idRefaccion, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idRefaccion = idRefaccion;
            this.empresa = empresa;
            this.area = area;
        }

        private void textBoxUsuario_TextChanged(object sender, EventArgs e)
        {
            try
            {

                object res = v.getaData("SELECT CONCAT(idpersona,';',nombres,' ',apPaterno,' ',apMaterno) FROM datosistema as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal=t2.idpersona WHERE password='" + v.Encriptar(((TextBox)sender).Text) + "' AND t2.empresa='"+empresa+"' AND t2.area='"+area+"'");
                if (res != null)
                {
                    string[] usu = res.ToString().Split(';');
                    idUsuario = usu[0];
                    lblusu.Text = usu[1];
                }
                else
                {
                    idUsuario = null;
                    lblusu.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtstock.Text.Trim()))
            {
                if (stockaNT != Convert.ToDouble(txtstock.Text) && idUsuario != null)
                {
                    observacionesEdicion obs = new observacionesEdicion(v);
                    obs.Owner = this;
                    if (obs.ShowDialog() == DialogResult.OK)
                    {
                        string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                        if (v.c.insertar("UPDATE crefacciones SET existencias ='" + Convert.ToDouble(txtstock.Text.Trim()) + "' WHERE  idrefaccion='" + this.idRefaccion + "'"))
                        {
                            var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Refacciones','" + idRefaccion + "','" + stockaNT + "','" + idUsuario + "',NOW(),'Actualización de Existencias','" + edicion + "','" + empresa + "','" + area + "')");
                            nuevaRefaccion n = (nuevaRefaccion)(Owner);
                            n.lblexistencias.Text = v.getExistenciasFromIDRefaccion(idRefaccion.ToString()).ToString();
                            n.stock = Convert.ToDouble(txtstock.Text.Trim());
                            MessageBox.Show("Se han Actualizado Las Existencias Exitosamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                }
                else
                {

                    if (stockaNT == Convert.ToDouble(txtstock.Text))
                    {
                        if (MessageBox.Show("No se Detectó Modificación\n¿Desea Cerrar La Ventana?", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            this.DialogResult = DialogResult.Cancel;
                        }
                        else
                        {
                            DialogResult = DialogResult.None;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(textBoxUsuario.Text.Trim()))
                    {
                        MessageBox.Show("La Contraseña es Incorrecta", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.None;
                    }
                    else
                    {
                        MessageBox.Show("Ingrese su Contraseña Para Actualizar Las Esxistencias de la Refacción", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.DialogResult = DialogResult.None;
                    }
                }
            }else
            {
                MessageBox.Show("El Campo Existencias No Puede Estar Vacío", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.DialogResult = DialogResult.None;
            }
        }
        
    }
}
