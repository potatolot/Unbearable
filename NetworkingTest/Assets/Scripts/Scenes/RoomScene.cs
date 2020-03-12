using TMPro;
using UnityEngine;
public class RoomScene : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI selfInformation;
	[SerializeField] private TextMeshProUGUI roomInformation;
	private void Start()
	{
		selfInformation.text = Client.Instance.self.Username;
		roomInformation.text = Client.Instance.roomcode;
	}

	private void OnClickReady()
	{
		//change status
	}

	private void OnClickStart()
	{
		//check if everyone is ready
	}
}