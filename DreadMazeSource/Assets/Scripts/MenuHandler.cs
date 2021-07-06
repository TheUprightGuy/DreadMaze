using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuHandler : MonoBehaviour
{
    public GameHandler gh;
    public PlayerMove PlayerMovement;
    public MazeGen maze;
    public GameObject end;
    public GameObject pause;

    public GameObject MonsterText;
    public GameObject EscapeText;

    public Image ForeGroundTop;
    public Image ForeGroundBot;

    public float FadeTime = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        pause.SetActive(false);
        end.SetActive(false);
        MonsterText.SetActive(false);
        EscapeText.SetActive(false);
    }

    bool lerpCalled = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(!pause.activeSelf);
        }

        if (gh.MonsterCurrentDistance <=0 && !lerpCalled)
        {
            lerpCalled = true;
            end.SetActive(true);
            StartCoroutine(ForeGroundLerp(1.0f, 0.0f, FadeTime));
            MonsterText.SetActive(true);
        }

        if (maze.exitCoord == PlayerMovement.CurrentCell && !lerpCalled)
        {
            lerpCalled = true;
            end.SetActive(true);
            StartCoroutine(ForeGroundLerp(1.0f, 0.0f, FadeTime));
            EscapeText.SetActive(true);
        }
    }

    public void Exit()
    {
        lerpCalled = false;
        Application.Quit();
    }
    public void TryAgain()
    {
        StartCoroutine(ForeGroundLerp(0.0f, 1.0f, FadeTime, LoadScene));

    }
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator ForeGroundLerp(float startAlpha, float endAlpha, float time, System.Action onEndAction = null)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            Color newColor = ForeGroundTop.color;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, (elapsedTime / time));
            ForeGroundTop.color = newColor;
            yield return new WaitForEndOfFrame();
        }

        elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            Color newColor = ForeGroundBot.color;
            newColor.a = Mathf.Lerp(startAlpha, endAlpha, (elapsedTime / time));
            ForeGroundBot.color = newColor;
            yield return new WaitForEndOfFrame();
        }

        if (onEndAction != null)
        {
            onEndAction();
        }
    }
}
