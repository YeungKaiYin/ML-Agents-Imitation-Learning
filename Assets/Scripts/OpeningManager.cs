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
    public GameObject b_story, b_start, b_learn,b_return;

    private void Start()
    {
        Invoke("ActiveOpeningButton", 3f);
        b_story.SetActive(false);
        b_learn.SetActive(false);
    }

    private void FixedUpdate()
    {
        //if (v_story.isPlaying == false)
        //    Debug.Log("video stop");
    }

    public void B_Story()
    {
        b_story.SetActive(true);
        b_return.SetActive(true);
    }

    public void B_Learn()
    {
        b_learn.SetActive(true);
        b_return.SetActive(true);
    }

    public void B_Return()
    {
        b_return.SetActive(false);
        b_story.SetActive(false);
        b_learn.SetActive(false); 
    }

    public void LoadOpenScene()
    {
        SceneManager.LoadScene("Opening", LoadSceneMode.Additive);
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
        SceneManager.LoadScene("level3", LoadSceneMode.Additive);
    }

    public void ActiveOpeningButton()
    {

    }

    public void PlayStory()
    {
        v_story.Play();
    }
}
