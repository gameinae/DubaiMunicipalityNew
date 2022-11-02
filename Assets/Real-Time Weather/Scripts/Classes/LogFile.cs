//
// Copyright(c) 2020 Real-Time Weather ASSIST-Software
// https://www.assist.ro
//
// For information about the licensing and copyright of this software please
// contact assist contact@assist.ro
//

using System;
using System.IO;
using UnityEngine;

namespace RealTimeWeather.AlertSystem
{
    /// <summary>
    /// This class it's used for writing logs into a file
    /// </summary>
    public static class LogFile
    {
        #region Private Const
        private const string kPath = "/LogFile.txt";
        #endregion

        #region Public Proprieties
          static  public string Path
        {
            get {  return Application.persistentDataPath + kPath; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Log a message into a file
        /// </summary>
        /// <param name="message"> The message that is write into the file</param>
        public static void Write(string message)
        {
            try
            {
                StreamWriter fileWriters = new StreamWriter(Path, true);
                fileWriters.Write(" \r\n" + message + "\r\n");
                fileWriters.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Can't write into the file!" + e);
            }

        }

        /// <summary>
        /// This function return Log File path
        /// </summary>
        /// <returns>Return the containt of a log file </returns>
        public static string GetLogText()
        {
            string text;
            StreamReader reader = new StreamReader(Path);
            text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        /// <summary>
        /// This function Clear text
        /// </summary>
        public static void ClearText()
        {
            try
            {
                StreamWriter fileWriters = new StreamWriter(Path, false);
                fileWriters.Write(string.Empty);
                fileWriters.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Can't write into the file!" + e);
            }
        }
        #endregion
    }
}