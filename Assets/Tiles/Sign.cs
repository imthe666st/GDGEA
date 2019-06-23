using UnityEngine;

public class Sign : InteractableObject
{

    private int Interactions = 0;

    public Sprite MathHard;
    public Sprite MathTooEasy;
    

    public override void Interact(Collider2D collider)
    {
        this.Interactions += 1;
        PictureBox pb;
        if (this.Interactions < 3)
        {
            pb = GameManager.Instance.CameraController.SpawnPictureBox(this.MathHard);
        }
        else
        {
            pb = GameManager.Instance.CameraController.SpawnPictureBox(this.MathTooEasy);
        }
        
        
    }
}