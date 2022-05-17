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
    public partial class ReporteCometadeOro : Form
    {

          validaciones v;
          int idUsuario, empresa, area;

        public ReporteCometadeOro(int idUsuario, int empresa, int area, validaciones v)
        {
            InitializeComponent();
            this.idUsuario = idUsuario;
            this.empresa = empresa;
            this.area = area;
            this.v = v;

        }

        public ReporteCometadeOro()
        {
            
        }

    

        }

    /*ACTUALIZACION 17-05-2022 REPORTE UNIDADES EXTERNAS*/
    }

