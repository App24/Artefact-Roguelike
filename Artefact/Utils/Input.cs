using Artefact.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal static class Input
    {
        private static ConsoleKeyInfo keyInfo;
        public static bool SkipNextKey { get; set; }
        public static event Action NextKeyPress;

        public static void GetInput()
        {
            keyInfo = new ConsoleKeyInfo();
            if (!SkipNextKey)
            {
                keyInfo = Console.ReadKey(true);
                NextKeyPress?.Invoke();
                NextKeyPress = null;
            }
            SkipNextKey = false;
        }

        public static bool IsKeyHeld(ConsoleKey key)
        {
            return IsKeyHeld(new ConsoleKey[] { key });
        }

        public static bool IsKeyHeld(params ConsoleKey[] keys)
        {
            List<ConsoleKey> list = new List<ConsoleKey>(keys);
            return list.Contains(keyInfo.Key);
        }
    }
}
