using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.U2D;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float SmoothingSpeed; //Smoothing camera has a really jittery feeling on low res. Could be fixed with some shader trickery or maybe something else, but idk how
    public bool SmoothingEnabled;
    public bool PixelPerfectPositions;
    public float ZOffset;
    public Vector2 TopLeftCorner;
    public Vector2 BottomRightCorner;
    private Vector2 currentPos;
    private PixelPerfectCamera ppc;

    void Start()
    {
        ppc = GetComponent<PixelPerfectCamera>();
        Application.targetFrameRate=60;
        currentPos = transform.position;
    }

    void LateUpdate()
    {
        Vector2 targetPos = Target.position;
        if (PixelPerfectPositions && ppc!= null)
        {
            targetPos = ppc.RoundToPixel(targetPos);
        }
        Vector2 _smoothedPosition = SmoothingEnabled ? Vector2.Lerp(currentPos,(Vector2)targetPos,SmoothingSpeed*Time.deltaTime) : targetPos;
        currentPos = new Vector2(Mathf.Clamp(_smoothedPosition.x,TopLeftCorner.x,BottomRightCorner.x),Mathf.Clamp(_smoothedPosition.y,BottomRightCorner.y,TopLeftCorner.y));
        if (PixelPerfectPositions && ppc!=null) {
            transform.position = ppc.RoundToPixel(new Vector3(currentPos.x,currentPos.y,ZOffset));
        } else
        {
            transform.position = new Vector3(currentPos.x,currentPos.y,ZOffset);
        }
    }
}
