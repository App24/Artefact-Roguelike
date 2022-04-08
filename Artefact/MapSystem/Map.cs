using Artefact.Entities;
using Artefact.Tiles;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MapSystem
{
    internal class Map
    {
        private List<Room> rooms = new List<Room>();
        private Random random = new Random();
        private List<Entity> entities = new List<Entity>();

        public static Map Instance { get; set; }

        public int Width { get; }
        public int Height { get; }

        public Map(int maxRooms, int width, int height)
        {
            Width = width;
            Height = height;
            for (int i = 0; i < maxRooms; i++)
            {
                SpawnRoom();
            }
            ConnectRooms();
            if (!entities.Contains(PlayerEntity.Instance))
                entities.Add(PlayerEntity.Instance);
            SpawnEnemies();
        }

        void SpawnEnemies()
        {
            foreach(Room room in rooms)
            {
                EnemyEntity entity = new EnemyEntity("TE", 10);
                entity.position = room.Position + new Vector2(3, 3);
                entities.Add(entity);
            }
        }

        internal void PrintEntities()
        {
            PlayerEntity.Instance.Inventory.PrintInventory();
            foreach (Entity entity in entities)
            {
                if (entity.CurrentRoom != PlayerEntity.Instance.CurrentRoom)
                    continue;
                PrintEntity(entity);
            }
        }

        private void ConnectRooms()
        {
            for (int i = 0; i < rooms.Count - 1; i++)
            {
                Room room = rooms[i];
                Room connectRoom = rooms[i + 1];

                List<TeleportTile> teleports = new List<TeleportTile>();

                int roomX = random.Next(1, room.Width - 1);
                int connectX = random.Next(1, connectRoom.Width - 1);

                teleports.Add(room.SetTile(roomX, 0, new TeleportTile(connectRoom, new Vector2(connectX, connectRoom.Height - 2), room.GetTile(new Vector2(roomX, 0)))));

                room.SetTile(roomX, 1, new RevealTile(teleports, room));

                teleports.Clear();

                teleports.Add(connectRoom.SetTile(connectX, connectRoom.Height - 1, new TeleportTile(room, new Vector2(roomX, 1), connectRoom.GetTile(new Vector2(connectX, connectRoom.Height - 1)))));

                connectRoom.SetTile(connectX, connectRoom.Height - 2, new RevealTile(teleports, connectRoom));
            }
        }

        private void SpawnRoom()
        {
            bool overlapping;
            int width;
            int height;
            Vector2 pos;
            int tries = 0;
            do
            {
                width = random.Next(10, 18);
                height = random.Next(10, 18);
                pos = random.NextVector2(new Vector2(Width, Height));

                overlapping = CheckOverlap(width, height, pos);
                tries++;
            } while (overlapping && tries < 5);

            if (tries < 5)
            {
                Room room = new Room(width, height, pos);

                rooms.Add(room);
            }
        }

        private bool CheckOverlap(int width, int height, Vector2 position)
        {
            return rooms.Find(room =>
            {
                return room.Position.x - 1 < position.x + width &&
                    room.Position.y - 1 < position.y + height &&
                    position.x - 1 < room.Position.x + room.Width &&
                    position.y - 1 < room.Position.y + room.Height;
            }) != null || position.x + width > Width || position.y + height > Height;
        }

        public Room GetRoom(Vector2 position)
        {
            return rooms.Find(room =>
            {
                return room.Position.x <= position.x &&
                room.Position.y <= position.y &&
                position.x <= room.Position.x + room.Width &&
                position.y <= room.Position.y + room.Height;
            });
        }

        public void Update()
        {
            foreach(Entity entity in entities)
            {
                entity.Update();
            }

            foreach (Entity entity in entities)
            {
                if (entity.CurrentRoom != PlayerEntity.Instance.CurrentRoom)
                    continue;
                Vector2 previousPos = entity.RelativePosition;
                Room previousRoom = entity.CurrentRoom;
                entity.Move();

                if (previousPos == entity.RelativePosition && previousRoom==entity.CurrentRoom)
                    continue;

                Tile currentTile = entity.CurrentRoom.GetTile(entity.RelativePosition);
                if (currentTile.Collidable)
                {
                    entity.position = previousPos + entity.CurrentRoom.Position;
                }
                currentTile.OnCollision(entity);

                if (entity.CurrentRoom != previousRoom)
                {
                    PrintRoom(previousRoom);
                    PrintRoom(entity.CurrentRoom);
                    Map.Instance.PrintEntities();
                }

                if (entity.RelativePosition != previousPos || entity.CurrentRoom != previousRoom)
                {
                    PrintEntity(entity);
                    if (entity.CurrentRoom == previousRoom)
                        PrintTile(entity.CurrentRoom, previousPos);
                }
            }

            entities.RemoveAll(e => e.Health <= 0);
        }

        public Room GetRandomRoom()
        {
            return rooms[random.Next(rooms.Count)];
        }

        public void PlaceEntityInRandomRoom(Entity entity)
        {
            Room room = GetRandomRoom();
            if (entity is PlayerEntity)
            {
                room.Known = true;
            }
            entity.position = room.Position + room.GetAvailablePosition();
        }

        public void PrintMap()
        {
            foreach (Room room in rooms)
            {
                if (!room.Known)
                    continue;
                PrintRoom(room);
            }

            PrintEntities();

            PlayerEntity.Instance.PrintHealth();
        }

        private void PrintEntity(Entity entity)
        {
            Console.CursorLeft = entity.position.x * 2;
            Console.CursorTop = entity.position.y;
            Console.Write(entity.Representation);
        }

        public void PrintRoom(Room room)
        {
            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    Console.CursorLeft = (room.Position.x + x) * 2;
                    Console.CursorTop = room.Position.y + y;
                    bool currentRoom = PlayerEntity.Instance.CurrentRoom == room;
                    if (currentRoom)
                    {
                        PrintTile(room, new Vector2(x, y));
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("@@");
                        Console.ResetColor();
                    }
                }
            }
        }

        private void PrintTile(Room room, Vector2 position)
        {
            Console.CursorLeft = (room.Position.x + position.x) * 2;
            Console.CursorTop = room.Position.y + position.y;
            Tile tile = room.GetTile(position);
            Console.ForegroundColor = tile.Foreground;
            for (int i = 0; i < 3 - tile.Representation.Length; i++)
            {
                Console.Write(tile.Representation);
            }
            Console.ResetColor();
        }
    }
}
