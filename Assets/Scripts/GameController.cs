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
    public GameObject ball;
    public Racket racketBot;
    public Racket racketPlayer;
    public Bot bot;
    public Player player;
    public int scoreBot;
    public int scorePlayer;
    public Text textScoreBot;
    public Text textScorePlayer;
    public Text textResult;
    public Text textGameResult;
    public bool botPaddleCollision = false;
    public bool playerPaddleCollision = false;
    private float time;
    public bool flagBot = false;

    public enum gameState
    {
        botStart,
        playerStart,

        botPaddle,
        playerPaddle,

        playerSide,
        botSide,

        losePoint,
        winPoint
    }
    public static gameState state;
    void Start()
    {
        
        StartGame();
    }

    public void StartGame()
    {
        state = gameState.playerStart;
        scoreBot = 0;
        scorePlayer = 0;
        textScoreBot.text = scoreBot.ToString();
        textScorePlayer.text = scorePlayer.ToString();
        time = -1f;
    }

    public void TextGameResultUpdate()
    {
        if (state == gameState.botStart)
        {
            textGameResult.text = "BotStart";
        }
        else if (state == gameState.playerStart)
        {
            textGameResult.text = "PlayerStart";
        }
        else if (state == gameState.botPaddle)
        {
            textGameResult.text = "BotPaddle";
        }
        else if (state == gameState.botPaddle)
        {
            textGameResult.text = "PlayerPaddle";
        }
    }

    void Update()
    {
        if (scoreBot >= 12)
        {
            textResult.text = "YOU LOSE :(";
        }
        else if (scorePlayer >= 12)
        {
            textResult.text = "YOU Win :(";
        }

        if (state == gameState.playerStart)
        {
            if (playerPaddleCollision == true)
            {
                state = gameState.playerPaddle;
                playerPaddleCollision = false;
            }
            else
            {
                Serving(racketPlayer);
            }
        }
        else if (state == gameState.botStart)
        {
            if (time == -1f)
            {
                time = Time.time;
            }
            else if (Time.time - time > 2f)
            {
                //BOT SERVİS KULLANACAK
                time = -1f;
                state = gameState.botPaddle;
            }
            if (botPaddleCollision == true)
            {
                state = gameState.botStart;
                botPaddleCollision = false;
            }
            else
            {
                Serving(racketBot);
            }
        }
        BotAction();
        TextGameResultUpdate();
    }

    public void BotAction()
    {
        if (flagBot == true)
        {
            bot.FollowBall();
        }
        else
        {
            racketBot.ResetPosition();
        }
    }

    public void Serving(Racket racket)
    {
        float averageZCoordinate;
        if (racket.name == "Racket_Bot")
        {
            averageZCoordinate = (0 + racketBot.transform.position.z) / 2;
            ball.transform.position = new Vector3(7, 7, averageZCoordinate);
        }
        else if (racket.name == "Racket_Player")
        {
            averageZCoordinate = (0 + racketPlayer.transform.position.z) / 2;
            ball.transform.position = new Vector3(-7, 7, averageZCoordinate);
        }
    }

    public void StartRound(){
        ball.transform.position = new Vector3(4, 10f, 0);
    }

    //X = -10, 0
    //Y = 5, 10
    //Z = 5, -5

    public void TrackingRacket(float x, float y, float recWidth, float recHeight, float camWidth, float camHeight){
        player.PlayerUpdate(x, y, recWidth, recHeight, camWidth, camHeight);
    }
}
