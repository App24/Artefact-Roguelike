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
            AddHeadings();
            AddOptions();
        }

        protected abstract void AddHeadings();
        protected abstract void AddOptions();

        protected void AddOption(string text, Action onSelect, bool ascii = false)
        {
            options.Add(new Option(ascii ? ASCIIGenerator.GenerateASCII(text) : text, onSelect));
        }

        protected void AddBackOption(string text = null, Action onSelection = null)
        {
            if (parentMenu == null)
            {
                AddOption(text == null ? "Quit" : text, () =>
                      {
                          Input.SkipNextKey = true;
                          onSelection?.Invoke();
                      });
            }
            else
            {
                AddOption(text == null ? "Back" : text, () =>
                      {
                          SwitchMenu(parentMenu);
                          Input.SkipNextKey = true;
                          onSelection?.Invoke();
                      });
            }
        }

        public static void SwitchMenu(Menu menu)
        {
            Input.SkipNextKey = true;
            Console.Clear();
            Instance = menu;
        }

        protected void AddHeading(string text, bool ascii=true)
        {
            string[] lines = text.Split("\n");
            foreach (string line in lines)
            {
                string headingText = ascii ? ASCIIGenerator.GenerateASCII(line) : line;
                foreach (string headingLine in headingText.Split("\n"))
                {
                    headings.Add(headingLine);
                }
            }
        }

        public void NavigateOptions()
        {
            if (Input.IsKeyHeld(ConsoleKey.S, ConsoleKey.DownArrow))
            {
                selectIndex++;
                if (selectIndex >= options.Count)
                    selectIndex = 0;
            }
            else if (Input.IsKeyHeld(ConsoleKey.W, ConsoleKey.UpArrow))
            {
                selectIndex--;
                if (selectIndex < 0)
                    selectIndex = options.Count - 1;
            }

            for (int i = 0; i < headings.Count; i++)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = i;
                Console.Write(headings[i]);
            }

            int spacing = 0;
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                }

                Option option=options[i];

                Console.CursorTop = i + headings.Count + 5 + spacing;
                if (option.resetOffset >= 0)
                {
                    Console.CursorLeft = option.resetOffset;
                    Console.Write(new string(' ', option.resetAmount));
                }

                Console.CursorLeft = 0;
                Console.Write(option.text);
                spacing += option.text.Split("\n").Length;

                Console.ResetColor();
            }

            if (Input.IsKeyHeld(ConsoleKey.Enter))
            {
                options[selectIndex].onSelect?.Invoke();
            }
        }
    }

    internal struct Option
    {
        public Action onSelect;
        public string text;
        public int resetOffset;
        public int resetAmount;

        public Option(string text, Action onSelect, int resetOff, int resetAmount)
        {
            this.onSelect = onSelect;
            this.text = text;
            this.resetOffset = resetOff;
            this.resetAmount = resetAmount;
        }

        public Option(string text, Action onSelect):this(text, onSelect, -1, 0)
        {
            this.onSelect = onSelect;
            this.text = text;
        }
    }
}
