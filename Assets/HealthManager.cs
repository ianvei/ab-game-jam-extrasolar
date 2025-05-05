using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 100f;
    public float damageConst;
    public bool isHealing;
    public Vector2 playerResetPosition;
    public Vector3 cameraResetPosition;
    public PlayerPullController player;
    public Camera mainCamera;
    public GameObject cinemachineCamera;
    public GameObject playerObject;
    public GameObject deathParticle;
    public GameObject playerLight;
    public float startTime = 0f;
    public float holdTime = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(healthAmount <= 0f)
        {
            playerObject.transform.localScale = new Vector3(0, 0, 0);
            deathParticle.SetActive(true);
            playerLight.SetActive(false);
            Invoke("RespawnChar", 1.5f);
        }

        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Z))
        {
            Debug.Log(holdTime);
            Debug.Log(Time.deltaTime);
            holdTime -= Time.deltaTime;
            if(holdTime < 0)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    RespawnChar();
                } else
                {
                    SceneManager.LoadScene("SampleScene");
                }
            }
        } else
        {
            holdTime = 0.7f;
        }

        //if (Input.GetKey(KeyCode.Z))
        //{
        //    Debug.Log(holdTime);
        //    Debug.Log(Time.deltaTime);
        //    holdTime -= Time.deltaTime;
        //    if (holdTime < 0)
        //    {
        //        SceneManager.LoadScene("SampleScene");
        //    }
        //}
        //else
        //{
        //    holdTime = 0.7f;
        //}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHealing)
        {
            TakeDamage(damageConst);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healingAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void RespawnChar()
    {
        player.transform.position = playerResetPosition;
        playerObject.transform.localScale = new Vector3(1, 1, 1);
        deathParticle.SetActive(false);
        playerLight.SetActive(true);
        mainCamera.transform.position = cameraResetPosition;
        cinemachineCamera.transform.position = cameraResetPosition;
        Heal(100f);
    }
}
