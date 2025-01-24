using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool gameOver;
    public GameObject gameOverPanel;
    public GameObject startingText;
    private Animator animator;
    public static bool isGameStarted;
    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGameStarted = false;
        animator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver){
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        if (SwipeManager.tap){
            isGameStarted = true;
            Destroy(startingText);
            animator.SetTrigger("Start");
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            isGameStarted = true;
            Destroy(startingText);
            animator.SetTrigger("Start");
        }
    }
}
