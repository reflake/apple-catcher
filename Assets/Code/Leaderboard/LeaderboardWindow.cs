﻿using UnityEngine;

namespace Leaderboard
{
	public class LeaderboardWindow : MonoBehaviour
	{
		[SerializeField] private CanvasGroup canvasGroup = null;
		[SerializeField] private LeaderboardSystem system = null;
		[SerializeField] private Table table = null;
		
		private void Start()
		{
			foreach (var entryData in system.Entries)
			{
				table.AddRow(entryData);
			}
		}

		public void Open()
		{
			canvasGroup.alpha = 1f;
			canvasGroup.blocksRaycasts = true;
		}

		public void Close()
		{
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;

			MainMenu.MainMenu.Instance.BackHome();
		}
	}
}