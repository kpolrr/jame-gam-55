using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float SmoothingSpeed; //Smoothing camera has a really jittery feeling on low res. Could be fixed with some shader trickery or maybe something else, but idk how
    public bool SmoothingEnabled;
    public float ZOffset;
    public Vector2 TopLeftCorner;
    public Vector2 BottomRightCorner;

    void Update()
    {
        Vector2 _smoothedPosition = SmoothingEnabled ? Vector2.Lerp((Vector2)transform.position,(Vector2)Target.position,SmoothingSpeed*Time.deltaTime) : Target.position;
        _smoothedPosition = new Vector2(Mathf.Clamp(_smoothedPosition.x,TopLeftCorner.x,BottomRightCorner.x),Mathf.Clamp(_smoothedPosition.y,BottomRightCorner.y,TopLeftCorner.y));
        transform.position = new Vector3(_smoothedPosition.x,_smoothedPosition.y,ZOffset);
    }
}
