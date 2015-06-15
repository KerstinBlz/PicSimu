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
        private delegate void updateStatus_ClickDelegate( object sender , EventArgs e );

        /// <summary>
        /// Registers clicks in the Status register fields
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private void updateStatus_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateStatus_ClickDelegate( updateStatus_Click );
                Invoke( invokeVar );
            }
            else
            {
                Control ctrl = (Control)sender;
                if (ctrl.Text == "0")
                {
                    ctrl.Text = "1";
                }
                else
                {
                    ctrl.Text = "0";
                }

                string statusReg = tStatus7.Text +
                                   tStatus6.Text +
                                   tStatus5.Text +
                                   tStatus4.Text +
                                   tStatus3.Text +
                                   tStatus2.Text +
                                   tStatus1.Text +
                                   tStatus0.Text;
                statusReg.PadLeft( 2 , '0' );
                iReg[0x03] = Convert.ToInt32( statusReg , 2 );
                iReg[0x83] = iReg[0x03];

                listCode.Focus( );

                refreshReg( );
            }
        }


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
