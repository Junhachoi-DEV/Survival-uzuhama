using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class menu_scene : MonoBehaviour
{
    public RectTransform ui_group;

    public void game_start()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void exit_game()
    {
        Application.Quit();
    }
    public void retrun_to_menu()
    {
        SceneManager.LoadScene(0);
    }
    public void click_guide()
    {
        ui_group.anchoredPosition = Vector2.zero;
    }
    public void click_guide_close()
    {
        ui_group.anchoredPosition = Vector2.down * 1000;
    }
}
