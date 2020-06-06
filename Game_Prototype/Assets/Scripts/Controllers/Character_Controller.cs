using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Character character;

    public void ChangeCharacter(string name, Elements element)
    {
        this.character.setElement(element);
        this.character.setName(name);
        this.character.ResetCharacter();
    }

    public string GetName()
    {
    	return this.character.characterName;
    }
}
