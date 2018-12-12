using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// Para cambiar de Scenes se usa esta libreria

public class MainMenu : MonoBehaviour {


    public void PlayGame()
    {   /// enviamos al Single player Game
        SceneManager.LoadScene("SinglePlayerExplain");
    }

    public void PlayGameMultiplayer()
    {   /// enviamos al Multi player Game
        SceneManager.LoadScene("MultiPlayerExplain");
    }

    public void QuitGame()
    {
        //Salimos del juego
        Debug.Log("Quit");
        Application.Quit();

    }





}
