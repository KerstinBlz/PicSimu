/**************************************************************************
**
**  KerTKDSim
**  
**
**  serialCom.cs: 
**  ---------
**  Enabled hardware control over COM Port
**
**  
**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;
using System.Drawing;

namespace Pic_Simulator
{
    public partial class KerTKDSim
    {
        SerialPort port;


        private delegate void sendSerialDataDelegate();
        /// <summary>
        /// Opens the Serial Port and tries to send and receive bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns>void</returns>
        
        private void sendSerialData()
        {
            if (InvokeRequired)
            {
                var invokeHelper = new sendSerialDataDelegate( sendSerialData );
                Invoke( invokeHelper );
            }
            else
            {
                port = new SerialPort( cbComPorts.Text , 4800 , Parity.None , 8 , StopBits.One );
                port.Handshake = Handshake.None;
                try
                {
                    port.Open( );
                    byte[] bytesToSend = new byte[9];

                    bytesToSend[0] = (byte)(0x30 + (iReg[0x85] / 16)); //TRISA-Low
                    bytesToSend[1] = (byte)(0x30 + (iReg[0x85] % 16)); //TRISA-High
                    bytesToSend[2] = (byte)(0x30 + (iReg[0x05] / 16)); //PortRA-Low
                    bytesToSend[3] = (byte)(0x30 + (iReg[0x05] % 16)); //PortRA-High
                    bytesToSend[4] = (byte)(0x30 + (iReg[0x86] / 16)); //TRISB-Low
                    bytesToSend[5] = (byte)(0x30 + (iReg[0x86] % 16)); //TRISB-High
                    bytesToSend[6] = (byte)(0x30 + (iReg[0x06] / 16)); //PortRB-Low
                    bytesToSend[7] = (byte)(0x30 + (iReg[0x06] % 16)); //PortRB-High
                    bytesToSend[8] = 0x0D; // Carriage Return
                    port.Write( bytesToSend , 0 , 9 );

                    // Read the received bytes
                    int higherBytePortA = port.ReadByte( ) - 48;
                    int lowerBytePortA = port.ReadByte( ) - 48;

                    int higherBytePortB = port.ReadByte( ) - 48;
                    int lowerBytePortB = port.ReadByte( ) - 48;

                    int byteEnd = port.ReadByte( );

                    bool bSimReset = false;

                    // Check the reset
                    if ((higherBytePortA & 0x2) == 0)
                    {
                        bSimReset = true;
                    }
                    else
                    {
                        higherBytePortA -= 2;
                    }

                    int iPortA = higherBytePortA * 16 + higherBytePortB;
                    int iPortB = higherBytePortB * 16 + lowerBytePortB;

                    // Writes the port values into the registry
                    iReg[0x5] = iPortA;
                    iReg[0x6] = iPortB;

                    if (bSimReset)
                    {
                        SimStartup( );
                    }

                    //close port
                    port.Close( );
                }
                catch (Exception e)
                {

                }
            }
        }

        /// <summary>
        /// Searches for all possible Ports on the Computer
        /// </summary>
        /// <param name="input"></param>
        /// <returns>void</returns>
        
        private delegate void searchForPortsDelegate();
        private void searchForPorts()
        {
            if ( InvokeRequired )
            {
                var invokeVar = new searchForPortsDelegate( searchForPorts );
                Invoke( invokeVar );
            }
            else
            {
                string[] COMPorts = SerialPort.GetPortNames( );
                
                if ( COMPorts.Length != 0 )
                {
                    for ( int i = 0 ; i < COMPorts.Length ; i++ )
                    {
                        cbComPorts.Items.Add( COMPorts[i] );
                    }

                    cbComPorts.SelectedIndex = 0;
                }
                else
                {
                    cbComPorts.Items.Clear( );
                }
            }
        }
    }
}
