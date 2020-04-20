using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stroll : MonoBehaviour {

    public float radius;
    public float probability;
    public float smoothTime;
    public float howFast;
    public int tick = 0;
    public int tickModulos;

    public Vector3 destination;
    public Collider colliderPlane;

    public LayerMask layerMask;

    public float howFarUp;

    Vector3 velocity, lastDirection;

    bool hasInit = false;

    // Start is called before the first frame update
    void Start()
    {                
        resetY();
    }

    Vector3 getNewDestination()
    {        
        Vector3 newPos = transform.position + colliderPlane.ClosestPointOnBounds(Random.insideUnitSphere * radius);
        return newPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 1) return;

        if (!hasInit)
        {
            hasInit = true;
            destination = getNewDestination();
        }
        tick += 1;

        if (tick % tickModulos == 0 && Random.value > probability)
        {
            destination = getNewDestination();
        }
    }

    void FixedUpdate()
    {
        Vector3 delta = (destination - transform.position);
        Vector3 direction = delta.normalized;
        lastDirection = Vector3.SmoothDamp(lastDirection, direction, ref velocity, smoothTime);
        
        if (Vector3.Distance(transform.position, destination) > 1f)
        {
            transform.position = transform.position + lastDirection * Time.fixedDeltaTime;
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
