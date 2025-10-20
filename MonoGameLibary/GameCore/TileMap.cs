using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 瓦片地图类，用于管理无限地图
    /// </summary>
    public class TileMap
    {
        private Dictionary<Point, Chunk> _chunks;
        private int _chunkSize;
        private int _tileSize;
        private Texture2D _tileTexture;

        public TileMap(int chunkSize, int tileSize)
        {
            _chunks = new Dictionary<Point, Chunk>();
            _chunkSize = chunkSize;
            _tileSize = tileSize;
        }

        /// <summary>
        /// 设置瓦片纹理
        /// </summary>
        public void SetTileTexture(Texture2D texture)
        {
            _tileTexture = texture;
        }

        /// <summary>
        /// 获取指定位置的瓦片
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            Point chunkPos = GetChunkPosition(x, y);
            if (_chunks.ContainsKey(chunkPos))
            {
                return _chunks[chunkPos].GetTile(x % _chunkSize, y % _chunkSize);
            }
            return null;
        }

        /// <summary>
        /// 设置指定位置的瓦片
        /// </summary>
        public void SetTile(int x, int y, Tile tile)
        {
            Point chunkPos = GetChunkPosition(x, y);
            if (!_chunks.ContainsKey(chunkPos))
            {
                _chunks[chunkPos] = new Chunk(_chunkSize);
            }
            _chunks[chunkPos].SetTile(x % _chunkSize, y % _chunkSize, tile);
        }

        /// <summary>
        /// 获取区块位置
        /// </summary>
        private Point GetChunkPosition(int x, int y)
        {
            return new Point(x / _chunkSize, y / _chunkSize);
        }

        /// <summary>
        /// 绘制地图
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Rectangle viewport)
        {
            // 计算需要绘制的区块范围
            int startChunkX = viewport.Left / (_chunkSize * _tileSize) - 1;
            int endChunkX = viewport.Right / (_chunkSize * _tileSize) + 1;
            int startChunkY = viewport.Top / (_chunkSize * _tileSize) - 1;
            int endChunkY = viewport.Bottom / (_chunkSize * _tileSize) + 1;

            for (int chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
            {
                for (int chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
                {
                    Point chunkPos = new Point(chunkX, chunkY);
                    if (_chunks.ContainsKey(chunkPos))
                    {
                        DrawChunk(spriteBatch, _chunks[chunkPos], chunkX, chunkY);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制区块
        /// </summary>
        private void DrawChunk(SpriteBatch spriteBatch, Chunk chunk, int chunkX, int chunkY)
        {
            int startX = chunkX * _chunkSize * _tileSize;
            int startY = chunkY * _chunkSize * _tileSize;

            for (int x = 0; x < _chunkSize; x++)
            {
                for (int y = 0; y < _chunkSize; y++)
                {
                    Tile tile = chunk.GetTile(x, y);
                    if (tile != null && tile.Texture != null)
                    {
                        Vector2 position = new Vector2(startX + x * _tileSize, startY + y * _tileSize);
                        spriteBatch.Draw(tile.Texture, position, tile.SourceRectangle, Color.White);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 区块类
    /// </summary>
    public class Chunk
    {
        private Tile[,] _tiles;
        private int _size;

        public Chunk(int size)
        {
            _size = size;
            _tiles = new Tile[size, size];
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < _size && y >= 0 && y < _size)
            {
                return _tiles[x, y];
            }
            return null;
        }

        public void SetTile(int x, int y, Tile tile)
        {
            if (x >= 0 && x < _size && y >= 0 && y < _size)
            {
                _tiles[x, y] = tile;
            }
        }
    }

    /// <summary>
    /// 瓦片类
    /// </summary>
    public class Tile
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public bool IsWalkable { get; set; } = true;
        public bool IsSolid { get; set; } = false;

        public Tile(Texture2D texture, Rectangle sourceRectangle)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
        }
    }
}
