using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float motorFoamMultiplier;
    public float motorFoamBase;
    public float frontFoamMultiplier;
    public float speed;

    private Vector3 start;

    public Rigidbody rb;
    public ParticleSystem motorPS, frontPS1, frontPS2;
    public ParticleSystem.EmissionModule motor, front1, front2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        motor = motorPS.emission;
        front1 = frontPS1.emission;
        front2 = frontPS2.emission;

        start = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
        transform.position = start;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.fixedDeltaTime, ForceMode.Impulse);

        motor.rateOverTime = motorFoamMultiplier * rb.velocity.magnitude + motorFoamBase;
        front1.rateOverTime = frontFoamMultiplier * rb.velocity.magnitude;
        front2.rateOverTime = frontFoamMultiplier * rb.velocity.magnitude;
    }
}
