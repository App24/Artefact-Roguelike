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
            AddOption("Start New Game", () =>
            {
                //SaveSystem.NewGame();
                StateMachine.AddState(new GameState());
                InputSystem.SkipNextKey = true;
                Console.Clear();
            });

            /*if (SaveSystem.HasSaveGame)
            {
                AddOption("Load Game", () =>
                {
                    LoadResult loadResult = SaveSystem.LoadGame();
                    if (loadResult == LoadResult.Success)
                    {
                        StateMachine.AddState(new GameState());
                        Input.SkipNextKey = true;
                    }
                    else
                    {
                        Console.Write("\n\n");
                        int currentY = Console.CursorTop;
                        Console.WriteLine("Failed to load game");
                        InputSystem.NextKeyPress += () =>
                        {
                            Console.SetCursorPosition(0, currentY);
                            Console.WriteLine(new string(' ', Console.WindowWidth));
                        };
                    }
                });
            }*/

            AddOption("Settings", () =>
            {
                //SwitchMenu(new SettingsMenu());
            });

            AddBackOption(onSelection: () => GlobalSettings.Running = false);
        }
    }
}
