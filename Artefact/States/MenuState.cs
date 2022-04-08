using Artefact.MenuSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class MenuState : State
    {
        public override void Init()
        {
            Menu.SwitchMenu(new MainMenu());
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();
        }
    }
}
