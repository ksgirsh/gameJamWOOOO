using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraM : MonoBehaviour
{
    // Start is called before the first frame update
    private float mouseDivisionFactor = 7;
    //[SerializeField] float playerMovementFactor;
    
    [SerializeField] Camera refrenceCamera;
    [SerializeField] Transform mover;
    [SerializeField] float OffsetZ;
    [SerializeField] float OffsetY;

    [SerializeField] float OffsetX;
    [SerializeField] Transform target;

    public float maxSize = 19;
    [SerializeField] float minSize = 3;

    void Start()
    {
        

        // Calculate the journey length.
        
    }

    void Update()
    {
        if (refrenceCamera.orthographicSize < maxSize && (refrenceCamera.orthographicSize > minSize))
        {
            //can zoom in or out
            refrenceCamera.orthographicSize += Input.mouseScrollDelta.y;

        } else if (Input.mouseScrollDelta.y < 0 && (refrenceCamera.orthographicSize >= maxSize))
        {
            refrenceCamera.orthographicSize += Input.mouseScrollDelta.y;

        } else if (Input.mouseScrollDelta.y > 0 && (refrenceCamera.orthographicSize <= minSize))
        {
            refrenceCamera.orthographicSize += Input.mouseScrollDelta.y;
        }
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {

        
        /*
        if (refrenceCamera.orthographicSize < maxSize)
        {
            refrenceCamera.orthographicSize += Input.mouseScrollDelta.y;

        } else if (refrenceCamera.orthographicSize >= maxSize && (Input.mouseScrollDelta.y < 0))
        {
            refrenceCamera.orthographicSize += Input.mouseScrollDelta.y;
        }
        */


        Vector2 mousePos = refrenceCamera.ScreenToWorldPoint(Input.mousePosition);
        float cameraPosX = (mousePos.x / mouseDivisionFactor);
        float cameraPosY = (mousePos.y / mouseDivisionFactor);
        //float cameraPosZ = OffsetZ;\

        mover.position = new Vector3(target.position.x + (OffsetX * cameraPosX), target.position.y + (OffsetY * cameraPosY) , target.position.z + OffsetZ);
        //MoveNearMouse(cameraPosX, cameraPosY);
    }
    void MoveNearMouse(float x, float y)
    {
        //mover.position = new Vector3(x, y, OffsetZ) + player.position;
   
    }

    
}
