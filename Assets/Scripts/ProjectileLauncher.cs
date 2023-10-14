using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{

    public Transform launchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile() {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;
        
        //Flip the projectile based on direction the character
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1 ,
            origScale.y * transform.localScale.x > 0 ? 1 : -1,
            origScale.z);

        
    }
}
