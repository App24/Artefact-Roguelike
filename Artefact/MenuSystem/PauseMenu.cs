using Artefact.Saving;
using Artefact.States;
using Artefact.Utils;
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
            AddOption("Save Game", () =>
            {
                SaveSystem.SaveGame();
                Console.Write("\n\n");
                int currentY = Console.CursorTop;
                Console.WriteLine("Game Saved!");
                Input.NextKeyPress += () =>
                {
                    Console.SetCursorPosition(0, currentY);
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                };
            });

            if (SaveSystem.HasSaveGame)
            {
                AddOption("Load Game", () =>
                {
                    GameState.SkipNextDraw = true;
                    for (int i = 0; i < 2; i++)
                    {
                        StateMachine.RemoveState();
                        StateMachine.ProcessStateChanges();
                    }

                    SaveSystem.LoadGame();
                    StateMachine.AddState(new GameState());
                    Input.SkipNextKey = true;
                });
            }

            AddOption("Main Menu", () =>
            {
                GameState.SkipNextDraw = true;
                for (int i = 0; i < 2; i++)
                {
                    StateMachine.RemoveState();
                    StateMachine.ProcessStateChanges();
                }

                StateMachine.AddState(new MenuState());
                Input.SkipNextKey = true;
            });

            AddBackOption("Back To Game", () => StateMachine.RemoveState());
        }
    }
}
