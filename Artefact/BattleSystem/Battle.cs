using Artefact.Audio;
using Artefact.Audio.SoundEffectsSystem;
using Artefact.Entities;
using Artefact.States;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.BattleSystem
{
    internal static class Battle
    {
        public static void StartBattle(EnemyEntity enemy)
        {
            Random random = new Random();
            int enemyCount = random.Next(0, 3);
            List<EnemyEntity> enemies = new List<EnemyEntity>();

            for (int i = 0; i < enemyCount; i++)
            {
                enemies.Add(new EnemyEntity(enemy.EnemyType));
            }

            enemies.Add(enemy);
            StateMachine.AddState(new BattleState(enemies), false);

            SFXSystem.AddSoundEffect(new SoundEffect(SoundEffectType.Entity,
                new Note(Tone.G, Duration.SIXTEENTH),
                new Note(Tone.G, Duration.SIXTEENTH),
                new Note(Tone.D, Duration.SIXTEENTH),
                new Note(Tone.G, Duration.SIXTEENTH),
                new Note(Tone.G, Duration.SIXTEENTH),
                new Note(Tone.D, Duration.SIXTEENTH)
                ));
            InputSystem.SkipNextKey = true;
        }
    }
}
