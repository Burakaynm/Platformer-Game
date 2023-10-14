using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthPickUp : MonoBehaviour{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3 (0, 180, 0);

    // Start is called before the first frame update
    void Start(){
        
    }

    

    private void OnTriggerEnter2D(Collider2D collision) {
        Damageable damageable = collision.GetComponent<Damageable>(); 

        if(damageable) {
             
            bool wasHealed = damageable.Heal(healthRestore);

            if(wasHealed) {
               
                Destroy(gameObject);
            }
        }
    }

    private void Update() {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

}