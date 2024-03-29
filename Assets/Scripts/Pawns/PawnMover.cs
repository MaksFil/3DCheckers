﻿using System.Collections;
using UnityEngine;

public class PawnMover : MonoBehaviour
{
    public float HorizontalMovementSmoothing;
    public float VerticalMovementSmoothing;
    public float PositionDifferenceTolerance;

    private GameObject lastClickedTile;
    private GameObject lastClickedPawn;
    private PawnMoveValidator pawnMoveValidator;
    private MoveChecker moveChecker;
    private PromotionChecker promotionChecker;
    private TurnHandler turnHandler;
    private bool isPawnMoving;
    private bool isMoveMulticapturing;

    private void Awake()
    {
        pawnMoveValidator = GetComponent<PawnMoveValidator>();
        moveChecker = GetComponent<MoveChecker>();
        promotionChecker = GetComponent<PromotionChecker>();
        turnHandler = GetComponent<TurnHandler>();
    }

    public void PawnClicked(GameObject pawn)
    {
        if (!CanPawnBeSelected(pawn))
            return;
        if (pawn != lastClickedPawn)
            SelectPawn(pawn);
        else
            UnselectPawn();
    }

    private bool CanPawnBeSelected(GameObject pawn)
    {
        PawnColor turn = turnHandler.GetTurn();
        if (isPawnMoving || turn != GetPawnColor(pawn) || isMoveMulticapturing ||
            !moveChecker.PawnHasAnyMove(pawn)) return false;
        if (moveChecker.PawnsHaveCapturingMove(turn) && !moveChecker.PawnHasCapturingMove(pawn)) return false;
        return true;
    }

    private void SelectPawn(GameObject pawn)
    {
        if (lastClickedPawn != null)
            UnselectPawn();
        lastClickedPawn = pawn;
        AddPawnSelection();
    }

    private void AddPawnSelection()
    {
        lastClickedPawn.GetComponent<IPawnProperties>().AddPawnSelection();
    }

    private void UnselectPawn()
    {
        RemoveLastClickedPawnSelection();
        lastClickedPawn = null;
    }

    private void RemoveLastClickedPawnSelection()
    {
        lastClickedPawn.GetComponent<IPawnProperties>().RemovePawnSelection();
    }

    private PawnColor GetPawnColor(GameObject pawn)
    {
        return pawn.GetComponent<IPawnProperties>().PawnColor;
    }

    public void TileClicked(GameObject tile)
    {
        if (!CanTileBeClicked()) return;
        lastClickedTile = tile;
        // print(IsMoveNoncapturingAndValid());
        if (IsMoveNoncapturingAndValid())
            MovePawn();
        else if (IsMoveCapturingAndValid())
            CapturePawn();
    }

    private bool CanTileBeClicked()
    {
        return !isPawnMoving && lastClickedPawn != null;
    }

    private bool IsMoveNoncapturingAndValid()
    {
        if (moveChecker.PawnHasCapturingMove(lastClickedPawn))
            return false;
        return pawnMoveValidator.IsValidMove(lastClickedPawn, lastClickedTile);
    }

    private void MovePawn()
    {
        ChangeMovedPawnParent();
        StartCoroutine(AnimatePawnMove());
        RemoveLastClickedPawnSelection();
    }

    private void ChangeMovedPawnParent()
    {
        lastClickedPawn.transform.SetParent(lastClickedTile.transform);
    }

    private IEnumerator AnimatePawnMove()
    {
        isPawnMoving = true;
        var targetPosition = lastClickedPawn.transform.parent.position;
        yield return MoveHorizontal(targetPosition);
        promotionChecker.CheckPromotion(lastClickedPawn);
        isPawnMoving = false;
        EndTurn();
    }

    private void EndTurn()
    {
        lastClickedPawn = null;
        isMoveMulticapturing = false;
        turnHandler.NextTurn();
    }

    private IEnumerator MoveHorizontal(Vector3 targetPosition)
    {
        var pawnTransform = lastClickedPawn.transform;
        while (Vector3.Distance(pawnTransform.position, targetPosition) > PositionDifferenceTolerance)
        {
            pawnTransform.position = Vector3.Lerp(pawnTransform.position, targetPosition,
                HorizontalMovementSmoothing * Time.deltaTime);
            yield return null;
        }
    }

    private bool IsMoveCapturingAndValid()
    {
        return pawnMoveValidator.IsCapturingMove(lastClickedPawn, lastClickedTile);
    }

    private void CapturePawn()
    {
        ChangeMovedPawnParent();
        StartCoroutine(AnimatePawnCapture());
        RemoveLastClickedPawnSelection();
    }

    private IEnumerator AnimatePawnCapture()
    {
        isPawnMoving = true;
        yield return DoCaptureMovement();
        RemoveCapturedPawn();
        yield return null;
        promotionChecker.CheckPromotion(lastClickedPawn);
        isPawnMoving = false;
        MulticaptureOrEndTurn();
    }

    private IEnumerator DoCaptureMovement()
    {
        var targetPosition = lastClickedPawn.transform.position + Vector3.up;
        yield return MoveVertical(targetPosition);
        targetPosition = lastClickedPawn.transform.parent.position + Vector3.up;
        yield return MoveHorizontal(targetPosition);
        targetPosition = lastClickedPawn.transform.position - Vector3.up;
        yield return MoveVertical(targetPosition);
    }

    private IEnumerator MoveVertical(Vector3 targetPosition)
    {
        var pawnTransform = lastClickedPawn.transform;
        while (Vector3.Distance(pawnTransform.position, targetPosition) > PositionDifferenceTolerance)
        {
            pawnTransform.position = Vector3.Lerp(pawnTransform.position, targetPosition,
                VerticalMovementSmoothing * Time.deltaTime);
            yield return null;
        }
    }

    private void RemoveCapturedPawn()
    {
        GameObject pawnToCapture = pawnMoveValidator.GetPawnToCapture();
        turnHandler.DecrementPawnCount(pawnToCapture);
        Destroy(pawnToCapture);
    }

    private void MulticaptureOrEndTurn()
    {
        if (moveChecker.PawnHasCapturingMove(lastClickedPawn))
            Multicapture();
        else
            EndTurn();
    }

    private void Multicapture()
    {
        isMoveMulticapturing = true;
        AddPawnSelection();
    }

}