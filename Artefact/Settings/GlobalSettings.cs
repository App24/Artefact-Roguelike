using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Settings
{
    internal class GlobalSettings
    {
        public static bool Running { get; set; } = true;

        public static GlobalSettings Instance { get; set; }

        public GlobalSettings()
        {
            Instance = this;
        }
    }
}
