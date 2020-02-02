using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IndieMarc.TopDown;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	[Header("Start Window")]
	[SerializeField] private GameObject startWindow;
	[SerializeField] private Button startGame;
	[SerializeField] private Button audioSetting;
	[SerializeField] private Button soundSetting;
	[SerializeField] private Button languageSetting;

	[Header("Game Window")]
	[SerializeField] private GameObject gameWindow;
	[SerializeField] private Text gameTimeText;
	[SerializeField] private Text teamWorkersText;
	[SerializeField] private Text[] leaderGameBoardsTexts;

	[Header("Leaderboards Window")]
	[SerializeField] private GameObject leaderboardsWindow;
	[SerializeField] private Text[] leaderFinishBoardsTexts;
	[SerializeField] private Button restartGame;

	[Header("Materials")]
	[SerializeField] private Sprite OnAudio;
	[SerializeField] private Sprite OffAudio;
	[SerializeField] private Sprite OnSound;
	[SerializeField] private Sprite OffSound;

	public bool isStartGame;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		isStartGame = false;

		startGame.onClick.AddListener(() => StartGame());
		audioSetting.onClick.AddListener(() => SetSetting("Audio"));
		soundSetting.onClick.AddListener(() => SetSetting("Sound"));
		languageSetting.onClick.AddListener(() => SetSetting("Lang"));
		restartGame.onClick.AddListener(() => RestartGame());
	}

	private void Update()
	{
		if (isStartGame)
		{
			teamWorkersText.text = (CharacterPlayer.Instance.getEmployeesCount()).ToString();

			for (var i = 0; i < leaderGameBoardsTexts.Length; i++)
			{
				if (i <= CharacterContainer.Instance.GetCharacterCount() - 1)
				{
					var score = CharacterContainer.Instance.GetSortedCharacters()[i].score;
					var build = CharacterContainer.Instance.GetSortedCharacters()[i].buildCount;
					var color = CharacterContainer.Instance.GetSortedCharacters()[i].playerColor;

					leaderGameBoardsTexts[i].color = color;
					leaderGameBoardsTexts[i].text = string.Concat(i + 1, ". ", score, " / ", build);
				}
				else
				{
					leaderGameBoardsTexts[i].gameObject.SetActive(false);
				}
			}
		}
	}

	private void StartGame()
	{
		isStartGame = true;

		startWindow.SetActive(false);
		gameWindow.SetActive(true);

		StartCoroutine(GameTimer());
	}

	private void FinishGame()
	{
		gameWindow.SetActive(false);
		leaderboardsWindow.SetActive(true);

		for (var i = 0; i < leaderFinishBoardsTexts.Length; i++)
		{
			if (i <= CharacterContainer.Instance.GetCharacterCount() - 1)
			{
				var score = CharacterContainer.Instance.GetSortedCharacters()[i].score;
				var build = CharacterContainer.Instance.GetSortedCharacters()[i].buildCount;
				var color = CharacterContainer.Instance.GetSortedCharacters()[i].playerColor;


				leaderFinishBoardsTexts[i].color = color;
				leaderFinishBoardsTexts[i].text = string.Concat(i + 1, ". ", score, " / ", build);
			}
			else
			{
				leaderFinishBoardsTexts[i].gameObject.SetActive(false);
			}
		}
	}

	private void RestartGame()
	{
		SceneManager.LoadScene(0);
	}

	private void SetSetting(string tag)
	{
		switch (tag)
		{
			case "Audio":
				{
					AudioManager.Instance.AudioOn = !AudioManager.Instance.AudioOn;
					audioSetting.image.sprite = AudioManager.Instance.AudioOn ? OnAudio : OffAudio;

					break;
				}
			case "Sound":
				{
					AudioManager.Instance.SoundOn = !AudioManager.Instance.SoundOn;
					soundSetting.image.sprite = AudioManager.Instance.SoundOn ? OnSound : OffSound;

					break;
				}
			case "Lang":
				{
					break;
				}
		}
	}

	IEnumerator GameTimer()
	{
		int time = 200;
		int min = time / 60;
		int sec = time % 60;
		gameTimeText.text = string.Concat(min < 10f ? ("0" + min).ToString() : min.ToString(), ":", sec < 10f ? ("0" + sec).ToString() : sec.ToString());

		while (time > 0f)
		{
			yield return new WaitForSeconds(1f);

			time -= 1;
			min = time / 60;
			sec = time % 60;
			gameTimeText.text = string.Concat(min < 10f ? ("0" + min).ToString() : min.ToString(), ":", sec < 10f ? ("0" + sec).ToString() : sec.ToString());
		}

		FinishGame();

		yield return null;
	}
}
