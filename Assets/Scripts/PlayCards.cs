using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayCards : MonoBehaviour
{
    private List<int> cardIDs;
    private int cardIDtoPlay;
    private List<string> cardNames;
    public GameObject cardPrefab;
    public GameObject cardPrefabTwoThirds;
    public GameObject cardPrefabHalf;
    public List<int> cardTypes;
    private List<int> discardedCards;
    private GameObject discardedCardsContainer;
    private GameObject newCard;
    public List<int> playerOneCards;
    private List<int> playerTwoCards;
    private List<int> playerThreeCards;
    private List<int> playerFourCards;
    private GameObject playerOneHand;
    private GameObject playerTwoHand;
    private GameObject playerThreeHand;
    private GameObject playerFourHand;
    public List<int> selectedCards;
    private GameObject selectedCardsContainer;
    private List<int> shuffled;
    public int turn;
    private bool turnInProgress;
    private const int NUM_PLAYERS = 4;

    void Start()
    {
        cardIDs = new List<int> {0,1,2,3,4,5,6,7,8,9,10,11,12,
                                 13,14,15,16,17,18,19,20,21,22,23,24,25,
                                 26,27,28,29,30,31,32,33,34,35,36,37,38,
                                 39,40,41,42,43,44,45,46,47,48,49,50,51};

        // might not need this variable
        cardNames = new List<string> {"Ace of Diamonds", "Ace of Clubs", "Ace of Hearts", "Ace of Spades", 
        "Two of Diamonds", "Two of Clubs", "Two of Hearts", "Two of Spades", 
        "Three of Diamonds", "Three of Clubs", "Three of Hearts", "Three of Spades", 
        "Four of Diamonds", "Four of Clubs", "Four of Hearts", "Four of Spades", 
        "Five of Diamonds", "Five of Clubs", "Five of Hearts", "Five of Spades", 
        "Six of Diamonds", "Six of Clubs", "Six of Hearts", "Six of Spades", 
        "Seven of Diamonds", "Seven of Clubs", "Seven of Hearts", "Seven of Spades", 
        "Eight of Diamonds", "Eight of Clubs", "Eight of Hearts", "Eight of Spades", 
        "Nine of Diamonds", "Nine of Clubs", "Nine of Hearts", "Nine of Spades", 
        "Ten of Diamonds", "Ten of Clubs", "Ten of Hearts", "Ten of Spades", 
        "Jack of Diamonds", "Jack of Clubs", "Jack of Hearts", "Jack of Spades", 
        "Queen of Diamonds", "Queen of Clubs", "Queen of Hearts", "Queen of Spades", 
        "King of Diamonds", "King of Clubs", "King of Hearts", "King of Spades"};

        cardTypes = new List<int> {0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 
                                   7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10, 10, 11, 11, 11, 11, 12, 12, 12, 12};

        discardedCardsContainer = GameObject.FindWithTag("Discarded");
        discardedCards = new List<int> {};

        playerOneHand = GameObject.FindWithTag("PlayerOneHand");
        playerTwoHand = GameObject.FindWithTag("PlayerTwoHand");
        playerThreeHand = GameObject.FindWithTag("PlayerThreeHand");
        playerFourHand = GameObject.FindWithTag("PlayerFourHand");

        selectedCards = new List<int> {};
        selectedCardsContainer = GameObject.FindWithTag("Selected");


        turn = 0;
        cardIDtoPlay = 0;
        turnInProgress = false;
    }

    void Update()
    {
        // if it isn't the player's turn
        if (!turnInProgress && turn % NUM_PLAYERS != 0) {
            turnInProgress = true;
            Invoke(nameof(takeNPCTurn), 3f);
        }
    }

    private void Shuffle(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1); 
            
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void LoadCards()
    {
        for (int i = playerOneHand.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerOneHand.transform.GetChild(i).gameObject);
        }

        for (int i = selectedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(selectedCardsContainer.transform.GetChild(i).gameObject);
        }

        selectedCards = new List<int> {};

        for (int i = discardedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(discardedCardsContainer.transform.GetChild(i).gameObject);
        }

        discardedCards = new List<int> {};

        shuffled = cardIDs.ToList();
        
        Shuffle(shuffled);
        
        playerOneCards = shuffled.GetRange(0, 13);
        playerTwoCards = shuffled.GetRange(13, 13);
        playerThreeCards = shuffled.GetRange(26, 13);
        playerFourCards = shuffled.GetRange(39, 13);

        playerOneCards.Sort();
        playerTwoCards.Sort();
        playerThreeCards.Sort();
        playerFourCards.Sort();

        Debug.Log(string.Join(",",playerOneCards));
        Debug.Log(string.Join(",",playerTwoCards));
        Debug.Log(string.Join(",",playerThreeCards));
        Debug.Log(string.Join(",",playerFourCards));

        playerTwoHand.GetComponent<Text>().text = string.Join(",",playerTwoCards);
        playerThreeHand.GetComponent<Text>().text = string.Join(",",playerThreeCards);
        playerFourHand.GetComponent<Text>().text = string.Join(",",playerFourCards);


        for (int i = 0; i < playerOneCards.Count(); i++)
        {
            if (playerOneCards.Count() > 39)
            {
                newCard = Instantiate(cardPrefabHalf, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(50,70);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(65,10);

            } else if (playerOneCards.Count() > 26)
            {
                newCard = Instantiate(cardPrefabTwoThirds, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200/3,280/3);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(47,10);
            }
            else
            {
                newCard = Instantiate(cardPrefab, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100,140);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(10,10);
            }

            newCard.name = cardNames[playerOneCards[i]].ToString();
            newCard.tag = playerOneCards[i].ToString();
        
            newCard.GetComponentInChildren<Text>().text = cardNames[playerOneCards[i]];
        }
    }

    public void CallBull(bool PlayerOneCalled = false)
    {
        // this just checks if the discard tiles are bull or not
        // we can probably make this return true or false and the use the turn number to find out who to punish

        cardIDtoPlay = turn % 13;

        Debug.Log(String.Join(",", selectedCards)); 
        Debug.Log(cardIDtoPlay);

        for (int i = 0; i < selectedCards.Count(); i++)
        {
            Debug.Log(cardTypes[selectedCards[i]]);
            if (cardTypes[selectedCards[i]] != cardIDtoPlay)
            {
                if (PlayerOneCalled)
                {
                    Debug.Log("Player 1");
                    // just slap the function to punish p1 here
                }
                Debug.Log("BULLLLLLLLLLL");
                // run function here for the punishment
                return;
            }
        }
        Debug.Log("not bull");
    }

    public void MakeDecision(int player)
    {
        if (player == 1)
        {
            
        } else if (player == 2)
        {
            
        } else if (player == 3)
        {
            
        }
    }

    public void Player1PlayCards()
    {
        if (turn % NUM_PLAYERS == 0)
        {
            for (int i = selectedCardsContainer.transform.childCount - 1; i >= 0; i--)
            {
                selectedCardsContainer.transform.GetChild(i).SetParent(discardedCardsContainer.transform, false);
                discardedCards = new List<int> (selectedCards);
                selectedCards = new List<int> {}; 
            }
        }

        NextTurn();
    }

    public void Player2PlayCards()
    {
        Debug.Log("Char 1 turn");
    }

    public void Player3PlayCards()
    {
        Debug.Log("Char 2 turn");

    }

    public void Player4PlayCards()
    {
        Debug.Log("Char 3 turn");

    }

    public void takeNPCTurn() {
        // Character 1
        if (turn % NUM_PLAYERS == 1) {
            Player2PlayCards();
        } 
        // Character 2
        else if (turn % NUM_PLAYERS == 2) {
            Player3PlayCards();
        }
        // Character 3
        else if (turn % NUM_PLAYERS == 3) {
            Player4PlayCards();
        }
        turnInProgress = false;
        turn++;
        cardIDtoPlay = turn % 13;
    }

    public void NextTurn()
    {
        turnInProgress = false;
        turn++;
        cardIDtoPlay = turn % 13;
    }

    public void UpdatePlayerHand()
    {
        for (int i = playerOneHand.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerOneHand.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerOneCards.Count(); i++)
        {
            if (playerOneCards.Count() > 39)
            {
                newCard = Instantiate(cardPrefabHalf, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(50,70);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(65,10);

            } else if (playerOneCards.Count() > 26)
            {
                newCard = Instantiate(cardPrefabTwoThirds, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(200/3,280/3);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(47,10);
            }
            else
            {
                newCard = Instantiate(cardPrefab, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100,140);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(10,10);
            }

            newCard.name = cardNames[playerOneCards[i]].ToString();
            newCard.tag = playerOneCards[i].ToString();
        
            newCard.GetComponentInChildren<Text>().text = cardNames[playerOneCards[i]];
        }   
    }
}
