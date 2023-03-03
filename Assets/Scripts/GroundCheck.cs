using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    public bool onGround;
    private void OnTriggerStay2D(Collider2D collider)
    {
        onGround = collider != null && (((1 << collider.gameObject.layer) & platformLayerMask) != 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onGround = false;
    }
}