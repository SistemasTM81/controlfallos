using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlFallos
{
    
    public class checarbotones
    {
        validaciones v;

        public checarbotones(validaciones v)
        {
            this.v = v;
        }
        public void cambiarestado(object sender, EventArgs e)
        {
            v.CambiarEstado_Click(sender, e);
        }

    }
}
