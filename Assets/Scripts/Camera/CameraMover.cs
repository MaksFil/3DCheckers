using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float RotationSmoothing;
    public float ScrollSmoothing;
    public float InitialRotation;
    public GameObject Board;

    private int boardSize;
    private float minOffset;
    private float maxOffset;
    private float localOffeset;
    private Vector3 localRotation;

    private void Start()
    {
        var tilesGenerator = Board.GetComponent<ITilesGenerator>();
        boardSize = tilesGenerator.BoardSize;
        SetInitialPosition();
        SetInitialRotation();
    }

    private void SetInitialPosition()
    {
        var initialOffset = boardSize * Mathf.Sqrt(2);
        localOffeset = initialOffset;
        minOffset = initialOffset / 10;
        maxOffset = initialOffset;
        transform.position = initialOffset * Vector3.back;
    }

    private void SetInitialRotation()
    {
        localRotation.y = InitialRotation;
        var initialRotation = Quaternion.Euler(localRotation.y, 0, 0);
        transform.parent.rotation = initialRotation;
    }

    private void LateUpdate()
    {
        ChangePosition();
        ChangeRotation();
    }

    private void ChangePosition()
    {
        var lerpOffset = Mathf.Lerp(transform.localPosition.z, -localOffeset, ScrollSmoothing * Time.deltaTime);
        transform.localPosition = lerpOffset * Vector3.forward;
    }

    private void ChangeRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        transform.parent.rotation =
            Quaternion.Lerp(transform.parent.rotation, targetRotation, RotationSmoothing * Time.deltaTime);
    }
}