using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
	public static LobbyScene Instance { set; get; }

	private void Start()
	{
		Instance = this;
	}
	public void OnClickJoinGame()
	{
		DisableInteraction();

		string username = GameObject.Find("Username").GetComponent<TMP_InputField>().text;
		string roomcode = GameObject.Find("Roomcode").GetComponent<TMP_InputField>().text;

		Client.Instance.SendJoinGame(username, roomcode);
	}
	public void ChangeWelcomeMessage(string msg)
	{
		GameObject.Find("WelcomeText").GetComponent<TextMeshProUGUI>().text = msg;
	}
	public void ChangeAuthenticationMessage(string msg)
	{
		GameObject.Find("AuthText").GetComponent<TextMeshProUGUI>().text = msg;
	}
	public void EnableInteraction()
	{
		GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = true;
	}
	public void DisableInteraction()
	{
		GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = false;
	}
}