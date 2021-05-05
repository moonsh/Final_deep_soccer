// Anthony Tiongson (ast119)

using UnityEngine;

public class RefereeController : MonoBehaviour
{
    private GameState currentGameState;
    private GameState lastGameState;

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public GameState GetLastGameState()
    {
        return lastGameState;
    }

    public void SetGameState(GameState newGameState)
    {
        lastGameState = currentGameState;
        currentGameState = newGameState;
    }

    public void PauseGame()
    {
        if (CoachController.coachMode) // Pause game/enable coach mode.
        {
//            Debug.Log("Coach mode enabled.");
            Time.timeScale = 0f;
        }
        else // Unpause game/disable coach mode.
        {
//            Debug.Log("Coach mode disabled.");
            Time.timeScale = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame();
        }
    }
}
