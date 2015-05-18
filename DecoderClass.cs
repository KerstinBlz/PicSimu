using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pic_Simulator
{
        class Decoder
        {
            //Argument

            public String decode(int iCmd)
            {
                //wandelt den Befehl in Bits um
                String bCmd = Convert.ToString(iCmd, 2).PadLeft(14, '0'); ;

                string bCmdLeft;

                /*Es erfolgt nun ein Abgleich mit allen möglichen Befehlen im PIC!*/
                switch (bCmd)
                {
                    // Sleep
                    case "00000001100011":
                        return "SLEEP";
                    //Clear Watchdog
                    case "00000001100100":
                        return "CLRWDT";
                    //RETURN from Subroutine
                    case "00000000001000":
                        return "RETURN";
                    //Return from Interrupt
                    case "00000000001001":
                        return "RETFIE";
                    default:
                        break;
                }

                //NOP
                if (bCmd.Contains("00000000000000") ||
                    bCmd.Contains("00000000100000") ||
                    bCmd.Contains("00000001000000") ||
                    bCmd.Contains("00000001000000") ||
                    bCmd.Contains("00000001100000"))
                {
                    return "NOP";
                }

                bCmdLeft = Left(bCmd, 7);

                switch (bCmdLeft)
                {
                    //Clear f
                    case "0000011":
                        return "CLRF";
                    //Clear w
                    case "0000010":
                        return "CLRW";
                    case "0000001":
                        return "MOVWF";
                    default:
                        break;
                }

                bCmdLeft = Left(bCmd, 6);

                switch (bCmdLeft)
                {
                    case "000111":
                        return "ADDWF";
                    case "000101":
                        return "ANDWF";
                    case "001001":
                        return "COMF";
                    case "000011":
                        return "DECF";
                    case "001011":
                        return "DECFSZ";
                    case "001010":
                        return "INCF";
                    case "001111":
                        return "INCFSZ";
                    case "000100":
                        return "IORWF";
                    case "001000":
                        return "MOVF";
                    case "001101":
                        return "RLF";
                    case "001100":
                        return "RRF";
                    case "000010":
                        return "SUBWF";
                    case "001110":
                        return "SWAPF";
                    case "000110":
                        return "XORWF";
                    case "111001":
                        return "ANDLW";
                    case "111000":
                        return "IORLW";
                    case "111010":
                        return "XORLW";
                    default:
                        break;
                }

                //ADDLW
                if (bCmdLeft.Contains("111110") ||
                    bCmdLeft.Contains("111111"))
                {
                    return "ADDLW";
                }

                //MOVLW
                if (bCmdLeft.Contains("110000") ||
                    bCmdLeft.Contains("110001") ||
                    bCmdLeft.Contains("110010") ||
                    bCmdLeft.Contains("110011"))
                {
                    return "MOVLW";
                }

                //RETLW
                if (bCmdLeft.Contains("110100") ||
                    bCmdLeft.Contains("110101") ||
                    bCmdLeft.Contains("110110") ||
                    bCmdLeft.Contains("110111"))
                {
                    return "RETLW";
                }

                //SUBLW
                if (bCmdLeft.Contains("111100") ||
                    bCmdLeft.Contains("111101"))
                {
                    return "SUBLW";
                }

                bCmdLeft = Left(bCmd, 4);
                switch (bCmdLeft)
                {
                    case "0100":
                        return "BCF";
                    case "0101":
                        return "BSF";
                    case "0110":
                        return "BTFSC";
                    case "0111":
                        return "BTFSS";
                    default:
                        break;
                }

                bCmdLeft = Left(bCmd, 3);
                switch (bCmdLeft)
                {
                    case "100":
                        return "CALL";
                    case "101":
                        return "GOTO";
                    default:
                        break;
                }

                return null;
            }

            public String decode(String iCmd)
            {

                //wandelt den Befehl in Bits um
                String bCmd = iCmd.PadLeft(14, '0'); 

                string bCmdLeft;

                /*Es erfolgt nun ein Abgleich mit allen möglichen Befehlen im PIC!*/
                switch (bCmd)
                {
                    // Sleep
                    case "00000001100011":
                        return "SLEEP";
                    //Clear Watchdog
                    case "00000001100100":
                        return "CLRWDT";
                    //RETURN from Subroutine
                    case "00000000001000":
                        return "RETURN";
                    //Return from Interrupt
                    case "00000000001001":
                        return "RETFIE";
                    default:
                        break;
                }

                //NOP
                if (bCmd.Contains("00000000000000") ||
                    bCmd.Contains("00000000100000") ||
                    bCmd.Contains("00000001000000") ||
                    bCmd.Contains("00000001000000") ||
                    bCmd.Contains("00000001100000"))
                {
                    return "NOP";
                }

                bCmdLeft = Left(bCmd, 7);

                switch (bCmdLeft)
                {
                    //Clear f
                    case "0000011":
                        return "CLRF";
                    //Clear w
                    case "0000010":
                        return "CLRW";
                    case "0000001":
                        return "MOVWF";
                    default:
                        break;
                }

                bCmdLeft = Left(bCmd, 6);

                switch (bCmdLeft)
                {
                    case "000111":
                        return "ADDWF";
                    case "000101":
                        return "ANDWF";
                    case "001001":
                        return "COMF";
                    case "000011":
                        return "DECF";
                    case "001011":
                        return "DECFSZ";
                    case "001010":
                        return "INCF";
                    case "001111":
                        return "INCFSZ";
                    case "000100":
                        return "IORWF";
                    case "001000":
                        return "MOVF";
                    case "001101":
                        return "RLF";
                    case "001100":
                        return "RRF";
                    case "000010":
                        return "SUBWF";
                    case "001110":
                        return "SWAPF";
                    case "000110":
                        return "XORWF";
                    case "111001":
                        return "ANDLW";
                    case "111000":
                        return "IORLW";
                    case "111010":
                        return "XORLW";
                    default:
                        break;
                }

                //ADDLW
                if (bCmdLeft.Contains("111110") ||
                    bCmdLeft.Contains("111111"))
                {
                    return "ADDLW";
                }

                //MOVLW
                if (bCmdLeft.Contains("110000") ||
                    bCmdLeft.Contains("110001") ||
                    bCmdLeft.Contains("110010") ||
                    bCmdLeft.Contains("110011"))
                {
                    return "MOVLW";
                }

                //RETLW
                if (bCmdLeft.Contains("110100") ||
                    bCmdLeft.Contains("110101") ||
                    bCmdLeft.Contains("110110") ||
                    bCmdLeft.Contains("110111"))
                {
                    return "RETLW";
                }

                //SUBLW
                if (bCmdLeft.Contains("111100") ||
                    bCmdLeft.Contains("111101"))
                {
                    return "SUBLW";
                }

                bCmdLeft = Left(bCmd, 4);
                switch (bCmdLeft)
                {
                    case "0100":
                        return "BCF";
                    case "0101":
                        return "BSF";
                    case "0110":
                        return "BTFSC";
                    case "0111":
                        return "BTFSS";
                    default:
                        break;
                }

                bCmdLeft = Left(bCmd, 3);
                switch (bCmdLeft)
                {
                    case "100":
                        return "CALL";
                    case "101":
                        return "GOTO";
                    default:
                        break;
                }

                return null;
            }

            public static string Left(string s, int len)
            {
                if (len == 0 || s.Length == 0)
                    return "";
                else if (s.Length <= len)
                    return s;
                else
                    return s.Substring(0, len);
            }
        }
    }
