using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public GameObject storyBattleObject, storyMazeObject, startObject, learnBattleObject, learnMazeObject,story,learn;
    public VideoPlayer videoPlayer1;
    public VideoPlayer videoPlayer2;

    void Start()
    {
        videoPlayer1.loopPointReached += OnVideo1End;
        videoPlayer1.Play();
    }

    void OnVideo1End(VideoPlayer vp)
    {
        videoPlayer1.loopPointReached -= OnVideo1End;
        storyMazeObject.SetActive(false);
        videoPlayer2.Play();
    }

    public void StoryBattleActive()
    {
        storyBattleObject.SetActive(true);
    }

    public void StoryMazeActive()
    {
        storyMazeObject.SetActive(true);
    }

    public void StartActive()
    {
        startObject.SetActive(true);
    }

    public void LearnBattleActive()
    {
        learnBattleObject.SetActive(true);
    }

    public void LearnMazeActive()
    {
        learnMazeObject.SetActive(true);
    }

    public void Return()
    {
        
    }

    //public void StoryInactive()
    //{
    //    storyObject.SetActive(false);
    //}

    public void StartInactive()
    {
        startObject.SetActive(false);
    }

    //public void LearnInactive()
    //{
    //    learnObject.SetActive(false);
    //}

    public void ObjectInactive(GameObject go)
    {
        go.GetComponent<VideoPlayer>().Pause();
        go.SetActive(false);
        videoPlayer2.Play();
    }
}
