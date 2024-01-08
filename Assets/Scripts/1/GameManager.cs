using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameMode gameMode;
    public enum GameMode
    {
        Normal,
        Dead,
        GamePlay,
        DialogueMoment
    }

    //private PlayableDirector currentPlayableDirector;
    private Black fade;

    private void Awake()
    {
        //单例
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        //开局Normal模式
        gameMode = GameMode.Normal;
    }

    private void Update()
    {
        //if (gameMode == GameMode.DialogueMoment)
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        ResumeTimeline();
        //    }
        //}
    }

    //角色死亡
    public static void PlayerDied()
    {
        instance.fade.SceneFadeOut();
        instance.Invoke("RestartScene", 2.5f);
    }

    //重新加载当前场景
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //注册fade过渡动画
    public static void RegisterLBL(Black obj)
    {
        instance.fade = obj;
    }

    //public void PauseTimeline(PlayableDirector _playableDirector)
    //{
    //    currentPlayableDirector = _playableDirector;
    //    gameMode = GameMode.DialogueMoment;
    //    currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);

    //    UIManager.instance.TogglePressE(true);
    //}

    //public void ResumeTimeline()
    //{
    //    gameMode = GameMode.GamePlay;
    //    currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);

    //    UIManager.instance.TogglePressE(false);
    //    UIManager.instance.ToggleDialogueBox(true);
    //}

    ////use signal to obtain Player movement
    //public void FinishCG()
    //{
    //    gameMode = GameMode.Normal;
    //}
}
