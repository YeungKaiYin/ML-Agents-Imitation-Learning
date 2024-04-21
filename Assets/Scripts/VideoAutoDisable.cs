using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoAutoDisable : MonoBehaviour
{
    VideoPlayer videoPlayer;
    VideoController vc;
    void Start()
    {
        vc = GameObject.Find("VideoController").GetComponent<VideoController>();
        Debug.Log(vc.name);
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideo1End;
        videoPlayer.Play();
    }

    void OnVideo1End(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnVideo1End;
        vc.ObjectInactive(gameObject);
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vc.ObjectInactive(gameObject);
        }
    }
}
