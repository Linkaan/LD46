using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slash : MonoBehaviour
{

    public Animator animator;
    public Transform axeHead;

    public float slashRadius;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] enemies = Physics.OverlapSphere(axeHead.position, slashRadius, layerMask);
            foreach (Collider enemy in enemies)
            {
                enemy.GetComponentInParent<whenSlashedDie>().die(axeHead.position);
            }

            animator.SetTrigger("slash");
        }
    }

}
