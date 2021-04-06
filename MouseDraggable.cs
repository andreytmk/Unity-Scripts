using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDraggable : MonoBehaviour
{
    private TargetJoint2D myTargetJoint;
    private Collider2D myCollider;
    private Vector3 touchObjectDelta;
    private bool initSucceded = false;

    // Start is called before the first frame update
    void Start()
    {
        myTargetJoint = gameObject.GetComponent<TargetJoint2D>();
        if (myTargetJoint == null)
        {
            Debug.LogError($"TargetJoint2D is not attached.");
            return;
        }

        if (myTargetJoint.enabled)
        {
            Debug.LogError($"TargetJoint2D should be disabled.");
            return;
        }

        myCollider = gameObject.GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError($"Collider2D is not attached.");
            return;
        }

        Debug.Log($"MouseDraggable inited!");
        initSucceded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initSucceded)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pressStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (myCollider.OverlapPoint(pressStartPosition))
            {
                touchObjectDelta = gameObject.transform.position - pressStartPosition;
                myTargetJoint.target = gameObject.transform.position;
                myTargetJoint.enabled = true;
            }
            else
            {
                myTargetJoint.enabled = false;
            }
        }

        if (myTargetJoint.enabled)
        {
            Vector3 pressPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetObjectPosition = pressPosition + touchObjectDelta;
            myTargetJoint.target = targetObjectPosition;
        }

        if (Input.GetMouseButtonUp(0) && myTargetJoint.enabled)
        {
            myTargetJoint.enabled = false;
        }
    }
}
