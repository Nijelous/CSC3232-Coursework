using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles all the state logic in the game, namely around difficulty and game state
public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused,
        Win,
        Lose
    }

    public enum Difficulty {
        Easy,
        Medium,
        Hard
    }

    public GameState gs = GameState.Playing;

    public Difficulty difficulty = Difficulty.Easy;

    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Selects the appropriate update method based on the current state
    private void Update()
    {
        switch (gs)
        {
            case GameState.Playing:
                PlayingUpdate();
                break;
            case GameState.Paused:
                PausedUpdate();
                break;
            case GameState.Win:
                WinUpdate();
                break;
            case GameState.Lose:
                LoseUpdate();
                break;
        }
    }

    private void PlayingUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausedStart();
        }
    }

    private void PausedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Pause Screen").SetActive(false);
            Time.timeScale = 1;
            gs = GameState.Playing;
        }
    }

    // Both send the user back to the menu screen
    private void WinUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject.Find("Win Screen").SetActive(false);
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    private void LoseUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject.Find("Lose Screen").SetActive(false);
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    // All 3 pause the scene, but only one can be exited out of
    public void PausedStart()
    {
        Time.timeScale = 0;
        foreach(GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject))){
            if (go.name == "Pause Screen") go.SetActive(true);
        }
        gs = GameState.Paused;
    }

    public void WinStart()
    {
        Time.timeScale = 0;
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject))){
            if (go.name == "Win Screen") go.SetActive(true);
        }
        gs = GameState.Win;
    }

    public void LoseStart()
    {
        GameObject.Find("Player").SetActive(false);
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject))){
            if (go.name == "Lose Screen") go.SetActive(true);
        }
        gs = GameState.Lose;
    }

    // Buttons for the menu screen
    public void Easy()
    {
        difficulty = Difficulty.Easy;
        SceneManager.LoadScene(1);
    }

    public void Medium()
    {
        difficulty = Difficulty.Medium;
        SceneManager.LoadScene(1);
    }

    public void Hard()
    {
        difficulty = Difficulty.Hard;
        SceneManager.LoadScene(1);
    }
}
