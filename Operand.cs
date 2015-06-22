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
using System.Threading.Tasks;

namespace Pic_Simulator
{
    public partial class KerTKDSim
    {

        public void ScanCommand(string aCmd, int bCmd)
        {
            byte d = 0;         // Destination bit - default 0.
            byte f;             // Register File Address
            byte b;             // Bit Address with 8-bit File Register
            byte k;             // Literal field
            short kBig;         // big literal field for call and goto



            if ( ( bCmd & 0x80 ) == 0x80 ) // destination bit gesetzt?
            {
                d = 1;
            }

            f =     (byte) ( bCmd & 0x7F );                 // Registeradresse aus Befehl holen. 
            b =     (byte) ( 1 << (bCmd & 0x380 ) >> 7 );   // bytestelle aus Befehl holen
            k =     (byte) ( bCmd & 0xFF );                 // literal aus Befehl holen
            kBig =  (byte) ( bCmd & 0x7FF );                // grosse literal fuer GOTO and CALL  


            switch( aCmd )
            {
                case "ADDWF" :
                    opADDWF( f , d );
                    break;
                
                case "ANDWF" :
                    opANDWF( f , d );
                    break;

                case "CLRF":
                    opCLRF( f );
                    break;

                case "CLRW":
                    opCLRW();
                    break;

                case "COMF":
                    opCOMF( f , d );
                    break;

                case "DECF":
                    opDECF(f, d);
                    break;

                case "DECFSZ":
                    opDECFSZ(f, d);
                    break;

                case "INCF":
                    opINCF(f, d);
                    break;

                case "INCFSZ":
                    opINCFSZ(f, d);
                    break;

                case "IORWF":
                    opIORWF(f, d);
                    break;

                case "MOVF":
                    opMOVF(f, d);
                    break;

                case "MOVWF":
                    opMOVWF(f);
                    break;

                case "NOP":
                    opNOP();
                    break;

                case "RLF":
                    opRLF( f , d );
                    break;

                case "RRF":
                    opRRF(f, d);
                    break;

                case "SUBWF":
                    opSUBWF(f, d);
                    break;

                case "SWAPF":
                    opSWAPF(f, d);
                    break;

                case "XORWF":
                    opXORWF(f, d);
                    break;

                case "BCF":
                    opBCF(f, b);
                    break;

                case "BSF":
                    opBSF(f, b);
                    break;

                case "BTFSC":
                    opBTFSC(f, b);
                    break;

                case "BTFSS":
                    opBTFSS(f, b);
                    break;

                case "ADDLW":
                    opADDLW( k );
                    break;

                case "ANDLW":
                    opANDLW(k);
                    break;

                case "CALL":
                    opCALL(kBig);
                    break;

                case "CLRWDT":
                    opCLRWDT();
                    break;

                case "GOTO":
                    opGOTO(kBig);
                    break;

                case "IORLW":
                    opIORLW(k);
                    break;

                case "MOVLW":
                    opMOVLW(k);
                    break;

                case "RETFIE":
                    opRETFIE();
                    break;

                case "RETLW":
                    opRETLW(k);
                    break;

                case "RETURN":
                    opRETURN();
                    break;

                case "SLEEP":
                    opSLEEP();
                    break;

                case "SUBLW":
                    opSUBLW(k);
                    break;

                case "XORLW":
                    opXORLW(k);
                    break;

                default:
                    break;
            }
        }

        private void opADDWF( byte f , byte d ) // add W and f
        { 
            if ( f == 0 ) f = (byte)iReg[0x04]; // Indirectly addressed?
            if ( ( iReg[0x03] & 0x20 ) > 0 ) f += 0x80;  // Bank 1 check

            int temp = iWReg + iReg[f];  // temporary varialbe to save the Addition result

            if ( temp > 255 ) // Is the result bigger than 1 byte?
            { 
                iReg[0x03] |= 0x01; // C = 1;
                temp = temp % 256;
            }
            else
            {
                iReg[0x03] &= 0xFE; // C = 0;
            }

            // Z status
            if ( temp == 0 ) // is the result of the logic operation zero?
            { 
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            // DC Status
            if (temp > 0x0F) // is the result bigger than 0x0f ?
            { 
                iReg[0x03] |= 0x02; // DC=1
            }
            else
            {
                iReg[0x03] &= 0xFD; // DC=0
            }

            if ( d == 0 )
            { 
                iWReg = temp;    
            }
            else
            { 
                iReg[f] = temp;
            }
            lCycles++;
            iPC++;
        } 

        private void opANDWF( byte f, byte d ) // and W with f 
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( ( iReg[0x03] & 0x20 ) > 0 ) f += 0x80;  // Bank 1 check

            int temp = ( iWReg & iReg[f] ) & 0xff ;  // last number because of how the  int works

            // Z status
            if (temp == 0) // is the result of the logic operation zero?
            {
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            if (d == 0)
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }
            lCycles++;
            iPC++;
        }

