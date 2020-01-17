using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controlFallos
{
    public partial class CatRoles : Form
    {
        validaciones v;
        int idUsuario, idRol, empresaAnterior, areaArenterior, servicioAnterior, ciclosAnterior, ecosAnterior, lapsoAnterior, statusAnterior, x = 5, y = 5;
        DateTime horaAnterior;
        delegate void empre();
        Thread hempresas, th;
        bool pinsertar { get; set; }
        bool pconsultar { get; set; }
        bool peditar { get; set; }
        bool pdesactivar { get; set; }
        public bool editar = false, nuevo;


        public CatRoles(int idUsuario, int empresa, int area, validaciones v)
        {
            th = new Thread(new ThreadStart(v.Splash));
            th.Start();
            this.v = v;
            this.idUsuario = idUsuario;
            InitializeComponent();
            cmbempresa.DrawItem += v.combos_DrawItem;
            cmbarea.DrawItem += v.combos_DrawItem;
            cmbservicio.DrawItem += v.combos_DrawItem;
            cmbdescanso.DrawItem += v.combos_DrawItem;
            cmbempresa.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbarea.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbservicio.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
            cmbdescanso.MouseWheel += new MouseEventHandler(v.paraComboBox_MouseWheel);
        }
        bool getboolfromint(int i)
        {
            return i == 1;
        }
        public void privilegios()
        {
            string sql = "SELECT privilegios as privilegios FROM privilegios where usuariofkcpersonal='" + idUsuario + "' and namform='CatRoles'";
            string[] privilegios = v.getaData(sql).ToString().Split('/');
            pinsertar = getboolfromint(Convert.ToInt32(privilegios[0]));
            pconsultar = getboolfromint(Convert.ToInt32(privilegios[1]));
            peditar = getboolfromint(Convert.ToInt32(privilegios[2]));
            pdesactivar = getboolfromint(Convert.ToInt32(privilegios[3]));
        }
        void mostrar()
        {
            privilegios();
            ptabla.Visible = (pconsultar ? true : false);
            gbRol.Visible = (pinsertar || peditar ? true : false);
            psave.Visible = (pinsertar ? true : false);
        }
        void mostrarEmpresas()
        {
            if (this.InvokeRequired)
            {
                empre e = new empre(mostrarEmpresas);
                this.Invoke(e);
            }
            v.iniCombos("call sistrefaccmant.companieswithstatus();", cmbempresa, "id", "nombre", "-- SELECCIONE UNA EMPRESA --");
            v.comboswithuot(cmbdescanso, new string[] { "--seleccione--", "si", "no" });
            hempresas.Abort();
        }
        void pecos()
        {
            DataTable dt = (DataTable)v.getData("SELECT * FROM sistrefaccmant.getecos;");
            foreach (DataRow item in dt.Rows)
            {
                createcontrols(item.ItemArray[0], item.ItemArray[1]);
            }
        }
        void createcontrols(object id, object text)
        {
            Label l = new Label();
            l.FlatStyle = FlatStyle.Flat;
            l.AutoSize = false;
            l.BackColor = Color.PaleGreen;
            l.BorderStyle = BorderStyle.Fixed3D;
            l.ForeColor = Color.FromArgb(75, 44, 52);
            l.Size = new Size(103, 24);
            l.Location = new Point(x, y);
            x += 107;
            y = (x >= paddeco.Size.Width ? y += 29 : y);
            x = (x >= paddeco.Size.Width ? 5 : x);
            l.Text = text.ToString();
            paddeco.Controls.Add(l);
        }
        private void CatRoles_Load(object sender, EventArgs e)
        {
            hempresas = new Thread(new ThreadStart(mostrarEmpresas));
            hempresas.Start();
            pecos();
            cargarroles();
            mostrar();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(SplashScreen))
                {
                    if (frm.InvokeRequired)
                    {
                        validaciones.delgado dm = new validaciones.delgado(v.cerrarForm);
                        Invoke(dm, frm);
                    }
                    break;
                }
            }
            th.Abort();
        }
        void cargarroles()
        {
            dgvroles.Rows.Clear();
            string sql = "select t1.idrol as id,(select upper(nombreEmpresa) from cempresas as x1 inner join careas as x2 on x1.idempresa=x2.empresafkcempresas where t2.AreafkCareas=x2.idarea)as empresa,(select upper(x1.nombreArea) from careas as x1 where x1.idarea=t2.AreafkCareas)as area, upper(concat(t2.nombre,' ',t2.descripcion)) as servicio,nciclos as ciclos, necos as ecos, date_format(horaincorporo,'%H:%i') as hora, CONCAT(diffciclos,' MINUTOS') as diferencia,(select upper(concat(coalesce(x1.apPaterno,''),' ',coalesce(x1.apMaterno,''),' ',x1.nombres)) from cpersonal as x1 where x1.idPersona=t1.usuariofkcpersonal)as persona, if(t1.status=1,'ACTIVO','NO ACTIVO') as estatus from croles as t1 inner join cservicios as t2 on t2.idservicio=t1.serviciofkcservicios";
            MySqlCommand cmd = new MySqlCommand(sql, v.c.dbconection());
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                dgvroles.Rows.Add(dr.GetString("id"), dr.GetString("empresa"), dr.GetString("area"), dr.GetString("servicio"), dr.GetString("ciclos"), dr.GetString("ecos"), dr.GetString("hora"), dr.GetString("diferencia"), dr.GetString("persona"), dr.GetString("estatus"));
            dr.Close();
            v.c.dbcon.Close();
            dgvroles.ClearSelection();
        }
        private void cmbempresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbempresa.SelectedIndex > 0)
                v.iniCombos("select idarea as id, upper(nombreArea) as nombre from careas as t1 inner join cempresas as t2 on t2.idempresa=t1.empresafkcempresas where t1.status='1' and empresafkcempresas='" + cmbempresa.SelectedValue + "' order by nombreArea;", cmbarea, "id", "nombre", "-- SELECCIONE UN ÁREA --");
            else cmbarea.DataSource = null;
            cmbarea.Enabled = (cmbempresa.SelectedIndex > 0 ? true : false);
        }

        private void cmbarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbarea.SelectedIndex > 0)
                v.iniCombos("select idservicio as id, upper(concat(Nombre,' ',Descripcion)) as nombre from cservicios where AreafkCareas='" + cmbarea.SelectedValue + "' and status='1' order by Nombre;", cmbservicio, "id", "nombre", "-- SELECCIONE UN SERVICIO--");
            else cmbservicio.DataSource = null;
            cmbservicio.Enabled = (cmbarea.SelectedIndex > 0 ? true : false);
        }

        private void cmbdescanso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbdescanso.SelectedIndex > 0)
                pdescansos.Visible = (Convert.ToInt32(cmbdescanso.SelectedIndex) == 1 ? true : false);
            else
                pdescansos.Visible = false;
        }

        private void btntime_Click(object sender, EventArgs e)
        {
            diferenciaecos d = new diferenciaecos(v);
            d.Owner = this;
            d.cecos = Convert.ToInt32(txtecos.Text) - 1;
            d.diferen = new string[d.cecos];
            d.ShowDialog();
        }

        private void txtciclos_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.Solonumeros(e);
        }

        private void cmbempresa_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editar && peditar)
                psave.Visible = (cambios() ? true : false);
            if (!string.IsNullOrWhiteSpace(txtecos.Text))
                ptime.Visible = (Convert.ToInt32(txtecos.Text) > 0 ? true : false);
            else ptime.Visible = false;
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (v.camposRol(Convert.ToInt32(cmbempresa.SelectedValue), Convert.ToInt32(cmbarea.SelectedValue), Convert.ToInt32(cmbservicio.SelectedValue), txtciclos.Text, txtecos.Text, dtpincorporo.Value /**, difEcos**/, Convert.ToInt32(nudlapso.Value)))
                if (!v.existeRol(Convert.ToInt32(cmbservicio.SelectedValue), idRol))
                    if (!editar)
                    {
                        if (v.c.insertar("Insert into croles(serviciofkcservicios, nciclos, necos, horaincorporo, diffciclos, usuariofkcpersonal) values('" + cmbservicio.SelectedValue + "', '" + Convert.ToInt32(txtciclos.Text) + "', '" + Convert.ToInt32(txtecos.Text) + "', '" + dtpincorporo.Value.ToString("HH:mm:ss") + "', '" + nudlapso.Value + "', '" + idUsuario + "')"))
                            if (v.c.insertar("insert into modificaciones_sistema(form,idregistro,usuariofkcpersonal,fechaHora,Tipo,empresa,area) values('Catálogo de Roles','" + v.getaData("select idrol from croles order by idrol desc limit 1;") + "','" + idUsuario + "',now(),'Inserción de Rol','1','1')"))
                            {
                                MessageBox.Show("Los datos se insertaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                limpiar();
                                cargarroles();
                            }
                            else MessageBox.Show("Error al registrar los datos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        observacionesEdicion o = new observacionesEdicion(v);
                        o.Owner = this;
                        if (o.ShowDialog() == DialogResult.OK)
                        {
                            string motivo = o.txtgetedicion.Text.Trim();
                            if (v.c.insertar("update croles set serviciofkcservicios='" + cmbservicio.SelectedValue + "',nciclos='" + Convert.ToInt32(txtciclos.Text) + "',necos='" + Convert.ToInt32(txtecos.Text) + "',horaincorporo='" + dtpincorporo.Value.ToString("HH:mm:ss") + "',diffciclos='" + nudlapso.Value + "' where idrol='" + idRol + "'"))
                                if (v.c.insertar("insert into modificaciones_sistema(form,idregistro,ultimaModificacion,usuariofkcpersonal,fechaHora,Tipo,motivoActualizacion,empresa,area) values('Catálogo de Roles','" + idRol + "','" + servicioAnterior + ";" + ciclosAnterior + ";" + ecosAnterior + ";" + horaAnterior.ToString("HH:mm") + ";" + lapsoAnterior + "','" + idUsuario + "',now(),'Actualización de Rol','" + v.mayusculas(motivo) + "','1','1')"))
                                {
                                    MessageBox.Show("Los datos se actualizaron de manera correcta", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    limpiar();
                                    cargarroles();
                                }
                                else MessageBox.Show("Error al modificar los datos", validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
        }


        private void btnstatus_Click(object sender, EventArgs e)
        {
            observacionesEdicion o = new observacionesEdicion(v);
            o.Owner = this;
            o.lblinfo.Text = "Ingrese el motivo de la " + (statusAnterior == 1 ? "desactivación" : "reactivación");
            if (o.ShowDialog() == DialogResult.OK)
            {
                string motivo = o.txtgetedicion.Text.Trim();
                if (v.c.insertar("update croles set status='" + (statusAnterior == 1 ? 0 : 1) + "' where idrol='" + idRol + "'"))
                    if (v.c.insertar("insert into modificaciones_sistema(form,idregistro,usuariofkcpersonal,fechaHora,Tipo,motivoActualizacion,empresa,area) values('Catálogo de Roles','" + idRol + "','" + idUsuario + "',now(),'" + (statusAnterior == 1 ? "Desactivación" : "Reactivación") + " de Rol','" + v.mayusculas(motivo) + "','1','1')"))
                    {
                        MessageBox.Show("El servicio se ha " + (statusAnterior == 1 ? "desactivado" : "reactivado") + " correctamente", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        limpiar();
                        cargarroles();
                    }
            }
        }

        private void dgvroles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvroles.Columns[e.ColumnIndex].Name == "status")
                e.CellStyle.BackColor = (e.Value.ToString() == "ACTIVO" ? Color.PaleGreen : Color.LightCoral);
        }

        private void btnCancelEmpresa_Click(object sender, EventArgs e)
        {
            if (cambios())
                if (MessageBox.Show("¿Desea guardar las modificaciones?", validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { }
                else
                    limpiar();
            else
                limpiar();
        }

        bool cambios()
        {
            bool res = false;
            if ((Convert.ToInt32(cmbempresa.SelectedValue) != empresaAnterior || Convert.ToInt32(cmbarea.SelectedValue) != areaArenterior || Convert.ToInt32(cmbservicio.SelectedValue) != servicioAnterior || Convert.ToInt32((string.IsNullOrWhiteSpace(txtciclos.Text) ? "0" : txtciclos.Text)) != ciclosAnterior || Convert.ToInt32((string.IsNullOrWhiteSpace(txtecos.Text) ? "0" : txtecos.Text)) != ecosAnterior || dtpincorporo.Value.ToString("HH:mm") != horaAnterior.ToString("HH:mm") || nudlapso.Value != lapsoAnterior))
            {
                if (empresaAnterior == 0 || areaArenterior == 0 || servicioAnterior == 0 || ciclosAnterior == 0 || ecosAnterior == 0 || horaAnterior.Hour == 0 || lapsoAnterior == 0)
                    nuevo = true;
                if (cmbempresa.SelectedIndex > 0 && cmbarea.SelectedIndex > 0 && cmbservicio.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtciclos.Text) && !string.IsNullOrWhiteSpace(txtecos.Text) && statusAnterior > 0)
                    res = true;
                return res;
            }
            else return res;
        }
        void limpiar()
        {
            mostrarEmpresas();
            nudlapso.Value = cmbempresa.SelectedIndex = empresaAnterior = areaArenterior = servicioAnterior = ciclosAnterior = ecosAnterior = lapsoAnterior = idRol = 0;
            txtecos.Clear();
            txtciclos.Clear();
            dtpincorporo.Value = horaAnterior = DateTime.Parse("00:00");
            btnguardar.BackgroundImage = controlFallos.Properties.Resources.save;
            pnuevo.Visible = pstatus.Visible = editar = nuevo = !(psave.Visible = true);
            dgvroles.ClearSelection();
            psave.Visible = (pinsertar ? true : false);
        }

        private void dgvroles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cambios() || nuevo)
                if (MessageBox.Show("¿Desesa " + (nuevo ? "concluir el registro" : "guardar las modificaciones"), validaciones.MessageBoxTitle.Confirmar.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { }
                else
                    cargarDatos(e);
            else cargarDatos(e);
        }
        void cargarDatos(DataGridViewCellEventArgs e)
        {
            limpiar();
            string[] datosrol = v.getaData("select concat(serviciofkcservicios,'|',nciclos,'|',necos,'|',date_format(horaincorporo,'%H:%i'),'|',diffciclos,'|',status)  from croles where idrol='" + (idRol = Convert.ToInt32(dgvroles.Rows[e.RowIndex].Cells[0].Value)) + "'").ToString().Split('|');
            empresaAnterior = Convert.ToInt32(v.getaData("select idempresa from cempresas as t1 inner join careas as t2 on t1.idempresa=t2.empresafkcempresas inner join cservicios as t3 on t2.idarea=t3.AreafkCareas where t3.idservicio='" + (servicioAnterior = Convert.ToInt32(datosrol[0])) + "';"));
            if (Convert.ToInt32(v.getaData("select status from cempresas where idempresa='" + empresaAnterior + "'")) == 0)
                v.iniCombos("select idempresa as id, upper(nombreEmpresa)as nombre from cempresas where status='1' or idempresa='" + empresaAnterior + "' order by nombreEmpresa;", cmbempresa, "id", "nombre", "-- SELECCIONE UNA EMPRESA --");
            cmbempresa.SelectedValue = empresaAnterior;
            areaArenterior = Convert.ToInt32(v.getaData("select idarea from careas as t1 inner join cservicios as t2 on t2.AreafkCareas=t1.idarea where t2.idservicio='" + servicioAnterior + "';"));
            if (Convert.ToInt32(v.getaData("select status from careas where idarea='" + areaArenterior + "';")) == 0)
                v.iniCombos("select idarea as id, upper(nombreArea) as nombre from careas as t1 inner join cempresas as t2 on t2.idempresa=t1.empresafkcempresas where  empresafkcempresas='" + empresaAnterior + "' and t1.status='1' or idarea='" + areaArenterior + "' order by nombreArea;", cmbarea, "id", "nombre", "-- SELECCIONE UN ÁREA --");
            cmbarea.SelectedValue = areaArenterior;
            if (Convert.ToInt32(v.getaData("select status from cservicios where idservicio='" + servicioAnterior + "';")) == 0)
                v.iniCombos("select idservicio as id, upper(concat(Nombre,' ',Descripcion)) as nombre from cservicios where AreafkCareas='" + areaArenterior + "' and status='1' or idservicio='" + servicioAnterior + "' order by Nombre;", cmbservicio, "id", "nombre", "-- SELECCIONE UN SERVICIO--");
            cmbservicio.SelectedValue = servicioAnterior;
            txtciclos.Text = (ciclosAnterior = Convert.ToInt32(datosrol[1])).ToString();
            txtecos.Text = (ecosAnterior = Convert.ToInt32(datosrol[2])).ToString();
            dtpincorporo.Value = horaAnterior = DateTime.Parse(datosrol[3]);
            nudlapso.Value = lapsoAnterior = Convert.ToInt32(datosrol[4]);
            pnuevo.Visible = editar = !(psave.Visible = false);
            lblstatus.Text = ((statusAnterior = Convert.ToInt32(datosrol[5])) == 1 ? "Desactivar" : "Reactivar");
            btnstatus.BackgroundImage = (statusAnterior == 1 ? controlFallos.Properties.Resources.delete__4_ : controlFallos.Properties.Resources.up);
            btnguardar.BackgroundImage = controlFallos.Properties.Resources.pencil;
            if (statusAnterior == 0 && peditar && pdesactivar)
                MessageBox.Show("Para editar el registro es necesario reactivar primero el rol", validaciones.MessageBoxTitle.Información.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            pstatus.Visible = (pdesactivar ? true : false);
        }
    }
}