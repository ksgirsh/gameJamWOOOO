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
    void Start()
    {
        

        // Calculate the journey length.
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
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
