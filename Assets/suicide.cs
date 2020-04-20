using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suicide : MonoBehaviour
{

    public float smoothTime;
    public float howFast;

    public Transform targetFire;    
    public Collider colliderPlane;

    public LayerMask layerMask;

    public float howFarUp;

    Vector3 velocity, lastDirection;

    void Start()
    {
        resetY();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 1) return;
        Vector3 delta = (targetFire.position - transform.position);
        Vector3 direction = delta.normalized;
        lastDirection = Vector3.SmoothDamp(lastDirection, direction, ref velocity, smoothTime);

        Vector3 withSameY = transform.position;
        withSameY.y = targetFire.position.y;
        if (Vector3.Distance(withSameY, targetFire.position) > 1f)
        {
            transform.position = transform.position + lastDirection * Time.fixedDeltaTime * howFast;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastDirection, Vector3.up), 10.0f * Time.deltaTime);
        transform.eulerAngles = new Vector3(90, transform.eulerAngles.y + 45, 0);
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

    void LateUpdate()
    {
        resetY();        
    }
}
