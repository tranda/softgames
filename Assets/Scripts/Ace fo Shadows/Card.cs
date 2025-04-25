using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField] private SpriteRenderer front;
    [SerializeField] private SpriteRenderer back;



    public void Init(Sprite frontSprite, Sprite backSprite)
    {
        front.sprite = frontSprite;
        back.sprite = backSprite;
    }
}
