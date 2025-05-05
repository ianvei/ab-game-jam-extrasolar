using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public bool jumpPowerup;
    public GameObject jumpPowerupObject;
    public HealthManager healthManager;
    public GameObject powerupText;
    public GameObject powerupPopup;
    public GameObject rightClickText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("JumpPowerup"))
        {
            jumpPowerup = true;
            jumpPowerupObject.SetActive(false);
            powerupText.SetActive(true);
            powerupPopup.SetActive(true);
            rightClickText.SetActive(true);
            StartCoroutine(DisablePopups());
            healthManager.cameraResetPosition = new Vector3(0f, 44.8f, -10f);
            healthManager.playerResetPosition = new Vector3(1.36f, 48.57f, 0f); 
        }
    }

    IEnumerator DisablePopups()
    {
        yield return new WaitForSeconds(5f);
        powerupText.SetActive(false);
        powerupPopup.SetActive(false);
        rightClickText.SetActive(false);
    }
}