        private void opCLRF( byte f ) // clear f
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( ( iReg[0x03] & 0x20 ) > 0 ) f += 0x80;  // Bank 1 check

            iReg[f] = 0x00; // Cleaning the register f 
            iReg[0x03] |= 0x04; // set z = 1
            lCycles++;
            iPC++;
        }

        private void opCLRW() // Clear W
        {
            iWReg = 0x00; // Clearing register W
            iReg[0x03] |= 0x04; // set z = 1

            lCycles++;
            iPC++;
        }

        private void opCOMF( byte f, byte d ) // complement f
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( (iReg[0x03] & 0x20 ) > 0 ) f += 0x80;  //Bank 1 check
             
            int temp = iReg[f] ^ 0xFF ;  

            // Z status
            if (temp == 0) // is the result of the logic operation zero?
            {
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            if (d == 0) // Save in W or f 
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }

            lCycles++;
            iPC++;
        }

        private void opDECF( byte f, byte d ) // decrement f
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( (iReg[0x03] & 0x20 ) > 0 ) f += 0x80;  // Bank 1 check

            int temp = iReg[f] - 0x01; // temporary result of decrementation

            if ( temp < 0 )
            { 
                temp = 256 + temp;
            }

            // Z status
            if (temp == 0) // is the result of the logic operation zero?
            {
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            if (d == 0) // Save in W or f 
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }

            lCycles++;
            iPC++;
        }

        private void opDECFSZ( byte f, byte d ) // decrement f, skip if 0
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( (iReg[0x03] & 0x20 ) > 0 ) f += 0x80;  // Bank 1 check

            int temp = iReg[f] - 0x01; // temporary result of decrementation

            if (temp < 0)
                temp += 256;
            
            if (d == 0) // Save in W or f 
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }
            if (temp == 0)
            {
                //NOP!
                lCycles += 2;
                iPC += 2;
            }
            else
            {
                lCycles++;
                iPC++;
            }
        }

        private void opINCF( byte f, byte d ) // increment f 
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( ( iReg[0x03] & 0x20) > 0 ) f += 0x80;  // Bank 1 check
            
            int temp = iReg[f] + 0x01; // temporary result

            if (temp > 255 )
            {
                temp %= 256 ;
            }

            // Z status
            if (temp == 0) // is the result of the logic operation zero?
            {
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            if (d == 0) // Save in W or f 
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }

            lCycles++;
            iPC++;
        }

        private void opINCFSZ( byte f, byte d ) // increment f , skip if 0 
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ( ( iReg[0x03] & 0x20) > 0 ) f += 0x80;  // Bank 1 check

            int temp = iReg[f] - 0x01; // temporary result

            if (temp > 255 )
                temp %= 256;

            if (d == 0) // Save in W or f 
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }
            if (temp == 0)
            {
                //NOP!
                lCycles += 2;
                iPC += 2;
            }
            else
            {
                lCycles++;
                iPC++;
            }
        }

        private void opIORWF( byte f, byte d ) // inclusive OR W with f  
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect 
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check
            
            int temp = iWReg | iReg[f] ; // temporary result 

            // Z status
            if (temp == 0) // is the result of the logic operation zero?
            {
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            if (d == 0) // Save in W or f 
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }

            lCycles++;
            iPC++;
        }

        private void opMOVF( byte f, byte d ) // move f 
        {
            if (f == 0) f = (byte)iReg[0x04]; // indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check 
            
            int temp = iReg[f]; // temporary result 

            // Z status
            if (temp == 0) // is the result of the logic operation zero?
            {
                iReg[0x03] |= 0x04; // Z = 1
            }
            else
            {
                iReg[0x03] &= 0xFB; // Z = 0
            }

            if (d == 0) // Save in W or f 
                iWReg = temp;
            else
                iReg[f] = temp;

            lCycles++;
            iPC++;
        }

        private void opMOVWF( byte f ) // move W to f  
        {
            if ( f == 0 ) f = (byte)iReg[0x04];  // indirect call
            if ( ( iReg[0x03] & 0x20) > 0 ) f += 0x80;  // Bank 1 check
            
            iReg[f] = iWReg;
            lCycles++;
            iPC++;
        }

        private void opNOP( ) // No Operation 
        {
            lCycles++;
            iPC++;
        }

        private void opRLF( byte f, byte d ) // rotate left f through Carry 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check

            int temp = iReg[f] << 1;    // temporary variable
            if (temp > 255) //Überlauf?
            {
                iReg[0x03] |= 0x01; //C=1;
                temp &= 0xff;   //  keep only 8 bits
            }
            else
            {
                iReg[0x03] &= 0xFE; //C=0;
            }
            if (d == 0) // save in either iWReg or iReg[f]
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }
            lCycles++;
            iPC++;
        }

        private void opRRF( byte f, byte d ) // rotate right f through Carry 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check

            int temp = iReg[f] >> 1;    // temporary variable
            if (temp < 0 ) 
            {
                iReg[0x03] |= 0x01; //C=1;
                temp &= 0xff;   //  keep  8 bits only
            }
            else
            {
                iReg[0x03] &= 0xFE; //C=0;
            }
            if (d == 0) // save in either iWReg or iReg[f]
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }
            lCycles++;
            iPC++;
        }

        private void opSUBWF( byte f, byte d ) // Substract W from f 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check

            int temp = iReg[f] - iWReg;    // temporary variable
            if (temp < 0)
            {
                iReg[0x03] &= 0xFE; //C=0;
                iReg[0x03] &= 0xFB; //Z = 0; //DC wird nach korrektur bestimmt
                temp += 256;  //Überlauf
            }
            else
            {
                iReg[0x03] |= 0x01; //C=1;
            }

            if (temp == 0)
            {
                iReg[0x03] |= 0x04; //Z=1;
                iReg[0x03] |= 0x01; //C=1;
            }
            else
            {
                iReg[0x03] &= 0xFB;   //Z=0;
                iReg[0x03] &= 0xFE;  //C=0;
            }

            if (temp >= 0x10)
            {
                iReg[0x03] |= 0x02; //DC=1;
            }
            else
            {
                iReg[0x03] &= 0xFD; //DC=0;
            }
            if (d == 0) // save in either iWReg or iReg[f]
            {
                iWReg = temp;
            }
            else
            {
                iReg[f] = temp;
            }
            lCycles++;
            iPC++;
        }

        private void opSWAPF( byte f, byte d ) // Swap nibbles in f 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check

            int temp1 = iReg[f];
            temp1 = (temp1 << 4) & 0xF0;  // Upper Nibble to lower Nibble
            int temp2 = iReg[f];
            temp2 = (temp2 >> 4) & 0x0F;  // Lower Nibble to upper Nibble

            if (d == 0) 
                iWReg = temp1 | temp2;
            else
                iReg[f] = temp1 | temp2;
            lCycles++;
            iPC++;
        }

        private void opXORWF( byte f, byte d ) // Exclusive OR W with f  
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  //Bank 1 check

            int temp = iWReg ^ iReg[f];   // temporary variable
            if (temp == 0)
            {
                iReg[0x03] |= 0x04;  //Z=1;
            }
            else
            {
                iReg[0x03] &= 0xFB;  //Z=0;
            }
            if (d == 0)    
                iWReg = temp;
            else
                iReg[f] = temp;
            lCycles++;
            iPC++;
        }

        private void opBCF( byte f, byte b ) // Bit clear f 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  //Bank 1 check

            iReg[f] = iReg[f] & (~b);    // delete bit
            lCycles++;
            iPC++;
        }

        private void opBSF( byte f, byte b ) // Bit set f
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check

            iReg[f] = iReg[f] | b;      // set bit
            lCycles++;
            iPC++;
        }

        private void opBTFSC( byte f, byte b ) // Bit test f , skip if clear 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  //    Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  // Bank 1 check

            if ((iReg[f] & b) > 0)  // Bit set?
            {
                lCycles++;
                iPC++;
            }
            else
            {
                //NOP
                lCycles += 2;
                iPC += 2;
            }
        }

        private void opBTFSS( byte f, byte b) // Bit test f , skip if set 
        {
            if (f == 0x00) f = (byte)iReg[0x04];  // Indirect
            if ((iReg[0x03] & 0x20) > 0) f += 0x80;  //Bank 1 check

            if ((iReg[f] & b) > 0) // Bit set?
            {
                //NOP
                lCycles += 2;
                iPC += 2;
            }
            else
            {
                lCycles++;
                iPC++;
            }
        }

        private void opADDLW( byte k ) // Add literal and W
        {
            int temp = iWReg + k;   // temporary variable

            if (temp >= 0x10)  // is the result bigger tan 0x0f?
            {
                iReg[0x03] |= 0x02; //DC = 1;
            }
            else
            {
                iReg[0x03] &= 0xFD; //DC=0;
            }
            if (temp > 255) 
            {
                iReg[0x03] |= 0x01; //C=1;
                temp %= 256;  // result correction
            }
            else
            {
                iReg[0x03] &= 0xFE;  //C=0;
            }
            if (temp == 0) // Result == 0? set bit Z 
            {
                iReg[0x03] |= 0x04;  //Z=1;
            }
            else
            {
                iReg[0x03] &= 0xFB;   //Z=0;
            }
            if (temp > 255)
                temp %= 256;    
            iWReg = temp;
            lCycles++;
            iPC++;
        }

        private void opANDLW( byte k ) // AND literal with W
        {
            iWReg = iWReg & k;  // temporary variable
            if (iWReg == 0)
            {
                iReg[0x03] |= 0x04;  // Z=1;
            }
            else
            {
                iReg[0x03] &= 0xFB;   // Z=0;
            }
            lCycles++;
            iPC++;
        }

        private void opCALL( short k ) // call subroutine
        {
            stack.Push(iPC + 1);
            lCycles += 2;
            iPC = k;
        }

        private void opCLRWDT() // clear watchdog timer
        {
            lCycles++;
            lWatchdog = 0; // clear the watchdog
        }

        private void opGOTO( short k ) // Go to address
        {

            lCycles += 2;
            iPC = k; // jump to adress
        }

        private void opIORLW( byte k ) // Inclusive OR literal with W
        {
            iWReg = iWReg | k;

            if (iWReg == 0)
                iReg[0x03] |= 0x04;   // Z=1; 
            else
                iReg[0x03] &= 0xFB;   // Z=0;
            lCycles++;
            iPC++;
        }

        private void opMOVLW( byte k ) // Move literal to W
        {
            iWReg = k;
            lCycles++;
            iPC++;
        }

        private void opRETFIE() // Return from interrupt
        {
            lCycles += 2;
            iPC = stack.Pop();  // Return to adress 
            iReg[0x0B] |= 0x80; // Enable Interrupts again
            iReg[0x8B] |= 0x80;
        }

        private void opRETLW( byte k ) // Return with literal in W
        {
            lCycles += 2;
            iWReg = k;          // write k to W
            iPC = stack.Pop();  // Return to adress 
        }

        private void opRETURN() // Return from Subroutine
        {
            lCycles += 2;
            iPC = stack.Pop();  // Return from Subroutine
        }

        private void opSLEEP() // Go into standby mode
        {
            lWatchdog = 0; // Clear the Watchdog 
            _timer0enabled = false;   // Deactivate Timer0
            _sleepMode = true; // Set the Simulator in Sleep Mode
            iPC++;
            iReg[0x03] |= 0x10; iReg[0x03] &= 0xF7;     // Set T0 and clear PD
            iReg[0x83] |= 0x10; iReg[0x83] &= 0xF7;
            //Simulator kann dann durch Interrupt an RB0 oder PortB Bit 4-7 oder EEPROM-Write geweckt werden
            // Simulator can be woken up, by setting Interrupt at RB0 or through PortB Bits 4-7, or EEPROM-Write
        }

        private void opSUBLW( byte k ) // Subtract W from literal
        {
            int temp = k - iWReg;   // Temporary variable
            if (temp < 0)       
            {
                iReg[0x03] &= 0xFA; //C=0; //Z = 0;
                temp += 256; 
            }
            else
            {
                iReg[0x03] |= 0x01; //C=1;
            }
            if (temp == 0)  // result == 0?
            {
                iReg[0x03] |= 0x05;  //Z=1; //C=1;
            }
            else
            {
                iReg[0x03] &= 0xFA; //Z=0; C=0;
            }
            if (temp >= 0x10)   // Result > 0x0F?
            {
                iReg[0x03] |= 0x02; // DC=1;
            }
            else
            {
                iReg[0x03] &= 0xFD; // DC=0;
            }
            iWReg = temp;
            lCycles++;
            iPC++;
        }

        private void opXORLW( byte k ) // Exclusive OR literal with W
        {
            iWReg = iWReg ^ k;  // XOR
            if (iWReg == 0)
                iReg[0x03] |= 0x04; // Z=1;
            else
                iReg[0x03] &= 0xFB; // Z=0;
            lCycles++;
            iPC++;
        }
    }
}
