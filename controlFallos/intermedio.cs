using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace controlFallos
{
    public class intermedio
    {
        metodos mt = new metodos();
        MySqlDataAdapter adaptador = new MySqlDataAdapter();
        MySqlDataAdapter adaptador2 = new MySqlDataAdapter();
        DataSet Ds = new DataSet();
        DataSet Ds2 = new DataSet();
        DataTable dt = new DataTable();
        DataSet DsE = new DataSet();
        DataSet Ds2E = new DataSet();
        DataTable dtE = new DataTable();
        int total = 0;
        public DataSet datosG(string sql, int valor)
        {
            dt.Clear();
            Ds.Clear();
            Ds2.Clear();
            adaptador = mt.obtener_informacion(sql);
            adaptador.Fill(dt);
            total = dt.Rows.Count;
            adaptador.Fill(Ds2);
            adaptador.Fill(Ds, valor, 10, "reportemantenimiento");
            return Ds;
        }

        public DataSet datosEn(string sql, int valor)
        {
            dtE.Clear();
            DsE.Clear();
            Ds2E.Clear();
            adaptador2 = mt.obtener_informacion(sql);
            adaptador2.Fill(dtE);
            total = dtE.Rows.Count;
            adaptador2.Fill(DsE);
            adaptador2.Fill(Ds2E, valor, 10, "Entradas");
            return Ds2E;
        }

        public DataSet exportar(string Probiene)
        {
            DataSet regresaExp = new DataSet();
            if (Probiene =="E")
            {
                regresaExp = DsE;
            }
            else if (Probiene == "S")
            {
                regresaExp = Ds2;
            }
            return regresaExp;
        }

        public void llenarCombo(string consulta, ComboBox cmb, string ValueMember, string DisplayMember, string primerdato)
        {
            cmb.DataSource = null;
            DataTable tb = new DataTable();
            tb = mt.obtener_cmb(consulta);
            DataRow nueva = tb.NewRow();
            nueva[ValueMember] = 0;
            nueva[DisplayMember] = primerdato.ToUpper();
            tb.Rows.InsertAt(nueva,0);
            cmb.ValueMember = ValueMember;
            cmb.DisplayMember = DisplayMember;
            cmb.DataSource = tb;
        }
        public DataSet recorrerS(int valor)
        {
            if (total >= valor && valor > 0)
            {
                adaptador.Fill(Ds, valor, 10, "reportemantenimiento");
                
            }
            return Ds;
         }

        public DataSet recorrerE(int valor)
        {
            if (total >= valor && valor > 0)
            {
                adaptador2.Fill(Ds2E, valor, 10, "Entradas");

            }
            return Ds2E;
        }
    }
}
