using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public Grid mainGrid;
    [SerializeField] float lerpPercentage;
    [SerializeField] float lerpDuration;
    private float cellSize;
    private Vector2 initPosition;

    private float movementX;
    private float movementY;
    private bool moving;
    private Coroutine movementCoroutine;

    Vector2 gridPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cellSize = mainGrid.cellSize.x;
        float initSpot = (mainGrid.cellSize.x / 2);
        initPosition = new Vector2(initSpot, initSpot);
        Debug.Log(initSpot);
        transform.position = initPosition;
        gridPosition = initPosition;
    }

    // Update is called once per frame
    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Horizontal") && !moving)
        {
            gridPosition.x += (movementX * cellSize);
            movementCoroutine = StartCoroutine(ShiftToCell(gridPosition, lerpPercentage, lerpDuration));
        }

        if (Input.GetButtonDown("Vertical") && !moving)
        {
            gridPosition.y += (movementY * cellSize);
            movementCoroutine = StartCoroutine(ShiftToCell(gridPosition, lerpPercentage, lerpDuration));
        }
    }

    void FixedUpdate()
    {
        

    }

    IEnumerator ShiftToCell(Vector2 worldCellPos, float lerpPoint = 0.75f, float duration = 0.02f)
    {




        moving = true;
        //worldCellPos just equals futureGridPosition
        Vector2 initPosition = transform.position;
        //magic number, fix later
        Vector2 anticpLerpPoint = Vector2.Lerp(initPosition, worldCellPos, 0.075f);
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(initPosition, anticpLerpPoint, i);
            yield return null;
        }



        Vector2 startLerpPoint = Vector2.Lerp(initPosition, worldCellPos, lerpPoint);
        
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(startLerpPoint, worldCellPos, i);
            yield return null;
        }
        transform.position = worldCellPos;
        moving = false;

    }
}
