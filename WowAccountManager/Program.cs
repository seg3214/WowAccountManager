// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="nick-cdev">
// Copyright (c) 2016-2026 nick-cdev (https://github.com)
// Licensed under the AGPL-3.0 license.
// 
// DISCLAIMER: This tool is for educational purposes only. 
// The author is not responsible for account bans or system damage.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace WowAccountManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
