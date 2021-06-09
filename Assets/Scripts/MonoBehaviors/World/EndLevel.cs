﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public void Start()
    {
        EventsManager.Instance.AllItemsCollected?.AddListener(() =>
        {
            var temp = GetComponent<SpriteRenderer>().color;
            temp.a = 255;
            GetComponent<SpriteRenderer>().color = temp;
        });
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        LoadWinScene();
    //    }
    //}

    public void LoadWinScene()
    {
        var name = "Level" + (int.Parse(SceneManager.GetActiveScene().name.Substring(5)) + 1);
        UIManager.Instance.PlayerInventory.Reset(false);
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            GameManager.Instance.LoadLevel(name);
        }
        else
        {
            UIManager.Instance.PlayerInventory.Reset(true);
            SoundManager.Instance.StopMusic();
            GameManager.Instance.LoadLevel("Win");
            UIManager.Instance.WinMenu.Show();
            UIManager.Instance.PlayerInventory.Hide();
            UIManager.Instance.Controls.Hide();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GetComponent<SpriteRenderer>().color.a == 255)
            {
                GetComponent<Animator>().Play("EndWin");
            }
        }
    }
}
