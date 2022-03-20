﻿using Artefact.Saving;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem
{
    internal class SettingsMenu : Menu
    {
        protected override void AddHeadings()
        {
            AddHeading("Settings");
        }

        protected override void AddOptions()
        {
            AddBackOption(onSelection: () => SaveSystem.SaveSettings());
        }
    }
}
