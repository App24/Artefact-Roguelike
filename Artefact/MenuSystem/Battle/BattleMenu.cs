using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MenuSystem.Battle
{
    internal class BattleMenu : Menu
    {
        private readonly List<AIEnemy> enemies;

        public BattleMenu(int yPlacement, List<AIEnemy> enemies) : base(yPlacement)
        {
            this.enemies = enemies;
        }

        protected override void AddHeadings()
        {

        }

        protected override void AddOptions()
        {
            AddOption("Attack", () =>
            {
                Menu.SwitchMenu(new FightMenu(yPlacement, enemies), false);
            });

            AddOption("Defend", () =>
            {

            });

            AddOption("Run Away", () =>
            {

            });
        }
    }
}
