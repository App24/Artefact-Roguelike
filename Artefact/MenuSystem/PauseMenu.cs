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

            AddBackOption("Back To Game", () => StateMachine.RemoveState());
        }
    }
}
