using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
	public void OnClickJoinGame()
	{
		string username = GameObject.Find("Username").GetComponent<TMP_InputField>().text;
		string roomcode = GameObject.Find("Roomcode").GetComponent<TMP_InputField>().text;

		Client.Instance.SendJoinGame(username, roomcode);
	}
}