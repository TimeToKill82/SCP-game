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
    public float timeBetweenStages = 0.0001f;
    public float bulletGravity = 1f;
    public float measureSectionLength = 0.0000001f;
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
        Vector3 initialHit;
        Vector3 endHit;
        RaycastHit hitInfo;
        float objectThickness = 0;
        bool measureFinished;
        GameObject grah = new GameObject();
        grah.transform.rotation = gun.transform.rotation;
        rayStart = rayStartRef.transform.position;
        rayDir = grah.transform.forward;
        while (true)
        {
            Debug.DrawRay(rayStart, rayDir, Color.red, 5);
            if (Physics.Raycast(rayStart, rayDir,out hitInfo, bulletVelocity))
            {
                Debug.Log("hit");
                if (hitInfo.transform.gameObject.tag == "DamageAble")
                {
                    processDamage();
                }
                measureFinished = false;
                initialHit = hitInfo.point;
                while(measureFinished == false)
                {
                    endHit = initialHit + (rayDir.normalized * measureSectionLength);
                    Collider[] hitColliders = Physics.OverlapSphere(endHit, 0f);
                    if(hitColliders.Length > 0)
                    {
                        Debug.Log("puuuurrrrrrrrr");
                    }
                    if(hitColliders.Length == 0)
                    {
                        measureFinished = true;
                        objectThickness = Vector3.Distance(initialHit, endHit);
                    }
                    yield return new WaitForSeconds(0.0000001f);
                }
                //Destroy(grah);
                Debug.Log(objectThickness);
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
