﻿using Artefact.Entities;
using Artefact.MapSystem;
using Artefact.Settings;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Artefact
{
    internal class Program
    {
        // https://stackoverflow.com/questions/22053112/maximizing-console-window-c-sharp/22053200
        // Châu Nguyễn

        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static void Main(string[] args)
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 3);
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            Console.CursorVisible = false;

            new PlayerEntity();

            Map map = new Map(15, 80, 35);
            Map.Instance = map;

            map.PlaceEntityInRandomRoom(PlayerEntity.Instance);

            map.PrintMap();

            while (GlobalSettings.Running)
            {
                InputSystem.GetInput();
                map.Update();
            }
        }
    }
}
