using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint; //Store our last checkpoint
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //Check if checkpoint available
        if (currentCheckpoint == null)
        {
            //Show gameover screen
            uiManager.GameOver();
            return;
        }

        transform.position = currentCheckpoint.position; //Move player to checkpoint position

        //Restore player health and reset animation
        playerHealth.Respawn();

        //Move camera to checkpoint room (the checkpoint has to be a child of the room object)
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;//Store the activated checkpoint as the current one
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //Deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
        if (collision.transform.tag == "EndGame")
        {
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //Deactivate collider
            StartCoroutine(EndGame());

            SceneManager.LoadScene(2);
        }
    }

    private IEnumerator EndGame()
    {

        yield return new WaitForSeconds(2);

    }

}
