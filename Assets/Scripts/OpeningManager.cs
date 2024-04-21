using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpeningManager : MonoBehaviour
{
    public string s1;
    public VideoPlayer v_story,v_maze,v_battle,v_home;
    public GameObject b_story, b_start, b_learn;

    private void Start()
    {
        Invoke("ActiveOpeningButton", 3f);
    }

    private void FixedUpdate()
    {
        if (v_story.isPlaying == false)
            Debug.Log("video stop");
    }

    public void LoadAIScene1()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }

    public void LoadAIScene2WithoutCat()
    {
        SceneManager.LoadScene("level2_without_Cat", LoadSceneMode.Additive);
    }

    public void LoadAIScene2WithCat()
    {
        SceneManager.LoadScene("level2_withCat", LoadSceneMode.Additive);
    }

    public void LoadAIScene3()
    {
        SceneManager.LoadScene("level_3", LoadSceneMode.Additive);
    }

    public void ActiveOpeningButton()
    {

    }

    public void PlayStory()
    {
        v_story.Play();
    }
}
