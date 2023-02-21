using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lamya.whackamole
{
    public class GameManager : MonoBehaviour
    {

        #region Variables

        public static GameManager Instance;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI gameTimeText;
        [SerializeField] private float gameTimer;
        [SerializeField] public float timer;
        private float fixedTimer;
        private int level;

        //array of Moles
        [SerializeField] Mole[] moles = new Mole[7];

        private int score;
        int Score
        {
            get => score;
            set
            {
                score = value;
                scoreText.text = score.ToString();
            }
        }
        private bool gameEnded = false;
        public bool GameEnded => gameEnded;
        public event Action<int> gameOverEvent;


        #endregion


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            Destroy(gameObject);
        }

        private void Start()
        {
            // subscribe to event
            Mole.IncreaseScoreEvent += IncreaseScore;

            fixedTimer = timer;
            level = PlayerPrefs.GetInt("level");
            Level(level);
            
        }

        // Update is called once per frame
        void Update()
        {
            
            if (!gameEnded)
            {
                if (gameTimer > 0)
                {
                    gameTimer -= Time.deltaTime;
                    timer -= Time.deltaTime;
                    gameTimeText.text = gameTimer.ToString("00s");
                }
                else
                {
                    gameTimer = 0;
                    gameEnded = true;
                }

                if (timer <= 0)
                {
                    RandomMole();
                    timer = fixedTimer; //resets the time 
                }
            }
            else
            {
                GameEnds();
            }
        }

        public void IncreaseScore()
        {
           
            Score++;
            // update high score
            if (Score > PlayerPrefs.GetInt("highScore", 0)) 
            {
                PlayerPrefs.SetInt("highScore", Score);
            }
        }

        private void RandomMole()
        {
            int size = moles.Length;

            while (true)
            {
                int index = Random.Range(0, size);
                if (moles[index].MovingUp == false)
                {
                    moles[index].MovingUp = true;
                    break;
                }
            }

            //  moles[(int)UnityEngine.Random.Range(0, moles.Length - 1)].SetMovingUp(true);
        }

        private void GameEnds()
        {
            for (int i = 0; i < moles.Length; i++)
            {
                moles[i].MovingUp = false;
            }
            // invoke event
            gameOverEvent?.Invoke(PlayerPrefs.GetInt("highScore", 0));
        }

        private void Level(int l)
        {
            if (l == 1)
            {
                fixedTimer = timer;
            }
            else if (l == 2)
            {
                timer = 0.5f;
                fixedTimer = timer;
                for (int i = 0; i < moles.Length; i++)
                {
                    moles[i].FixedTimer = 0.75f;
                }

            }
            else if (l == 3)
            {
                timer = 0.25f;
                fixedTimer = timer;
                for (int i = 0; i < moles.Length; i++)
                {
                    moles[i].FixedTimer = 0.5f;
                }
            }
        }

        private void OnDestroy()
        {
            Mole.IncreaseScoreEvent -= IncreaseScore;
        }

    }
}
