using TMPro;
using UnityEngine;
public class RoomScene : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI selfInformation;
	private void Start()
	{
		selfInformation.text = Client.Instance.self.Username;
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