using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal static class Input
    {
        private static ConsoleKeyInfo keyInfo;

        public static void GetInput()
        {
            keyInfo = Console.ReadKey(true);
        }

        public static bool IsKeyHeld(ConsoleKey key)
        {
            return IsKeyHeld(key);
        }

        public static bool IsKeyHeld(params ConsoleKey[] keys)
        {
            List<ConsoleKey> list = new List<ConsoleKey>(keys);
            return list.Contains(keyInfo.Key);
        }
    }
}
