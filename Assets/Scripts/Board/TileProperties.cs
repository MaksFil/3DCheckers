using UnityEngine;

public class TileProperties : MonoBehaviour
{
    public bool IsOccupied()
    {
        return GetComponentInChildren<IPawnProperties>() != null;
    }

    public GameObject GetPawn()
    {
        return GetComponentInChildren<IPawnProperties>().gameObject;
    }

    public TileIndex GetTileIndex()
    {
        return new TileIndex(transform.parent.GetSiblingIndex() % 8, transform.GetSiblingIndex(), transform.parent.GetSiblingIndex() / 8);
    }
}