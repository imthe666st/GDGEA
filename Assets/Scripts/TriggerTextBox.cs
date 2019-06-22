using UnityEngine;

public class TriggerTextBox : MonoBehaviour
{
    public string[] Input;

    private int access = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        this.NextBox();
	}

	private void NextBox()
	{
		if (this.access >= this.Input.Length)
		{
			Destroy(this.gameObject);
			return;
		}
		
		GameManager.Instance.CameraController.SpawnTextBox(this.Input[this.access++]).OnClosed(() =>
																							   {
                                                                                                 this.NextBox();
																							   });
	}
}
