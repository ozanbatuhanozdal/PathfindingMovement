using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public static int pieceCount = 7;    
    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;
    public GameObject singleGameMenu;
    public GameObject photo;



    public GameObject serverPrefab;
    public GameObject clientPrefab;

    Grid grid;
    int piececount;


    private void Start()
    {
        grid = new Grid();
        Instance = this;
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        singleGameMenu.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void ConnectButton()
    {

        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }
    public void HostButton()
    {
        try
        {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();

            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.ConnectToServer("127.0.0.1", 6321);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            
        }
        mainMenu.SetActive(false);
        serverMenu.SetActive(true);

    }

    public void SingleButton()
    {
        mainMenu.SetActive(false);

        singleGameMenu.SetActive(true);
        
    }

    public void ConnectToServerButton()
    {        
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (hostAddress == "")
            hostAddress = "127.0.0.1";


        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.ConnectToServer(hostAddress, 6321);
            Debug.Log("Connected");            
            connectMenu.SetActive(false);

        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        singleGameMenu.SetActive(false);
        Server s = FindObjectOfType<Server>();
        if (s != null)
            Destroy(s.gameObject);
        Client c = FindObjectOfType<Client>();
        if (c != null)
            Destroy(s.gameObject);

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void EasyButton()
    {
        PlayerPrefs.SetInt("pieceCount", 6);
        PlayerPrefs.SetInt("cubeCount", 5);
        StartGame();   

    }

    public void MediumButton()
    {
        PlayerPrefs.SetInt("pieceCount", 7);
        PlayerPrefs.SetInt("cubeCount", 6);
        StartGame();
    }

    public void HardButton()
    {
        PlayerPrefs.SetInt("pieceCount", 8);
        PlayerPrefs.SetInt("cubeCount", 7);
        StartGame();
    }

}
