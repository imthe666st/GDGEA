
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesertWind : MonoBehaviour
{

    public FadeOut FadeOut;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "OverworldPlayer")
        {
            var fo = Instantiate(this.FadeOut);

            fo.OnDone = () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);


                Destroy(fo);

            };

        }
    }

    
}