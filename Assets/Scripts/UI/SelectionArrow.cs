using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound; //When we move up/down
    [SerializeField] private AudioClip interactSound; //When we select enter
    private RectTransform rect; //The selection arrow
    private int currentPosition; //Which option the player is choosing

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (_change != 0)
        {
            SoundManager.instance.PlaySound(changeSound);
        }

        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition > options.Length - 1)
        {
            currentPosition = 0;
        }
        //Moving the arrow up/down
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Change position of the arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(-1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(1);
        }

        //Interact with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }

    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        //Access the button component on each option and call its function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
