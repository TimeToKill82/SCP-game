using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject rayStartRef;
    public GameObject shootDirRef;
    public GameObject gun;
    public float stageLength = 0.001f;
    private float timeBetweenStages = 0.1f;
    public float bulletGravity = 2f;
    private float measureSectionLength = 0.001f;
    public float bulletPenVal;

    public void shoot()
    {
        StartCoroutine(processShoot());
    }

    private IEnumerator processShoot()
    {
        //declaring local variables so there can be multiple bullets at once
        Vector3 rayDir;
        Vector3 rayStart;
        RaycastHit hitInfo;
        float modTimeBetweenStages = timeBetweenStages;
        float modBulletPenVal = bulletPenVal;
        float thickness = 0;
        GameObject grah = new GameObject();
        //setting the locations and values of vector3s / gameobjects
        grah.transform.rotation = gun.transform.rotation;
        rayStart = rayStartRef.transform.position;
        rayDir = grah.transform.forward;
        //main while loop
        while (true)
        {
            //draw ray for debug and if statement with raycast code
            Debug.DrawRay(rayStart, rayDir.normalized * stageLength, Color.red, 5);
            if (Physics.Raycast(rayStart, rayDir,out hitInfo, stageLength))
            {
                //damage ang debug
                Debug.Log("hit");
                if (hitInfo.transform.gameObject.tag == "DamageAble")
                {
                    processDamage();
                }
                Debug.Log(measureThickness(hitInfo, rayDir, modBulletPenVal));
                thickness = measureThickness(hitInfo, rayDir, modBulletPenVal);
                if (measureThickness(hitInfo, rayDir, modBulletPenVal) > modBulletPenVal)
                {
                    yield break;
                }
                else
                {
                    //timeBetweenStages = timeBetweenStages 
                    modBulletPenVal = modBulletPenVal - thickness;
                }
            }
            //prepares values for next cycle of loop
            grah.transform.position = rayStart + rayDir.normalized * stageLength;
            rayStart = grah.transform.position;
            rayDir = grah.transform.forward;
            if (grah.transform.rotation.x != 90 || grah.transform.rotation.x < 90)
            {
                grah.transform.Rotate(bulletGravity, 0, 0);
                //Debug.Log("chess");
            }
            //waits for x seconds to control bullet velocity
            yield return new WaitForSeconds(modTimeBetweenStages);
        }
    }

    private float measureThickness(RaycastHit mHitInfo, Vector3 mRayDir, float mBulletPenVal)
    {
        Vector3 initialHit;
        Vector3 endHit = new Vector3(0, 0, 0);
        float objectThickness = 0;
        bool measureFinished;
        measureFinished = false;
        initialHit = mHitInfo.point;
        endHit = initialHit + (mRayDir.normalized * measureSectionLength);
        while (measureFinished == false)
        {
            //Debug.Log(measureFinished);
            //moving point for measure think "extending the tapemeasure"
            endHit = endHit + (mRayDir.normalized * measureSectionLength);
            //debug stuff
            //Debug.Log(endHit);
            //declares and sets colliders used to figure out if the point(end of tape measure) is in an object
            Collider[] hitColliders = Physics.OverlapSphere(endHit, 0f);
            //Debug.Log(hitColliders.Length);
            //checks if point(end of tape measure) is in an object
            if (hitColliders.Length == 0)
            {
                //sets the bool that controls loop to true and sets object thickness output
                measureFinished = true;
                objectThickness = Vector3.Distance(initialHit, endHit);
            }
        }
        return objectThickness;
    }

    private void processDamage()
    {

    }
}