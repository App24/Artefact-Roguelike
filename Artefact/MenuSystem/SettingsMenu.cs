using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal class SettingsMenu : Menu
    {
        public SettingsMenu()
        {
            AddHeading("Settings");

            AddBackOption();
        }
    }
}
