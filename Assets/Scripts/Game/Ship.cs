using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public enum State { SAILING, DOCKING, LEAVING, DOCKED };
    public static Ship instance;

    public Transform target;
    public State shipState;
    public float speed;

    public Vector3[] targets;

    private Rigidbody rb;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }

        shipState = State.DOCKED;
        rb = GetComponent<Rigidbody>();
        targets = new Vector3[3]
        {
            //Sailing target
            new Vector3(0, 0, -240),
            //Docking target
            new Vector3(-200, 0, -240),
            //Leaving target
            new Vector3(1000, 0, -240)
        };
    }

    private void FixedUpdate()
    {
        switch (shipState)
        {
            //Set the target position for each case
            case State.SAILING:
                target.position = targets[0];
                break;
            case State.DOCKING:
                target.position = targets[1];
                break;
            case State.LEAVING:
                target.position = targets[2];
                break;
        }

        //Dock if close to target position
        if((target.position - transform.position).magnitude <= 0.3f)
        {
            shipState = State.DOCKED;
        }
        
        //Move if not docked
        if(shipState != State.DOCKED)
        {
            Vector3 targetPos = (target.position - transform.position).normalized;

            rb.AddForce(targetPos * speed * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
}
