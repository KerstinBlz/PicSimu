/**************************************************************************
**
**  MisCip
**  ~~~~~~~~~~
**
**  FormRefresh.cs: 
**  ---------
**  Aktualisiert die Anzeige des Hauptfensters
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

        /*********************************************************************/
        /**   updateStatus_Click
        **
        **  Ein Klick auf ein Bit im Status-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateStatus_ClickDelegate( object sender , EventArgs e );
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

                lvCode.Focus( );

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
                string trisAReg = tbTrisA4.Text +
                                   tbTrisA3.Text +
                                   tbTrisA2.Text +
                                   tbTrisA1.Text +
                                   tbTrisA0.Text;

                trisAReg = trisAReg.Replace( "o" , "0" );
                trisAReg = trisAReg.Replace( "i" , "1" );
                trisAReg.PadLeft( 2 , '0' );
                iReg[0x85] = Convert.ToInt32( trisAReg , 2 );

                lvCode.Focus( );

                refreshReg( );
            }
        }
        private delegate void cbDelay_SelectedIndexChangedDelegate( object sender , EventArgs e );
        private void cbDelay_SelectedIndexChanged( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new cbDelay_SelectedIndexChangedDelegate( cbDelay_SelectedIndexChanged );
                Invoke( invokeVar );
            }
            else
            {
                _iDelay = Convert.ToInt32( cbDelay.SelectedItem );
            }
        }

        /*********************************************************************/
        /**   updateTrisB_Click
        **
        **  Ein Klick auf ein Bit im TRISB-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

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

                lvCode.Focus( );

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   registryChanged
        **
        **  Ein Klick auf ein Bit im PortRA-Register wird registriert
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
                lvCode.Focus( );

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
                        lvCode.Focus( );

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
                    catch (Exception /*ex*/)
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

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   updateOptionReg_Click
        **
        **  Ein Klick auf ein Bit im Option-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateOptionReg_ClickkDelegate( object sender , EventArgs e );
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

                string OptionReg = tbOption7.Text +
                                   tbOption6.Text +
                                   tbOption5.Text +
                                   tbOption4.Text +
                                   tbOption3.Text +
                                   tbOption2.Text +
                                   tbOption1.Text +
                                   tbOption0.Text;
                OptionReg.PadLeft( 2 , '0' );
                iReg[0x81] = Convert.ToInt32( OptionReg , 2 );

                lvCode.Focus( );

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   updateIntReg_Click
        **
        **  Ein Klick auf ein Bit im INTCON-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateIntReg_ClickkDelegate( object sender , EventArgs e );
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

                string IntReg = tbInt7.Text +
                                tbInt6.Text +
                                tbInt5.Text +
                                tbInt4.Text +
                                tbInt3.Text +
                                tbInt2.Text +
                                tbInt1.Text +
                                tbInt0.Text;
                IntReg.PadLeft( 2 , '0' );
                iReg[0xB] = Convert.ToInt32( IntReg , 2 );
                iReg[0x8B] = iReg[0xB];

                lvCode.Focus( );

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   updateSerialCom
        **
        **  Sets a Serial Connection 
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateSerielConDelegate( object sender , EventArgs e );
        private void updateSerialCom( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateSerielConDelegate( updateSerialCom );
                Invoke( invokeVar );
            }
            else
            {
                if (bSerialCon)
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
                lvCode.Focus( );
            }
        }

        /*********************************************************************/
        /**   updateWatchDog
        **
        **  Schaltet den Watchdog an oder aus
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateWatchDogDelegate( object sender , EventArgs e );
        private void updateWatchDog( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateWatchDogDelegate( updateWatchDog );
                Invoke( invokeVar );
            }
            else
            {
                if (bWatchdog)
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
                lvCode.Focus( );
            }
        }

        /*********************************************************************/
        /**   updateEECONReg_Click
        **
        **  Ein Klick auf ein Bit im EECON1-Register wird registriert
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void updateEECONReg_ClickDelegate( object sender , EventArgs e );
        private void updateEECONReg_Click( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeVar = new updateEECONReg_ClickDelegate( updateEECONReg_Click );
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

                string EECONReg = tbEECON4.Text +
                                   tbEECON3.Text +
                                   tbEECON2.Text +
                                   tbEECON1.Text +
                                   tbEECON0.Text;
                EECONReg.PadLeft( 2 , '0' );
                iReg[0x88] = Convert.ToInt32( EECONReg , 2 );

                lvCode.Focus( );

                refreshReg( );
            }
        }

        /*********************************************************************/
        /**   refreshReg
        **
        **  Aktualisiert alle Registerfelder auf der GUI sowie den Stack
        **
        **  Ret: void
        **
        **************************************************************************/

        private delegate void refreshRegkDelegate();
        private void refreshReg()
        {
            if (InvokeRequired)
            {
                var invokeVar = new refreshRegkDelegate( refreshReg );
                Invoke( invokeVar );
            }

            tbWReg.Text = iWReg.ToString( "X" ).PadLeft( 2 , '0' );   //W-Register aktualisieren
            tbPC.Text = iPC.ToString( "X" ).PadLeft( 2 , '0' );    //Programmcounter
            tblCycles.Text = lCycles.ToString( "D" ).PadLeft( 2 , '0' );    //Zyklen            
            tbRuntime.Text = lRuntime.ToString( "D" ).PadLeft( 2 , '0' );   //Laufzeitzähler
            tbWatchdog.Text = lWatchdog.ToString( "D" ).PadLeft( 2 , '0' ); //Watchdog
            
            
            refreshGridValue( );



            //RegStatusupdate fuer GUI

            if ((iReg[0x03] & 0x01) == 0x01)
            {
                tStatus0.Text = "1";
            }
            else
            {
                tStatus0.Text = "0";
            }
            if ((iReg[0x03] & 0x02) == 0x02)
            {
                tStatus1.Text = "1";
            }
            else
            {
                tStatus1.Text = "0";
            }

            if ((iReg[0x03] & 0x04) == 0x04)
            {
                tStatus2.Text = "1";
            }
            else
            {
                tStatus2.Text = "0";
            }
            if ((iReg[0x03] & 0x08) == 0x08)
            {
                tStatus3.Text = "1";
            }
            else
            {
                tStatus3.Text = "0";
            }
            if ((iReg[0x03] & 0x10) == 0x10)
            {
                tStatus4.Text = "1";
            }
            else
            {
                tStatus4.Text = "0";
            }
            if ((iReg[0x03] & 0x20) == 0x20)
            {
                tStatus5.Text = "1";
            }
            else
            {
                tStatus5.Text = "0";
            }
            if ((iReg[0x03] & 0x40) == 0x40)
            {
                tStatus6.Text = "1";
            }
            else
            {
                tStatus6.Text = "0";
            }
            if ((iReg[0x03] & 0x01) == 0x01)
            {
                tStatus0.Text = "1";
            }
            else
            {
                tStatus0.Text = "0";
            }
            if ((iReg[0x03] & 0x02) == 0x02)
            {
                tStatus1.Text = "1";
            }
            else
            {
                tStatus1.Text = "0";
            }

            if ((iReg[0x03] & 0x04) == 0x04)
            {
                tStatus2.Text = "1";
            }
            else
            {
                tStatus2.Text = "0";
            }
            if ((iReg[0x03] & 0x08) == 0x08)
            {
                tStatus3.Text = "1";
            }
            else
            {
                tStatus3.Text = "0";
            }
            if ((iReg[0x03] & 0x10) == 0x10)
            {
                tStatus4.Text = "1";
            }
            else
            {
                tStatus4.Text = "0";
            }
            if ((iReg[0x03] & 0x20) == 0x20)
            {
                tStatus5.Text = "1";
            }
            else
            {
                tStatus5.Text = "0";
            }
            if ((iReg[0x03] & 0x40) == 0x40)
            {
                tStatus6.Text = "1";
            }
            else
            {
                tStatus6.Text = "0";
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
                tbOption0.Text = "1";
            }
            else
            {
                tbOption0.Text = "0";
            }
            if ((iReg[0x81] & 0x02) == 0x02)
            {
                tbOption1.Text = "1";
            }
            else
            {
                tbOption1.Text = "0";
            }
            if ((iReg[0x81] & 0x04) == 0x04)
            {
                tbOption2.Text = "1";
            }
            else
            {
                tbOption2.Text = "0";
            }
            if ((iReg[0x81] & 0x08) == 0x08)
            {
                tbOption3.Text = "1";
            }
            else
            {
                tbOption3.Text = "0";
            }
            if ((iReg[0x81] & 0x10) == 0x10)
            {
                tbOption4.Text = "1";
            }
            else
            {
                tbOption4.Text = "0";
            }
            if ((iReg[0x81] & 0x20) == 0x20)
            {
                tbOption5.Text = "1";
            }
            else
            {
                tbOption5.Text = "0";
            }
            if ((iReg[0x81] & 0x40) == 0x40)
            {
                tbOption6.Text = "1";
            }
            else
            {
                tbOption6.Text = "0";
            }
            if ((iReg[0x81] & 0x80) == 0x80)
            {
                tbOption7.Text = "1";
            }
            else
            {
                tbOption7.Text = "0";
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
                tbInt0.Text = "1";
            }
            else
            {
                tbInt0.Text = "0";
            }
            if ((iReg[0x0B] & 0x02) == 0x02)
            {
                tbInt1.Text = "1";
            }
            else
            {
                tbInt1.Text = "0";
            }
            if ((iReg[0x0B] & 0x04) == 0x04)
            {
                tbInt2.Text = "1";
            }
            else
            {
                tbInt2.Text = "0";
            }
            if ((iReg[0x0B] & 0x08) == 0x08)
            {
                tbInt3.Text = "1";
            }
            else
            {
                tbInt3.Text = "0";
            }
            if ((iReg[0x0B] & 0x10) == 0x10)
            {
                tbInt4.Text = "1";
            }
            else
            {
                tbInt4.Text = "0";
            }
            if ((iReg[0x0B] & 0x20) == 0x20)
            {
                tbInt5.Text = "1";
            }
            else
            {
                tbInt5.Text = "0";
            }
            if ((iReg[0x0B] & 0x40) == 0x40)
            {
                tbInt6.Text = "1";
            }
            else
            {
                tbInt6.Text = "0";
            }
            if ((iReg[0x0B] & 0x80) == 0x80)
            {
                tbInt7.Text = "1";
            }
            else
            {
                tbInt7.Text = "0";
            }
            //EEPROM-Register
            if ((iReg[0x88] & 0x01) == 0x01)
            {
                tbEECON0.Text = "1";
            }
            else
            {
                tbEECON0.Text = "0";
            }
            if ((iReg[0x88] & 0x02) == 0x02)
            {
                tbEECON1.Text = "1";
            }
            else
            {
                tbEECON1.Text = "0";
            }
            if ((iReg[0x88] & 0x04) == 0x04)
            {
                tbEECON2.Text = "1";
            }
            else
            {
                tbEECON2.Text = "0";
            }
            if ((iReg[0x88] & 0x08) == 0x08)
            {
                tbEECON3.Text = "1";
            }
            else
            {
                tbEECON3.Text = "0";
            }
            if ((iReg[0x88] & 0x10) == 0x10)
            {
                tbEECON4.Text = "1";
            }
            else
            {
                tbEECON4.Text = "0";
            }

            _iPrescalerValue = Convert.ToInt32( Math.Pow( 2 , 1 + (iReg[0x81] & 0x07) ) );
            if ((iReg[0x81] & 0x08) == 0x08)
            {
                _iPrescalerValue = (_iPrescalerValue / 2);
                tbPSCAssign.Text = "WDT";
            }
            else
            {
                tbPSCAssign.Text = "TMR0";
            }
            tbPrescaler.Text = "1:" + Convert.ToString( _iPrescalerValue );


            //Stack anzeigen mithilfe eines temporären Stacks
            Stack<int> tempStack = new Stack<int>( );
            string[] sStack = new string[8] { "  " , "  " , "  " , "  " , "  " , "  " , "  " , "  " };

            while (stack.Count > 0)
            {
                if (stack.Count == 1)
                {
                    try
                    {
                        sStack[0] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 2)
                {
                    try
                    {
                        sStack[1] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 3)
                {
                    try
                    {
                        sStack[2] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 4)
                {
                    try
                    {
                        sStack[3] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 5)
                {
                    try
                    {
                        sStack[4] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 6)
                {
                    try
                    {
                        sStack[5] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 7)
                {
                    try
                    {
                        sStack[6] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }
                if (stack.Count == 8)
                {
                    try
                    {
                        sStack[7] = stack.Peek( ).ToString( "X" );
                    }
                    catch (Exception /*ex*/)
                    {
                    }
                }

                tempStack.Push( stack.Pop( ) );
            }
            while (tempStack.Count > 0)
            {
                stack.Push( tempStack.Pop( ) );
            }

            lbStack0.Text = sStack[0];
            lbStack1.Text = sStack[1];
            lbStack2.Text = sStack[2];
            lbStack3.Text = sStack[3];
            lbStack4.Text = sStack[4];
            lbStack5.Text = sStack[5];
            lbStack6.Text = sStack[6];
            lbStack7.Text = sStack[7];
        }

        /*********************************************************************/
        /**   enableEEPReg
        **
        **  Alle EEPROM-Register Felder werden auf Enabled gesetzt, nachdem eine
        **  *.eep Datei geöffnet wurde
        **
        **  Ret: void
        **
        **************************************************************************/
        private delegate void enableEEPRegDelegate();
        private void enableEEPReg()
        {
            if (InvokeRequired)
            {
                var invokeVar = new enableEEPRegDelegate( enableEEPReg );
                Invoke( invokeVar );
            }
            else
            {
                
            }
        }
    }
}
