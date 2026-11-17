using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NotesScript : MonoBehaviour
{
    public GameObject notesPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notesPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame & !notesPanel.activeInHierarchy)
        {
            Debug.Log("pressed n");
            showNotes();
        }
    }

    public void showNotes()
    {
        notesPanel.SetActive(true);
    }

    public void hideNotes()
    {
        notesPanel.SetActive(false);
    }
}


