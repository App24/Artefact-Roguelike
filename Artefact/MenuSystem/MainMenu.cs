using Artefact.Audio;
using Artefact.Audio.MusicSystem;
using Artefact.Saving;
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
        }

        protected override void AddOptions()
        {
            AddOption("Start New Game", () =>
            {
                SaveSystem.NewGame();
                StateMachine.AddState(new GameState());
                InputSystem.SkipNextKey = true;
                Console.Clear();
                Music.ClearQueue();
            });

            if (SaveSystem.HasSaveGame)
            {
                AddOption("Load Game", () =>
                {
                    LoadResult loadResult = SaveSystem.LoadGame();
                    if (loadResult == LoadResult.Success)
                    {
                        StateMachine.AddState(new GameState());
                        InputSystem.SkipNextKey = true;
                        Console.Clear();
                        Music.ClearQueue();
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
            }

            AddOption("Play Jingle", () =>
            {
                //SwitchMenu(new SettingsMenu());
            });

            AddBackOption(onSelection: () => GlobalSettings.Running = false);
        }
    }
}
