using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DialogueBehavior : PlayableBehaviour
{
    private PlayableDirector playableDirector;

    public string characterName;
    [TextArea(8, 1)] public string dialogueLine;
    public int dialogueSize;

    private bool isClipPlayed;//the clip over or not
    public bool requirePause;//need to press "E" or not
    private bool pauseScheduled;

    public override void OnPlayableCreate(Playable playable)
    {
        playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
    }

    //just like the update
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isClipPlayed == false && info.weight > 0)
        {
            UIManager.instance.SetupDialogue(characterName, dialogueLine, dialogueSize);

            if (requirePause)
                pauseScheduled = true;

            isClipPlayed = true;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        isClipPlayed = false;
        Debug.Log("clip is stoooooop");
        if (pauseScheduled)
        {
            pauseScheduled = false;

            //GameManager.instance.PauseTimeline(playableDirector);
        }
        else
        {
            UIManager.instance.ToggleDialogueBox(false);
        }
    }

}
