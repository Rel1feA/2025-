using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Bot,
    Top,
    Mid,
    System
}


public class UIManager : Singleton<UIManager>
{
    public Dictionary<string,BasePanel> panelDic=new Dictionary<string,BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    public RectTransform canvas;

    protected override void Awake()
    {
        base.Awake();
        //在预设体加载canvas
        GameObject obj= ResourcesManager.Instance.Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        DontDestroyOnLoad(obj);

        //找到各层级
        bot = canvas.Find("Bot");
        top = canvas.Find("Top");
        mid = canvas.Find("Mid");
        system = canvas.Find("System");

        //在预设体加载eventsystem
        obj = ResourcesManager.Instance.Load<GameObject>("UI/EventSystem");
        DontDestroyOnLoad (obj);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示的层级</param>
    /// <param name="callback">当面板创建成功后处理的方法</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callback = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            if (callback != null)
            {
                callback(panelDic[panelName] as T);
            }
            return;
        }
        ResourcesManager.Instance.LoadAsync<GameObject>("UI/Panels/"+panelName, (obj) =>
        {
            //把他设置为canvas的子对象
            //并且设置相对位置
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到需要打开的预设体面板上的脚本
            T panel = obj.GetComponent<T>();
            panel.ShowMe();
            panelDic.Add(panelName, panel);
            //处理面板完成后的逻辑
            if (callback != null)
            {
                callback(panel);
            }
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            panelDic.Remove(panelName);
        }
    }


    /// <summary>
    /// 得到一个已经显示的面板
    /// </summary>
    /// <param name="panelName"></param>
    public T GetPanel<T>(string panelName) where T:BasePanel
    {
        if(panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }


    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return bot;
            case E_UI_Layer.Mid:
                return mid;
            case E_UI_Layer.Top:
                return top;
            case E_UI_Layer.System:
                return system;
        }
        return null;
    }



    /// <summary>
    /// 给控件添加自定义事件，比如（拖拽，长按）所需要执行的逻辑
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="triggerType">事件类型</param>
    /// <param name="callback">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType triggerType,UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger=control.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger=control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry=new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
    }

}
