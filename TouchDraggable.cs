using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDraggable : MonoBehaviour
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

        initSucceded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initSucceded)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchStartPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (myCollider.OverlapPoint(touchStartPosition))
                {
                    touchObjectDelta = gameObject.transform.position - touchStartPosition;
                    myTargetJoint.enabled = true;
                }
                else
                {
                    myTargetJoint.enabled = false;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (myTargetJoint.enabled)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector3 targetObjectPosition = touchPosition + touchObjectDelta;
                    myTargetJoint.target = targetObjectPosition;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (myTargetJoint.enabled)
                {
                    myTargetJoint.enabled = false;
                }
            }
        }
    }
}
