using UnityEngine;

public class SelectCards : MonoBehaviour

{
    private GameObject cards;
    private PlayCards cardsScript;
    private bool isSelected;
    private GameObject playerHandContainer;
    private GameObject selectedCardsContainer;
    private int selectedCardTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cards = GameObject.FindWithTag("GameController");
        cardsScript = cards.GetComponent<PlayCards>();
        isSelected = false;
        playerHandContainer = GameObject.FindWithTag("PlayerOneHand");
        selectedCardsContainer = GameObject.FindWithTag("Selected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickCard()
    {
        if (!isSelected & selectedCardsContainer.transform.childCount < 4)
        {
            gameObject.transform.SetParent(selectedCardsContainer.transform, false);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 140);
            selectedCardTag = int.Parse(gameObject.tag);
            cardsScript.playerOneCards.Remove(selectedCardTag);
            cardsScript.selectedCards.Add(selectedCardTag);
            cardsScript.selectedCards.Sort();

            isSelected = true;

            cardsScript.UpdatePlayerHand();
        } else if (isSelected)
        {
            gameObject.transform.SetParent(playerHandContainer.transform, false);
            selectedCardTag = int.Parse(gameObject.tag);
            cardsScript.playerOneCards.Add(selectedCardTag);
            cardsScript.selectedCards.Remove(selectedCardTag);
            cardsScript.playerOneCards.Sort();

            isSelected = false;

            cardsScript.UpdatePlayerHand();
        }
    }
}
