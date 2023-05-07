using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    INTRO,
    LOBBY,
    IN_GAME_PLAY,
    IN_GAME_WAIT,
    LOADING
}

public enum SceneState
{
    INTRO,
    LOBBY,
    INGAME
}

public enum EndGameState
{
    FAILED,
    CLEAR
}

public enum EndResultStar
{
    ZEROSTAR,
    ONESTAR,
    TWOSTAR,
    THREESTAR
}

public static class PlayingGameManager
{
    static public int? gameLevel = null;
    static public GameState gameState = GameState.INTRO;
    static public SceneState sceneState = SceneState.INTRO;

    static public bool IsInGame()
    {
        return gameState == GameState.IN_GAME_PLAY || gameState == GameState.IN_GAME_WAIT;
    }

    static public void OnStatic()
    {

    }
}

