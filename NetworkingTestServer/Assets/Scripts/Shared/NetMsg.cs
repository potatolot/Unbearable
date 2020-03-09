public static class NetOP
{
	//what type ofmessage are we sending?
	public const int None = 0;

	public const int CreateAccount = 1;
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