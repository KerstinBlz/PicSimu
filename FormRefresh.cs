/**************************************************************************
**
**  KerTKDSim
**
**  FormRefresh.cs: 
**  ---------
**  Refreshes the program page.
**
**  
**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace Pic_Simulator
{
    public partial class KerTKDSim : Form
    {
        private delegate void refreshRegkDelegate();
        
        private void refreshReg()
        {
            if (InvokeRequired)
            {
                var invokeVar = new refreshRegkDelegate(refreshReg);
                Invoke(invokeVar);
            }
            else
            {
                int zeile;
                int spalte;

                for(int i = 0 ; i < 127 ; i++ )
                {
                    zeile  = i / 8;
                    spalte = i % 8;
                    
                    ArrayBank0[zeile,spalte] = iReg[i].ToString("X").PadLeft(2, '0');
                }

                for (int i = 128; i < 255; i++)
                {
                    zeile = i / 8;
                    zeile %= 16;
                    spalte = i % 8;

                    ArrayBank1[zeile, spalte] = iReg[i].ToString("X").PadLeft(2, '0');
                }
            }
        }

        private delegate void refreshStatusRegkDelegate();

        private void refreshStatusReg()
        {
            if(InvokeRequired)
            {
                var invokeVar = new refreshStatusRegkDelegate( refreshStatusReg );
                Invoke(invokeVar);
            }
            else 
            {
            
            }
        }
    }
}
