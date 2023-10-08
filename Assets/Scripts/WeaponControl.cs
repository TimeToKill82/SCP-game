using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{

    public GameObject rayStartRef;
    public GameObject shootDirRef;
    public GameObject gun;
    Vector3 X2 = new Vector3(2,2,2);
    public float bulletVelocity;
    public float timeBetweenStages = 0.0001f;
    public float bulletGravity = 1f;
    public Vector3 gravity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gravity = new Vector3(0f, bulletGravity, 0f);
    }

    public void shoot()
    {
        StartCoroutine(processShoot());
    }

    IEnumerator processShoot()
    {
        Vector3 rayDir;
        Vector3 rayStart;
        GameObject grah = new GameObject();
        grah.transform.rotation = gun.transform.rotation;
        rayStart = rayStartRef.transform.position;
        rayDir = grah.transform.forward;
        while (true)
        {
            Debug.DrawRay(rayStart, rayDir, Color.red, 5);
            if (Physics.Raycast(rayStart, rayDir, bulletVelocity))
            {
                Debug.Log("hit");
                processDamage();
                Destroy(grah);
                yield break;
            }
            grah.transform.position = rayStart + rayDir;
            rayStart = grah.transform.position;
            rayDir = grah.transform.forward;
            grah.transform.Rotate(2,0,0);
            yield return new WaitForSeconds(timeBetweenStages);
        }
    }

    void processDamage()
    {

    }
}
