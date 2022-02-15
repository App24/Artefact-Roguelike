using Artefact.Tiles;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artefact.Utils
{
    static class AStarPathfinding
    {
        public static List<Vector2i> Calculate(Vector2i currentPos, Vector2i targetPos)
        {
            List<AStarTileData> activeTiles = new List<AStarTileData>();
            activeTiles.Add(new AStarTileData(World.Instance.GetTile(currentPos), currentPos, null, 0, GetNodeDistance(currentPos, targetPos)));
            List<AStarTileData> visitedTiles = new List<AStarTileData>();

            while (activeTiles.Any())
            {

                AStarTileData checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

                if (checkTile.Position == targetPos)
                {
                    AStarTileData tile = checkTile;
                    List<Vector2i> positions = new List<Vector2i>();
                    while (tile.ParentTile != null)
                    {
                        positions.Insert(0, tile.Position);
                        tile = tile.ParentTile;
                    }
                    if (positions.Count <= 0)
                        positions.Insert(0, currentPos);
                    return positions;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                List<AStarTileData> walkableTiles = GetWalkableTiles(checkTile, new AStarTileData(World.Instance.GetTile(targetPos), targetPos, null, 0, 0));

                foreach (var walkableTile in walkableTiles)
                {
                    if (visitedTiles.Any(x => x.Position == walkableTile.Position))
                    {
                        continue;
                    }

                    if (activeTiles.Any(x => x.Position == walkableTile.Position))
                    {
                        AStarTileData existingTile = activeTiles.First(x => x.Position == walkableTile.Position);
                        if(existingTile.CostDistance > checkTile.CostDistance)
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

            return new List<Vector2i>() { currentPos };
        }

        static int GetNodeDistance(Vector2i currentPos, Vector2i targetPos)
        {
            return Math.Abs(targetPos.X - currentPos.X) + Math.Abs(targetPos.Y - currentPos.Y);
        }

        static List<AStarTileData> GetWalkableTiles(AStarTileData currentTile, AStarTileData targetTile)
        {
            List<AStarTileData> possibleTiles = new List<AStarTileData>()
            {
                new AStarTileData(World.Instance.GetTile(currentTile.Position+Vector2i.Up), currentTile.Position+Vector2i.Up, currentTile, currentTile.Cost+1, 0),
                new AStarTileData(World.Instance.GetTile(currentTile.Position+Vector2i.Down), currentTile.Position+Vector2i.Down, currentTile, currentTile.Cost+1, 0),
                new AStarTileData(World.Instance.GetTile(currentTile.Position+Vector2i.Left), currentTile.Position+Vector2i.Left, currentTile, currentTile.Cost+1, 0),
                new AStarTileData(World.Instance.GetTile(currentTile.Position+Vector2i.Right), currentTile.Position+Vector2i.Right, currentTile, currentTile.Cost+1, 0)
            };

            possibleTiles.ForEach(tile => tile.Distance = GetNodeDistance(currentTile.Position, targetTile.Position));

            return possibleTiles.Where(tile => tile.Tile !=null && !tile.Tile.Collidable).ToList();
        }
    }

    class AStarTileData
    {
        public AStarTileData(Tile tile, Vector2i position, AStarTileData parentPos, int cost, int distance)
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
        public Vector2i Position { get; }
        public AStarTileData ParentTile { get; }
        public int Cost { get; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
    }
}
