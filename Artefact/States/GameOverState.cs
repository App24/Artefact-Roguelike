using Artefact.MenuSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class GameOverState : State
    {
        public override void Init()
        {
            Menu.SwitchMenu(null);
            Menu.SwitchMenu(new GameOverMenu());
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();
        }
    }
}
