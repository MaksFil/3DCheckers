using UnityEngine;

public class BorderGenerator : MonoBehaviour
{
    public GameObject Border;
    public GameObject Corner;

    private int boardSize;
    private int countBoard;

    private GameObject borderGameObject;
    private Vector3 currentPosition;
    private Quaternion currentRotation;
    private Vector3 currentDirection;

    private void Awake()
    {
        ITilesGenerator tilesGenerator = GetComponent<ITilesGenerator>();
        boardSize = tilesGenerator.BoardSize;
        countBoard = tilesGenerator.CountBoard;
    }

    private void Start()
    {
        for(int i = 0; i < countBoard; i++)
        { 
            CreateBorderGameObject(30*i);
            AssignInitialValues();
            CreateBorder(); 
        }
    }

    private void CreateBorderGameObject(float x)
    {
        borderGameObject = new GameObject("Border");
        borderGameObject.transform.parent = this.gameObject.transform;
        borderGameObject.transform.position = (Vector3.left + Vector3.back);
        borderGameObject.transform.position = new Vector3(borderGameObject.transform.position.x + transform.position.x + x, this.gameObject.transform.position.y, borderGameObject.transform.position.z);
    }

    private void AssignInitialValues()
    {
        currentPosition = borderGameObject.transform.position;
        currentRotation = borderGameObject.transform.rotation;
        currentDirection = Vector3.forward;
    }

    private void CreateBorder()
    {
        for (var side = 0; side < 4; ++side)
            CreaterBorderLine();
    }

    private void CreaterBorderLine()
    {
        CreateCornerElement();
        for (var i = 0; i < boardSize; ++i)
            CreateBorderElement();
        RotateBy90Degrees();
    }

    private void CreateCornerElement()
    {
        CreateElement(Corner);
    }

    private void CreateElement(GameObject objectToCreate)
    {
        GameObject instantiatedCorner = Instantiate(objectToCreate, currentPosition,
            objectToCreate.transform.rotation * currentRotation, borderGameObject.transform);
        IncrementCurrentPosition();
    }

    private void IncrementCurrentPosition()
    {
        currentPosition += currentDirection;
    }

    private void CreateBorderElement()
    {
        CreateElement(Border);
    }

    private void RotateBy90Degrees()
    {
        Quaternion rotationBy90Degrees = Quaternion.Euler(0, 90, 0);
        currentDirection = rotationBy90Degrees * currentDirection;
        currentRotation *= rotationBy90Degrees;
    }
}