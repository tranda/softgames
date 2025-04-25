using UnityEngine;

public class CardSprites : MonoBehaviour
{
    [SerializeField] private Sprite[] cardFronts;
    [SerializeField] private Sprite[] cardBacks;

    public Sprite[] CardFronts { get => cardFronts; }
    public Sprite[] CardBacks { get => cardBacks; }
}
