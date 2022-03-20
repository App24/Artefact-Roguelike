using Artefact.MenuSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class PauseState : State
    {
        public override void Init()
        {
            Menu.SwitchMenu(new PauseMenu());
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();

            if (Input.IsKeyHeld(ConsoleKey.Escape))
            {
                StateMachine.RemoveState();
                Input.SkipNextKey = true;
            }
        }
    }
}
