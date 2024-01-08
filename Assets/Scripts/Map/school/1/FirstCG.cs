using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstCG : MonoBehaviour
{
    [SerializeField]private bool isUsed;
    public PlayableDirector playableDirector;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&isUsed==false)
        {
            playableDirector.Play();
            GameManager.instance.gameMode = GameManager.GameMode.GamePlay;
            PlayerWSADMove.WSAD.rb.velocity = Vector2.zero;
            isUsed = true;
        }
    }
}
