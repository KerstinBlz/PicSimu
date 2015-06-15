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
                    statusReg += ArrayStatusReg[i];   // that should work
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

        private delegate void updateTrisB_ClickDelegate( object sender , EventArgs e );
        private void updateTrisB_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateTrisB_ClickDelegate( updateTrisB_Click );
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

                string trisBReg = tbTrisB7.Text +
                                   tbTrisB6.Text +
                                   tbTrisB5.Text +
                                   tbTrisB4.Text +
                                   tbTrisB3.Text +
                                   tbTrisB2.Text +
                                   tbTrisB1.Text +
                                   tbTrisB0.Text;
                trisBReg = trisBReg.Replace( "o" , "0" );
                trisBReg = trisBReg.Replace( "i" , "1" );
                trisBReg.PadLeft( 2 , '0' );
                iReg[0x86] = Convert.ToInt32( trisBReg , 2 );

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

                // Status register



                //RegStatusupdate fuer GUI

                if ( ( iReg[0x03] & 0x01 ) == 0x01)
                {
                    ArrayStatusReg[0] = "1";
                }
                else
                {
                    ArrayStatusReg[0] = "0";
                }
                if ((iReg[0x03] & 0x02) == 0x02)
                {
                    ArrayStatusReg[1] = "1";
                }
                else
                {
                    ArrayStatusReg[1] = "0";
                }

                if ((iReg[0x03] & 0x04) == 0x04)
                {
                    ArrayStatusReg[2] = "1";
                }
                else
                {
                    ArrayStatusReg[2] = "0";
                }
                if ((iReg[0x03] & 0x08) == 0x08)
                {
                    ArrayStatusReg[3] = "1";
                }
                else
                {
                    ArrayStatusReg[3] = "0";
                }
                if ((iReg[0x03] & 0x10) == 0x10)
                {
                    ArrayStatusReg[4] = "1";
                }
                else
                {
                    ArrayStatusReg[4] = "0";
                }
                if ((iReg[0x03] & 0x20) == 0x20)
                {
                    ArrayStatusReg[5] = "1";
                }
                else
                {
                    ArrayStatusReg[5] = "0";
                }
                if ((iReg[0x03] & 0x40) == 0x40)
                {
                    ArrayStatusReg[6] = "1";
                }
                else
                {
                    ArrayStatusReg[6] = "0";
                }
                if ((iReg[0x03] & 0x01) == 0x01)
                {
                    ArrayStatusReg[0] = "1";
                }
                else
                {
                    ArrayStatusReg[0] = "0";
                }
                if ((iReg[0x03] & 0x02) == 0x02)
                {
                    ArrayStatusReg[1] = "1";
                }
                else
                {
                    ArrayStatusReg[1] = "0";
                }

                if ((iReg[0x03] & 0x04) == 0x04)
                {
                    ArrayStatusReg[2] = "1";
                }
                else
                {
                    ArrayStatusReg[2] = "0";
                }
                if ((iReg[0x03] & 0x08) == 0x08)
                {
                    ArrayStatusReg[3] = "1";
                }
                else
                {
                    ArrayStatusReg[3] = "0";
                }
                if ((iReg[0x03] & 0x10) == 0x10)
                {
                    ArrayStatusReg[4] = "1";
                }
                else
                {
                    ArrayStatusReg[4] = "0";
                }
                if ((iReg[0x03] & 0x20) == 0x20)
                {
                    ArrayStatusReg[5] = "1";
                }
                else
                {
                    ArrayStatusReg[5] = "0";
                }
                if ((iReg[0x03] & 0x40) == 0x40)
                {
                    ArrayStatusReg[6] = "1";
                }
                else
                {
                    ArrayStatusReg[6] = "0";
                }


                //PortRA
                if ((iReg[0x05] & 0x01) == 0x01)
                {
                    tRA0.Text = "1";
                }
                else
                {
                    tRA0.Text = "0";
                }

                if ((iReg[0x05] & 0x02) == 0x02)
                {
                    tRA1.Text = "1";
                }
                else
                {
                    tRA1.Text = "0";
                }


                if ((iReg[0x05] & 0x04) == 0x04)
                {
                    tRA2.Text = "1";
                }
                else
                {
                    tRA2.Text = "0";
                }
                if ((iReg[0x05] & 0x08) == 0x08)
                {
                    tRA3.Text = "1";
                }
                else
                {
                    tRA3.Text = "0";
                }
                if ((iReg[0x05] & 0x10) == 0x10)
                {
                    tRA4.Text = "1";
                }
                else
                {
                    tRA4.Text = "0";
                }

                //PortRB
                if ((iReg[0x06] & 0x01) == 0x01)
                {
                    tRB0.Text = "1";
                }
                else
                {
                    tRB0.Text = "0";
                }
                if ((iReg[0x06] & 0x02) == 0x02)
                {
                    tRB1.Text = "1";
                }
                else
                {
                    tRB1.Text = "0";
                }
                if ((iReg[0x06] & 0x04) == 0x04)
                {
                    tRB2.Text = "1";
                }
                else
                {
                    tRB2.Text = "0";
                }
                if ((iReg[0x06] & 0x08) == 0x08)
                {
                    tRB3.Text = "1";
                }
                else
                {
                    tRB3.Text = "0";
                }
                if ((iReg[0x06] & 0x10) == 0x10)
                {
                    tRB4.Text = "1";
                }
                else
                {
                    tRB4.Text = "0";
                }
                if ((iReg[0x06] & 0x20) == 0x20)
                {
                    tRB5.Text = "1";
                }
                else
                {
                    tRB5.Text = "0";
                }
                if ((iReg[0x06] & 0x40) == 0x40)
                {
                    tRB6.Text = "1";
                }
                else
                {
                    tRB6.Text = "0";
                }
                if ((iReg[0x06] & 0x80) == 0x80)
                {
                    tRB7.Text = "1";
                }
                else
                {
                    tRB7.Text = "0";
                }

                //OptionRegister

                if ((iReg[0x81] & 0x01) == 0x01)
                {

                    ArrayOptionReg[0] = "1";
                }
                else
                {
                    ArrayOptionReg[0] = "0";
                }
                if ((iReg[0x81] & 0x02) == 0x02)
                {
                    ArrayOptionReg[1] = "1";
                }
                else
                {
                    ArrayOptionReg[1] = "0";
                }
                if ((iReg[0x81] & 0x04) == 0x04)
                {
                    ArrayOptionReg[2] = "1";
                }
                else
                {
                    ArrayOptionReg[2] = "0";
                }
                if ((iReg[0x81] & 0x08) == 0x08)
                {
                    ArrayOptionReg[3] = "1";
                }
                else
                {
                    ArrayOptionReg[3] = "0";
                }
                if ((iReg[0x81] & 0x10) == 0x10)
                {
                    ArrayOptionReg[4] = "1";
                }
                else
                {
                    ArrayOptionReg[4] = "0";
                }
                if ((iReg[0x81] & 0x20) == 0x20)
                {
                    ArrayOptionReg[5] = "1";
                }
                else
                {
                    ArrayOptionReg[5] = "0";
                }
                if ((iReg[0x81] & 0x40) == 0x40)
                {
                    ArrayOptionReg[6] = "1";
                }
                else
                {
                    ArrayOptionReg[6] = "0";
                }
                if ((iReg[0x81] & 0x80) == 0x80)
                {
                    ArrayOptionReg[7] = "1";
                }
                else
                {
                    ArrayOptionReg[7] = "0";
                }


                //TRISA
                if ((iReg[0x85] & 0x01) == 0x01)
                {
                    tbTrisA0.Text = "i";
                }
                else
                {
                    tbTrisA0.Text = "o";
                }
                if ((iReg[0x85] & 0x02) == 0x02)
                {
                    tbTrisA1.Text = "i";
                }
                else
                {
                    tbTrisA1.Text = "o";
                }
                if ((iReg[0x85] & 0x04) == 0x04)
                {
                    tbTrisA2.Text = "i";
                }
                else
                {
                    tbTrisA2.Text = "o";
                }
                if ((iReg[0x85] & 0x08) == 0x08)
                {
                    tbTrisA3.Text = "i";
                }
                else
                {
                    tbTrisA3.Text = "o";
                }
                if ((iReg[0x85] & 0x10) == 0x10)
                {
                    tbTrisA4.Text = "i";
                }
                else
                {
                    tbTrisA4.Text = "o";
                }
                //TRISB
                if ((iReg[0x86] & 0x01) == 0x01)
                {
                    tbTrisB0.Text = "i";
                }
                else
                {
                    tbTrisB0.Text = "o";
                }
                if ((iReg[0x86] & 0x02) == 0x02)
                {
                    tbTrisB1.Text = "i";
                }
                else
                {
                    tbTrisB1.Text = "o";
                }
                if ((iReg[0x86] & 0x04) == 0x04)
                {
                    tbTrisB2.Text = "i";
                }
                else
                {
                    tbTrisB2.Text = "o";
                }
                if ((iReg[0x86] & 0x08) == 0x08)
                {
                    tbTrisB3.Text = "i";
                }
                else
                {
                    tbTrisB3.Text = "o";
                }
                if ((iReg[0x86] & 0x10) == 0x10)
                {
                    tbTrisB4.Text = "i";
                }
                else
                {
                    tbTrisB4.Text = "o";
                }
                if ((iReg[0x86] & 0x20) == 0x20)
                {
                    tbTrisB5.Text = "i";
                }
                else
                {
                    tbTrisB5.Text = "o";
                }
                if ((iReg[0x86] & 0x40) == 0x40)
                {
                    tbTrisB6.Text = "i";
                }
                else
                {
                    tbTrisB6.Text = "o";
                }
                if ((iReg[0x86] & 0x80) == 0x80)
                {
                    tbTrisB7.Text = "i";
                }
                else
                {
                    tbTrisB7.Text = "o";
                }


                //InterruptRegister
                if ((iReg[0x0B] & 0x01) == 0x01)
                {
                    ArrayInterruptReg[0] = "1";
                }
                else
                {
                    ArrayInterruptReg[0] = "0";
                }
                if ((iReg[0x0B] & 0x02) == 0x02)
                {
                    ArrayInterruptReg[1] = "1";
                }
                else
                {
                    ArrayInterruptReg[1] = "0";
                }
                if ((iReg[0x0B] & 0x04) == 0x04)
                {
                    ArrayInterruptReg[2] = "1";
                }
                else
                {
                    ArrayInterruptReg[2] = "0";
                }
                if ((iReg[0x0B] & 0x08) == 0x08)
                {
                    ArrayInterruptReg[3] = "1";
                }
                else
                {
                    ArrayInterruptReg[3] = "0";
                }
                if ((iReg[0x0B] & 0x10) == 0x10)
                {
                    ArrayInterruptReg[4] = "1";
                }
                else
                {
                    ArrayInterruptReg[4] = "0";
                }
                if ((iReg[0x0B] & 0x20) == 0x20)
                {
                    ArrayInterruptReg[5] = "1";
                }
                else
                {
                    ArrayInterruptReg[5] = "0";
                }
                if ((iReg[0x0B] & 0x40) == 0x40)
                {
                    ArrayInterruptReg[6] = "1";
                }
                else
                {
                    ArrayInterruptReg[6] = "0";
                }
                if ((iReg[0x0B] & 0x80) == 0x80)
                {
                    ArrayInterruptReg[7] = "1";
                }
                else
                {
                    ArrayInterruptReg[7] = "0";
                }


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
            refreshGridValue();
        }
        

        /*********************************************************************/
        /**   registryChanged
        **
        **  Click at the portRA registry 
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void portRA_ClickDelegate( object sender , EventArgs e );
        private void portRA_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new portRA_ClickDelegate( updateStatus_Click );
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

                string portRA = tRA4.Text +
                                tRA3.Text +
                                tRA2.Text +
                                tRA1.Text +
                                tRA0.Text;
                
                portRA.PadLeft( 2 , '0' );
                iReg[0x05] = Convert.ToInt32( portRA , 2 );

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   registryChanged
        **
        **  Wenn ein Wert in der Registry geändert wurde, wird der in der Registry übernommen
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void registryChangedDelegate( object sender , KeyPressEventArgs e );
        private void registryChanged( object sender , KeyPressEventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeHelper = new registryChangedDelegate( registryChanged );
                Invoke( invokeHelper , sender , e );
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    try
                    {
                        Control ctrl = (Control)sender;

                        int iValue = Convert.ToInt32( ctrl.Text , 16 );
                        int iRegAdress = Convert.ToInt32( ctrl.Name.Remove( 0 , 5 ) );

                        if (iRegAdress == 0x05 || iRegAdress == 0x0A || iRegAdress == 0x85 || iRegAdress == 0x88 || iRegAdress == 0x8A)
                        {
                            if (iValue >= 0x00 && iValue <= 0x1F)
                            {
                                iReg[iRegAdress] = iValue;
                                refreshReg( );
                            }
                            else
                            {
                                refreshReg( );
                                MessageBox.Show( "Es sind nur Hex-Werte von 0 bis 1F in diesem Register erlaubt!" , "Falsche Eingabe" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                            }
                        }

                        else
                        {
                            if (iValue >= 0x00 && iValue <= 0xFF)
                            {
                                iReg[iRegAdress] = iValue;
                                refreshReg( );
                            }
                            else
                            {
                                refreshReg( );
                                MessageBox.Show( "Es sind nur Hex-Werte von 0 bis FF in diesem Register erlaubt!" , "Falsche Eingabe" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        refreshReg( );
                        MessageBox.Show( "Es sind nur Hex-Werte von 0 bis FF in diesem Register erlaubt!" , "Falsche Eingabe" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                    }
                }
            }
        }

        /*********************************************************************/
        /**   portRB_Click
        **
        **  Ein Klick auf ein Bit im PortRB-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void portRB_ClickDelegate( object sender , EventArgs e );
        private void portRB_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new portRB_ClickDelegate( updateStatus_Click );
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

                string portRB = tRB7.Text +
                                tRB6.Text +
                                tRB5.Text +
                                tRB4.Text +
                                tRB3.Text +
                                tRB2.Text +
                                tRB1.Text +
                                tRB0.Text;

                portRB.PadLeft( 2 , '0' );

                iReg[0x06] = Convert.ToInt32( portRB , 2 );

                lvCode.Focus( );

                refreshGridValue( );
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
