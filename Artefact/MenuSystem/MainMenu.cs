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
        public MainMenu()
        {
            AddHeading("Artefact");
            AddHeading("   Roguelike");
            AddHeading("          Made");
            AddHeading("             By");
            AddHeading("              Ricardo");

            AddOption("Play Game", () =>
            {
                StateMachine.AddState(new GameState());
                Input.SkipNextKey = true;
            });

            AddOption("Settings", () =>
            {
                SwitchMenu(new SettingsMenu());
            });

            AddBackOption();
        }
    }
}
