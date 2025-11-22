using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.U2D;

public class spritePixelSnap : MonoBehaviour
{
    private PixelPerfectCamera ppc;
    void Start()
    {
        ppc = Camera.main.GetComponent<PixelPerfectCamera>();
    }

    void LateUpdate()
    {
        if (ppc == null) return;
        Vector3 ParentPos = transform.parent.position;
        transform.localPosition = ppc.RoundToPixel(ParentPos)-ParentPos;
    }
}
