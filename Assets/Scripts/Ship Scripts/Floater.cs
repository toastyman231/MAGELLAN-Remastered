using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Rigidbody rb;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;

    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    public Collider water;

    private Vector3 start;
    private Mesh mesh;
    private Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        start = rb.position;
        mesh = water.gameObject.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        //Debug.Log("Floater pos: " + transform.position);
        //Debug.Log("Floater local pos: " + transform.localPosition);
        //float waveHeight = GetWaterHeight(transform.position.x, transform.position.z);
        float waveHeight = WaveManager.GetWaveHeight(transform.position.x, transform.position.z);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.position = start;
        }
    }

    float GetWaterHeight(float _x, float _z)
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(_x, 200, _z), Vector3.down);
        Debug.DrawRay(new Vector3(_x, 200, _z), Vector3.down * 400f, Color.red);
        Debug.Log("In XZ: " + _x + " " + _z);
        if (water.Raycast(ray, out hit, Mathf.Infinity)) {
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.transform.position);
            return hit.transform.position.y;
        } else
        {
            return -1f;
        }
    }
}
