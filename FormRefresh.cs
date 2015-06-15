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

                string statusReg = null;

                for ( int i = 0 ; i < 8 ; i++ )
                { 
                    statusReg += ArrayStatusReg[i].ToString();   // that should work
                }

                statusReg.PadLeft( 2 , '0' );
                iReg[0x03] = Convert.ToInt32( statusReg , 2 );
                iReg[0x83] = iReg[0x03];

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   updateTrisA_Click
        **
        **  Ein Klick auf ein Bit im TRISA-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateTrisA_ClickDelegate( object sender , EventArgs e );
        private void updateTrisA_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateTrisA_ClickDelegate( updateTrisA_Click );
                Invoke( invokeVar );
            }
            else
            {
                Control ctrl = (Control)sender;
                if (ctrl.Text == "o")
                {
                    ctrl.Text = "i";
                }
                else
                {
                    ctrl.Text = "o";
                }

                //String trisAReg;
                string trisAReg =  tbTrisA4.Text +
                                   tbTrisA3.Text +
                                   tbTrisA2.Text +
                                   tbTrisA1.Text +
                                   tbTrisA0.Text;

                trisAReg = trisAReg.Replace( "o" , "0" );
                trisAReg = trisAReg.Replace( "i" , "1" );
                trisAReg.PadLeft( 2 , '0' );
                iReg[0x85] = Convert.ToInt32( trisAReg , 2 );

                refreshReg( );
            }
        }

        private delegate void updateOptionReg_ClickkDelegate( object sender , EventArgs e );
        /// <summary>
        /// Registers the click on the Option Register
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private void updateOptionReg_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateOptionReg_ClickkDelegate( updateOptionReg_Click );
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

                string OptionReg = null;

                for (int i = 0 ; i < 8 ; i++)
                {
                    OptionReg += ArrayInterruptReg[i].ToString( );
                }

                OptionReg.PadLeft( 2 , '0' );
                iReg[0x81] = Convert.ToInt32( OptionReg , 2 );

                refreshReg( );
            }
        }


        private delegate void updateIntReg_ClickkDelegate( object sender , EventArgs e );

        /// <summary>
        /// Registers the click on the INTCON Register
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private void updateIntReg_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateIntReg_ClickkDelegate( updateIntReg_Click );
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

                string IntReg = null;

                for (int i = 0 ; i < 8 ; i++)
                {
                    IntReg += ArrayInterruptReg[i].ToString( ); 
                }

                IntReg.PadLeft( 2 , '0' );
                iReg[0xB] = Convert.ToInt32( IntReg , 2 );
                iReg[0x8B] = iReg[0xB];

                refreshReg( );
            }
        }

        private delegate void updateWatchDogDelegate( object sender , EventArgs e );

        /// <summary>
        /// Sets the watchdog on or off
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private void updateWatchDog( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateWatchDogDelegate( updateWatchDog );
                Invoke( invokeVar );
            }
            else
            {
                if ( bWatchdog )
                {
                    bWatchdog = false;
                    tbWatchDogSw.Text = "OFF";
                    tbWatchDogSw.BackColor = Color.Red;
                }
                else
                {
                    bWatchdog = true;
                    tbWatchDogSw.Text = "ON";
                    tbWatchDogSw.BackColor = Color.Green;
                }
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

                tbWReg.Text = iWReg.ToString( "X" ).PadLeft( 2 , '0' );         // W-Register
                tbPC.Text = iPC.ToString( "X" ).PadLeft( 2 , '0' );             // Programme counter
                tblCycles.Text = lCycles.ToString( "D" ).PadLeft( 2 , '0' );    // Cycles       
                tbRuntime.Text = lRuntime.ToString( "D" ).PadLeft( 2 , '0' );   // Runtime counter
                tbWatchdog.Text = lWatchdog.ToString( "D" ).PadLeft( 2 , '0' ); // Watchdog


                // Prescaler

                _iPrescalerValue = Convert.ToInt32( Math.Pow( 2 , 1 + (iReg[0x81] & 0x07) ) );
                if ((iReg[0x81] & 0x08) == 0x08)
                {
                    _iPrescalerValue = ( _iPrescalerValue / 2);
                    tbPSCAssign.Text = "WDT";
                }
                else
                {
                    tbPSCAssign.Text = "TMR0";
                }
                tbPrescaler.Text = "1:" + Convert.ToString( _iPrescalerValue );
            }
        }


        private delegate void updateSerielConDelegate( object sender , EventArgs e );

        /// <summary>
        /// Sets up the serial connection with the right port
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private void updateSerialCom( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateSerielConDelegate( updateSerialCom );
                Invoke( invokeVar );
            }
            else
            {
                if ( bSerialCon )
                {
                    bSerialCon = false;
                    tbSerialState.Text = "OFF";
                    tbSerialState.BackColor = Color.Red;
                }
                else
                {
                    bSerialCon = true;
                    tbSerialState.Text = "ON";
                    tbSerialState.BackColor = Color.Green;
                }
            }
        }
    }
}
