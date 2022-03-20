using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Camera cam;
    private float playerBasePositionX, enemyBasePositionX;

    [SerializeField] [Range(1, 5)] private float mousePositionMovementReference = 2.5f;
    private float mousePositionMovementPoint;

    [SerializeField] [Range(0.5f, 1.5f)] private float cameraMovementSpeed = 1f;
    void Start()
    {
        cam = GetComponent<Camera>();
        mousePositionMovementPoint = 1f - mousePositionMovementReference * 0.1f;

        playerBasePositionX = GameObject.FindWithTag("friendBase").transform.position.x; // or find with name .Find("")
        enemyBasePositionX = GameObject.FindWithTag("enemyBase").transform.position.x; // or find with name .Find("")
    }

    private Vector3 worldPosition;
    private void Update()
    {
        float mousePosX = Input.mousePosition.x; // get current mouse position x coordinate
        float mouseScreenPos = mousePosX / Screen.width; // mouse position by screen (0 - 1, 0:left corner, 1:right corner)

        float cameraPositionX = // calculating camera left and right border to detect limitations of camera moving
            cam.transform.position.x > 0 
                ? cam.transform.position.x + cam.orthographicSize*2 -1.5f // -1.5f is a offset that allows it to get closer to the base. 
                : cam.transform.position.x - cam.orthographicSize*2 +1.5f ; // +1.5f is a offset that allows it to get closer to the base.
        
        if (mouseScreenPos < 1f-mousePositionMovementPoint && cameraPositionX >= playerBasePositionX)
        {
            float camMovementSpeed = (1f-mousePositionMovementPoint - mouseScreenPos)*100*cameraMovementSpeed;
            transform.Translate(Time.deltaTime*camMovementSpeed*-1, 0, 0);
        }
        else if (mouseScreenPos > mousePositionMovementPoint && cameraPositionX <= enemyBasePositionX)
        {
            float camMovementSpeed = (mouseScreenPos - mousePositionMovementPoint)*100*cameraMovementSpeed;
            transform.Translate(Time.deltaTime*camMovementSpeed, 0, 0);
        }
        
        
       // Debug.Log("ref: " + mousePositionMovementReference + " point: " + mousePositionMovementPoint + " screen pos: " + screenPos);

    }
}
