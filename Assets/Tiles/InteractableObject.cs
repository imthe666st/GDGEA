using System.Collections;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract void Interact(Collider2D collider);

    void OnTriggerEnter2D(Collider2D other)
    {
        this.Interact(other);
    }
}