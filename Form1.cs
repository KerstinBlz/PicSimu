using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Pic_Simulator
{
    public partial class KerTKDSim : Form
    {
        const int MAX_REGISTER = 256;
        // Variables for the simulation 
        bool _running;
        bool _sleepMode;
        bool _bank0;
        bool _bOpenedFile = false;
        int _iDelay;            // Delay between the Simulation commands
        int iPC;                // Programm Counter 
        long lCycles, lRuntime; //Zyklen und Laufzeit
        long lWatchdog; // Watchdog Counter

        bool _timer0enabled = true;
        int _iPrescalerCnt;
        int _iPrescalerValue;

        // Change RB Port 
        int iRA, iRB;   // Helpvariables for changes between RA and RB 

        // ******** Simulation Register  ************

        // Array for Register
        int[] iReg = new int[MAX_REGISTER];

        // Stack 
        Stack<int> stack = new Stack<int>(8);

        // Boolean for Serial Connection
        bool bSerialCon = false;

        // W Register;
        int iWReg;

        // Object of the Decoder 
        Decoder _obDecoder = new Decoder();

        // bool for Watchdog
        bool bWatchdog = false;

        List<String> OperandList = new List<String>();

        int[] iEEPROM = new int[64];

        // Arrays für die Visualisierung
        string[,] ArrayBank0 = new string[16, 8]; //16 Zeilen, 8 Spalten
        string[,] ArrayBank1 = new string[16, 8];
        string[,] ArrayEEPROM = new string[8, 8];
        //string[] ArrayStatusReg = new string[8];
        //string[] ArrayOptionReg = new string[8];
        //string[] ArrayInterruptReg = new string[8];
        //string[] ArrayStack = new string[8];


        public KerTKDSim()
        {

            iPC = 0;
            _iDelay = 250;
            lCycles = 0;
            lRuntime = 0;
            lWatchdog = 0;
            _iPrescalerCnt = 0;
            _timer0enabled = true;
            _sleepMode = false;
            _bank0 = true;
            bWatchdog = false;
            iRA = 0x00;
            iRB = 0x00;


            SimStartup();
            InitializeComponent();
            



            #region Tabellen/Grid initialisierung
            //Initialisieren der tabelle Bank1 und Bank2
            int Row = 17;
            int Col = 9;

            gridBank_1.EnableSort = false;
            gridBank_0.EnableSort = false;
            gridEEPROM.EnableSort = false;
            // gridEECON1.EnableSort = false;
            gridInterrupt.EnableSort = false;
            gridOption.EnableSort = false;
            gridStack.EnableSort = false;
            gridStatus.EnableSort = false;

            gridBank_1.Redim(Row, Col);
            gridBank_0.Redim(Row, Col);
            gridEEPROM.Redim(9, Col);


            #region Bank0/1,EEPROM initialisierung
            for (int r = 0; r < Row; r++)
            {
                for (int c = 0; c < Col; c++)
                {

                    if (c == 0)
                    {
                        gridBank_1[r, c] = new SourceGrid.Cells.ColumnHeader("");
                        gridBank_0[r, c] = new SourceGrid.Cells.ColumnHeader("");

                        if (r < 9) { gridEEPROM[r, c] = new SourceGrid.Cells.ColumnHeader(""); }
                    }
                    else if (r == 0)
                    {
                        gridBank_1[r, c] = new SourceGrid.Cells.ColumnHeader("");
                        gridBank_0[r, c] = new SourceGrid.Cells.ColumnHeader("");

                        if (r < 9) { gridEEPROM[r, c] = new SourceGrid.Cells.ColumnHeader(""); }
                    }
                    else
                    {
                        // typeof(string) muss für editierbare zellen durchgeführt werden
                        gridBank_1[r, c] = new SourceGrid.Cells.Cell("", typeof(string));
                        gridBank_0[r, c] = new SourceGrid.Cells.Cell("", typeof(string));
                        if (r < 9) { gridEEPROM[r, c] = new SourceGrid.Cells.Cell("", typeof(string)); }

                    }
                    gridBank_0.Columns[c].Width = 27;
                    gridBank_1.Columns[c].Width = 27;
                    if (r < 9) { gridEEPROM.Columns[c].Width = 27; }
                    gridBank_0.Rows[r].Height = 22;
                    gridBank_1.Rows[r].Height = 22;
                    if (r < 9) { gridEEPROM.Rows[r].Height = 22; }
                }
            }
            #endregion Bank0/1,EEPROM initialisierung

            #region StatusRegister
            //initialisieren des Stausregisters
            gridStatus.Redim(2, 8);
            gridStatus[0, 0] = new SourceGrid.Cells.ColumnHeader("Bit0");
            gridStatus[0, 1] = new SourceGrid.Cells.ColumnHeader("Bit1");
            gridStatus[0, 2] = new SourceGrid.Cells.ColumnHeader("Bit2");
            gridStatus[0, 3] = new SourceGrid.Cells.ColumnHeader("Bit3");
            gridStatus[0, 4] = new SourceGrid.Cells.ColumnHeader("Bit4");
            gridStatus[0, 5] = new SourceGrid.Cells.ColumnHeader("Bit5");
            gridStatus[0, 6] = new SourceGrid.Cells.ColumnHeader("Bit6");
            gridStatus[0, 7] = new SourceGrid.Cells.ColumnHeader("Bit7");

            gridStatus[1, 0] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 1] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 2] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 3] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 4] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 5] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 6] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStatus[1, 7] = new SourceGrid.Cells.Cell("0", typeof(string));

            for (int i = 0; i < 8; i++) { gridStatus.Columns[i].Width = 30; }

            #endregion StatusRegister

            #region OptionRegister
            //initialisieren des Stausregisters
            gridOption.Redim(2, 8);
            gridOption[0, 0] = new SourceGrid.Cells.ColumnHeader("Bit0");
            gridOption[0, 1] = new SourceGrid.Cells.ColumnHeader("Bit1");
            gridOption[0, 2] = new SourceGrid.Cells.ColumnHeader("Bit2");
            gridOption[0, 3] = new SourceGrid.Cells.ColumnHeader("Bit3");
            gridOption[0, 4] = new SourceGrid.Cells.ColumnHeader("Bit4");
            gridOption[0, 5] = new SourceGrid.Cells.ColumnHeader("Bit5");
            gridOption[0, 6] = new SourceGrid.Cells.ColumnHeader("Bit6");
            gridOption[0, 7] = new SourceGrid.Cells.ColumnHeader("Bit7");

            gridOption[1, 0] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 1] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 2] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 3] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 4] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 5] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 6] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridOption[1, 7] = new SourceGrid.Cells.Cell("0", typeof(string));

            for (int i = 0; i < 8; i++) { gridOption.Columns[i].Width = 30; }

            #endregion OptionRegister

            #region Stack
            //initialisieren des Stausregisters
            gridStack.Redim(2, 8);
            gridStack[0, 0] = new SourceGrid.Cells.ColumnHeader("0");
            gridStack[0, 1] = new SourceGrid.Cells.ColumnHeader("1");
            gridStack[0, 2] = new SourceGrid.Cells.ColumnHeader("2");
            gridStack[0, 3] = new SourceGrid.Cells.ColumnHeader("3");
            gridStack[0, 4] = new SourceGrid.Cells.ColumnHeader("4");
            gridStack[0, 5] = new SourceGrid.Cells.ColumnHeader("5");
            gridStack[0, 6] = new SourceGrid.Cells.ColumnHeader("6");
            gridStack[0, 7] = new SourceGrid.Cells.ColumnHeader("7");

            gridStack[1, 0] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 1] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 2] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 3] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 4] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 5] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 6] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridStack[1, 7] = new SourceGrid.Cells.Cell("0", typeof(string));

            for (int i = 0; i < 8; i++) { gridStack.Columns[i].Width = 30; }

            #endregion Stack

            #region InterruptRegister
            //initialisieren des Stausregisters
            gridInterrupt.Redim(2, 8);
            gridInterrupt[0, 0] = new SourceGrid.Cells.ColumnHeader("Bit0");
            gridInterrupt[0, 1] = new SourceGrid.Cells.ColumnHeader("Bit1");
            gridInterrupt[0, 2] = new SourceGrid.Cells.ColumnHeader("Bit2");
            gridInterrupt[0, 3] = new SourceGrid.Cells.ColumnHeader("Bit3");
            gridInterrupt[0, 4] = new SourceGrid.Cells.ColumnHeader("Bit4");
            gridInterrupt[0, 5] = new SourceGrid.Cells.ColumnHeader("Bit5");
            gridInterrupt[0, 6] = new SourceGrid.Cells.ColumnHeader("Bit6");
            gridInterrupt[0, 7] = new SourceGrid.Cells.ColumnHeader("Bit7");

            gridInterrupt[1, 0] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 1] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 2] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 3] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 4] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 5] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 6] = new SourceGrid.Cells.Cell("0", typeof(string));
            gridInterrupt[1, 7] = new SourceGrid.Cells.Cell("0", typeof(string));

            for (int i = 0; i < 8; i++) { gridInterrupt.Columns[i].Width = 30; }

            #endregion OptionRegister

            //#region EECON1
            //gridEECON1.Redim( 2 , 5 );
            //gridEECON1[0 , 0] = new SourceGrid.Cells.ColumnHeader( "EEIF" );
            //gridEECON1[0 , 1] = new SourceGrid.Cells.ColumnHeader( "WRERR" );
            //gridEECON1[0 , 2] = new SourceGrid.Cells.ColumnHeader( "WREN" );
            //gridEECON1[0 , 3] = new SourceGrid.Cells.ColumnHeader( "WR" );
            //gridEECON1[0 , 4] = new SourceGrid.Cells.ColumnHeader( "RD" );

            //gridEECON1[1 , 0] = new SourceGrid.Cells.Cell( "0" , typeof( string ) );
            //gridEECON1[1 , 1] = new SourceGrid.Cells.Cell( "0" , typeof( string ) );
            //gridEECON1[1 , 2] = new SourceGrid.Cells.Cell( "0" , typeof( string ) );
            //gridEECON1[1 , 3] = new SourceGrid.Cells.Cell( "0" , typeof( string ) );
            //gridEECON1[1 , 4] = new SourceGrid.Cells.Cell( "0" , typeof( string ) );

            //gridEECON1.AutoSizeCells( );
            //for (int i = 0 ; i < 5 ; i++) { gridEECON1.Columns[i].Width = 53; }

            //#endregion EECON1

            #region Beschriftung der Tabellen
            gridBank_1[0, 0].Value = "0x";
            gridBank_0[0, 0].Value = "0x";
            gridEEPROM[0, 0].Value = "0x";

            for (int c = 1; c < Col; c++)
            {
                gridBank_1[0, c].Value = "+0" + (c - 1);
                gridBank_0[0, c].Value = "+0" + (c - 1);
                gridEEPROM[0, c].Value = "+0" + (c - 1);
            }

            //Grid der Bank 0 Beschriftung
            gridBank_0[1, 0].Value = "00";
            gridBank_0[2, 0].Value = "08";
            gridBank_0[3, 0].Value = "10";
            gridBank_0[4, 0].Value = "18";
            gridBank_0[5, 0].Value = "20";
            gridBank_0[6, 0].Value = "28";
            gridBank_0[7, 0].Value = "30";
            gridBank_0[8, 0].Value = "38";
            gridBank_0[9, 0].Value = "40";
            gridBank_0[10, 0].Value = "48";
            gridBank_0[11, 0].Value = "50";
            gridBank_0[12, 0].Value = "58";
            gridBank_0[13, 0].Value = "60";
            gridBank_0[14, 0].Value = "68";
            gridBank_0[15, 0].Value = "70";
            gridBank_0[16, 0].Value = "78";

            //Grid der bank 1 Beschriftung          
            gridBank_1[1, 0].Value = "80";
            gridBank_1[2, 0].Value = "88";
            gridBank_1[3, 0].Value = "90";
            gridBank_1[4, 0].Value = "98";
            gridBank_1[5, 0].Value = "A0";
            gridBank_1[6, 0].Value = "A8";
            gridBank_1[7, 0].Value = "B0";
            gridBank_1[8, 0].Value = "B8";
            gridBank_1[9, 0].Value = "C0";
            gridBank_1[10, 0].Value = "C8";
            gridBank_1[11, 0].Value = "D0";
            gridBank_1[12, 0].Value = "D8";
            gridBank_1[13, 0].Value = "E0";
            gridBank_1[14, 0].Value = "E8";
            gridBank_1[15, 0].Value = "F0";
            gridBank_1[16, 0].Value = "F8";

            //Grid des EEPROM
            //Grid der Bank 0 Beschriftung
            gridEEPROM[1, 0].Value = "00";
            gridEEPROM[2, 0].Value = "08";
            gridEEPROM[3, 0].Value = "10";
            gridEEPROM[4, 0].Value = "18";
            gridEEPROM[5, 0].Value = "20";
            gridEEPROM[6, 0].Value = "28";
            gridEEPROM[7, 0].Value = "30";
            gridEEPROM[8, 0].Value = "38";
            #endregion Beschriftung der Tabellen


            refreshReg();
            searchForPorts( );
            
            #endregion Tabellen/Grid initialisierung
        }


        private delegate void SimResetDelegate();
        /// <summary>
        /// Resets the programme with normal, non-hardware way
        /// </summary>
        private void SimReset()
        {
            if (InvokeRequired)
            {
                var invokeVar = new SimResetDelegate(SimReset);
                Invoke(invokeVar);
            }
            else
            {
                iPC = 0;

                //// Vorgeschriebene Werte !!!!!!!!!!!
                //for (int r = 0 ; r < 16 ; r++)
                //{
                //    for (int c = 0 ; c < 8 ; c++)
                //    {
                //        ArrayBank0[r , c] = "00";
                //        ArrayBank1[r , c] = "00";
                //    }
                //}
                //ArrayBank1[0 , 1] = "FF";
                //ArrayBank1[0 , 3] = "18";
                //ArrayBank1[0 , 5] = "1F";
                //ArrayBank1[0 , 6] = "FF";
                //ArrayBank0[0 , 3] = "18";

                //Bank 0
                iReg[0x03] = iReg[0x03] & 0x1F;
                iReg[0x05] = iReg[0x05] & 0x1F;
                iReg[0x0B] = iReg[0x0B] & 0x01;
                //Bank 1
                iReg[0x81] = 0xFF;
                iReg[0x83] = iReg[0x83] | 0x18; iReg[0x83] = iReg[0x83] & 0x1F;
                iReg[0x85] = 0x1F;
                iReg[0x86] = 0xFF;
                iReg[0x88] = iReg[0x88] & 0x08;
                for (int i = 0; i <= 255; i++)
                {
                    if (i != 0x03)
                        if (i != 0x05)
                            if (i != 0x06)
                                if (i != 0x0B)
                                    if (i != 0x81)
                                        if (i != 0x83)
                                            if (i != 0x85)
                                                if (i != 0x86)
                                                    if (i != 0x88)
                                                    {
                                                        iReg[i] = 0x00;
                                                    }
                }

                iWReg = 0;
                stack.Clear();
                _markCommand( iPC );

                refreshReg();
            }
        }


        private delegate void SimStartupDelegate();
        private void SimStartup()
        {
            if (InvokeRequired)
            {
                var invokeVar = new SimStartupDelegate(SimStartup);
                Invoke(invokeVar);
            }
            else
            {
                // Vorgeschriebene Werte !!!!!!!!!!!
                for (int r = 0 ; r < 16 ; r++)
                {
                    for (int c = 0 ; c < 8 ; c++)
                    {
                        ArrayBank0[r , c] = "00";
                        ArrayBank1[r , c] = "00";
                    }
                }
                ArrayBank1[0 , 1] = "FF";
                ArrayBank1[0 , 3] = "18";
                ArrayBank1[0 , 5] = "1F";
                ArrayBank1[0 , 6] = "FF";
                ArrayBank0[0 , 3] = "18";

                iPC = 0;
                //Bank 0
                iReg[0x03] = iReg[0x03] | 0x18; iReg[0x03] = iReg[0x03] & 0x1F;
                iReg[0x05] = iReg[0x05] & 0x1F;
                iReg[0x0B] = iReg[0x0B] & 0x01;
                //Bank 1
                iReg[0x81] = 0xFF;
                iReg[0x83] = iReg[0x83] | 0x18; iReg[0x83] = iReg[0x83] & 0x1F;
                iReg[0x85] = 0x1F;
                iReg[0x86] = 0xFF;
                iReg[0x88] = iReg[0x88] & 0x08;

                iWReg = 0;
                stack.Clear(); // Clear the Stack

                for (int i = 0; i <= 255; i++)
                {
                    if (i != 0x03)
                        if (i != 0x05)
                            if (i != 0x06)
                                if (i != 0x0B)
                                    if (i != 0x81)
                                        if (i != 0x83)
                                            if (i != 0x85)
                                                if (i != 0x86)
                                                    if (i != 0x88)
                                                    {
                                                        iReg[i] = 0x00;
                                                    }
                }

                if (_bOpenedFile)
                {
                    lCycles = 0;
                    lRuntime = 0;
                    refreshReg();
                }
            }

        }


        ///<summary>
        /// Überprüft den Watchdog
        /// Ret: void
        /// </summary>
        private void watchdog()
        {
            if ( bWatchdog )
            {
                if ((iReg[0x81] & 0x08) == 0x00) // Prescaler TMR0 
                {
                    ExecuteWDT();    
                }
                else  // Prescaler WDT 
                {
                    _iPrescalerCnt++;

                    if ( _iPrescalerCnt >= _iPrescalerValue )  // ansonsten Prescalratio(mindestens 1:1)
                    {
                        _iPrescalerCnt = 0;
                        ExecuteWDT();
                    }
                }
            }
        }



        ///<summary>
        /// Watchdog hochzählen und Interrupt bei Überlauf
        /// Ret: void
        ///</summary>
        private void ExecuteWDT()
        {
            lWatchdog++;
            if (lWatchdog >= 18000 * 5)     // 18ms - 200ns every Cycle
            {
                // TimeOut-Flag setzen
                iReg[0x03] &= 0xEF;
                iReg[0x83] &= 0xEF;

                
                SimReset();     // Reset the Simulation
                lWatchdog = 0;  // Reset the Watchdog
                _sleepMode = false;
            }
        }


        ///<summary>
        /// Simuliert den TMR0
        /// Ret: void
        ///</summary>
        private void simulateTimer0()
        {
            if ( _timer0enabled )
            {
                if ( ( iReg[0x81] & 0x20) == 0 ) // Timer0 in Timer-Mode?
                {
                    if ((iReg[0x81] & 0x08) == 0x08) // Prescaler WDT
                    {
                        ExecuteTimer(); 
                    }
                    else 
                    {
                        _iPrescalerCnt++;
                        if ( _iPrescalerCnt >= _iPrescalerValue )
                        {
                            _iPrescalerCnt = 0;
                            ExecuteTimer();
                        }
                    }
                }
                else   
                {
                    ExecuteTimer();
                }
            }
        }


        ///<summary>
        /// Führt den Timer aus
        /// Ret: void
        ///</summary>
        private void ExecuteTimer()
        {
            if ((iReg[0x81] & 0x20) == 0)    // Timer0 in Timer-Mode?
            {
                iReg[0x01]++;   // Increment the Timer
                if (iReg[0x01] > 255)   
                {
                    iReg[0x0B] |= 0x04;     // Set the Timer Interrupt flag 
                    iReg[0x01] = 0;         // Reset the Timer
                    Console.WriteLine("Timer0 Überlauf TimerMode");
                }
            }
            else   //Timer0 in Counter-Mode
            {
                if (((iReg[0x05] & 0x10) > 0) && ((iRA & 0x10)) == 0) //Wechsel von 0 auf 1?
                {
                    iRA = iRA | 0x10; // Updating help Variables

                    if ((iReg[0x81] & 0x10) == 0)   // Count every growing Flank
                    {
                        _iPrescalerCnt++;
                        if (_iPrescalerCnt >= _iPrescalerValue)
                        {
                            _iPrescalerCnt = 0;
                            iReg[0x01]++; //Timer inkrementieren
                        }
                    }
                }
                else if (((iReg[0x05] & 0x10) == 0) && ((iRA & 0x10) > 0))   //Wechsel von 1 auf 0?
                {
                    iRA = iRA & 0xEF; //Hilfsvariable aktualisieren
                    if ((iReg[0x81] & 0x10) > 1)        //Jede fallende Taktflanke zählen
                    {
                        _iPrescalerCnt++;
                        if (_iPrescalerCnt >= _iPrescalerValue)
                        {
                            _iPrescalerCnt = 0;
                            iReg[0x01]++; //Timer inkrementieren
                        }
                    }
                }
                if (iReg[0x01] > 255) //Counter-Überlauf?
                {
                    iReg[0x0B] |= 0x04;     //Timerinterruptflag setzen
                    iReg[0x01] = 0; //Timer inkrementieren
                    Console.WriteLine("Timer0 Überlauf CounterMode");
                }
            }
        }


        ///<summary>
        /// Endlosschleife solange der Thread läuft (nachdem "Start"-Gedrückt wurde)
        ///  Ret: void
        ///</summary>
        private void simulateProgram()
        {
            while (_running)
            {
                ExecuteCmd();
                Thread.Sleep(_iDelay);
            }
        }


        ///<summary>
        /// pausiert das Programm
        /// Ret: void
        ///</summary>
        private void bStop_Click(object sender, EventArgs e)
        {
            _running = false;
            Stop();
        }


        ///<summary>
        /// Pausiert das Programm
        /// Ret: void
        ///</summary>        
        private delegate void StopDelegate();
        private void Stop()
        {
            if (InvokeRequired)
            {
                var invokeVar = new StopDelegate(Stop);
                Invoke(invokeVar);
            }
            else
            {
                _running = false;
                btnStart.Enabled = true;
                btnStep.Enabled = true;
                btnStop.Enabled = false;
                // disable buttons
            }
        }




        ///<summary>
        /// Schaut ob ein Interrupt ausgelöst wird
        /// Ret: void
        /// </summary>
        private void checkInterrupt()
        {
            //Auf Globalinterrupt enabled überprüfen
            if ((iReg[0x0B] & 0x80) > 0)     //Globalinterrupt enabled?
            {
                // Einzelne Intterupts überprüfen, Priorität?
                if ((iReg[0x0B] & 0x24) >= 0x24)    //Timer0 enabled und Überlauf aufgetreten?
                {
                    //Aktuelle Adresse im Stack speichern wegen RETFIE
                    stack.Push(iPC);
                    //Springe zu Adress 0x04 und führe alle aufgetretenen Interrupts aus
                    iPC = 0x04;
                    iReg[0x0B] &= 0x7F; iReg[0x8B] &= 0x7F; //Global Interrupt löschen, damit keine Endlosschleife entsteht
                }
                else if ((iReg[0x0B] & 0x12) >= 0x12)    //RB0/Int enabled und Interrupt aufgetreten?
                {
                    //Aktuelle Adresse im Stack speichern wegen RETFIE
                    stack.Push(iPC);
                    //Springe zu Adress 0x04 und führe alle aufgetretenen Interrupts aus
                    iPC = 0x04;
                    iReg[0x0B] &= 0x7F; iReg[0x8B] &= 0x7F; //Global Interrupt löschen, damit keine Endlosschleife entsteht
                    //  iReg[0x0B] &= 0xFD;     //Interruptflag löschen  Macht der User selbst in der ISR!?
                }
                else if ((iReg[0x0B] & 0x09) >= 0x09)    //RB-Port changed enabled und Interrupt aufgetreten?
                {
                    _sleepMode = false;  //Pic wecken
                    //Aktuelle Adresse im Stack speichern wegen RETFIE
                    stack.Push(iPC);
                    //Springe zu Adress 0x04 und führe alle aufgetretenen Interrupts aus
                    iPC = 0x04;
                    iReg[0x0B] &= 0x7F; iReg[0x8B] &= 0x7F; //Global Interrupt löschen, damit keine Endlosschleife entsteht
                    //  iReg[0x0B] &= 0xFE;     //Interruptflag löschen  Macht der User selbst in der ISR!?
                }
                else if (((iReg[0x88] & 0x10) > 0) && ((iReg[0x0B] & 0x40) > 0))    //EEPROM-Write finished Interrupt
                {
                    _sleepMode = false;  //Pic wecken
                    //Aktuelle Adresse im Stack speichern wegen RETFIE
                    stack.Push(iPC);
                    //Springe zu Adress 0x04 und führe alle aufgetretenen Interrupts aus
                    iPC = 0x04;
                    iReg[0x0B] &= 0x7F; iReg[0x8B] &= 0x7F; //Global Interrupt löschen, damit keine Endlosschleife entsteht
                }
            }
        }


        ///<summary>
        /// Setzt einen Interrupt
        /// Ret: void
        /// </summary>
        private void setInterrupt()
        {
            //-------Set Interrupts -----
            //Interruptflags setzen wenn jeweilige Bedingung erfüllt
            //RB0/INT set //Nur das 0. Bit abfragen
            if (((iReg[0x06] & 0x01) == 1) && ((iRB & 0x01) == 0))   //Wechsel von 0 auf 1?
            {
                iRB = iRB | 0x01;
                if ((iReg[0x81] & 0x40) > 0)
                {
                    iReg[0x0B] |= 0x02;
                }
            }
            else if (((iReg[0x06] & 0x01) == 0) && ((iRB & 0x01)) > 0) //Wechsel von 1 auf 0?
            {
                iRB = iRB & 0xFE;
                if ((iReg[0x81] & 0x40) == 0)
                {
                    iReg[0x0B] |= 0x02;
                }
            }
            //RB-Port changed (1 of Bit 4-7)
            if (((iReg[0x06] & 0xF0) ^ (iRB & 0xF0)) > 0)
            {
                int hilf;
                hilf = iRB & 0x0F;
                iRB = (iReg[0x06] & 0xF0);
                iRB |= hilf;

                iReg[0x0B] |= 0x01;
            }
            //-------Set Interrupts Ende ---- 
        }




        private delegate void ExecuteCommandDelegate();
        /// <summary>
        /// Führt einen Befehl aus
        /// Ret: void
        /// </summary>
        public void ExecuteCmd()
        {
            if (InvokeRequired)
            {
                var invokeVar = new ExecuteCommandDelegate(ExecuteCmd);
                Invoke(invokeVar);
            }
            else
            {
                string hexValue = OperandList[iPC];
                int bCmd = Int16.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                String strCurCmd = _obDecoder.decode( bCmd );

                //Execute Command
                tbTest.Text = strCurCmd;
                ScanCommand( strCurCmd, bCmd );

                

                // Spiegelung
                // PC wird gespiegelt
                iReg[0x02] = iPC & 0xFF;
                iReg[0x82] = iPC & 0xFF;
                iReg[0x0A] = (iPC & 0x1F00) >> 8;
                iReg[0x8A] = (iPC & 0x1F00) >> 8;

                // Register die nicht änderbar sind
                iReg[0x00] = 0;
                iReg[0x80] = 0;
                iReg[0x07] = 0;
                iReg[0x87] = 0;
                if (_bank0)  //Bank 1 check
                {
                    //Spiegelung von Bank 0 nach Bank 1
                    iReg[0x83] = iReg[0x03];
                    iReg[0x84] = iReg[0x04];
                    iReg[0x87] = iReg[0x07];
                    iReg[0x8A] = iReg[0x0A];
                    iReg[0x8B] = iReg[0x0B];
                    if ((iReg[0x03] & 0x20) > 0)
                    {
                        _bank0 = false;
                    }
                }
                else
                {
                    //Spiegelung von Bank 1 nach Bank 0
                    iReg[0x00] = iReg[0x80];
                    iReg[0x03] = iReg[0x83];
                    iReg[0x04] = iReg[0x84];
                    iReg[0x07] = iReg[0x87];
                    iReg[0x0A] = iReg[0x8A];
                    iReg[0x0B] = iReg[0x8B];
                    if ( (iReg[0x03] & 0x20 ) == 0)
                    {
                        _bank0 = true;
                    }
                }
                setInterrupt();
                checkInterrupt();
                
                if (bSerialCon)
                {
                    _sendSerialData( );
                }
                refreshReg();
                _markCommand( iPC );
                lRuntime = lCycles * 200;
                checkBreakPoint();
                watchdog();
                simulateTimer0();
            }

        }



        /// <summary>
        /// Refresh der Anzeigeinhalte
        /// Schreibt die Datenarrays in die dafür vorgesehenen Grids
        /// </summary>
        private void refreshGridValue()
        {
            // steering with iReg
            int zeile;
            int spalte;

            for (int i = 0 ; i < 127 ; i++)
            {
                zeile = i / 8;
                spalte = i % 8;

                //              ArrayBank0[zeile , spalte] = null;
                ArrayBank0[zeile , spalte] = iReg[i].ToString( "X" ).PadLeft( 2 , '0' );
            }



            for (int i = 128 ; i < 255 ; i++) // Update 
            {
                zeile = i / 8;
                zeile %= 16;
                spalte = i % 8;

                //              ArrayBank1[zeile , spalte] = null;
                ArrayBank1[zeile , spalte] = iReg[i].ToString( "X" ).PadLeft( 2 , '0' );
            }

            //EEPROM-Register aktualisieren

            for (int i = 0 ; i < 63 ; i++) // Update 
            {
                zeile = i / 8;
                spalte = i % 8;

                //                ArrayEEPROM[zeile , spalte] = null;
                ArrayEEPROM[zeile , spalte] = iEEPROM[i].ToString( "X" ).PadLeft( 2 , '0' );
            }



            int Col = 9;
            int Row = 17;

            // Bank1
            for (int r = 1; r < Row; r++)
            {
                for (int c = 1; c < Col; c++)
                {
                    gridBank_1[r, c].Value = ArrayBank1[r - 1, c - 1];
                }
            }
            // Bank0
            for (int r = 1; r < Row; r++)
            {
                for (int c = 1; c < Col; c++)
                {
                    gridBank_0[r, c].Value = ArrayBank0[r - 1, c - 1];
                }
            }
            // EEPROM
            for (int r = 1; r < 9; r++)
            {
                for (int c = 1; c < 9; c++)
                {
                    gridEEPROM[r, c].Value = ArrayEEPROM[r - 1, c - 1];
                }
            }

            
            //for (int c = 1; c < 9; c++)
            //{
            //    // Status Register 
            //    ArrayStatusReg = new string[8];
            //    if ( ArrayStatusReg[c - 1] != null) 
            //    {
            //        gridStatus[1, c].Value = ArrayStatusReg[c - 1];
            //    }

            //    // Option Register
            //    ArrayOptionReg = new string[8];
            //    if ( ArrayOptionReg[c - 1] != null) 
            //    {
            //        gridOption[1, c].Value = ArrayOptionReg[c - 1];
            //    }

            //    ArrayInterruptReg = new string[8];           
            //    if ( ArrayInterruptReg[c - 1] != null) 
            //    {
            //        gridInterrupt[1, c].Value = ArrayInterruptReg[c - 1];
            //    }

            //    ArrayStack = new string[8];
            //    if ( ArrayStack[c - 1] != null) 
            //    {
            //        gridStack[1, c].Value = ArrayStack[c - 1];
            //    }
            //}
        }

        #region CommandList


        private void lvCode_DoubleClick(object sender, EventArgs e)// Einfärben der BreakePoint Zeilen
        {

            int myindex = lvCode.SelectedItems[0].Index;
            if (lvCode.Items[myindex].BackColor == Color.Red)
            {
                lvCode.Items[myindex].BackColor = Color.White;
            }
            else { lvCode.Items[myindex].BackColor = Color.Red; }

        }


        private bool checkBreakPoint()
        {
            {
                if (lvCode.Items[iPC].Selected == true)
                {
                    if (lvCode.Items[iPC].BackColor == Color.Red)
                    {
                        Stop( );
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private delegate void MarkCommandDelegate( int cmdNumber );
        private void _markCommand( int cmdNumber )
        {
            if ( InvokeRequired )
            { 
                var invokeVar = new MarkCommandDelegate( _markCommand );
                Invoke(invokeVar, cmdNumber);
            }
            else
            {
                if (!_bOpenedFile)
                { 
                    return;
                }
                // markieren des nächsten Befehls
                lvCode.Items[cmdNumber].Selected = true;
                lvCode.Items[cmdNumber].EnsureVisible();
                lvCode.Items[cmdNumber].Focused = true;
                lvCode.TopItem = lvCode.Items[cmdNumber]; //listView.TopItem = listView1.Items[row#];
                lvCode.Select( );
            }
        }
        #endregion CommandList

        #region SimButtons

        /// <summary>
        /// Startet den Prorgammdurchlauf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (OperandList == null || OperandList.Count <= 0) { MessageBox.Show("Bitte laden sie eine .LST Datei"); }
            else
            {
                // start programme
                // Set running to true
                _running = true;
                //Threads anlegen
                _iDelay = Convert.ToInt32(cbDelay.SelectedItem);
                Thread simulateThread = new Thread(simulateProgram);
                simulateThread.Start(); //Programmablauf
                btnStart.Enabled = false;
                btnStep.Enabled = false;
                btnStop.Enabled = true;
            }
        }

        

        /// <summary>
        /// "Pausiert" das Programm/ Stoppt es
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            _running = false;
            Stop();
        }

        /// <summary>
        /// Setzt alles zurück(Programmzähler, Register, etc)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            _running = false;
            SimReset();
        }

        /// <summary>
        /// Einen Befeh des Programms ausführen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStep_Click(object sender, EventArgs e)
        {
            if (_obDecoder == null) return;

            if (OperandList == null || OperandList.Count <= 0) { MessageBox.Show("Bitte laden sie eine .LST Datei"); }
            else
            {
                ExecuteCmd();
            }
        }

        #endregion SimButtons

        #region MenuItems

        private void beendenToolStripMenuItem_Click( object sender , EventArgs e ) // End the Application
        {
            this.Close( );
        }

        /// <summary>
        /// Der Button zum Datei Laden im ToolStrip Menü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateiLadenToolStripMenuItem_Click( object sender , EventArgs e ) // Laden der .LST
        {
            OpenFileDialog MyOpenFileDialog = new OpenFileDialog( );
            MyOpenFileDialog.Filter = "MASM Listing (*.LST)|*.LST|All files (*.*)|*.*";
            DialogResult myresult = MyOpenFileDialog.ShowDialog( );

            if ( myresult == DialogResult.OK )
            {
                string filepath = MyOpenFileDialog.FileName;
                
                try
                {
                    List<string> StrList = new List<string>( );

                    _bOpenedFile = true;

                    System.IO.StreamReader myReader = new System.IO.StreamReader( filepath );
                    string FileLine;
                    
                    while ( ( FileLine = myReader.ReadLine( ) ) != null )
                    {
                        StrList.Add( FileLine );
                    }

                    StringCutter myCutter = new StringCutter( );
                    OperandList = myCutter.StringCutterFkt( StrList );
                    List<string> TStrList = myCutter.CodeView( StrList );
                    lvCode.Items.Clear( ); // Leeren der Code Anzeige

                    lvCode.Columns.Add( "" , 500 ); //Eine Spalte einfügen( Überschrift,breite )

                    for (int i = 0 ; i < TStrList.Count ; i++)
                    {
                        lvCode.Items.Add( new ListViewItem( new String[] { TStrList[i] } ) );
                    }
                }
                catch { MessageBox.Show( "Fehler bei Dateiaufruf" ); }
            } // Dialogresult

            // Reset 
            SimReset();
            refreshReg();

        }


        private void documentationToolStripMenuItem_Click( object sender , EventArgs e )
        {
            string Path = Application.StartupPath + "/KerTKDSim-Documentation.pdf";

            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process( );
                p.StartInfo.FileName = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( Application.ExecutablePath ) , Path ); ;
                p.Start( );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }
        #endregion DebugButtons
    }
}
