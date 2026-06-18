using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public Grid mainGrid;
    private float cellSize;
    private Vector2 initPosition;

    private float movementX;
    private float movementY;
    private bool moving;

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
            StartCoroutine(ShiftToCell(gridPosition));
        }

        if (Input.GetButtonDown("Vertical") && !moving)
        {
            gridPosition.y += (movementY * cellSize);
            StartCoroutine(ShiftToCell(gridPosition));
        }
    }

    void FixedUpdate()
    {
        

    }

    IEnumerator ShiftToCell(Vector2 worldCellPos, float lerpPoint = 0.75f, float duration = 0.02f)
    {
        //worldCellPos just equals futureGridPosition
        Vector2 initPosition = transform.position;

        Vector2 startLerpPoint = Vector2.Lerp(initPosition, worldCellPos, lerpPoint);
        moving = true;
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(startLerpPoint, worldCellPos, i);
            yield return null;
        }
        transform.position = worldCellPos;
        moving = false;

    }
}
