using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchScreens : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartScreen() {
        SceneManager.LoadScene("Title");
    }  

    public void WinningEndScreen() {
        SceneManager.LoadScene("Win");
    }   

    public void LosingEndScreen() {
        SceneManager.LoadScene("Lose");
    }   

    public void GameScreen() {
        SceneManager.LoadScene("Cards");
    }    
}
