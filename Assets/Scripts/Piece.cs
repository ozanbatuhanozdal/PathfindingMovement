using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Piece : MonoBehaviour
{
    public Transform target;
    float speed = 1f;
    Vector3[] path;
    int targetIndex;
    public bool isWhite;
    public bool isKing;
    float seconds = Mathf.FloorToInt(15 % 60);
    public bool timerIsRunning = false;    
    public void Start()
    {
        timerIsRunning = true;
      
        if (isKing)
        {

        }
        else
        {

        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);        
        }
    }

    void Update()
    {
        if(isKing)
        {

        }
        else
        {   
            
            if (gameObject.transform.position == target.position)
            {
                Destroy(gameObject);
               

            }
            
        }

        int count = GameObject.FindGameObjectsWithTag("pieces").Length;      

        if (timerIsRunning)
        {
            if (seconds > 0)
            {
                seconds -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                seconds = 0;
                timerIsRunning = false;
                if (count == 1)
                {
                    SceneManager.LoadScene("Lose");
                }
                else
                {
                    SceneManager.LoadScene("Victory");
                }
            }
        }


    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if(path != null)
        {
            for (int i = 0; i < path.Length; i++)
            {

         Vector3 currentWaypoint = path[0];
        
        while(true)
        {

             if(transform.position == currentWaypoint)
             {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];                        
                
                   
             }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
            }
        }
        else
        {

            yield return null;
        }
    }

   



}
