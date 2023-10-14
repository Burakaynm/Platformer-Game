using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPreFab;
    public GameObject healthTextPreFab;

    public Canvas gameCanvas;

    private void Awake() {
        gameCanvas = FindObjectOfType <Canvas>();


    }

    private void OnEnable() {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable() {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed ;
    }

    public void CharacterTookDamage(GameObject character,int damageReceived) {

        Vector3 spawnPosition =  Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPreFab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString(); 
    }

    public void CharacterHealed(GameObject character, int healthRestored) {

        //TODO
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPreFab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        
        tmpText.text = healthRestored.ToString();   
    }

}
