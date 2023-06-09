﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace OpenSkiJumping
{
    [CreateAssetMenu]
    public class MainMenuController : ScriptableObject
    {
        public void LoadEditor()
        {
            SceneManager.LoadScene("Scenes/Hills/HillTemplate");
        }
        public void LoadTournament()
        {
            SceneManager.LoadScene("Scenes/Hills/HillTemplate");
        }
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("Scenes/MainMenu");
        }
        public void LoadTournamentMenu()
        {
            SceneManager.LoadScene("Scenes/TournamentMenu");
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
