using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* This exists solely to allow the camera to track the player
* without flipping it's position to a negative z value when the
* player turns around. It tracks the players position, but does
* not rotate with the player.
*
* There is probably a much simpler way to do this with Cinemachine,
* but I haven't found it yet and I want to move on - gabbi :)
*/
public class CameraAvatar2D : MonoBehaviour
{
    [SerializeField]
    private GameObject followObject;

    void Update()
    {
        transform.position = followObject.transform.position;
    }
}