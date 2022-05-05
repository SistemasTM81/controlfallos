using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

using Word = Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace controlFallos
{
    public partial class catPuestos : Form
    {
        validaciones v;
        int idUsuario, empresa, area, idpuesto, status;
        string puestoAnterior;
        public bool Pinsertar { set; get; }
        public bool Peditar { get; set; }
        public bool Pconsultar { set; get; }
        public bool Pdesactivar { set; get; }
        bool yaAparecioMensaje = false;
        bool editar;
        public catPuestos(int idUsuario, int empresa, int area,validaciones v)
        {
            this.v = v;
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            tbpuestos.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        public void iniCombos(string sql, ComboBox cbx, string ValueMember, string DisplayMember, string TextoInicial)
        {
            cbx.DataSource = null;
            DataTable dt = (DataTable)v.getData(sql);
            DataRow nuevaFila = dt.NewRow();
            nuevaFila[ValueMember] = 0;
            nuevaFila[DisplayMember] = TextoInicial.ToUpper();
            dt.Rows.InsertAt(nuevaFila, 0);
            cbx.DisplayMember = DisplayMember;
            cbx.ValueMember = ValueMember;
            cbx.DataSource = dt;
        }
        public void privilegiosPuestos()
        {
            string[] privilegiosTemp = v.getaData(string.Format("SELECT privilegios FROM privilegios WHERE usuariofkcpersonal ='{0}' AND namForm ='{1}'", idUsuario, this.Name)).ToString().Split('/');
            if (privilegiosTemp.Length > 0)
            {

                Pconsultar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[1]));
                Pinsertar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[0]));
                Peditar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[2]));
                if (privilegiosTemp.Length > 3)
                {
                    Pdesactivar = v.getBoolFromInt(Convert.ToInt32(privilegiosTemp[3]));
                }
            }
            mostrar();
        }
        void mostrar()
        {
            if (Pinsertar || Peditar)
            {

                gbpuesto.Visible = true;
            }
            if (Pconsultar)
            {
                tbpuestos.Visible = true;
            }
            if (Peditar)
            {
                label22.Visible = true;
                label23.Visible = true;
            }
            if (Peditar && !Pinsertar)
            {
                btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                lblsave.Text = "Editar Puesto";
                editar = true;
            }
        }
        void limpiar()
        {
            if (Pinsertar)
            {
                editar = false;
                btnsave.BackgroundImage = controlFallos.Properties.Resources.save;
                gbpuesto.Text = "Agregar Puesto";
                lblsave.Text = "Agregar";
                txtgetpuesto.Focus();
            }
            if (Pconsultar)
            {
                busquedapuestos();
            }
            btnsave.Visible = lblsave.Visible = true;
            txtgetpuesto.Clear();
            idpuesto = 0;
            pcancel.Visible = true;
            yaAparecioMensaje = false;
            catPersonal cat = (catPersonal)Owner;
            var _puesto = cat.csetpuestos.SelectedValue;
            cat.busemp();
            cat.busqPuestos();
            if (Convert.ToInt32(_puesto) >= 0)
            {
                cat.csetpuestos.SelectedValue = _puesto;
            }
            pdelete.Visible = false;
            pcancel.Visible = false;

        }

        private void btnguardarpuesto_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtgetpuesto.Text))
            {

                if (!editar)
                {

                    insertar();
                }
                else
                {
                    _editar();
                }
            }
        }

        private void gbpuestos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (idpuesto > 0 && !v.mayusculas(txtgetpuesto.Text.Trim().ToLower()).Equals(puestoAnterior) && Peditar && status == 1)
                {
                    if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        yaAparecioMensaje = true;
                        btnsave_Click(null, e);
                    }
                    else
                    {
                        guardarReporte(e);
                    }
                }
                else
                {
                    guardarReporte(e);
                }
            }
        }
        void guardarReporte(DataGridViewCellEventArgs e)
        {

            try
            {
                idpuesto = Convert.ToInt32(tbpuestos.Rows[e.RowIndex].Cells[0].Value.ToString());
                status = v.getStatusInt(tbpuestos.Rows[e.RowIndex].Cells[3].Value.ToString());
                if (Pdesactivar)
                {
                    if (status == 0)
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.up;
                        lbldelete.Text = "Reactivar";
                    }
                    else
                    {
                        btndelete.BackgroundImage = controlFallos.Properties.Resources.delete__4_;
                        lbldelete.Text = "Desactivar";
                    }
                    pdelete.Visible = true;
                }
                if (Peditar)
                {
                    txtgetpuesto.Text = puestoAnterior = v.mayusculas(tbpuestos.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower());
                    tbpuestos.ClearSelection();
                    gbpuesto.Visible = true;
                    if (Pinsertar) pcancel.Visible = true;
                    editar = true;
                    btnsave.BackgroundImage = controlFallos.Properties.Resources.pencil;
                    gbpuesto.Text = "Actualizar Puesto";
                    lblsave.Text = "Guardar";
                    btnsave.Visible = lblsave.Visible = false;
                    if (status == 0) MessageBox.Show(v.mayusculas("Para Modificar La Información Necesita Reactivar El Registro"), validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Usted No Cuenta Con Privilegios Para Editar", validaciones.MessageBoxTitle.Advertencia.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtgetpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Sololetras(e);
        }

        private void gbpuestos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbpuestos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo")
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void catPuestos_Load(object sender, EventArgs e)
        {
            privilegiosPuestos();
            if (Pconsultar)
            {

                busquedapuestos();
            }
        }

        private void gbpuestos_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (tbpuestos.Columns[e.ColumnIndex].Name == "Estatus")
            {
                if (Convert.ToString(e.Value) == "Activo".ToUpper())
                {

                    e.CellStyle.BackColor = Color.PaleGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void btndelete_Click_1(object sender, EventArgs e)
        {
            string msg;
            int state;
            if (this.status == 0)
            {

                msg = "Re";
                state = 1;
            }
            else
            {
                state = 0;
                msg = "Des";

            }
            observacionesEdicion obs = new observacionesEdicion(v);
            obs.Owner = this;
            obs.lblinfo.Text = "Ingrese el Motivo de la " + msg + "activación Del Puesto";
            obs.lblinfo.Location = new Point(obs.lblinfo.Location.X + 15, obs.lblinfo.Location.Y);
            if (obs.ShowDialog() == DialogResult.OK)
            {
                string edicion = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());

                try
                {
                    String sql = "UPDATE puestos SET status = " + state + " WHERE idpuesto  = " + idpuesto;
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion ,empresa,area) VALUES('Catálogo de Puestos','" + idpuesto + "','" + msg + "activación de Puesto','" + idUsuario + "',NOW(),'" + msg + "activación de Puesto','" + edicion + "','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Puesto ha sido " + msg + "activado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!editar)
                {
                    insertar();
                }
                else
                {
                    _editar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Control de Fallos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            if (!v.mayusculas(txtgetpuesto.Text.Trim().ToLower()).Equals(puestoAnterior) && status == 1)
            {
                if (MessageBox.Show("¿Desea Guardar La Información?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    yaAparecioMensaje = true;
                    btnsave_Click(null, e);
                }
                else
                {
                    limpiar();
                }
            }
            else
            {
                limpiar();
            }
        }

        private void txtgetpuesto_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) btnsave_Click(null, e);
            else
                v.letrasnumerosdiagonalyguion(e);
        }

        private void gbpuesto_Enter(object sender, EventArgs e)
        {

        }

        private void gbpuestos_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            v.paraDataGridViews_ColumnAdded(sender, e);
        }

        private void txtgetpuesto_Validating(object sender, CancelEventArgs e)
        {
            v.espaciosenblanco(sender, e);
        }

        private void txtgetpuesto_TextChanged(object sender, EventArgs e)
        {
            if (editar)
            {
                if (status == 1 && (!string.IsNullOrWhiteSpace(txtgetpuesto.Text) && puestoAnterior != v.mayusculas(txtgetpuesto.Text.ToLower().Trim())))
                {
                    btnsave.Visible = lblsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = lblsave.Visible = false;
                }
            }
        }

        private void gbpuesto_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            v.DrawGroupBox(box, e.Graphics, Color.FromArgb(75, 44, 52), Color.FromArgb(75, 44, 52), this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                if (!string.IsNullOrWhiteSpace(txtgetpuesto.Text))
                    if (MessageBox.Show("¿Desea concluir el registro?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
            else
            {
                if (puestoAnterior != v.mayusculas(txtgetpuesto.Text.Trim().ToLower()))
                    if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        this.Close();
                    else
                        DialogResult = DialogResult.None;
                else
                    this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Export_Data_To_Word(tbpuestos, null);
        }
        
        public void Export_Data_To_Word(DataGridView dgvTabla1, string filename)
        {
            if (dgvTabla1.Rows.Count != 0)
            {
                List<object> obj = new List<object>();
                //  int row = ;
                int column = 0;
                int RowCount = dgvTabla1.Rows.Count;
                List<string> headerText = new List<string>();
                //int ColumnCount = dgvTabla1.Columns.Count;
                Word.Document oDoc = new Word.Document();
                //add rows
                int r = 0;
                for (int i = 0; i < dgvTabla1.Columns.Count; i++)
                {
                    if (dgvTabla1.Columns[i].Visible)
                    {
                        headerText.Add(dgvTabla1.Columns[i].HeaderText);
                        column++;
                    }
                }
                for (int j = 0; j < dgvTabla1.Rows.Count; j++)
                {

                    for (int i = 0; i < dgvTabla1.Columns.Count; i++)
                    {
                        if (dgvTabla1.Columns[i].Visible)
                            obj.Add(dgvTabla1.Rows[j].Cells[i].Value.ToString().Replace("\n", " "));
                    }
                }



                //end column loop

                //page orintation
                oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape; ///Cambiar Imagen de forma horizontal

                dynamic oRange = oDoc.Content.Application.Selection.Range;
                //   oDoc.Content.Application.Selection.Range.HighlightColorIndex = Word.WdColorIndex.wdGreen;
                string oTemp = ""; object[] DataArray = obj.ToArray();
                for (r = 0; r < DataArray.Length; r++)
                    oTemp += (!string.IsNullOrWhiteSpace(oTemp) ? "\t" : "") + DataArray[r];
                //table format
                oRange.Text = oTemp;
                oRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                object Separator = Word.WdTableFieldSeparator.wdSeparateByTabs;
                object ApplyBorders = true;
                object AutoFit = false;
                object AutoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitContent;

                oRange.ConvertToTable(ref Separator, ref RowCount, ref column,
                                      Type.Missing, Type.Missing, ref ApplyBorders,
                                      Type.Missing, Type.Missing, Type.Missing,
                                      Type.Missing, Type.Missing, Type.Missing,
                                      Type.Missing, ref AutoFit, ref AutoFitBehavior, Type.Missing);

                oRange.Select();

                oDoc.Application.Selection.Tables[1].Select();

                oDoc.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
                oDoc.Application.Selection.Tables[1].Rows.Alignment = Word.WdRowAlignment.wdAlignRowCenter;
                oDoc.Application.Selection.InsertRowsAbove(1);
                oDoc.Application.Selection.Tables[1].Rows[1].Select();
                //   oDoc.Application.Selection.Tables[1].AutoFormat();
                //header row style
                for (int i = 2; i < oDoc.Application.Selection.Tables[1].Rows.Count + 1; i++)
                {
                    oDoc.Application.Selection.Tables[1].Rows[i].Range.Bold = 0;
                    oDoc.Application.Selection.Tables[1].Rows[i].Range.Font.Name = "Franklin Gothic Book";
                    oDoc.Application.Selection.Tables[1].Rows[i].Range.Font.Size = 9;
                    //      oDoc.Application.Selection.Tables[1].Rows[i].Range.Font.Color = Word.WdColor.wdColorGold;
                    oDoc.Application.Selection.Tables[1].Rows[i].Range.Borders.Enable = 1;

                    oDoc.Application.Selection.Tables[1].Rows.Borders.Enable = 1; ;
                    //    oDoc.Application.Selection.Tables[1].Rows[i].Cells.Shading.BackgroundPatternColor = Word.WdColor.wdColorPink;
                }
                //add header row manually
                string[] headerTemp = headerText.ToArray();
                for (int c = 0; c < headerTemp.Length; c++)
                {
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text = headerTemp[c];
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Bold = 1;
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Font.Name = "Franklin Gothic Book";
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Font.Size = 9;
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).WordWrap = false;
                    oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Borders.Enable = 1;


                }
                //table style 
                //     oDoc.Application.Selection.Tables[1].set_Style("Grid Table 4 - Accent 5");
                oDoc.Application.Selection.Tables[1].Rows[1].Select();
                oDoc.Application.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                //header text
                foreach (Word.Section section in oDoc.Application.ActiveDocument.Sections)
                {
                    Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                    headerRange.Text = "your header text";
                    headerRange.Font.Size = 8;
                    headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                }

                //save the file
                //oDoc.SaveAs2(filename);
                oDoc.Application.Visible = true;
                //NASSIM LOUCHANI
                //NASSIM LOUCHANI
            }
        }

        private void tbpuestos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbltitle_MouseDown(object sender, MouseEventArgs e)
        {
            v.mover(sender, e, this);
        }

        void _editar()
        {

            if (idpuesto > 0)
            {

                string puesto = v.mayusculas(txtgetpuesto.Text.ToLower()).Trim();
                if (!string.IsNullOrWhiteSpace(puesto))
                {
                    if (!puesto.Equals(puestoAnterior))
                    {
                        if (!v.yaExistePuesto(puesto, empresa, area))
                        {
                            observacionesEdicion obs = new observacionesEdicion(v);
                            obs.Owner = this;
                            if (obs.ShowDialog() == DialogResult.OK)
                            {
                                string observaciones = v.mayusculas(obs.txtgetedicion.Text.Trim().ToLower());
                                String sql = "UPDATE puestos SET puesto =LTRIM(RTRIM('" + puesto + "')) WHERE idpuesto = " + this.idpuesto;

                                if (v.c.insertar(sql))
                                {
                                    var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,motivoActualizacion,empresa,area) VALUES('Catálogo de Puestos','" + idpuesto + "','" + puestoAnterior + "','" + idUsuario + "',NOW(),'Actualización de Puesto','" + observaciones + "','" + empresa + "','" + area + "')");
                                    if (!yaAparecioMensaje) MessageBox.Show("El Puesto Se Ha Actualizado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    limpiar();
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Se Realizó Ningún Cambio Al Puesto", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("¿Desea Limpiar Los Campos?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            limpiar();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("El Nombre del Puesto no puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un Puesto Para Actualizar", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void insertar()
        {
            string puesto = v.mayusculas(txtgetpuesto.Text.ToLower());
            if (!string.IsNullOrWhiteSpace(puesto))
            {
                if (!v.yaExistePuesto(puesto, empresa, area))
                {

                    String sql = "INSERT INTO puestos (puesto,empresa,area,usuariofkcpersonal) VALUES(LTRIM(RTRIM('" + puesto + "')),'" + empresa + "','" + area + "','" + idUsuario + "')";
                    if (v.c.insertar(sql))
                    {
                        var res2 = v.c.insertar("INSERT INTO modificaciones_sistema(form, idregistro, ultimaModificacion, usuariofkcpersonal, fechaHora, Tipo,empresa,area) VALUES('Catálogo de Puestos',(SELECT idpuesto FROM puestos WHERE puesto='" + puesto + "' and empresa='" + empresa + "' and area='" + area + "'),'Inserción de Puesto','" + idUsuario + "',NOW(),'Inserción de Puesto','" + empresa + "','" + area + "')");
                        MessageBox.Show("El Puesto Se Ha Insertado Correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();

                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un error");
                    }
                }
            }
            else
            {
                MessageBox.Show("El Nombre del Puesto no puede Estar Vacío", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void busquedapuestos()
        {
            tbpuestos.Rows.Clear();
            String sql = "SELECT t1.idpuesto as id, UPPER(t1.puesto) AS puesto, t1.status, UPPER(CONCAT(coalesce(t2.nombres,''),' ',coalesce(t2.apPaterno,''),' ',coalesce(t2.apMaterno,''))) as persona  FROM puestos as t1 INNER JOIN cpersonal as t2 ON t1.usuariofkcpersonal = t2.idpersona WHERE t1.empresa = '" + empresa + "' and t1.area ='" + area + "' ORDER BY puesto ASC";
            MySqlCommand cm = new MySqlCommand(sql,v.c.dbconection());
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbpuestos.Rows.Add(dr.GetString("id"), dr.GetString("puesto"), dr.GetString("persona"), v.getStatusString(dr.GetInt32("status")));
            }
            dr.Close();
            v.c.dbcon.Close();
            tbpuestos.ClearSelection();
        }
    }
}
