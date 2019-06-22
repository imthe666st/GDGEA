using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public void Open()
    {


        this.GetComponent<Animator>().SetTrigger("open");
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.transform.position += new Vector3(-0.25f, 0.25f, 0.0f);
    }

    public void AnimationEndEvent()
    {
        Debug.Log("YAY");
        this.enabled = false;
    }
}