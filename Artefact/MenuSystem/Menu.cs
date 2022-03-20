using Artefact.Settings;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal abstract class Menu
    {
        public static Menu Instance { get; private set; }
        private Menu parentMenu;
        private List<string> headings = new List<string>();
        private List<Option> options = new List<Option>();

        private int selectIndex;

        public Menu()
        {
            parentMenu = Instance;
        }

        protected void AddOption(string text, Action onSelect)
        {
            options.Add(new Option(text, onSelect));
        }

        protected void AddBackOption()
        {
            if (parentMenu == null)
            {
                AddOption("Quit", () =>
                {
                    GlobalSettings.Running = false;
                    Input.SkipNextKey = true;
                });
            }
            else
            {
                AddOption("Back", () =>
                {
                    SwitchMenu(parentMenu);
                    Input.SkipNextKey = true;
                });
            }
        }

        public static void SwitchMenu(Menu menu)
        {
            Input.SkipNextKey = true;
            Console.Clear();
            Instance = menu;
        }

        protected void AddHeading(string text)
        {
            string[] lines = text.Split("\n");
            foreach (string line in lines)
            {
                headings.Add(line);
            }
        }

        public void NavigateOptions()
        {
            if (Input.IsKeyHeld(ConsoleKey.S, ConsoleKey.DownArrow))
            {
                selectIndex++;
                if (selectIndex >= options.Count)
                    selectIndex = options.Count - 1;
            }
            else if (Input.IsKeyHeld(ConsoleKey.W, ConsoleKey.UpArrow))
            {
                selectIndex--;
                if (selectIndex < 0)
                    selectIndex = 0;
            }

            for (int i = 0; i < headings.Count; i++)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = i;
                Console.Write(headings[i]);
            }

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.CursorLeft = 0;
                Console.CursorTop = i + headings.Count + 1;
                Console.Write(options[i].text);

                Console.ResetColor();
            }

            if (Input.IsKeyHeld(ConsoleKey.Enter))
            {
                options[selectIndex].onSelect();
            }
        }
    }

    internal struct Option
    {
        public Action onSelect;
        public string text;

        public Option(string text, Action onSelect)
        {
            this.onSelect = onSelect;
            this.text = text;
        }
    }
}
