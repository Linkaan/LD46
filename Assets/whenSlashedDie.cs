using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whenSlashedDie : MonoBehaviour
{

    public float howHard, howBig;
    public Rigidbody myBody;
    public GameObject theUnwantedParent;
    public AudioSource playMe;
    public bool changeEnemyCount;

    public bool dead = false;

    fireSizeReducer reducer;

    // Start is called before the first frame update
    void Start()
    {
        reducer = GameObject.FindObjectOfType<fireSizeReducer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void die(Vector3 where)
    {
        if (dead)
        {
            playMe.Stop();
            playMe.Play();
        }
        else
        {
            FindObjectOfType<turnIntoFuel>().ConnectMyBody(myBody);
            dead = true;
            Destroy(theUnwantedParent);
            myBody.AddExplosionForce(howHard, where, howBig);
            Destroy(gameObject, 5f);
            if (changeEnemyCount) reducer.addFuel();
            playMe.pitch = Random.value * 2f + 1f;
            playMe.Play();
        }        
    }
}
