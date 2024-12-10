
using System;
using System.Runtime.InteropServices;

static class PROTOCOL_LOGIN_CONSTANT
{
    public const string LOGIN_SERVER_IP = "127.0.0.1";
    public const UInt16 LOGIN_SERVER_PORT = 12110;
    public const int MAX_OF_ACCOUNT_ID = 20;
    public const int MAX_OF_ACCOUNT_PASSWORD = 20;
    public const int TOKEN_LENGTH = 20;
}

enum enPacketType_Login
{
    LOGIN_PACKET_TYPE = 10000,
    REQ_CREATE_ACCOUNT,
    REPLY_CREATE_ACCOUNT,
    REQ_LOGIN,
    REPLY_LOGIN,
}

enum enReplyCode_Login
{
    CRETAE_ACCOUNT_SUCCESS,
    CRETAE_ACCOUNT_FAILURE,

    LOGIN_SUCCESS,
    LOGIN_FAILURE,
}


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_AUTH_REQUEST_CREATE_ACCOUNT
{
    public ushort type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_LOGIN_CONSTANT.MAX_OF_ACCOUNT_ID * sizeof(char))]
    public byte[] accountId;
    public int accountIdLen;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_LOGIN_CONSTANT.MAX_OF_ACCOUNT_PASSWORD * sizeof(char))]
    public byte[] accountPassword;
    public int accountPasswordLen;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_AUTH_REPLY_CREATE_ACCOUNT
{
    public ushort replyCode;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_AUTH_REQUEST_LOGIN
{
    public ushort type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_LOGIN_CONSTANT.MAX_OF_ACCOUNT_ID * sizeof(char))]
    public byte[] accountId;
    public int accountIdLen;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_LOGIN_CONSTANT.MAX_OF_ACCOUNT_PASSWORD * sizeof(char))]
    public byte[] accountPassword;
    public int accountPasswordLen;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_AUTH_REPLY_LOGIN
{
    public ushort replyCode;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_LOGIN_CONSTANT.TOKEN_LENGTH * sizeof(char))]
    public byte[] token;
    public int tokenLength;
    public ushort accountNo;
};