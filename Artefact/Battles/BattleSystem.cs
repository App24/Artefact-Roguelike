using Artefact.Entities;
using Artefact.States;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Battles
{
    internal static class BattleSystem
    {
        public static void StartFight(AIEnemy enemy)
        {
            List<AIEnemy> enemies = new List<AIEnemy>();
            int enemyCount = World.Instance.Random.Next(3);
            enemies.Add(enemy);
            for (int i = 0; i < enemyCount; i++)
            {
                enemies.Add(enemy.Clone());
            }
            StateMachine.AddState(new BattleState(enemies), false);
        }
    }
}
