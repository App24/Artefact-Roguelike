using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class GameState : State
    {
        public override void Init()
        {
            World.Instance = new OverworldWorld(60, 60);
            World.Instance.PlacePlayer();
            World.Instance.PrintTiles();
        }

        public override void Update()
        {

        }
    }
}
