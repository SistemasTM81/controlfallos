using System;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
namespace controlFallos
{
    public class conexion
    {
        validaciones v;
        public string host { protected internal set; get; }
        public string user { protected internal set; get; }
        public string password { protected internal set; get; }
        public string port { protected internal set; get; }
        public string hostLocal { protected internal set; get; }
        public string userLocal { protected internal set; get; }
        public string passwordLocal { protected internal set; get; }
        public string portLocal { protected internal set; get; }
        public MySqlConnection dbcon;
        MySqlConnection localConnection;
        public conexion(validaciones v)
        {
            this.v = v;
            string path = Application.StartupPath + @"\conexion.txt";
            if (!File.Exists(path))
            {
                StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
                sw.Write("0yLCd4LvwPo9xeMPa3Xo60R8ubmf9ZS4hs58llM/Lovd0yqGbDjTyz2KnbCOiM+bcf37rsKzAOUAU0rVJ8p3MJc4c6X+gGpk39iuKOx48Va645A5bRjQnefB1JmW3H+a");
                sw.Close();
            }
            StreamReader lector = new StreamReader(path);
            var res = lector.ReadLine();
            lector.Close();
            try
            {
                string[] arreglo = Desencriptar(res).Split(';');
                this.host = arreglo[0];
                this.user = arreglo[1];
                this.password = arreglo[2];
                this.port = arreglo[3];
                this.hostLocal = arreglo[4];
                this.userLocal = arreglo[5];
                this.passwordLocal = arreglo[6];
                this.portLocal = arreglo[7];
                localConnection = new MySqlConnection("Server = " + hostLocal + "; user=" + userLocal + "; password = " + passwordLocal + " ; database = sistrefaccmant ;port=" + portLocal);
            }
            catch { }
        }
        public string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "sistemafallos";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception) { }
            return textoEncriptado;
        }
        public MySqlConnection dbconection()
        {
            try
            {
                if (conexionOriginal())
                    dbcon = new MySqlConnection(string.Format("Server = {0}; user={1}; password ={2}; database = sistrefaccmant; port={3}", new string[] { host, user, password, port }));
                else
                    dbcon = new MySqlConnection("Server = " + hostLocal + "; user=" + userLocal + "; password = " + passwordLocal + "; database = sistrefaccmant ;port=" + portLocal);
                if (dbcon.State != System.Data.ConnectionState.Open) dbcon.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                foreach (Form frm in Application.OpenForms)
                    frm.Close();
                Application.Exit();
            }
            return dbcon;
        }
        public bool insertar(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, dbconection());
            int i = cmd.ExecuteNonQuery();
            //if (conexionOriginal())
            //{
            //    if (localConnection.State != System.Data.ConnectionState.Open) localConnection.Open();
            //    cmd = new MySqlCommand(sql, localConnection);
            //    i = cmd.ExecuteNonQuery();
            //    localConnection.Close();
            //    localConnection.Dispose();
            //}
            dbcon.Close();
            dbcon.Dispose();
            if (!conexionOriginal())
                WriteLocalSequence(sql);
            if (i >= 0) return true;
            else return false;


        }
        public object setData(string sql)
        {

            MySqlCommand cmd = new MySqlCommand(sql, dbconection());
            int i = cmd.ExecuteNonQuery();
            /** if (conexionOriginal())
                {
                    if (localConnection.State != System.Data.ConnectionState.Open) localConnection.Open();
                    cmd = new MySqlCommand(sql, localConnection);
                    i = cmd.ExecuteNonQuery();
                    localConnection.Close();
                    localConnection.Dispose();
                }*/
            dbcon.Close();
            dbcon.Dispose();
            if (!conexionOriginal())
                WriteLocalSequence(sql);
            return cmd.LastInsertedId;

        }
        public void referencia(int idUsuario)
        {
            string path = Application.StartupPath + @"\contains.txt";
            StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
            sw.Write(idUsuario + ";");
            sw.Close();
        }
        public void insertarGlobal()
        {
            try
            {
                string path = Application.StartupPath + @"\updates.srf";
                using (StreamReader lector = new StreamReader(path))
                {
                    string sql;
                    while (!string.IsNullOrWhiteSpace((sql = lector.ReadLine())))
                        insertar(v.Desencriptar(sql));
                }
                File.Delete(path);
            }
            catch (Exception ex) { MessageBox.Show(ex.HResult + ": " + ex.Message, validaciones.MessageBoxTitle.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        public bool conexionOriginal()
        {
            try
            {
                Ping p = new Ping();
                return (p.Send(host).Status == IPStatus.Success);
            }
            catch { return false; }
        }
        protected internal void WriteLocalSequence(string seq)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(Application.StartupPath + @"\updates.srf"), true, Encoding.ASCII);
            sw.WriteLine(v.Encriptar(seq));
            sw.Close();
        }
        public string[] tableNames = new string[] { "bloqueologin", "canaqueles", "careas", "catcategorias", "catincidencias", "cattipos", "ccharolas", "cdescfallo", "cempresas", "cestaciones", "cfallosesp", "cfallosgrales", "cfamilias", "cgiros", "civa", "cladas", "cmarcas", "cmedidas", "cmodelos", "cnfamilias", "cniveles", "comparativas", "cpasillos", "cpersonal", "cproveedores", "crefacciones", "cservicios", "cunidades", "cunidadmedida", "datosistema", "detallesordencompra", "encabezadoreportes", "estatusvalidado", "huellasupervision", "incidenciapersonal", "ladanac", "modificaciones_sistema", "nombresoc", "ordencompra", "pedidosrefaccion", "privilegios", "proveedorescomparativa", "puestos", "refaccionescomparativa", "relacservicioestacion", "reportemantenimiento", "reportepercance", "reportepersonal", "reportesupervicion", "reportetri", "sepomex", "vigencias_supervision" };

        public string[] fieldsbloqueologin = new string[] { "idloginstatus", "usuario", "fechaHora", "ipclient", "statusbloqueo", "tipobloqueo" };
        public string[] fieldscanaqueles = new string[] { "idanaquel", "anaquel", "nivelfkcniveles", "usuariofkcpersonal", "status", "empresa" };
        public string[] fieldscareas = new string[] { "idarea", "empresafkcempresas", "identificador", "nombreArea", " usuariofkcpersonal", "status" };
        public string[] fieldscatcategorias = new string[] { "idcategoria", "subgrupofkcdescfallo", "categoria", "usuariofkcpersonal", "empresa", "status" };
        public string[] fieldscatincidencias = new string[] { "idincidencia", "numeroIncidencia", "concepto", "personafkcpersonal", "status" };
        public string[] fieldscattipos = new string[] { "idcattipos", "Tipo", "Descripcion", " usuariofkcpersonal", "status", "empresa", "area" };
        public string[] fieldsccharolas = new string[] { "idcharola", "charola", "anaquelfkcanaqueles", "status", "empresa" };
        public string[] fieldscdescfallo = new string[] { "iddescfallo", "falloGralfkcfallosgrales", "descfallo", "usuariofkcpersonal", "empresa", "status" };
        public string[] fieldscempresas = new string[] { "idempresa", "nombreEmpresa", "usuariofkcpersonal", "logo", "empresa", "area", "status" };
        public string[] fieldscestaciones = new string[] { "idestacion", "estacion", "usuariofkcpersonal", "status" };
        public string[] fieldscfallosesp = new string[] { "idfalloEsp", "descfallofkcdescfallo", "codfallo", "falloesp", "usuariofkcpersonal", "status", "empresa" };
        public string[] fieldsfallosgrales = new string[] { " idFalloGral", "nombreFalloGral", "usuariofkcpersonal", "empresa", "status" };
        public string[] fieldscfamilias = new string[] { "idfamilia", "familiafkcnfamilias", "descripcionFamilia", "usuariofkcpersonal", "status", "umfkcunidadmedida", "empresa" };
        public string[] fieldscgiros = new string[] { "idgiro", "giro", "usuariofkcpersonal", "status", "empresa", "area" };
        public string[] fieldsciva = new string[] { "idiva", "iva", "personaFKcpersonal", "empresa" };
        public string[] fieldscladas = new string[] { "id", "iso", "name", "nicename", "iso3", "numcode", "phonecode" };
        public string[] fieldscmarcas = new string[] { "idmarca", "descripcionfkcfamilias", "marca", "personafkcpersonal", "status", "empresa" };
        public string[] fieldscmedidas = new string[] { "idcmedidas", "medida", "umfkcunidadmedida", "usuariofkcpersonal", "status" };
        public string[] fieldscmodelos = new string[] { "idmodelo", "modelo", "empresaMantenimiento", "usuariofkcpersonal", "status" };
        public string[] fieldscnfamilias = new string[] { "idcnFamilia", "Familia", "usuariofkcpersonal", "status", "empresa" };
        public string[] fieldscniveles = new string[] { "idnivel", "nivel", "pasillofkcpasillos", "usuariofkcpersonal", "status", "empresa" };
        public string[] fieldscomparativas = new string[] { "idcomparativa", "nombreComparativa", "descripcionComparativa", "observacionesComparativa", "fechaHoraCreacion", "IVA", "status", "usuariofkcpersonal", "empresa" };
        public string[] fieldscpasillos = new string[] { "idpasillo", "pasillo", "usuariofkcpersonal", "status", "empresa" };
        public string[] fieldscpersonal = new string[] { "idPersona", "credencial", "ApPaterno", "ApMaterno", "nombres", "cargofkcargos", "empresa", "idPersonalaltafkpersona", " area", "status" };
        public string[] fieldscproveedores = new string[] { "idproveedor", "aPaterno", "aMaterno", "nombres", "correo", "telefonoEmpresaUno", "TelefonoEmpresaDos", "TelefonoContactoUno", "TelefonoContactoDos", "empresa", "domiciliofksepomex", "Calle", "Numero", "Referencias", "Clasificacionfkcgiros", "idlada", "idladados", "idladatres", "idladacuatro", "paginaweb", "observaciones", "status", "ext1", "ext2", "ext3", "ext4", "usuariofkcpersonal", "lada1", "lada2", "lada3", "lada4", "empresaS", "Puesto" };
        public string[] fieldscrefacciones = new string[] { "idrefaccion", "codrefaccion", "nombreRefaccion", "modeloRefaccion", "proximoAbastecimiento", "charolafkcharolas", "existencias", "marcafkcmarcas", "fechaHoraalta", "usuarioaltafkcpersonal", "media", "abastecimiento", "descripcionRefaccion", "status", "empresa" };
        public string[] fieldscservicios = new string[] { "idservicio", "Nombre", "Descripcion", "usuariofkcpersonal", "status", "AreafkCareas" };
        public string[] fieldscunidades = new string[] { "idunidad", " consecutivo", "descripcioneco", "areafkcareas", " modelofkcmodelos", "serviciofkcservicios", "status", "bin", "nmotor", "ntransmision", "modelo", " Marca", "usuariofkcpersonal", "usuariofkcpersonaltri" };
        public string[] fieldscunidadmedida = new string[] { "idunidadmedida", "Nombre", "Simbolo", "status", "usuariofkcpersonal", "empresa" };
        public string[] fieldsdatosistema = new string[] { "iddato", "usuariofkcpersonal", "usuario", "password", "statusiniciosesion", "statuslogincellphone" };
        public string[] fieldsdetallesordencompra = new string[] { "idDetOrdCompra", "OrdfkOrdenCompra", "NumRefacc", "ClavefkCRefacciones", "Refacciones", "Cantidad", "Precio", "Total", "usuariofkcpersonal", "ObservacionesRefacc", "empresa" };
        public string[] fieldsencabezadoreportes = new string[] { "idencabezadoreportes", "reporte", "nombrereporte", "codigoreporte", "vigencia", "revision", "usuariofkcpersonal", "FechaHoraRegistro" };
        public string[] fieldsestatusvalidado = new string[] { "idestatusValidado", "idreportefkreportesupervicion", "seen" };
        public string[] fieldshuellasupervision = new string[] { "idHuella", "PersonafkCpersonal", "template", "calidad" };
        public string[] fieldsincidenciapersonal = new string[] { "idIncidencia", "Consecutivo", "ColaboradorfkCpersonal", "Fecha", "Hora", "Lugar", "Acta", "IncidenciafkCatIncidencias", "Sintesis", "Comentario", "SupervisorfkCpersonal", "JefefkCpersonal", "CoperativofkCpersonal", "testigofkCpersonal", "Estatus", "Conductorfkcpersonal", "FechaHoraRegistro", "usuarioFKcpersonal", "usuariofinalFKcpersonal" };
        public string[] fieldsladanac = new string[] { "idLadaNac", "Localidad", "Estado", "Clave" };
        public string[] fieldsmodificaciones_sistema = new string[] { "idmodificacion", "form", "idregistro", "ultimaModificacion", "usuariofkcpersonal", "fechaHora", "Tipo", "motivoActualizacion", "empresa", "area" };
        public string[] fieldsnombresoc = new string[] { "idnombresOC", "Almacen", "Autoriza", "personafkcpersonal" };
        public string[] fieldsordencompra = new string[] { "idOrdCompra", "FolioOrdCompra", "ProveedorfkCProveedores", "FacturadafkCEmpresas", "FechaOCompra", "FechaEntregaOCompra", "Subtotal", "IVA", "Total", "Estatus", "PersonaFinal", "usuariofkcpersonal", "ObservacionesOC", "ComparativaFKComparativas", "empresa" };
        public string[] fieldspedidosrefaccion = new string[] { "idPedRef", "FolioPedfkSupervicion", "NumRefacc", "RefaccionfkCRefaccion", "fechaHoraPedido", "FechaPedido", "Horapedido", "Cantidad", "EstatusRefaccion", "CantidadEntregada", "usuariofkcpersonal", "estatus" };
        public string[] fieldsprivilegios = new string[] { "idprivilegio", "usuariofkcpersonal", "namform", "ver", "privilegios" };
        public string[] fieldsproveedorescomparativa = new string[] { "idproveedorComparativa", "refaccionfkrefaccionesComparativa", "proveedorfkcproveedores", "precioUnitario", "observaciones", "mejorOpcion", "usuariofkcpersonal", "empresa" };
        public string[] fieldspuestos = new string[] { "idpuesto", "puesto", "empresa", "area", "usuariofkcpersonal", "status" };
        public string[] fieldsrefaccionescomparativa = new string[] { "idrefaccioncomparativa", "comparativafkcomparativas", "refaccionfkcrefacciones", "nombreRefaccion", "cantidad", "observaciones", "usuariofkcpersonal", "empresa" };
        public string[] fieldsrelacservicioestacion = new string[] { "idrelacServicioEstacion", "serviciofkcservicios", "estacionfkcestaciones", "usuariofkcpersonal", "status" };
        public string[] fieldsreportemantenimiento = new string[] { "IdReporte", "FoliofkSupervicion", "FalloGralfkFallosGenerales", "TrabajoRealizado", "MecanicofkPersonal", " MecanicoApoyofkPersonal", "FechaHoraI", "FechaHoraT", "PersonaFinal", " FolioFactura", "Estatus", "SupervisofkPersonal", "StatusRefacciones", "ObservacionesM", "seen", "seenAlmacen", "empresa" };
        public string[] fieldsreportepercance = new string[] { "idreportePercance", "consecutivo", "ecofkcunidades", "conductorfkcpersonal", "fechaHoraAccidente", "servicioenlaborfkcservicios", "lugaraccidente", "direccion", "estacion1fkcestaciones", "estacion2fkcestaciones", "estacion3fkcestaciones", "estacion4fkcestaciones", "ecorecuperacionfkcunidades", "estacionfkcestaciones", "sintesisocurrido", "coordenadasimagenes", "descripcion", "marcavehiculotercero", "yearvehiculotercero", "placasvehiculotercero", "nombreconductortercero", "telefonoconductortercero", "domicilioconductortercero", "numreporteseguro", "horaotorgamiento", "horallegadaseguro", "nombreajustador", "solucion", "numacta", "supervisorkcpersonal", "unidadmedica", "perteneceunidad", "nombreResponsableunidad", "encasolesionados", "dibujo", "comentarios", "firmaconductorfkcpersonal", "firmasupervisorfkcpersonal", "finalizado", "usuarioFinalizofkcpersonal", "dibujoExportado", "fechaHoraInsercion", "usuarioinsertofkcpersonal", "evidencia1", "evidencia2", "evidencia3", "evidencia4", "firmas" };
        public string[] fieldsreportepersonal = new string[] { "idreportepersonal", "ConsecutivoRP", "credencialfkcpersonal", "Estatus", "Fecha", "Hora", "LugarIncidente", "TipoVehObj", "Kilometraje", "responsablefkcpersonal", "coordinadorfkcpersonal", "Observaciones", "FechaHoraRegistro", "usuariofkcpersonal", "usuariofinalizofkcpersonal" };
        public string[] fieldsreportesupervicion = new string[] { "idReporteSupervicion", " Folio", "UnidadfkCUnidades", "FechaReporte", " SupervisorfkCPersonal", "CredencialConductorfkCPersonal", "Serviciofkcservicios", "HoraEntrada", "KmEntrada", "TipoFallo", "DescrFallofkcdescfallo", "CodFallofkcfallosesp", " DescFalloNoCod", "ObservacionesSupervision", " seen" };
        public string[] fieldsreportetri = new string[] { "idReporteTransinsumos", "idreportemfkreportemantenimiento", "FolioFactura", "FechaEntrega", "PersonaEntregafkcPersonal", "ObservacionesTrans", "empresa" };
        public string[] fieldssepomex = new string[] { "id", "idEstado", "estado", "idMunicipio", "municipio", "ciudad", "zona", "cp", "asentamiento", "tipo" };
        public string[] fieldsvigencias_supervision = new string[] { "idvigencia", "usuariofkcpersonal", "fechaEmisionTarjeton", "fechaVencimientoTarjeton", "tipolicenciafkcattipos", "fechaEmisionConducir", "fechaVencimientoConducir", "empresa", "area" };
    }
}