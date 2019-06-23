using UnityEngine;

public class RitualPlate : MonoBehaviour
{

    [HideInInspector] public int Steps = 0;
    public Gate[] Gates;

    private bool Finished = false;

    // audio
    public AudioClip Tick;
    public AudioClip Correct;
    public AudioClip Wrong;

    private AudioSource _audioSource;

    void Awake()
    {
        this._audioSource = this.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.Finished) return;

        this.Steps += 1;
        this._audioSource.PlayOneShot(Tick);
    }

    public void PerformRitual()
    {
        if (this.Finished) return;

        Debug.Log(this.Steps);
        if (this.Steps == 5)
        {
            this._audioSource.PlayOneShot(Correct);
            foreach (var gate in this.Gates)
            {
                gate.Open();
            }

            this.Finished = true;
            
            for (var i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            // fight
            this._audioSource.PlayOneShot(Wrong);
            this.Steps = 0;
        }

    }
}