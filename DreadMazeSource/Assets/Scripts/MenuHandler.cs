using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour
{
    public GameHandler gh;
    public PlayerMove PlayerMovement;
    public MazeGen maze;
    public GameObject end;
    public GameObject pause;

    public GameObject MonsterText;
    public GameObject EscapeText;

    // Start is called before the first frame update
    void Start()
    {
        pause.SetActive(false);
        end.SetActive(false);
        MonsterText.SetActive(false);
        EscapeText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(!pause.activeSelf);
        }

        if (gh.MonsterCurrentDistance <=0)
        {
            end.SetActive(true);
            MonsterText.SetActive(true);
        }

        if (maze.exitCoord == PlayerMovement.CurrentCell)
        {
            end.SetActive(true);
            EscapeText.SetActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
