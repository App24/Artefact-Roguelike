using Artefact.Saving;
using Artefact.Utils;
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
            World.Instance.PlacePlayer();
            World.Instance.PrintTiles();
            SaveSystem.SaveGame();
        }

        public override void Update()
        {
            World.Instance.Update();
            if (Input.IsKeyHeld(ConsoleKey.Escape))
            {
                StateMachine.AddState(new PauseState(), false);
                Input.SkipNextKey = true;
            }
        }

        public override void Resume()
        {
            World.Instance.PrintTiles();
        }
    }
}
