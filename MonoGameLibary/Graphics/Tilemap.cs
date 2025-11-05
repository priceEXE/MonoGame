using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Tilemap
{
    private readonly Tileset _tileset;
    private readonly int[] _tiles;

    /// <summary>
    /// 瓦片地图行数
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// 瓦片地图列数
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// 瓦片地图总瓦片数
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 瓦片大小缩放比例
    /// </summary>
    public Vector2 Scale { get; set; }

    /// <summary>
    /// 瓦片像素宽度
    /// </summary>
    public float TileWidth => _tileset.TileWidth * Scale.X;

    /// <summary>
    /// 瓦片像素高度
    /// </summary>
    public float TileHeight => _tileset.TileHeight * Scale.Y;

    /// <summary>
    /// 创建一个瓦片地图实例
    /// </summary>
    /// <param name="tileset">使用的瓦片集</param>
    /// <param name="columns">瓦片地图总列数</param>
    /// <param name="rows">瓦片地图总行数</param>
    public Tilemap(Tileset tileset, int columns, int rows)
    {
        _tileset = tileset;
        Rows = rows;
        Columns = columns;
        Count = Columns * Rows;
        Scale = Vector2.One;
        _tiles = new int[Count];
    }

    /// <summary>
    /// 将Tilemap中指定索引的瓦片设置为使用来自Tileset的指定瓦片ID的瓦片
    /// </summary>
    /// <param name="index">瓦片索引</param>
    /// <param name="tilesetID">瓦片ID</param>
    public void SetTile(int index, int tilesetID)
    {
        _tiles[index] = tilesetID;
    }

    /// <summary>
    /// 将Tilemap中指定列和行的瓦片设置为使用来自Tileset的指定瓦片ID的瓦片
    /// </summary>
    /// <param name="column">瓦片所在的列</param>
    /// <param name="row">瓦片所在的行</param>
    /// <param name="tilesetID">瓦片id</param>
    public void SetTile(int column, int row, int tilesetID)
    {
        int index = row * Columns + column;
        SetTile(index, tilesetID);
    }

    /// <summary>
    /// 获取此Tilemap中指定索引的瓦片的TextureRegion
    /// </summary>
    /// <param name="index">瓦片索引</param>
    /// <returns>该索引瓦片的TileRegion</returns>
    public TextureRegion GetTile(int index)
    {
        return _tileset.GetTile(_tiles[index]);
    }

    /// <summary>
    /// 获取此Tilemap中指定列和行的瓦片的TextureRegion
    /// column and row.
    /// </summary>
    /// <param name="column">瓦片所在的列</param>
    /// <param name="row">瓦片所在的行</param>
    /// <returns>特定行列的瓦片的TextureRegion</returns>
    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }

    /// <summary>
    /// 绘制此瓦片地图
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw this tilemap.</param>
    /// <param name="position">The position to draw this tilemap at.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        for (int i = 0; i < Count; i++)
        {
            int tileSetIndex = _tiles[i];
            TextureRegion tile = _tileset.GetTile(tileSetIndex);

            int x = i % Columns;
            int y = i / Columns;

            Vector2 tilePosition = new Vector2(x * TileWidth, y * TileHeight);
            tile.Draw(spriteBatch, position + tilePosition, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 1.0f);
        }
    }

    /// <summary>
    /// 从XML文件创建瓦片地图
    /// </summary>
    /// <param name="content">The content manager used to load the texture for the tileset.</param>
    /// <param name="filename">The path to the xml file, relative to the content root directory.</param>
    /// <returns>The tilemap created by this method.</returns>
    public static Tilemap FromFile(ContentManager content, string filename)
    {
        string filePath = Path.Combine(content.RootDirectory, filename);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                XDocument doc = XDocument.Load(reader);
                XElement root = doc.Root;

                // The <Tileset> element contains the information about the tileset
                // used by the tilemap.
                //
                // Example
                // <Tileset region="0 0 100 100" tileWidth="10" tileHeight="10">contentPath</Tileset>
                //
                // The region attribute represents the x, y, width, and height
                // components of the boundary for the texture region within the
                // texture at the contentPath specified.
                //
                // the tileWidth and tileHeight attributes specify the width and
                // height of each tile in the tileset.
                //
                // the contentPath value is the contentPath to the texture to
                // load that contains the tileset
                XElement tilesetElement = root.Element("Tileset");

                string regionAttribute = tilesetElement.Attribute("region").Value;
                string[] split = regionAttribute.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                int width = int.Parse(split[2]);
                int height = int.Parse(split[3]);

                int tileWidth = int.Parse(tilesetElement.Attribute("tileWidth").Value);
                int tileHeight = int.Parse(tilesetElement.Attribute("tileHeight").Value);
                string contentPath = tilesetElement.Value;

                // Load the texture 2d at the content path
                Texture2D texture = content.Load<Texture2D>(contentPath);

                // Create the texture region from the texture
                TextureRegion textureRegion = new TextureRegion(texture, x, y, width, height);

                // Create the tileset using the texture region
                Tileset tileset = new Tileset(textureRegion, tileWidth, tileHeight);

                // The <Tiles> element contains lines of strings where each line
                // represents a row in the tilemap.  Each line is a space
                // separated string where each element represents a column in that
                // row.  The value of the column is the id of the tile in the
                // tileset to draw for that location.
                //
                // Example:
                // <Tiles>
                //      00 01 01 02
                //      03 04 04 05
                //      03 04 04 05
                //      06 07 07 08
                // </Tiles>
                XElement tilesElement = root.Element("Tiles");

                // Split the value of the tiles data into rows by splitting on
                // the new line character
                string[] rows = tilesElement.Value.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);

                // Split the value of the first row to determine the total number of columns
                int columnCount = rows[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;

                // Create the tilemap
                Tilemap tilemap = new Tilemap(tileset, columnCount, rows.Length);

                // Process each row
                for (int row = 0; row < rows.Length; row++)
                {
                    // Split the row into individual columns
                    string[] columns = rows[row].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    // Process each column of the current row
                    for (int column = 0; column < columnCount; column++)
                    {
                        // Get the tileset index for this location
                        int tilesetIndex = int.Parse(columns[column]);

                        // Get the texture region of that tile from the tileset
                        TextureRegion region = tileset.GetTile(tilesetIndex);

                        // Add that region to the tilemap at the row and column location
                        tilemap.SetTile(column, row, tilesetIndex);
                    }
                }

                return tilemap;
            }
        }
    }
}
