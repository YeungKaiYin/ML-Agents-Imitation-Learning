using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public GameObject storyObject, startObject, learnObject, learnObject2;
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
        storyObject.SetActive(false);
        videoPlayer2.Play();
    }

    public void StoryActive()
    {
        storyObject.SetActive(true);
    }

    public void StartActive()
    {
        startObject.SetActive(true);
    }

    public void LearnActive()
    {
        learnObject.SetActive(true);
    }

    public void StoryInactive()
    {
        storyObject.SetActive(false);
    }

    public void StartInactive()
    {
        startObject.SetActive(false);
    }

    public void LearnInactive()
    {
        learnObject.SetActive(false);
    }

    public void ObjectInactive(GameObject go)
    {
        go.GetComponent<VideoPlayer>().Pause();
        go.SetActive(false);
        videoPlayer2.Play();
    }
}
