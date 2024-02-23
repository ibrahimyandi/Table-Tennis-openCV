using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using OpenCvSharp.Demo;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public AnimationController animationController;
    public Table table;
    public Transform ballTransform;
    public Rigidbody ballRigidBody;
    public Collider ballCollider;
    public Bot bot;
    public Player player;
    public Text textScoreBot;
    public Text textScorePlayer;
    public Text textResult;
    public Text textGameResult;
    public GameObject btnResume;
    public GameObject btnPause;
    public Image menuBG;
    public bool botPaddleCollision = false;
    public bool playerPaddleCollision = false;
    private int totalServe = 0;
    private float time = 0;
    private float timeShot = 0;
    private int finalScore = 12;

    private enum gameState
    {
        pause,
        botStart,
        playerStart,

        botPaddle,
        playerPaddle,

        playerSide,
        botSide,

        winPoint,
        losePoint
    }
    private static gameState state = gameState.pause;
    private static gameState lastState = gameState.pause;
    private float ballMaxX = 12f;  // Maximum x koordinatı
    private float ballMinY = 3f; // Minimum y koordinatı
    private float ballMaxY = 12f;  // Maximum y koordinatı
    private float ballMinX = -12f; // Minimum x koordinatı
    private void Awake()
    {
        
    }
    private void Start()
    {
        animationController.CamIdleAnimation();
    }

    private void Update()
    {
        if (state != gameState.pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
            Vector3 ballPosition = ballTransform.position;
            if (ballPosition.x < ballMinX || ballPosition.x > ballMaxX || ballPosition.y < ballMinY || ballPosition.y > ballMaxY)
            {
                OutsideCollider();
            }
            if (state == gameState.playerStart)
            {
                if (playerPaddleCollision == true)
                {
                    state = gameState.playerPaddle;
                    TextGameResultUpdate();
                    playerPaddleCollision = false;
                }
                else
                {
                    Serving();
                }
            }
            else if (state == gameState.botStart)
            {
                if (botPaddleCollision == true)
                {
                    state = gameState.botPaddle;
                    TextGameResultUpdate();
                    botPaddleCollision = false;
                }
                else
                {
                    Serving();
                }
            }
            else if (state == gameState.botPaddle)
            {
                if (playerPaddleCollision == true)
                {
                    state = gameState.playerPaddle;
                    TextGameResultUpdate();
                    playerPaddleCollision = false;
                }
                else
                {
                    timeShot -= Time.deltaTime;
                    if (timeShot <= 0f)
                    {
                        OutsideCollider();
                    }
                }
            }
            else if (state == gameState.playerPaddle)
            {
                if (botPaddleCollision == true)
                {
                    state = gameState.botPaddle;
                    TextGameResultUpdate();
                    botPaddleCollision = false;
                }
                else
                {
                    timeShot -= Time.deltaTime;
                    if (timeShot <= 0f)
                    {
                        OutsideCollider();
                    }
                }
            }
            BotAction();
        }
    }

    public void StartGame()
    {
        menuBG.enabled = false;
        totalServe = 0;
        animationController.MenuUpAnimation();
        animationController.CamStartAnimation();
        StartRound();
        ScoreUpdate(textScoreBot, 0);
        ScoreUpdate(textScorePlayer, 0);
        TextGameResultUpdate();
        btnPause.SetActive(true);
    }
    
    public void ResumeGame()
    {
        animationController.MenuUpAnimation();
        btnPause.SetActive(true);
        menuBG.enabled = false;
        state = lastState;
    }

    public void PauseGame()
    {
        lastState = state;
        state = gameState.pause;
        menuBG.enabled = true;
        animationController.MenuDownAnimation();
        animationController.CamStartAnimation();
        btnResume.SetActive(true);
        btnPause.SetActive(false);
        textResult.text = "PAUSED";
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    private void FinishGame()
    {
        if (textScorePlayer.text == finalScore.ToString())
        {
            textResult.text = "VICTORY";
        }
        else if (textScoreBot.text == finalScore.ToString())
        {
            textResult.text = "DEFEAT";
        }
        state = gameState.pause;
        btnResume.SetActive(false);
        animationController.CamFinishAnimation();
    }

    private void ScoreUpdate(Text text, int score)
    {
        text.text = score.ToString();
        if (score >= finalScore)
        {
            FinishGame();
        }
    }


    private void TextGameResultUpdate()
    {
        if (state == gameState.botStart)
        {
            textGameResult.text = "AI makes a shot.";
        }
        else if (state == gameState.playerStart)
        {
            textGameResult.text = "Player makes a shot.";
        }
        else if (state == gameState.botPaddle)
        {
            textGameResult.text = "The AI has made a shot.";
        }
        else if (state == gameState.playerPaddle)
        {
            textGameResult.text = "The Player has made a shot.";
        }
        else if (state == gameState.pause)
        {
            textGameResult.text = "Paused";
        }
    }

    private void BotAction()
    {
        if (state == gameState.botStart)
        {
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                ballCollider.enabled = true;
                bot.FollowTarget(ballTransform);
            }
            else
            {
                ballCollider.enabled = false;
                bot.FirstPosition();
            }
        }
        else if (state == gameState.playerPaddle)
        {
            bot.FollowTarget(ballTransform);
        }
        else if (state == gameState.playerStart)
        {
            time -= Time.deltaTime;
            if (time <= 0f)
            {
                ballCollider.enabled = true;
            }
            else
            {
                ballCollider.enabled = false;
                bot.ResetPosition();
            }
        }
        else if (state == gameState.botPaddle)
        {
            bot.ResetPosition();
        }
    }

    private void Serving()
    {
        BallReset();
    }

    private void BallReset()
    {
        float averageZCoordinate;
        if (state == gameState.botStart)
        {
            averageZCoordinate = (0 + bot.transform.position.z) / 4;
            ballTransform.position = new Vector3(8, 7, averageZCoordinate);
        }
        else if (state == gameState.playerStart)
        {
            averageZCoordinate = (0 + player.transform.position.z) / 4;
            ballTransform.position = new Vector3(-8, 7, averageZCoordinate);
        }
        ballRigidBody.velocity = Vector3.zero;
        ballRigidBody.angularVelocity = Vector3.zero;
    }

    protected void StartRound()
    {
        if (state == gameState.winPoint)
        {
            ScoreUpdate(textScorePlayer, int.Parse(textScorePlayer.text)+1);

        }
        else if (state == gameState.losePoint)
        {
            ScoreUpdate(textScoreBot, int.Parse(textScoreBot.text)+1);
        }

        table.ResetBounce();
        totalServe++;
        time = 3f;
        timeShot = 8f;
        if (totalServe % 4 == 1 || totalServe % 4 == 2)
        {
            state = gameState.botStart;
            bot.FirstPositionRatio();
        }
        else
        {
            state = gameState.playerStart;
        }

        bot.FirstPositionRatio();
        TextGameResultUpdate();
        Serving();
    }

    protected void OutsideCollider()
    {
        if (state == gameState.playerPaddle)
        {
            if (table.botAreaBounce >= 1)
            {
                state = gameState.winPoint;
            }
            else
            {
                state = gameState.losePoint;
            }
        }
        else if (state == gameState.botPaddle)
        {
            if (table.playerAreaBounce >= 1)
            {
                state = gameState.losePoint;
            }
            else
            {
                state = gameState.winPoint;
            }
        }
        StartRound();
    }

    public void TrackingRacket(float x, float y, float recWidth, float recHeight, float camWidth, float camHeight){
        player.PlayerUpdate(x, y, recWidth, recHeight, camWidth, camHeight);
    }
}
