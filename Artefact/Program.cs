using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 160;
            //Console.WindowHeight = 65;
            Console.WindowHeight = 40;

            World world = new World();
            while (true)
            {
                world.PrintTiles();
                world.Update();
            }
        }
    }
}
