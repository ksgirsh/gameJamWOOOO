using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public int cellSize;
    private Vector2 initPosition;

    private float movementX;
    private float movementY;

    Vector2 gridPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int initSpot = (cellSize / 2);
        initPosition = new Vector2(0.5f, 0.5f);
        Debug.Log(initSpot);
        transform.position = initPosition;
        gridPosition = initPosition;
    }

    // Update is called once per frame
    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Horizontal"))
        {
            gridPosition.x += movementX;
            transform.position = gridPosition;
        }

        if (Input.GetButtonDown("Vertical"))
        {
            gridPosition.y += movementY;
            transform.position = gridPosition;
        }
    }

    void FixedUpdate()
    {
        

    }
}
