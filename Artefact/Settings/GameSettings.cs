using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Settings
{
    [Serializable]
    internal class GameSettings
    {
        public static bool InBattle { get { return Instance.inBattle; } set { Instance.inBattle = value; } }

        public static GameSettings Instance { get; set; }

        private bool inBattle;

        public GameSettings()
        {
            Instance = this;
        }
    }
}
