using Artefact.Entities;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class GameState : State
    {
        Map map;

        public override void Init()
        {
            map = new Map(50, (int)(Console.WindowWidth*0.35f), (int)(Console.WindowHeight*0.8f));
            Map.Instance = map;

            map.PlaceEntityInRandomRoom(PlayerEntity.Instance);

            map.PrintMap();
        }

        public override void Update()
        {
            map.Update();
        }
    }
}
