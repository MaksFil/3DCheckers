using UnityEngine;

public class PawnMoveValidator : MonoBehaviour
{
    private TileGetter tileGetter;
    private GameObject pawn;
    private GameObject targetTile;
    private GameObject pawnToCapture;
    private TileIndex targetTileIndex;
    private TileIndex currentTileIndex;
    private TileIndex positionDifferenceInIndex;

    private void Awake()
    {
        tileGetter = GetComponent<TileGetter>();
    }

    public bool IsValidMove(GameObject pawnToCheck, GameObject targetTileToCheck)
    {
        SetValues(pawnToCheck, targetTileToCheck);
        if ((!IsMoveDiagonal() || IsTileOccupied(targetTileIndex)) && !IsMoveVertical())
            return false;
        if (!IsPawnKing() && IsMoveDiagonal())
            return positionDifferenceInIndex.Row == GetPawnRowMoveDirection();
        else 
            return IsPathCollidingWithOtherPawns();
    }

    private void SetValues(GameObject pawnToCheck, GameObject targetTileToCheck)
    {
        pawn = pawnToCheck;
        targetTile = targetTileToCheck;
        targetTileIndex = targetTile.GetComponent<TileProperties>().GetTileIndex();
        currentTileIndex = pawn.GetComponent<IPawnProperties>().GetTileIndex();
        positionDifferenceInIndex = targetTileIndex - currentTileIndex;
    }

    private bool IsMoveDiagonal()
    {
        return Mathf.Abs(positionDifferenceInIndex.Column) == Mathf.Abs(positionDifferenceInIndex.Row) && positionDifferenceInIndex.IndexBoard == 0;
    }

    private bool IsMoveVertical() 
    {
        return positionDifferenceInIndex.Column == 0 && positionDifferenceInIndex.Column == positionDifferenceInIndex.Row && Mathf.Abs(positionDifferenceInIndex.IndexBoard) == 1;
    }

    private bool IsPawnKing()
    {
        return pawn.GetComponent<IPawnProperties>().IsKing;
    }

    private int GetPawnRowMoveDirection()
    {
        var pawnProperties = pawn.GetComponent<IPawnProperties>();
        return pawnProperties.PawnColor == PawnColor.White ? 1 : -1;
    }

    private bool IsPathCollidingWithOtherPawns()
    {
        var moveDirectionInIndex = GetMoveDirectionInIndex();
        for (var checkedTileIndex = currentTileIndex + moveDirectionInIndex;
            checkedTileIndex != targetTileIndex;
            checkedTileIndex += moveDirectionInIndex)
            if (IsTileOccupied(checkedTileIndex))
                return false;
        
        return true;
    }

    private TileIndex GetMoveDirectionInIndex()
    {
        if(positionDifferenceInIndex.Column != 0 && positionDifferenceInIndex.Row != 0)
            return new TileIndex(positionDifferenceInIndex.Column / Mathf.Abs(positionDifferenceInIndex.Column),
                positionDifferenceInIndex.Row / Mathf.Abs(positionDifferenceInIndex.Row), positionDifferenceInIndex.IndexBoard);
        else
            return new TileIndex(positionDifferenceInIndex.Column, positionDifferenceInIndex.Row, positionDifferenceInIndex.IndexBoard);
    }

    private bool IsTileOccupied(TileIndex tileIndex)
    {
        return tileGetter.GetTile(tileIndex).GetComponent<TileProperties>().IsOccupied();
    }

    public bool IsCapturingMove(GameObject pawnToCheck, GameObject targetTileToCheck)
    {
        SetValues(pawnToCheck, targetTileToCheck);
        if (!IsMoveDiagonal() || IsTileOccupied(targetTileIndex))
            return false;
        return IsCapturePositionChangeValid() && IsOpponentsPawnOnOneBeforeTargetTile();
    }

    private bool IsCapturePositionChangeValid()
    {
        return (!IsPawnKing() && Mathf.Abs(positionDifferenceInIndex.Row) == 2) ||
               (IsPawnKing() && Mathf.Abs(positionDifferenceInIndex.Row) >= 2);
    }

    private bool IsOpponentsPawnOnOneBeforeTargetTile()
    {
        if (!IsPawnOnOneBeforeTargetTile())
            return false;
        var potentialPawnToCapture = GetPotentialPawnToCapture();
        if (!IsPawnDifferentColorThanLastClickedPawn(potentialPawnToCapture))
            return false;
        pawnToCapture = potentialPawnToCapture;
        return true;
    }

    private bool IsPawnOnOneBeforeTargetTile()
    {
        var moveDirectionInIndex = GetMoveDirectionInIndex();
        for (var checkedTileIndex = currentTileIndex + moveDirectionInIndex;
            checkedTileIndex != targetTileIndex;
            checkedTileIndex += moveDirectionInIndex)
            if (IsTileOccupied(checkedTileIndex) && checkedTileIndex != targetTileIndex - moveDirectionInIndex)
                return false;
        return IsTileOccupied(targetTileIndex - moveDirectionInIndex);
    }

    private GameObject GetPotentialPawnToCapture()
    {
        var moveDirectionInIndex = GetMoveDirectionInIndex();
        return tileGetter.GetTile(targetTileIndex - moveDirectionInIndex).GetComponent<TileProperties>().GetPawn();
    }

    private bool IsPawnDifferentColorThanLastClickedPawn(GameObject pawnToCheck)
    {
        return pawnToCheck.GetComponent<IPawnProperties>().PawnColor !=
               pawn.GetComponent<IPawnProperties>().PawnColor;
    }

    public GameObject GetPawnToCapture()
    {
        return pawnToCapture;
    }
}