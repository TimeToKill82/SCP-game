using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{

    public GameObject rayStartRef;
    public GameObject shootDirRef;
    Vector3 X2 = new Vector3(2,2,2);
    public float bulletVelocity;
    public float gravity = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void shoot()
    {
        StartCoroutine(processShoot());
    }

    IEnumerator processShoot()
    {
        Vector3 shootVector;
        Vector3 stageLength;
        Vector3 finalTarget;
        Vector3 finalDir;
        Vector3 rayStart;
        Vector3 shootDir;
        Vector3 shootTempDir;
        shootVector = rayStartRef.transform.position - shootDirRef.transform.position;
        rayStart = rayStartRef.transform.position;
        while (true)
        {
            stageLength = shootVector * bulletVelocity;
            finalTarget = stageLength;
            //finalTarget.y += gravity;
            finalDir = rayStart - finalTarget;
            Debug.DrawRay(rayStart, finalTarget, Color.red, 10);
            Debug.Log("Shoot Performed");
            if (Physics.Raycast(rayStart, finalDir, Vector3.Distance(rayStart, finalTarget)))
            {
                Debug.Log("ray Hit");
                yield break;
            }
            yield return new WaitForSeconds(5f);
            shootTempDir = shootVector * bulletVelocity;
            shootDir = shootTempDir * 2;
            rayStart = finalTarget;
            shootVector = rayStart - shootDir;
        }
    }

    void processDamage()
    {

    }
}
