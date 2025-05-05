using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MouseOverPlayButton : MonoBehaviour
{
    public Vector3 maxScale = new Vector3(2f, 2f, 2f); // Maximum scale size
    public float scaleSpeed = 0.5f; // Speed of scaling

    private Vector3 originalScale;
    private bool isHovering = false;

    public Animator targetAnimator;

    public GameObject bringText;
    public GameObject destroyText;
    void Start()
    {
        originalScale = transform.localScale;
    }

    void OnMouseOver()
    {
        if (!isHovering)
        {
            isHovering = true;
            StopAllCoroutines();
            StartCoroutine(ScaleOverTime(maxScale));
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("CLICKED");
            targetAnimator.SetBool("FadeToWhiteTrigger", true);
            //StartCoroutine(ActivateObjectsWithDelay());
            Invoke("ActivateObjectsWithDelay", 3f);
            Invoke("ActivateObjectsWithDelay2", 4f);
            Invoke("TransitionAfterDelay", 10f);
            //StartCoroutine(ActivateObjectsWithDelay2());
            //StartCoroutine(TransitionAfterDelay());
        }
    }

    void OnMouseExit()
    {
        if (isHovering)
        {
            isHovering = false;
            StopAllCoroutines();
            StartCoroutine(ScaleOverTime(originalScale));
        }
    }

    IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        Vector3 currentScale = transform.localScale;
        float progress = 0f;
        while (progress <= 1f)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, progress);
            progress += Time.deltaTime * scaleSpeed;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    void ActivateObjectsWithDelay()
    {
        destroyText.SetActive(true);

        //yield return new WaitForSeconds(4f);
        //bringText.SetActive(true);
        //Debug.Log("Activating object2");
    }

    void ActivateObjectsWithDelay2()
    {
        bringText.SetActive(true);
    }

    void TransitionAfterDelay()
    {
        SceneManager.LoadScene("SampleScene");
    }
}