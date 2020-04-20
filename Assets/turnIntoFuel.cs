using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnIntoFuel : MonoBehaviour
{
    public SpringJoint joint;

    public GameObject fireDudePrefab;

    GameObject toDestroy;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<SpringJoint>();    
    }

    public void ConnectMyBody(Rigidbody body)
    {
        joint.connectedBody = body;
    }

    void Update()
    {
        if (Time.timeScale < 1) return;
        if (joint.connectedBody == null)
        {
            whenSlashedDie[] allFuels = FindObjectsOfType<whenSlashedDie>();
            foreach (whenSlashedDie fuel in allFuels)
            {
                if (fuel.dead)
                {
                    joint.connectedBody = fuel.myBody;
                    return;
                }
            }
        }    

        if (toDestroy != null)
        {
            if (!toDestroy.GetComponent<whenSlashedDie>().changeEnemyCount && !toDestroy.GetComponent<whenSlashedDie>().dead)
            {
                Instantiate(fireDudePrefab, transform.position, transform.rotation);
            }
            DestroyImmediate(toDestroy);
            toDestroy = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (joint.connectedBody != null && other.transform == joint.connectedBody.transform) joint.connectedBody = null;        

        // TODO like add explowssion and sound
        toDestroy = other.GetComponentInParent<whenSlashedDie>().gameObject;
    }
}
