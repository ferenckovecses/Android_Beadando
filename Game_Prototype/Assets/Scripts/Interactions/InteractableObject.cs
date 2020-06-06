using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Dialogue dialogues;
    bool actionRange = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            actionRange = true;
            GameObject.Find("Data").GetComponent<Data_Controller>().AddObject(this);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        {
            actionRange = false;
            GameObject.Find("Data").GetComponent<Data_Controller>().RemoveObject();
        }
    }

    public string[] GetDialogues()
    {
        return dialogues.sentences;
    }

    public bool InRange()
    {
        return this.actionRange;
    }

    //Dialógus lejátszása
    public void TriggerDialogue()
    {
        if(dialogues != null)
            FindObjectOfType<Dialogue_Controller>().StartDialogue(this);
    }
}
