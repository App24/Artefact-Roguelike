using Artefact.Settings;
using Artefact.States;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal class MainMenu : Menu
    {
        protected override void AddHeadings()
        {
            AddHeading("Artefact");
            AddHeading("Made By Ricardo Xavier - 2112018", false);
        }

        protected override void AddOptions()
        {
            AddOption("Play Game", () =>
            {
                StateMachine.AddState(new GameState());
                Input.SkipNextKey = true;
            });

            AddOption("Settings", () =>
            {
                SwitchMenu(new SettingsMenu());
            });

            AddBackOption(onSelection: () => GlobalSettings.Running = false);
        }
    }
}
