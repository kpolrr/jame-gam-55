using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float SmoothingSpeed; //Smoothing camera has a really jittery feeling on low res. Could be fixed with some shader trickery or maybe something else, but idk how
    public bool SmoothingEnabled;
    public bool PixelPerfectPositions;
    public int PixelsPerUnit;
    public float ZOffset;
    public Vector2 TopLeftCorner;
    public Vector2 BottomRightCorner;
    private Vector2 currentPos;

    void Start()
    {
        Application.targetFrameRate=60;
        currentPos = transform.position;
    }

    void Update()
    {
        Vector2 _smoothedPosition = SmoothingEnabled ? Vector2.Lerp(currentPos,(Vector2)Target.position,SmoothingSpeed*Time.deltaTime) : Target.position;
        currentPos = new Vector2(Mathf.Clamp(_smoothedPosition.x,TopLeftCorner.x,BottomRightCorner.x),Mathf.Clamp(_smoothedPosition.y,BottomRightCorner.y,TopLeftCorner.y));
        if (PixelPerfectPositions) {
            transform.position = new Vector3(Mathf.Round(currentPos.x*PixelsPerUnit)/PixelsPerUnit,Mathf.Round(currentPos.y*PixelsPerUnit)/PixelsPerUnit,ZOffset);
        } else
        {
            transform.position = new Vector3(currentPos.x,currentPos.y,ZOffset);
        }
    }
}
