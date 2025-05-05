using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class onBreakableCollide : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject breakParticles;
    private CinemachineImpulseSource impulseSource;
    public Animator targetAnimator;
    public GameObject healthManager;
    public GameObject globalVolume;
    public GameObject player;
    public GameObject regularGameAudio;
    public GameObject WinAudio;
    public GameObject YouDidIt;
    public GameObject SunBack;
    public GameObject timerTextObj;
    public Text timerText;
    public RunTime runtime;

    private float startTime;
    private float elapsedTime;
    private TimeSpan ts;

    private string constantElapsedTime;

    public GameObject timeTextConstantObj;

    private void Update()
    {
        elapsedTime = Time.timeSinceLevelLoad - startTime;
        ts = TimeSpan.FromSeconds(elapsedTime);

        constantElapsedTime = GetElapsedTimeConstant();
        timeTextConstantObj.GetComponent<UnityEngine.UI.Text>().text =
            "TIME: " + constantElapsedTime;
    }

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        timerText = GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {
            regularGameAudio.SetActive(false);
            WinAudio.SetActive(true);
            Debug.Log("WIN!");
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(EnableFirstText());
            StartCoroutine(EnableSecondText());
            targetAnimator.SetBool("FadeToWhiteWinScreenTrigger", true);
            StartCoroutine(DisablePlayerAfterDelay());

            breakParticles.SetActive(true);
            CinemachineShake.instance.CameraShake(impulseSource, 1f);
            Time.timeScale = Mathf.Lerp(1, 0.1f, 5);
            healthManager.SetActive(false);
            globalVolume.SetActive(false);

        }
    }

    IEnumerator DisablePlayerAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("PAUSE");
        player.SetActive(false);
        //Time.timeScale = 0f;
        
    }

    IEnumerator EnableFirstText()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("PAUSE");
        YouDidIt.SetActive(true);
        //Time.timeScale = 0f;

    }
    IEnumerator EnableSecondText()
    {
        yield return new WaitForSeconds(0.75f);
        SunBack.SetActive(true);
        timerTextObj.SetActive(true);
        timerTextObj.GetComponent<UnityEngine.UI.Text>().text = "TIME: " + constantElapsedTime;
        //timerText.text = elapsedTime;
    }

    public string GetElapsedTime()
    {
        return string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
    }

    public string GetElapsedTimeConstant()
    {
        int minutes = (int)ts.TotalMinutes;
        int seconds = (int)Math.Floor(ts.TotalSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
