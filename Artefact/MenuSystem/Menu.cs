using Artefact.Audio;
using Artefact.Audio.SoundEffectsSystem;
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
        protected readonly Vector2 offset;

        public Menu() : this(Vector2.Zero)
        {

        }

        public Menu(Vector2 offset)
        {
            parentMenu = Instance;
            this.offset = offset;
            AddHeadings();
            AddOptions();
        }

        protected abstract void AddHeadings();
        protected abstract void AddOptions();

        protected void AddOption(string text, Action onSelect, bool ascii = false)
        {
            AddOption(() => ascii ? ASCIIGenerator.GenerateASCII(text) : text, onSelect);
        }

        protected void AddOption(Func<string> text, Action onSelect)
        {
            options.Add(new Option(text, onSelect));
        }

        protected void AddBackOption(string text = null, Action onSelection = null, bool clearScreen = true)
        {
            if (parentMenu == null)
            {
                AddOption(text == null ? "Quit" : text, () =>
                {
                    InputSystem.SkipNextKey = true;
                    onSelection?.Invoke();
                });
            }
            else
            {
                AddOption(text == null ? "Back" : text, () =>
                {
                    Back(clearScreen);
                    InputSystem.SkipNextKey = true;
                    onSelection?.Invoke();
                });
            }
        }

        public void Back(bool clearScreen = true)
        {
            SwitchMenu(parentMenu, clearScreen);
        }

        public static void SwitchMenu(Menu menu, bool clearScreen = true)
        {
            InputSystem.SkipNextKey = true;
            if (menu != null)
            {
                if (clearScreen)
                {
                    Console.Clear();
                }
                else
                {
                    Console.CursorTop = menu.offset.y;
                    Console.CursorLeft = menu.offset.x;
                    for (int i = 0; i < 20; i++)
                    {
                        Console.WriteLine(new string(' ', Console.WindowWidth));
                    }
                }
            }
            Instance = menu;
        }

        protected void AddHeading(string text, bool ascii = true)
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
            if (InputSystem.IsKeyHeld(ConsoleKey.S, ConsoleKey.DownArrow))
            {
                SFXSystem.AddSoundEffect(new SoundEffect(SoundEffectType.Menu, new Note(Tone.Gsharp, Duration.SIXTEENTH)));
                selectIndex++;
                if (selectIndex >= options.Count)
                    selectIndex = 0;
            }
            else if (InputSystem.IsKeyHeld(ConsoleKey.W, ConsoleKey.UpArrow))
            {
                SFXSystem.AddSoundEffect(new SoundEffect(SoundEffectType.Menu, new Note(Tone.Gsharp, Duration.SIXTEENTH)));
                selectIndex--;
                if (selectIndex < 0)
                    selectIndex = options.Count - 1;
            }

            for (int i = 0; i < headings.Count; i++)
            {
                Console.CursorLeft = offset.x;
                Console.CursorTop = offset.y + i;
                Console.Write(headings[i]);
            }

            int spacing = 0;
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                }

                Option option = options[i];

                Console.CursorTop = offset.y + i + headings.Count + 5 + spacing;
                if (option.resetOffset >= 0)
                {
                    Console.CursorLeft = option.resetOffset + offset.x;
                    Console.Write(new string(' ', option.resetAmount));
                }

                Console.CursorLeft = offset.x;
                string text = option.text();
                Console.Write(text);
                spacing += text.Split("\n").Length;

                Console.ResetColor();
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.Enter))
            {
                SFXSystem.AddSoundEffect(new SoundEffect(SoundEffectType.Menu, new Note(Tone.Fsharp, Duration.SIXTEENTH)));
                options[selectIndex].onSelect?.Invoke();
            }
        }
    }

    internal struct Option
    {
        public Action onSelect;
        public Func<string> text;
        public int resetOffset;
        public int resetAmount;

        public Option(Func<string> text, Action onSelect, int resetOff, int resetAmount)
        {
            this.onSelect = onSelect;
            this.text = text;
            this.resetOffset = resetOff;
            this.resetAmount = resetAmount;
        }

        public Option(Func<string> text, Action onSelect) : this(text, onSelect, -1, 0)
        {
            this.onSelect = onSelect;
            this.text = text;
        }
    }
}
