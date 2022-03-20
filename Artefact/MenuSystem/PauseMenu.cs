using Artefact.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal class PauseMenu : Menu
    {
        protected override void AddHeadings()
        {
            AddHeading("Game Paused");
        }

        protected override void AddOptions()
        {
            AddBackOption("Back To Game", () => StateMachine.RemoveState());
        }
    }
}
