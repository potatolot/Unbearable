using System.Collections.Generic;

[System.Serializable]
public class Net_SendID : NetMsg
{
	public Net_SendID()
	{
		OperationCode = NetOP.SendID;
	}

	public List<int> Connections;
}
