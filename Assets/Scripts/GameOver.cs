using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lamya.whackamole
{
    public class GameOver : MonoBehaviour
    {

        [SerializeField] private GameObject elements;
        [SerializeField] private GameObject bg;
        [SerializeField] private TextMeshProUGUI highScoreText;



        private void Start()
        {
            GameManager.Instance.gameOverEvent += GameEnded;
        }

        private void GameEnded(int score)
        {
            highScoreText.text = score.ToString();
            elements.SetActive(true);
            bg.SetActive(true);
            LeanTween.scale(elements, Vector3.one, 0.2f);

        }


        private void RestartGame()
        {
            SceneManager.LoadScene(1);
        }

        private void MainMenuLoad()
        {
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            GameManager.Instance.gameOverEvent -= GameEnded;
        }

    }
}


