using Artefact.Entities;
using Artefact.Items;
using Artefact.MapSystem;
using Artefact.MenuSystem;
using Artefact.Settings;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.States
{
    internal class BattleState : State
    {
        List<EnemyEntity> enemyEntities;

        const int Y_OFFSET = 6;

        Random random = new Random();

        public BattleState(List<EnemyEntity> enemyEntities)
        {
            this.enemyEntities = enemyEntities;
        }

        public override void Remove()
        {
            Console.Clear();
            GameSettings.InBattle = false;
        }

        public override void Pause()
        {
            Remove();
        }

        public override void Init()
        {
            GameSettings.InBattle = true;
            Console.Clear();

            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Write(ASCIIGenerator.GenerateASCII("Battle"));

            WriteEntities();
            WritePlayerStats();
            WriteEnemiesStats();

            Vector2 offset = new Vector2(0, GetPlayerStatsPosition() + 3);

            GenericMenu battleMenu = new GenericMenu(offset);
            battleMenu.AddOption("Attack", () =>
            {
                if(enemyEntities.Count > 1)
                {
                    GenericMenu attackMenu = new GenericMenu(offset);

                    attackMenu.AddOption("Attack all", () =>
                    {
                        for (int i = 0; i < enemyEntities.Count; i++)
                        {
                            AttackEnemy(i);
                        }
                        EnemyTurn();
                    });

                    for (int i = 0; i < enemyEntities.Count; i++)
                    {
                        attackMenu.AddOption(enemyEntities[i].Representation, () =>
                        {
                            AttackEnemy(i);
                            EnemyTurn();
                        });
                    }

                    attackMenu.AddBackOption(clearScreen: false);

                    Menu.SwitchMenu(attackMenu, false);
                }
                else
                {
                    AttackEnemy(0);
                    EnemyTurn();
                }
            });

            battleMenu.AddOption("Defend", () =>
            {
                PlayerEntity.Instance.Defending = true;
                EnemyTurn();
                PlayerEntity.Instance.Defending = false;
            });

            battleMenu.AddOption("Use Item", () =>
            {
                List<Item> usableItems = PlayerEntity.Instance.Inventory.items.FindAll(i => i is IUsable);
                GenericMenu itemsMenu = new GenericMenu(offset);
                if(usableItems.Count > 0)
                {
                    foreach(Item item in usableItems)
                    {
                        IUsable usable = (IUsable)item;
                        itemsMenu.AddOption(() => $"{item.Name}: {item.Quantity}", () =>
                        {
                            if (usable.OnUse())
                            {
                                PlayerEntity.Instance.Inventory.RemoveItem(item);
                                WritePlayerStats();
                                Menu.Instance.Back(false);
                            }
                        });
                    }
                }
                else
                {
                    itemsMenu.AddOption("No Usable Items!", null);
                }

                itemsMenu.AddBackOption(clearScreen: false);

                Menu.SwitchMenu(itemsMenu, false);
            });

            Menu.SwitchMenu(battleMenu, false);
        }

        void EnemyTurn()
        {
            enemyEntities.RemoveAll(e => e.Health <= 0);

            foreach (EnemyEntity enemy in enemyEntities)
            {
                enemy.Defending = random.NextBool();
                if (!enemy.Defending)
                {
                    PlayerEntity.Instance.Damage(enemy.HitDamage);
                }
            }

            InputSystem.SkipNextKey = true;

            if (PlayerEntity.Instance.Health <= 0)
            {
                StateMachine.CleanStates();
                StateMachine.AddState(new GameOverState());
                return;
            }

            Console.Clear();

            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Write(ASCIIGenerator.GenerateASCII("Battle"));

            WriteEntities();
            WritePlayerStats();
            WriteEnemiesStats();

            if(enemyEntities.Count <= 0)
            {
                StateMachine.RemoveState();
            }
        }

        void AttackEnemy(int index)
        {
            EnemyEntity enemy = enemyEntities[index];
            enemy.Damage(PlayerEntity.Instance.HitDamage);
        }

        public override void Update()
        {
            Menu.Instance.NavigateOptions();
        }

        private void WriteEntities()
        {
            for (int i = 0; i < enemyEntities.Count; i++)
            {
                Console.CursorLeft = 4;
                Console.CursorTop = i * 2 + Y_OFFSET;
                if (enemyEntities[i].Health > 0)
                    Console.Write(enemyEntities[i].Representation);
                else
                    Console.Write("XX");
            }

            Console.CursorLeft = 0;
            Console.CursorTop = (int)(MathF.Floor(enemyEntities.Count / 2f) * 2) + Y_OFFSET;
            Console.Write(PlayerEntity.Instance.Representation);
        }

        int GetPlayerStatsPosition()
        {
            return Y_OFFSET + (enemyEntities.Count * 2) + 3;
        }

        void WritePlayerStats()
        {
            int pos = GetPlayerStatsPosition();
            Console.CursorLeft = 0;
            Console.CursorTop = pos;

            Console.CursorLeft = 0;
            Console.Write($"Player Health: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(PlayerEntity.Instance.Health);
            Console.ResetColor();

            Console.Write("Attack Damage: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(PlayerEntity.Instance.HitDamage);
            Console.ResetColor();

            Console.Write("Defense: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerEntity.Instance.Defense);
            Console.ResetColor();
        }

        void WriteEnemiesStats()
        {
            int xOffset = 20;
            for (int i = 0; i < enemyEntities.Count; i++)
            {
                Console.CursorLeft = xOffset;
                Console.CursorTop = Y_OFFSET + i * 5;
                EnemyEntity enemyEntity = enemyEntities[i];
                Console.Write($"Enemy: {enemyEntity.Representation}");
                Console.CursorTop++;
                Console.CursorLeft = xOffset;
                Console.Write($"Health: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(enemyEntity.Health);
                Console.ResetColor();
            }
        }
    }
}
