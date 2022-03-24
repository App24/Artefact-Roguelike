using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem.Battle
{
    internal class FightMenu : Menu
    {
        private readonly List<AIEnemy> enemies;
        private bool ignoreAddOptions = true;

        public FightMenu(int yPlacement, List<AIEnemy> enemies) : base(yPlacement)
        {
            this.enemies = enemies;
            ignoreAddOptions = false;
            AddOptions();
        }

        protected override void AddHeadings()
        {

        }

        protected override void AddOptions()
        {
            if (ignoreAddOptions) return;
            foreach (AIEnemy enemy in enemies)
            {
                AddOption(() => $"{enemy.Representation}: {enemy.Health}", () =>
                {

                });
            }

            AddBackOption(clearScreen: false);
        }
    }
}
