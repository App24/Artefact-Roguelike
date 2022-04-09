using Artefact.Settings;
using Artefact.States;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal class GameOverMenu : Menu
    {
        protected override void AddHeadings()
        {
            AddHeading("GAME OVER");
        }

        protected override void AddOptions()
        {
            AddOption("Back To Main Menu", () =>
            {
                StateMachine.AddState(new MenuState());
                InputSystem.SkipNextKey = true;
            });

            AddBackOption(onSelection: () => GlobalSettings.Running = false);
        }
    }
}
