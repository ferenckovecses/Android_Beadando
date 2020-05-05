using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Character character;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter(string name, Elements element)
    {
        this.character.setElement(element);
        this.character.setName(name);
        this.character.ResetCharacter();
    }
}
