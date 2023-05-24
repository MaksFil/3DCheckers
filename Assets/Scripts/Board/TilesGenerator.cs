using UnityEngine;

public class TilesGenerator : MonoBehaviour, ITilesGenerator
{
    public int BoardSize { get; private set; } = 8;
    public int CountBoard { get; private set; } = 3;
    public GameObject Tile;
    public Material WhiteMaterial;
    public Material BlackMaterial;

    private void Start()
    {
        CreateTileColumns();
        CreateTiles();
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("BoardSize"))
            BoardSize = PlayerPrefs.GetInt("BoardSize");
        
        if (PlayerPrefs.HasKey("CountBoard"))
            CountBoard = PlayerPrefs.GetInt("CountBoard");
    }

    private void CreateTileColumns()
    {
        for(var j = 0; j < CountBoard; j++)
        {
            for (var i = 0; i < BoardSize; ++i)
                CreateTileColumn(i, j);
        }
    }

    private void CreateTileColumn(int columnIndex, int boardIndex)
    {
        GameObject tileColumn = new GameObject("TileColumn" + (columnIndex + BoardSize*boardIndex));
        tileColumn.transform.parent = this.gameObject.transform;
        tileColumn.transform.position = tileColumn.transform.parent.position + Vector3.right * (columnIndex + boardIndex*30);
    }

    private void CreateTiles()
    {
        for(var boardIndex = 0; boardIndex < CountBoard; boardIndex++)
        {
            for (var columnIndex = 0; columnIndex < BoardSize; ++columnIndex)
            {
                for (var rowIndex = 0; rowIndex < BoardSize; ++rowIndex)
                    CreateTile(columnIndex, rowIndex, boardIndex);
            }
        }
    }

    private void CreateTile(int columnIndex, int rowIndex, int boardIndex)
    {
        var columnTransform = transform.GetChild(columnIndex + BoardSize*boardIndex);
        GameObject instantiatedTile = Instantiate(Tile,
            columnTransform.position + Vector3.forward * rowIndex, Tile.transform.rotation,
            columnTransform);
        instantiatedTile.name = "Tile" + rowIndex;
        instantiatedTile.GetComponent<Renderer>().material =
            (columnIndex + rowIndex) % 2 != 0 ? WhiteMaterial : BlackMaterial;
    }
}