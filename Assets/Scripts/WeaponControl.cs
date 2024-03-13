using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class WeaponControl : MonoBehaviour
{
    public GameObject rayStartRef;
    public GameObject shootDirRef;
    public GameObject gun;
    public float stageLength = 1f;
    private float timeBetweenStages = 0.1f;
    public int bulletGravity = 2;
    private float measureSectionLength = 0.001f;
    public float bulletPenVal;// 1
    public GameObject globalDownRef;
    Dictionary<string, float> ricochetAngle = new Dictionary<string, float>();
    //declaring dictionary to hold material modifiers
    Dictionary<string, float> matMod = new Dictionary<string, float>();
    
    private void Start()
    {
        Debug.DrawRay(globalDownRef.transform.position, globalDownRef.transform.forward, Color.red);
        Debug.Log("pain");
        //assigning values for matMod
        matMod.Add("Untagged", 0f);
        matMod.Add("wood", 1f);
        matMod.Add("dirt", 1.5f);
        matMod.Add("metal", 5f);
        //assigning values for ricochetAngle !!currently not in use!!
        ricochetAngle.Add("wood", 1f);
    }

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
        bool rotateToggle = true;
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
                Debug.Log(Vector3.Angle(hitInfo.normal, rayDir));
                if (Vector3.Angle(hitInfo.normal, rayDir) < 45){

                    //damage ang debug
                    //Debug.Log("hit");
                    if (hitInfo.transform.gameObject.tag == "DamageAble")
                    {
                        processDamage();
                    }
                    //Debug.Log(measureThickness(hitInfo, rayDir, modBulletPenVal));
                    thickness = measureThickness(hitInfo, rayDir, modBulletPenVal);
                    float modThickness = thickness * matMod[hitInfo.transform.gameObject.tag];
                    //Debug.Log(modThickness);
                    if (modThickness > modBulletPenVal)
                    {
                        yield break;
                    }
                   else if (modThickness == 0)
                    {
                        //way to make some things stop any bullet
                    }
                    else
                    {
                        //timeBetweenStages = timeBetweenStages 
                        modBulletPenVal = modBulletPenVal + modThickness;
                        modTimeBetweenStages = modBulletPenVal / 10;
                    }
                }
                else
                {
                    //grah.transform.Rotate(Vector3.Angle(hitInfo.normal, rayDir));
                    Debug.Log("ricochet");
                }
            }
            //prepares values for next cycle of loop
            grah.transform.position = rayStart + rayDir.normalized * stageLength;
            rayStart = grah.transform.position;
            rayDir = grah.transform.forward;
            if (grah.transform.eulerAngles.x >= 88 && grah.transform.eulerAngles.x <= 92)
            {
                grah.transform.Rotate(findDifference(90f, grah.transform.eulerAngles.x), 0, 0);
                rotateToggle = false;
            }
            if (rotateToggle)
            {
                grah.transform.Rotate(bulletGravity, 0, 0);
            }
            //Debug.Log(grah.transform.rotation.x);
            //Debug.Log("chess");
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

    float findDifference(float inputA, float inputB)
    {
        if(inputA > inputB)
        {
            return inputA - inputB;
        }
        else
        {
            return inputB - inputA;
        }
    }

    float intFromString(string input)
    {
        string b = string.Empty;
        float output = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (Char.IsDigit(input[i]))
                b += input[i];
        }
        if (b.Length > 0)
            output = float.Parse(b);
        return output;
    }

    private void processDamage()
    {

    }
}