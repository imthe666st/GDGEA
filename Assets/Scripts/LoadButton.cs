using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Button>().interactable = GameManager.Instance.SaveExists();
    }

    public void Reload()
    {
        GameManager.Instance.Load();
    }
}
