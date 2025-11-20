using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class jumpPad : MonoBehaviour
{
    public float PushForce;
    public string PushableTagName;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == PushableTagName) {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Vector2 pushDirection = PushForce*transform.up;
            if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(pushDirection.y) || Mathf.Sign(rb.velocity.y) != Mathf.Sign(pushDirection.y) && Mathf.Abs(pushDirection.y) > 0.01) rb.velocity = new Vector2(rb.velocity.x,pushDirection.y);
            if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(pushDirection.x) || Mathf.Sign(rb.velocity.x) != Mathf.Sign(pushDirection.x) && Mathf.Abs(pushDirection.x) > 0.01) rb.velocity = new Vector2(pushDirection.x,rb.velocity.y);
        }
    }
}
