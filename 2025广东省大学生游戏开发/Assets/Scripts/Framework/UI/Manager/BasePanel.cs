using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    private Dictionary<string,List<UIBehaviour>> controlDic = new Dictionary<string,List<UIBehaviour>>();

    private void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
    }

    private void FindChildrenControl<T>() where T:UIBehaviour
    {
        T[] controls= GetComponentsInChildren<T>();
        string objName;
        for(int i=0; i<controls.Length; i++)
        {
            objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(controls[i].gameObject.name, new List<UIBehaviour> { controls[i] });
            }

        }
    }

    protected T GetControl<T>(string objName) where T : UIBehaviour
    {
        if(controlDic.ContainsKey(objName))
        {
            for(int i=0;i<controlDic[objName].Count;i++)
            {
                if (controlDic[objName][i] is T)
                {
                    return controlDic[objName][i] as T;
                }
            }
        }

        return null;
    }

    public virtual void ShowMe()
    {

    }
    public virtual void HideMe()
    {
        Destroy(gameObject);
    }
}
