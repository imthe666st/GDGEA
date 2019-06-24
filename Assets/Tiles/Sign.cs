using UnityEngine;

public class Sign : InteractableObject
{

    private int Interactions = 0;

    public Sprite MathHard;
    public Sprite MathTooEasy;

    public string HiddenText1;
    public string HiddenText2;

    public override void Interact(Collider2D collider)
    {
        if (GameManager.Instance.GateOpen)
        {
            var tb = GameManager.Instance.CameraController.SpawnTextBox(this.HiddenText1);
            tb.OnClosed(() =>
                        {
                            var tb2 = GameManager.Instance.CameraController.SpawnTextBox(this.HiddenText2);
                            tb2.OnClosed(() =>
                                         {
                                             Destroy(this.gameObject);
                                         });
                        });
            return;
        }
        
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