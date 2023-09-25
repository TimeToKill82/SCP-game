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
    public Vector3 gravity = new Vector3(0f,-9.8f,0f);
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
                yield break;
            }
            grah.transform.position = rayStart + rayDir;
            rayStart = grah.transform.position;
            rayDir = grah.transform.forward;
            yield return new WaitForSeconds(0.0001f);
        }
    }

    void processDamage()
    {

    }
}
