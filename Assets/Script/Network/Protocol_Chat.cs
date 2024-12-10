
using System;
using System.Runtime.InteropServices;

static class PROTOCOL_CHAT_CONSTANT
{
    public const string CHAT_SERVER_IP = "127.0.0.1";
    public const UInt16 CHAT_SERVER_PORT = 12130;
    public const int MAX_OF_CHAT_LENGTH = 100;

    // test
    public const int ACCOUNT_ID_LENGTH = 20;
}

enum enPacketType_Chat
{
    CHAT_PACKET_TYPE = 15000,
    REQ_LOGIN,
    REQ_ENTER_MATCH,
    REQ_LEAVE_MATCH,
    REPLY_CODE,
    SEND_CHAT_MSG,
    RECV_CHAT_MSG,
}

enum enReplyCode_Chat
{
    LOGIN_SUCCESS,
    LOGIN_FAIL,
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_REQ_LOGIN_CHAT
{
    public ushort type;
    public ushort accountNo;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_LOGIN_CONSTANT.TOKEN_LENGTH * sizeof(char))]
    public byte[] token;
    public int tokenLength;

    // test
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_CHAT_CONSTANT.ACCOUNT_ID_LENGTH * sizeof(char))]
    public byte[] accountID;
    public int accountLength;
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_REQ_ENTER_MATCH
{
    public ushort type;
    public ushort roomID;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_REQ_LEAVE_MATCH
{
    public ushort type;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_REPLY_CODE_CHAT
{
    public ushort replyCode;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_SEND_CHAT_MSG
{
    public ushort type;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_CHAT_CONSTANT.MAX_OF_CHAT_LENGTH * sizeof(char))]
    public byte[] chat;
    public int chatLength;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class MSG_RECV_CHAT_MSG
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_CHAT_CONSTANT.MAX_OF_CHAT_LENGTH * sizeof(char))]
    public byte[] chat;
    public int chatLength;
    public ushort accountNo;

    // test
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = PROTOCOL_CHAT_CONSTANT.ACCOUNT_ID_LENGTH * sizeof(char))]
    public byte[] accountID;
    public int accountLength;
};