using UnityEngine;
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


