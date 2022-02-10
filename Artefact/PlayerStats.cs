using Artefact.Entities;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    internal static class PlayerStats
    {
        static void ClearStats()
        {
            for (int i = 0; i < 1; i++)
            {
                Console.CursorLeft = (World.Instance.Width * World.TILE_CHAR_WIDTH);
                Console.CursorTop = Console.WindowTop + i;
                Console.Write(new string(' ', Console.BufferWidth - (World.Instance.Width * World.TILE_CHAR_WIDTH)));
            }
        }

        public static void DrawStats()
        {
            //ClearStats();
            Console.CursorLeft = (World.Instance.Width * World.TILE_CHAR_WIDTH) + 1;
            int yPos = Console.WindowTop;
            if (Console.WindowTop >= 0)
                yPos += 1;
            Console.CursorTop = yPos;
            Console.Write($"Health: {PlayerEntity.Instance.Health}/{PlayerEntity.Instance.MaxHealth}");
        }
    }
}
