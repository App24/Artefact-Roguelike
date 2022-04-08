using Artefact.Entities;
using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class GameState : State
    {
        public static bool SkipNextDraw { get; set; }

        public override void Init()
        {
            Map.Instance.PrintMap();
        }

        public override void Update()
        {
            Map.Instance.Update();
            if (InputSystem.IsKeyHeld(ConsoleKey.Escape))
            {
                StateMachine.AddState(new PauseState(), false);
                InputSystem.SkipNextKey = true;
            }
        }

        public override void Resume()
        {
            if (!SkipNextDraw)
            {
                Map.Instance.PrintMap();
            }
            SkipNextDraw = false;
        }
    }
}
