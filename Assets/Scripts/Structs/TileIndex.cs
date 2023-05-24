public class TileIndex
{
    public int Column { get; }
    public int Row { get; }
    public int IndexBoard { get; }

    public TileIndex(int column, int row, int indexBoard)
    {
        Column = column;
        Row = row;
        IndexBoard = indexBoard;
    }

    public static TileIndex operator +(TileIndex firstTileIndex, TileIndex secondTileIndex)
    {
        return new TileIndex(firstTileIndex.Column + secondTileIndex.Column,
            firstTileIndex.Row + secondTileIndex.Row, firstTileIndex.IndexBoard + secondTileIndex.IndexBoard);
    }

    public static TileIndex operator -(TileIndex firstTileIndex, TileIndex secondTileIndex)
    {
        return new TileIndex(firstTileIndex.Column - secondTileIndex.Column,
            firstTileIndex.Row - secondTileIndex.Row, firstTileIndex.IndexBoard - secondTileIndex.IndexBoard);
    }

    public static bool operator !=(TileIndex firstTileIndex, TileIndex secondTileIndex)
    {
        return !(firstTileIndex == secondTileIndex);
    }

    public static bool operator ==(TileIndex firstTileIndex, TileIndex secondTileIndex)
    {
        if (ReferenceEquals(null, firstTileIndex)) return false;
        return firstTileIndex.Equals(secondTileIndex);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TileIndex)obj);
    }

    protected bool Equals(TileIndex other)
    {
        return Column == other.Column && Row == other.Row && IndexBoard == other.IndexBoard;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Column * 397) ^ Row;
        }
    }
}