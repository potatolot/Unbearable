public static class NetOP
{
	//what type of message are we sending?
	public const int None = 0;

	public const int JoinGame = 1;
	public const int OnJoinGame = 2;

	public const int ReadyStatus = 3;
	public const int OnReadyStatus = 4;
}

[System.Serializable]
public abstract class NetMsg
{
	public byte OperationCode { set; get; }
	
	public NetMsg()
	{
		OperationCode = NetOP.None;
	}
}