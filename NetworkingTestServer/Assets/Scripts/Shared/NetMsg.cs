public static class NetOP
{
	//what type of message are we sending?
	public const int None = 0;

	public const int JoinGame = 1;
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