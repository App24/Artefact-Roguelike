using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal class GenericMenu : Menu
    {
        public GenericMenu(Vector2 offset) : base(offset)
        {

        }

        protected override void AddHeadings()
        {

        }

        protected override void AddOptions()
        {

        }

        public new void AddOption(string text, Action onSelect, bool ascii = false)
        {
            base.AddOption(text, onSelect, ascii);
        }

        public new void AddOption(Func<string> text, Action onSelect)
        {
            base.AddOption(text, onSelect);
        }

        public new void AddBackOption(string text = null, Action onSelection = null, bool clearScreen = true)
        {
            base.AddBackOption(text, onSelection, clearScreen);
        }

        public new void AddHeading(string text, bool ascii = true)
        {
            base.AddHeading(text, ascii);
        }
    }
}
