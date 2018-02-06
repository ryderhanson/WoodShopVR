using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDelegator : MonoBehaviour
{
    public delegate void CollisionCallback(Collision collision);
    public delegate void TriggerCallback(Collider other);

    public CollisionCallback OnCollisionEnterDel = (collision) => { };
    public CollisionCallback OnCollisionStayDel = (collision) => { };
    public CollisionCallback OnCollisionExitDel = (collision) => { };

    public TriggerCallback OnTriggerEnterDel = (collider) => { };
    public TriggerCallback OnTriggerStayDel = (collider) => { };
    public TriggerCallback OnTriggerExitDel = (collider) => { };

    public void OnCollisionEnter(Collision collision)
    {
        if(OnCollisionEnterDel != null)
        {
            OnCollisionEnterDel(collision);
        }   
    }

    public void OnCollisionStay(Collision collision)
    {
        if (OnCollisionStayDel != null)
        {
            OnCollisionStayDel(collision);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (OnCollisionExitDel != null)
        {
            OnCollisionExitDel(collision);
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (OnTriggerEnterDel != null)
        {
            OnTriggerEnterDel(collider);
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (OnTriggerStayDel != null)
        {
            OnTriggerStayDel(collider);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (OnTriggerExitDel != null)
        {
            OnTriggerExitDel(collider);
        }
    }
}
