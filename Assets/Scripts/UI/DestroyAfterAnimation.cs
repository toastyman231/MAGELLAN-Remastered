using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class DestroyAfterAnimation : MonoBehaviour
{
    public bool destroy = false;
    public Rigidbody shipRB;
    public CinemachineVirtualCamera vcam;
    public PlayableDirector director;
    public OceanGeometry geometry;

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            //shipRB.AddForce(shipRB.transform.forward * 50, ForceMode.Impulse);
            Ship.instance.shipState = Ship.State.SAILING;
            vcam.Priority += 2;
            director.Play();
            StartCoroutine(geometry.LerpOverTime(geometry.offsetFromCamera, new Vector3(0, 0, -350), 4, 0.3f));
            StartCoroutine(geometry.LerpOverTime(geometry.lengthScale, 100f, 4, 0.3f));
            StartCoroutine(DestroyMe());
        }
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitUntil(() => geometry.offsetFromCamera == new Vector3(0, 0, -350)
                                    && geometry.lengthScale == 100f);
        Destroy(transform.parent.gameObject);
    }
}
