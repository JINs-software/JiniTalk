
using System;
using System.Net;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginUI : UI_Base
{
    // 로그인 네트워크 세션
    NetworkHandler LoginSession = new NetworkHandler();

    enum InputFields
    {
        LoginServerIpInput,
        LoginServerPortInput,
        IdInput,
        PasswordInput,
    }
    
    enum Buttons
    {
        LoginBtn,
        SignupBtn,
        DeleteBtn,
    }

    enum Texts
    {
        StatusText,
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        LoginSession.Update();  
    }

    private void OnDisable()
    {
        LoginSession.Clear();
    }

    public override void Init()
    {
        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));   
        Bind<Text>(typeof(Texts));

        Button loginBtn = GetButton((int)Buttons.LoginBtn);
        Button signupBtn = GetButton((int)Buttons.SignupBtn);
        Button deleteBtn = GetButton((int)Buttons.DeleteBtn);

        BindEvent(loginBtn.gameObject, OnLoginBtnClicked);
        BindEvent(signupBtn.gameObject, OnSignupBtnClicked);
        BindEvent(deleteBtn.gameObject, OnDeleteBtnClicked);

        LoginSession.RegistPacketHandler((ushort)enPacketType_Login.REPLY_LOGIN, OnLoginReplyRecved);
        LoginSession.RegistPacketHandler((ushort)enPacketType_Login.REPLY_CREATE_ACCOUNT, OnSignupReplyRecved);
    }

    /*********************************************************************
     * UI Handler
     ********************************************************************/
    // '로그인' 버튼 클릭
    void OnLoginBtnClicked(PointerEventData eventData)
    {
        string ip = GetIpString();
        UInt16 port = GetPortNum();

        // 1. 로그인 서버 연결
        if(LoginSession.Connect(ip, port))
        {
            // 2. 로그인 요청
            string id = GetIdString();
            string password = GetPasswordString();

            GlobalManager.Instance.AccountID = id;  

            MSG_AUTH_REQUEST_LOGIN loginMsg = new MSG_AUTH_REQUEST_LOGIN();
            loginMsg.type = (ushort)enPacketType_Login.REQ_LOGIN;
            loginMsg.accountId = Encoding.Unicode.GetBytes(id);
            loginMsg.accountIdLen = loginMsg.accountId.Length;
            loginMsg.accountPassword = Encoding.Unicode.GetBytes(password); 
            loginMsg.accountPasswordLen = loginMsg.accountPassword.Length;

            LoginSession.Send<MSG_AUTH_REQUEST_LOGIN>(loginMsg, true);
        }
        else
        {
            ResetStatusText("로그인 서버 연결 실패");
        }
    }

    // '등록' 버튼 클릭
    void OnSignupBtnClicked(PointerEventData eventData)
    {
        string ip = GetIdString();
        UInt16 port = GetPortNum();

        // 1. 로그인 서버 연결
        if (LoginSession.Connect(ip, port))
        {
            // 2. 계정 생성 요청
            string id = GetIdString();
            string password = GetPasswordString();

            MSG_AUTH_REQUEST_CREATE_ACCOUNT signupMsg = new MSG_AUTH_REQUEST_CREATE_ACCOUNT();
            signupMsg.type = (ushort)enPacketType_Login.REQ_CREATE_ACCOUNT;
            signupMsg.accountId = Encoding.Unicode.GetBytes(id);
            signupMsg.accountIdLen = signupMsg.accountId.Length;
            signupMsg.accountPassword = Encoding.Unicode.GetBytes(password);
            signupMsg.accountPasswordLen = signupMsg.accountPassword.Length;

            LoginSession.Send<MSG_AUTH_REQUEST_CREATE_ACCOUNT>(signupMsg, true);
        }
        else
        {
            ResetStatusText("로그인 서버 연결 실패");
        }
    }

    // '해지' 버튼 클릭
    void OnDeleteBtnClicked(PointerEventData eventData)
    {
        // to do
    }


    /*********************************************************************
     * Message Recv Handler
     ********************************************************************/
    void OnLoginReplyRecved(byte[] payload)
    {
        MSG_AUTH_REPLY_LOGIN reply = LoginSession.BytesToMessage<MSG_AUTH_REPLY_LOGIN>(payload);
        if (reply.replyCode == (ushort)enReplyCode_Login.LOGIN_SUCCESS)
        {
            GlobalManager.Instance.AuthToken = reply.token;
            GlobalManager.Instance.TokenLength = reply.tokenLength; 
        }
        else if (reply.replyCode == (ushort)enReplyCode_Login.LOGIN_FAILURE)
        {
            ResetStatusText("enReplyCode_Login: LOGIN_FAILURE");
        }
        else
        {
            ResetStatusText("enReplyCode_Login: invalid reply code");
        }
    }
    void OnSignupReplyRecved(byte[] payload)
    {
        MSG_AUTH_REPLY_CREATE_ACCOUNT reply = LoginSession.BytesToMessage<MSG_AUTH_REPLY_CREATE_ACCOUNT>(payload);  
        if(reply.replyCode == (ushort)enReplyCode_Login.CRETAE_ACCOUNT_SUCCESS)
        {
            ResetStatusText("계정 생성 성공");
        }
        else if(reply.replyCode == (ushort)enReplyCode_Login.CRETAE_ACCOUNT_FAILURE)
        {
            ResetStatusText("enReplyCode_Login: CRETAE_ACCOUNT_FAILURE");
        }
        else
        {
            ResetStatusText("enReplyCode_Login: invalid reply code");
        }
    }
    void OnDeleteReplyRecved(byte[] payload)
    {
        // to do
    }


    string GetIpString()
    {
        InputField ipInputField = Get<InputField>((int)InputFields.LoginServerIpInput);
        return ipInputField.text;
    }
    UInt16 GetPortNum()
    {
        InputField ipInputField = Get<InputField>((int)InputFields.LoginServerPortInput);
        return UInt16.Parse(ipInputField.text);
    }
    string GetIdString()
    {
        InputField ipInputField = Get<InputField>((int)InputFields.IdInput);
        return ipInputField.text;
    }
    string GetPasswordString()
    {
        InputField ipInputField = Get<InputField>((int)InputFields.PasswordInput);
        return ipInputField.text;
    }
    void ResetStatusText(string text)
    {
        Get<Text>((int)Texts.StatusText).text = "Status: " + text;
    }
}