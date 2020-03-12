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
		string username = Client.Instance.self.Username;
		bool status = Client.Instance.self.Status;

		if (status == false)
			status = true;
		else if (status == true)
			status = false;

		Client.Instance.SendReadyStatus(username,status);
	}

	private void OnClickStart()
	{
		//check if everyone is ready
	}
}