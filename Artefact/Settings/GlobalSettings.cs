using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Settings
{
    [Serializable]
    internal class GlobalSettings
    {
        public static bool Running { get; set; } = true;

        public string text;

        public static GlobalSettings Instance { get; set; }

        public GlobalSettings()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
