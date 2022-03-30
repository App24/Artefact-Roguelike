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

        public Map(int maxRooms)
        {
            for (int i = 0; i < maxRooms; i++)
            {
                SpawnRoom();
            }
            ConnectRooms();
            if (!entities.Contains(PlayerEntity.Instance))
                entities.Add(PlayerEntity.Instance);
        }

        private void ConnectRooms()
        {
            for (int i = 0; i < rooms.Count - 1; i++)
            {
                Room room = rooms[i];
                Room connectRoom = rooms[i + 1];

                for (int j = 0; j < (room.Width % 2 == 0 ? 2 : 1); j++)
                {
                    room.SetTile((room.Width / 2) - j, 0, new TeleportTile(connectRoom, new Vector2(connectRoom.Width / 2, connectRoom.Height - 2)));
                }

                for (int j = 0; j < (connectRoom.Width % 2 == 0 ? 2 : 1); j++)
                {
                    connectRoom.SetTile((connectRoom.Width / 2) - j, connectRoom.Height - 1, new TeleportTile(room, new Vector2(room.Width / 2, 1)));
                }
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
                width = random.Next(8, 15);
                height = random.Next(8, 15);
                pos = random.NextVector2(new Vector2(60, 30));

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
            }) != null;
        }

        public Room GetRoom(Vector2 position)
        {
            return rooms.Find(room =>
            {
                return room.Position.x <= position.x && room.Position.y <= position.y && position.x <= room.Position.x + room.Width && position.y <= room.Position.y + room.Height;
            });
        }

        public void Update()
        {
            foreach (Entity entity in entities)
            {
                if (entity.CurrentRoom != PlayerEntity.Instance.CurrentRoom)
                    continue;
                Vector2 previousPos = entity.RelativePosition;
                Room previousRoom = entity.CurrentRoom;
                entity.Move();

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
                }

                if (entity.RelativePosition != previousPos || entity.CurrentRoom != previousRoom)
                {
                    PrintEntity(entity);
                    if (entity.CurrentRoom == previousRoom)
                        PrintTile(entity.CurrentRoom, previousPos);
                }
            }
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

            foreach (Entity entity in entities)
            {
                PrintEntity(entity);
            }
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
