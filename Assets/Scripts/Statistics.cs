using System;

using TMPro;

using UnityEngine;

public class Statistics : MonoBehaviour
{
	private void Awake()
	{
		GameManager.Instance.CountingTime = false;
	}

	private void Start()
	{
		var textComp = this.GetComponent<TextMeshProUGUI>();

		var time = GameManager.Instance.timePlayed;
		var minute = (int) Mathf.Floor(time / 60);
		time -= (minute * 60f);
		var seconds = (int)time;

		var timeText = $"Time played - {minute}:{seconds :D2}";

		var fightedText = $"Fights encountered - {GameManager.Instance.FightsEncountered}";

		var killedText = $"Enemies killed - {GameManager.Instance.EnemiesKilled}";

		var sb = timeText + "\n" + fightedText + "\n" + killedText + "\n";

		textComp.text = sb;
	}
}
