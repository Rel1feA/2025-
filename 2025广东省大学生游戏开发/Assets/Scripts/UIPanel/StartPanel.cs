using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    private void Start()
    {
        GetControl<Button>("StartBTN").onClick.AddListener(() =>
        {
            LoadSceneManager.Instance.LoadSceneAsync("level1", null, null);
        });

        GetControl<Button>("QuitBTN").onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
