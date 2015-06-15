using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Pic_Simulator
{
    public partial class KerTKDSim
    {
        string eepromFilePath = "";

        private delegate void tbEECON_TextChangedDelegate( object sender , EventArgs e );
        private void EECON_TextChanged( object sender , EventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeHelper = new tbEECON_TextChangedDelegate( EECON_TextChanged );
                Invoke( invokeHelper , sender , e );
            }
            else
            {
                Control ctrl = (Control)sender;
                int iEEPAdress = iReg[0x9];

                if (ctrl.Name == "tbEECON0" && ctrl.Text == "1")
                {
                    if (iEEPAdress > 0x3F)
                    {
                        MessageBox.Show( "EEPROM Address is not in the value area" , "EEPROM Addressing Error" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    }
                    else
                    {
                        // Read
                        iReg[0x8] = iEEPROM[iEEPAdress];
                        iReg[0x88] &= 0x1E;
                    }
                }
                else if (ctrl.Name == "tbEECON1" && ctrl.Text == "1" && tbEECON2.Text == "1")
                {
                    if (iEEPAdress > 0x3F)
                    {
                        MessageBox.Show( "EEPROM Adresse nicht Adressierungsbereich!" , "EEPROM Adressen Fehler" , MessageBoxButtons.OK , MessageBoxIcon.Error );
                    }
                    else
                    {
                        // Write-Operation
                        iEEPROM[iEEPAdress] = iReg[0x8];
                        iReg[0x88] |= 0x10;//Set EEEIF
                        iReg[0x88] &= 0x1D;//Unset Writable-Bit
                    }
                }
            }
        }

        private void saveEEPROM( object sender , EventArgs e )
        {
            if (eepromFilePath != "")
            {
                StreamWriter swEEPROM = new StreamWriter( eepromFilePath );
                for (int i = 0 ; i < 64 ; i++)
                {
                    swEEPROM.Write( iEEPROM[i] );
                    swEEPROM.Write( "\n" );
                }
                swEEPROM.Close( );
            }
        }

        private void loadEEPROM( object sender , EventArgs e )
        {
            using (OpenFileDialog Dialog = new OpenFileDialog( ))
            {
                DialogResult Result;
                Dialog.CheckFileExists = true;
                Dialog.Title = "Datei Laden";
                Dialog.Filter = "EEPROM-Dateien|*.eep";
                Dialog.RestoreDirectory = true;
                Result = Dialog.ShowDialog( );

                if (Result == DialogResult.OK)
                {
                    btnSaveEEP.Enabled = true;
                    eepromFilePath = Dialog.FileName;
                    tbEEPName.Text = eepromFilePath;
                    StreamReader srEEPROM = new StreamReader( eepromFilePath );
                    for (int i = 0 ; i < 64 ; i++)
                    {
                        iEEPROM[i] = Convert.ToInt32( srEEPROM.ReadLine( ) );
                    }
                    srEEPROM.Close( );
                    refreshReg( );
                }
            }
        }

        private delegate void eepRegChangedDelegate( object sender , KeyPressEventArgs e );
        private void eepRegChanged( object sender , KeyPressEventArgs e )
        {
            if (InvokeRequired)
            {
                var invokeHelper = new eepRegChangedDelegate( eepRegChanged );
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
                        int iEEPAdress = Convert.ToInt32( ctrl.Name.Remove( 0 , 8 ) );

                        if (iValue >= 0x00 && iValue <= 0xFF)
                        {
                            iEEPROM[iEEPAdress] = iValue;
                            refreshReg( );
                        }
                        else
                        {
                            refreshReg( );
                            MessageBox.Show( "EEPROM Register: You can only use values from 0 to FF" , "Wrong Input" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                        }
                    }
                    catch (Exception ex)
                    {
                        refreshReg( );
                        MessageBox.Show( "EEPROM Register: You can only use values from 0 to FF" , "Wrong Input" , MessageBoxButtons.OK , MessageBoxIcon.Information );
                    }
                }
            }
        }
    }
}
