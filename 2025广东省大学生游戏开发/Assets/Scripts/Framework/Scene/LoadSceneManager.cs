using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadSceneManager : NormalSingleton<LoadSceneManager>
{

    #region 重新切换当前场景
    //重新切换当前场景
    public void LoadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region 切换到下一个场景
    //切换到下一个场景，场景是否循环
    public void LoadNextScene(bool isCyclical = false)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (buildIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            if (isCyclical)
            {
                buildIndex = 0;
            }
            else
            {
                Debug.LogWarning("加载场景失败，场景索引越界");
                return;
            }
        }
        SceneManager.LoadScene(buildIndex);
    }
    #endregion

    #region 切换到上一个场景
    //切换到上一个场景，场景是否循环
    public void LoadPreviousScene(bool isCyclical = false)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (buildIndex < 0)
        {
            if (isCyclical)
            {
                buildIndex = SceneManager.sceneCountInBuildSettings - 1;
            }
            else
            {
                Debug.LogWarning("加载场景失败，场景索引越界");
                return;
            }
        }
        SceneManager.LoadScene(buildIndex);
    }
    #endregion

    #region 异步加载场景（根据名字）
    //异步加载场景，根据名字加载场景
    public void LoadSceneAsync(string sceneName, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null, bool setActiveAfterCompleted = true,
        LoadSceneMode mode = LoadSceneMode.Single)
    {
        MonoController.Instance.StartCoroutine(LoadSceneCoroutine(sceneName, loading, completed, setActiveAfterCompleted, mode));
    }

    IEnumerator LoadSceneCoroutine(string sceneName, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null,
        bool setActiveAfterCompleted = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            loading?.Invoke(asyncOperation.progress);
            yield return null;
        }
        loading?.Invoke(1);
        asyncOperation.allowSceneActivation = setActiveAfterCompleted;
        yield return new WaitForEndOfFrame();
        completed?.Invoke(asyncOperation);
    }
    #endregion

    #region 异步加载场景，根据场景索引
    //异步加载场景，根据索引加载场景
    public void LoadSceneAsync(int sceneIndex, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null, bool setActiveAfterCompleted = true,
    LoadSceneMode mode = LoadSceneMode.Single)
    {
        MonoController.Instance.StartCoroutine(LoadSceneCoroutine(sceneIndex, loading, completed, setActiveAfterCompleted, mode));
    }

    IEnumerator LoadSceneCoroutine(int sceneIndex, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null,
        bool setActiveAfterCompleted = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, mode);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            loading?.Invoke(asyncOperation.progress);
            yield return null;
        }
        loading?.Invoke(1);
        asyncOperation.allowSceneActivation = setActiveAfterCompleted;
        completed?.Invoke(asyncOperation);
    }
    #endregion

    #region 将一个游戏物体转移到另一个场景
    public void MoveGameObjToOtherScene(GameObject obj, string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid() && scene.isLoaded)
        {
            SceneManager.MoveGameObjectToScene(obj, scene);
        }
        else
        {
            Debug.Log("你所传入的场景有误");
        }
    }
    #endregion
}
