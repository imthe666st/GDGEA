using System;

using UnityEngine;

public class RitualPlate : MonoBehaviour
{

    [HideInInspector] public int Steps = 0;
    public Gate[] Gates;

    public Enemy.Enemy Enemy;
    
    private bool Finished = false;

    private void Awake()
    {
        if (GameManager.Instance.GateOpen)
        {
            this.Steps = 5;
            this.PerformRitual();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        this.Steps += 1;
    }

    public void PerformRitual()
    {
        if (this.Finished) return;

        Debug.Log(this.Steps);
        if (this.Steps == 5)
        {
            foreach (var gate in this.Gates)
            {
                gate.Open();
            }

            GameManager.Instance.GateOpen = true;

            this.Finished = true;
            this.gameObject.SetActive(false);
            for (var i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            GameManager.Instance.StartPredefinedBattle(this.Enemy, null, this.Steps, false);
            this.Steps = 0;
        }

    }
}