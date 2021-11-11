using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    public static bool isMulti = false;
    Piece p;

    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public bool ConnectToServer(string host,int port)
    {
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        }
        catch(Exception e)
        {
            Debug.Log("Socket Error" + e);

        }

        return socketReady;
    }

    private void Update()
    {
        if(socketReady)
        {
            if(stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();

    }


    private void OnIncomingData(string data)
    {
        string[] aData = data.Split('|');

        switch (aData[0])
        {
            case "SWHO":
                for (int i = 1; i < aData.Length - 1; i++)
                {
                    UserConnected(aData[i], false);
                }
                Send("CWHO|User");
                break;

            case "SCNN":
                UserConnected(aData[1], false);
                break;

            case "SMOV":
                Grid.Instance.CreateGrid();
               break;
        }

    }

    private void UserConnected(string name, bool host)
    {
        GameClient gc = new GameClient();
        gc.name = name;

        players.Add(gc);

        if (players.Count == 2)
        {
            isMulti = true;

        GameManager.Instance.StartGame();
        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }
 
    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;


        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }



}

public class GameClient
{
    public string name;
    public bool isHost;
}
