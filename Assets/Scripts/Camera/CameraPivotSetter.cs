﻿using UnityEngine;

public class CameraPivotSetter : MonoBehaviour
{
    public GameObject Board;

    [SerializeField] private float _xOffset;

    private int boardSize;

    private void Start()
    {
        var tilesGenerator = Board.GetComponent<ITilesGenerator>();
        boardSize = tilesGenerator.BoardSize;
        SetPivotInCenter();
    }

    private void SetPivotInCenter()
    {
        var centerValue = boardSize / 2.0f - 1.0f / 2.0f;
        transform.position = new Vector3(centerValue + _xOffset, Board.transform.position.y, centerValue);
    }
}