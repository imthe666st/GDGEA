using UnityEngine;

public class Sign : InteractableObject
{

    private int Interactions = 0;


    public override void Interact(Collider2D collider)
    {
        this.Interactions += 1;
        TextBox tb;
        if (this.Interactions < 3)
        {
            tb = GameManager.Instance.CameraController.SpawnTextBox("MEOW MEOW MEOW");
        }
        else
        {
            tb = GameManager.Instance.CameraController.SpawnTextBox("MEOW 3x");
        }
        
        
    }
}