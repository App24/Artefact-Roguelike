using Artefact.Entities;
using Artefact.MenuSystem;
using Artefact.MenuSystem.Battle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class BattleState : State
    {
        private readonly List<AIEnemy> enemies;

        public BattleState(List<AIEnemy> enemies)
        {
            this.enemies = enemies;
        }

        public override void Init()
        {
            Console.Clear();

            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Write(PlayerEntity.Instance.Representation);

            Console.CursorLeft = 0;
            Console.CursorTop += 2;

            foreach (AIEnemy enemy in enemies)
            {
                Console.Write(enemy.Representation);
                Console.Write(" ");
            }

            Menu.SwitchMenu(new BattleMenu(3, enemies), false);
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();
        }
    }
}
