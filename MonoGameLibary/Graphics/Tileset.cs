namespace MonoGameLibrary.Graphics;

public class Tileset
{
    private readonly TextureRegion[] _tiles;

    /// <summary>
    /// 获取此瓦片集中每个瓦片的宽度（以像素为单位）
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// 获取此瓦片集中每个瓦片的高度（以像素为单位）
    /// </summary>
    public int TileHeight { get; }

    /// <summary>
    /// 获取此瓦片集中总列数
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// 获取此瓦片集中总行数
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// 获取此瓦片集中瓦片总数
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// 根据指定的纹理区域和瓦片尺寸创建一个新的瓦片集
    /// </summary>
    /// <param name="textureRegion">The texture region that contains the tiles for the tileset.</param>
    /// <param name="tileWidth">The width of each tile in the tileset.</param>
    /// <param name="tileHeight">The height of each tile in the tileset.</param>
    public Tileset(TextureRegion textureRegion, int tileWidth, int tileHeight)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Columns = textureRegion.Width / tileWidth;
        Rows = textureRegion.Height / tileHeight;
        Count = Columns * Rows;

        // Create the texture regions that make up each individual tile
        _tiles = new TextureRegion[Count];

        for (int i = 0; i < Count; i++)
        {
            int x = i % Columns * tileWidth;
            int y = i / Columns * tileHeight;
            _tiles[i] = new TextureRegion(textureRegion.Texture, textureRegion.SourceRectangle.X + x, textureRegion.SourceRectangle.Y + y, tileWidth, tileHeight);
        }
    }

    /// <summary>
    /// 根据指定索引获取此瓦片集中对应的纹理区域
    /// </summary>
    /// <param name="index">The index of the texture region in this tile set.</param>
    /// <returns>The texture region for the tile form this tileset at the given index.</returns>
    public TextureRegion GetTile(int index) => _tiles[index];

    /// <summary>
    /// 根据指定列和行获取此瓦片集中对应的纹理区域
    /// </summary>
    /// <param name="column">The column in this tileset of the texture region.</param>
    /// <param name="row">The row in this tileset of the texture region.</param>
    /// <returns>The texture region for the tile from this tileset at given location.</returns>
    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }
}
