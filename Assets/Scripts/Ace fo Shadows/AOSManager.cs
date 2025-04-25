using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AOSManager : MonoBehaviour
{
    [SerializeField] private AOSMenu aOSMenu;
    [SerializeField] private Transform deck1;
    [SerializeField] private Transform deck2;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private CardSprites cardSprites;

    private int totalCardsInDeck = 144;
    private List<Card> cards;

    private Vector3 cardOffset = new Vector3(0.02f, 0.02f, -0.01f);
    private float flipInterval = 01f;
    private float flipDuration = 02f;
    private Vector3 cardUpOffset = new Vector3(0f, 2f, -4f);

    void Start()
    {
        subscribeToUIEvents();
        Init();
        StartFlippingCards();
    }

    private void OnDestroy()
    {
        unsubscribeToUIEvents();
    }

    private void subscribeToUIEvents()
    {
        aOSMenu.OnResetClicked += ResetDeck;
        aOSMenu.OnBackClicked += GoBackToMainMenu;
    }

    private void unsubscribeToUIEvents()
    {
        aOSMenu.OnResetClicked -= ResetDeck;
        aOSMenu.OnBackClicked -= GoBackToMainMenu;
    }

    private void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    void Init()
    {
        var offset = Vector3.zero;
        cards = new List<Card>();
        for (int j = 0; j < totalCardsInDeck; j++)
        {
            Card card = Instantiate(cardPrefab, deck1);
            card.transform.localPosition = offset;
            card.transform.localRotation = Quaternion.Euler(0, 180, 0);
            var cardFrontIndex = UnityEngine.Random.Range(0, cardSprites.CardFronts.Length);
            card.Init(cardSprites.CardFronts[cardFrontIndex], cardSprites.CardBacks[0]);
            cards.Add(card);

            offset += cardOffset;
        }
    }

    void StartFlippingCards()
    {
        StopAllCoroutines();
        StartCoroutine(flippingCards());
    }

    IEnumerator flippingCards()
    {
        cards.Reverse();
        var offset = Vector3.zero;
        foreach (var card in cards)
        {
            yield return new WaitForSeconds(flipInterval);
            yield return animateCard(card, deck2.position - deck1.position + offset, Quaternion.Euler(0, 0, 0));
            card.transform.SetParent(deck2);

            offset += cardOffset;
        }
    }
    
    IEnumerator animateCard(Card card, Vector3 targetPosition, Quaternion targetRotation)
    {
        var elapsedTime = 0f;
        var startingPosition = card.transform.localPosition;
        var startingRotation = card.transform.localRotation;

        var midTargetposition = targetPosition / 2 + cardUpOffset;
        var midTargetRotation = Quaternion.Euler(0, 90, 0);

        while (elapsedTime < flipDuration / 2)
        {
            card.transform.localPosition = Vector3.Lerp(startingPosition, midTargetposition, elapsedTime / (flipDuration / 2));
            card.transform.localRotation = Quaternion.Slerp(startingRotation, midTargetRotation, elapsedTime / (flipDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        while (elapsedTime < flipDuration)
        {
            card.transform.localPosition = Vector3.Lerp(midTargetposition, targetPosition, (elapsedTime - flipDuration / 2) / (flipDuration / 2));
            card.transform.localRotation = Quaternion.Slerp(midTargetRotation, targetRotation, (elapsedTime - flipDuration / 2) / (flipDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.localPosition = targetPosition;
        card.transform.localRotation = targetRotation;
    }

    public void ResetDeck()
    {
        StopAllCoroutines();

        cards.Reverse();
        var offset = Vector3.zero;
        foreach (var card in cards)
        {
            card.transform.SetParent(deck1);
            card.transform.localPosition = offset;
            card.transform.localRotation = Quaternion.Euler(0, 180, 0);

            offset += cardOffset;
        }

        StartFlippingCards();
    }
}
