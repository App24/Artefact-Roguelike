using Artefact.MapSystem;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artefact.Utils
{
    internal static class AStarPathfinding
    {
        public static List<Vector2> Calculate(Vector2 currentPos, Vector2 targetPos)
        {
            Room currentRoom = Map.Instance.GetRoom(currentPos);
            Room targetRoom = Map.Instance.GetRoom(targetPos);

            Vector2 localCurrentPos = currentPos - currentRoom.Position;
            Vector2 localTargetPos = targetPos - targetRoom.Position;

            List<AStarTileData> activeTiles = new List<AStarTileData>();
            activeTiles.Add(new AStarTileData(currentRoom.GetTile(localCurrentPos), currentPos, null, 0, GetNodeDistance(currentPos, targetPos)));
            List<AStarTileData> visitedTiles = new List<AStarTileData>();

            while (activeTiles.Any())
            {
                AStarTileData checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.Position == targetPos)
                {
                    AStarTileData tile = checkTile;
                    List<Vector2> positions = new List<Vector2>();
                    while (tile.ParentTile != null)
                    {
                        positions.Insert(0, tile.Position);
                        tile = tile.ParentTile;
                    }
                    if (positions.Count <= 0)
                        positions.Insert(0, localCurrentPos);
                    return positions;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                List<AStarTileData> walkableTiles = GetWalkableTiles(checkTile, new AStarTileData(targetRoom.GetTile(localTargetPos), targetPos, null, 0, 0));

                foreach (var walkableTile in walkableTiles)
                {
                    if (visitedTiles.Any(x => x.Position == walkableTile.Position))
                    {
                        continue;
                    }

                    if (activeTiles.Any(x => x.Position == walkableTile.Position))
                    {
                        AStarTileData existingTile = activeTiles.First(x => x.Position == walkableTile.Position);
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return new List<Vector2>() { currentPos };
        }

        private static int GetNodeDistance(Vector2 currentPos, Vector2 targetPos)
        {
            return Math.Abs(targetPos.x - currentPos.x) + Math.Abs(targetPos.y - currentPos.y);
        }

        private static List<AStarTileData> GetWalkableTiles(AStarTileData currentTile, AStarTileData targetTile)
        {
            Room currentRoom = Map.Instance.GetRoom(currentTile.Position);
            Vector2 localPos = currentTile.Position - currentRoom.Position;
            List<AStarTileData> possibleTiles = new List<AStarTileData>()
            {
                new AStarTileData(currentRoom.GetTile(localPos + Vector2.Up), currentTile.Position + Vector2.Up, currentTile, currentTile.Cost + 1, 0),

                new AStarTileData(currentRoom.GetTile(localPos + Vector2.Down), currentTile.Position+Vector2.Down, currentTile, currentTile.Cost + 1, 0),

                new AStarTileData(currentRoom.GetTile(localPos + Vector2.Left), currentTile.Position + Vector2.Left, currentTile, currentTile.Cost + 1, 0),
                new AStarTileData(currentRoom.GetTile(localPos + Vector2.Right), currentTile.Position + Vector2.Right, currentTile, currentTile.Cost + 1, 0),
            };

            possibleTiles.ForEach(tile => tile.Distance = GetNodeDistance(currentTile.Position, targetTile.Position));

            return possibleTiles.Where(tile => tile.Tile != null && !tile.Tile.Collidable).ToList();
        }
    }



    internal class AStarTileData
    {
        public AStarTileData(Tile tile, Vector2 position, AStarTileData parentPos, int cost, int distance)
        {
            Tile = tile;
            Position = position;
            ParentTile = parentPos;
            Cost = cost;
            Distance = distance;
        }

        public AStarTileData()
        {

        }

        public Tile Tile { get; }
        public Vector2 Position { get; }
        public AStarTileData ParentTile { get; }
        public int Cost { get; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
    }
}
