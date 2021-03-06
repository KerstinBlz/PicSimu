﻿/**************************************************************************
**
**  KerTKDSim
**
**  StringCutter.cs: 
**  ---------
**  Cuts the string... reads the programme parameters
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
    class StringCutter
    {
        public List<string> _lstOperandString = new List<string>();

        /// <summary>
        /// Erstellen der Liste mit den auszuführenden Operanden
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<string> StringCutterFkt(List<string> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i].ToString();

                string MyString = line.Substring(5, 4);
                if ( MyString[1] != ' ' )
                {
                    _lstOperandString.Add(MyString);
                }
            } 
            return _lstOperandString;
        }

        /// <summary>
        /// Gibt eine Liste der Befehsrelevanten Zeilen als Strings zurück
        /// </summary>

        public List<string> CodeView ( List<string> input )
        {
            List<string> _lstOutput = new List<string>();
            
            for ( int i = 0; i < input.Count; i++ )
            {
                string line = input[i].ToString();

                if ( line[1] != ' ' )
                {
                    _lstOutput.Add(line);
                }
            }

            // Kommentare entfernen
            for (int i = 0 ; i < _lstOutput.Count ; i++)
            {
                if (_lstOutput[i].Contains( ';' ))
                {
                    int stelle = _lstOutput[i].IndexOf( ';' );
                    _lstOutput[i] = _lstOutput[i].Remove( stelle );
                }
            }

            return _lstOutput;
        }
    }
}