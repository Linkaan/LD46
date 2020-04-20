using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class searchAndDestroy : MonoBehaviour
{
    public float smoothTime;
    public float howFast;

    public Transform thePlayer;
    public LayerMask layerMask;

    public float howFarUp;

    Vector3 velocity, lastDirection;
    Rigidbody myBody;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        thePlayer = FindObjectOfType<FirstPersonAIO>().transform;
        Destroy(gameObject, 5f);
        resetY();
    }

    void resetY()
    {
        Ray groundRay = new Ray(transform.position + Vector3.up * 10f, -Vector3.up);
        RaycastHit hit;

        Debug.DrawRay(groundRay.origin, groundRay.direction * 100f, Color.red);
        if (Physics.Raycast(groundRay.origin, groundRay.direction, out hit, 100f, layerMask))
        {
            this.transform.position = new Vector3(this.transform.position.x, hit.point.y + howFarUp, this.transform.position.z); // TODO like i dunno probably raycast from above
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 1) return;
        Transform target = thePlayer;
        Transform curTarget = null;
        float closest = 9999999;
        Stroll[] allFuels = FindObjectsOfType<Stroll>();
        foreach (Stroll fuel in allFuels)
        {
            if (curTarget == null) curTarget = fuel.transform;
            else if (Vector3.Distance(target.position, fuel.transform.position) < closest)
            {
                closest = Vector3.Distance(target.position, fuel.transform.position);
                curTarget = fuel.transform;
            }
        }
        if (curTarget != null) target = curTarget;
        
        Vector3 delta = (target.position - transform.position);
        Vector3 direction = delta.normalized;
        lastDirection = direction;

        Vector3 withSameY = transform.position;
        withSameY.y = target.position.y;
        Debug.Log(grounded());
        if (Vector3.Distance(withSameY, target.position) > 1f && allFuels.Length > 0)
        {
            transform.position = transform.position + lastDirection * Time.fixedDeltaTime * howFast;
            myBody.AddForce(lastDirection * howFast, ForceMode.Force);
        }
        else if (allFuels.Length > 0 && target != thePlayer)
        {
            // TODO like i dunno spawn a fireDude
            Destroy(gameObject); // EXXPLOWWSION!
            Destroy(target.GetComponentInParent<whenSlashedDie>().gameObject);
        } else if (target == thePlayer && grounded())
        {
            myBody.AddForce(Vector3.up * 100f, ForceMode.Acceleration);
        }
    }

    bool grounded()
    {
        Ray groundRay = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;
        
        if (Physics.Raycast(groundRay.origin, groundRay.direction, out hit, 1f, layerMask))
        {
            return true;
        }

        return false;
    }

    void LateUpdate()
    {
        if (lastDirection.magnitude > 0.1)
        {
            Quaternion dirQ = Quaternion.LookRotation(lastDirection);
            Quaternion slerp = Quaternion.Slerp(transform.rotation, dirQ, lastDirection.magnitude * smoothTime * Time.deltaTime);
            myBody.MoveRotation(slerp);
        }
    }
}
