using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject rayStartRef;
    public GameObject shootDirRef;
    public GameObject gun;
    public float bulletVelocity;
    public float timeBetweenStages = 0.5f;
    public float bulletGravity = 1f;
    public float measureSectionLength = 0.0001f;

    public void shoot()
    {
        StartCoroutine(processShoot());
    }

    IEnumerator processShoot()
    {
        //declaring local variables so there can be multiple bullets at once
        Vector3 rayDir;
        Vector3 rayStart;
        Vector3 initialHit;
        Vector3 endHit = new Vector3(0,0,0);
        RaycastHit hitInfo;
        float objectThickness = 0;
        bool measureFinished;
        GameObject grah = new GameObject();
        //setting the locations and values of vector3s / gameobjects
        grah.transform.rotation = gun.transform.rotation;
        rayStart = rayStartRef.transform.position;
        rayDir = grah.transform.forward;
        //main while loop
        while (true)
        {
            //draw ray for debug and if statement with raycast code
            Debug.DrawRay(rayStart, rayDir, Color.red, 5);
            if (Physics.Raycast(rayStart, rayDir,out hitInfo, bulletVelocity))
            {
                //damage ang debug
                Debug.Log("hit");
                if (hitInfo.transform.gameObject.tag == "DamageAble")
                {
                    processDamage();
                }
                //start of measure system for bullet pen
                //declaring values and some debug
                measureFinished = false;
                initialHit = hitInfo.point;
                endHit = initialHit + (rayDir * measureSectionLength);
                //main measure loop
                while (measureFinished == false)
                {
                    //Debug.Log(measureFinished);
                    //moving point for measure think "extending the tapemeasure"
                    endHit = endHit + (rayDir * measureSectionLength);
                    //debug stuff
                    grah.transform.position = endHit;
                    //Debug.Log(endHit);
                    //declares and sets colliders used to figure out if the point(end of tape measure) is in an object
                    Collider[] hitColliders = Physics.OverlapSphere(endHit, 0f);
                    //Debug.Log(hitColliders.Length);
                    //checks if point(end of tape measure) is in an object
                    if(hitColliders.Length == 0)
                    {
                        //sets the bool that controls loop to true and sets object thickness output
                        measureFinished = true;
                        objectThickness = Vector3.Distance(initialHit, endHit);
                    }
                    //wait for x seconds so that unity doesnt crash
                    yield return new WaitForSeconds(0.01f);
                }
                //destroys object, prints thickness output and stops coroutine
                //Destroy(grah);
                Debug.Log(initialHit);
                Debug.Log(endHit);
                Debug.Log(objectThickness);
                yield break;
            }
            //prepares values for next cycle of loop
            grah.transform.position = rayStart + rayDir;
            rayStart = grah.transform.position;
            rayDir = grah.transform.forward;
            if (grah.transform.rotation.x != 90 || grah.transform.rotation.x < 90)
            {
                grah.transform.Rotate(2, 0, 0);
                //Debug.Log("chess");
            }
            //waits for x seconds to control bullet velocity
            yield return new WaitForSeconds(timeBetweenStages);
        }
    }

    void processDamage()
    {

    }
}
