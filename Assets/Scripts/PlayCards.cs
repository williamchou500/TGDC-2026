using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayCards : MonoBehaviour
{
    public Sprite cardBack;
    private GameObject cardCount;
    private List<int> cardIDs;
    private int cardIDtoPlay;
    public GameObject cardToPlayContainer;
    private List<string> cardNames;
    private List<string> cardNameTypes;
    public GameObject cardPrefab;
    public GameObject cardPrefabTwoThirds;
    public GameObject cardPrefabHalf;
    public List<Sprite> cardSprites;
    public List<int> cardTypes;
    private List<int> discardedCards;
    private GameObject discardedCardsContainer;
    public GameObject dialogueSpeaker;
    public GameObject dialogueText;
    public GameObject friendCounts;
    private GameObject newCard;
    public GameObject pause;
    public List<int> playerOneCards;
    private List<int> playerTwoCards;
    private List<int> playerThreeCards;
    private List<int> playerFourCards;
    private GameObject playerOneHand;
    private GameObject playerTwoHand;
    private GameObject playerThreeHand;
    private GameObject playerFourHand;
    private bool resolvingTurn;
    public List<int> selectedCards;
    private GameObject selectedCardsContainer;
    public SwitchScreens sceneChanger;
    private List<int> shuffled;
    public int turn;
    public GameObject turnContainer;
    private bool turnInProgress;
    private const int NUM_PLAYERS = 4;

    void Start()
    {
        pause.SetActive(false);

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

        cardNameTypes = new List<String> {"Ace", "Two", "Three", "Four", "Five", "Six", 
                                          "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", 
                                          "King"};

        cardTypes = new List<int> {0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 
                                   7, 7, 7, 7, 8, 8, 8, 8, 9, 9, 9, 9, 10, 10, 10, 10, 11, 11, 11, 11, 12, 12, 12, 12};

        cardCount = GameObject.FindWithTag("Card Count");

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
        if (turn % 13 == 5)
        {
            cardToPlayContainer.GetComponent<Text>().text = $"The Desired Cards: {cardNameTypes[turn % 13]}es";
        } else
        {
            cardToPlayContainer.GetComponent<Text>().text = $"The Desired Cards: {cardNameTypes[turn % 13]}";
        }
        
        turnContainer.GetComponent<Text>().text = $"Round {turn + 1} / Your Turn";

        LoadCards();
    }

    void Update()
    {   
        if (turn % 4 == 0 && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space pressed");
            Player1PlayCards();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && !pause.activeInHierarchy)
        {
            pause.SetActive(true);
        } else if (Keyboard.current.escapeKey.wasPressedThisFrame && pause.activeInHierarchy)
        {
            pause.SetActive(false);
        }

        // if it isn't the player's turn
        if (!turnInProgress && turn % NUM_PLAYERS != 0) {
            Invoke(nameof(takeNPCTurn), 0f);
        }
    }

    public void Pause()
    {
        if (!pause.activeInHierarchy)
        {
            pause.SetActive(true);
        } else
        {
            pause.SetActive(false);
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
        resolvingTurn = false;
        turnInProgress = false;

        for (int i = playerOneHand.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(playerOneHand.transform.GetChild(i).gameObject);
        }

        for (int i = selectedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(selectedCardsContainer.transform.GetChild(i).gameObject);
        }

        selectedCards = new List<int> {};
        cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

        for (int i = discardedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(discardedCardsContainer.transform.GetChild(i).gameObject);
        }

        discardedCards = new List<int> {};

        turn = 0;
        cardIDtoPlay = 0;

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

        // playerTwoHand.GetComponent<Text>().text = string.Join(",",playerTwoCards);
        // playerThreeHand.GetComponent<Text>().text = string.Join(",",playerThreeCards);
        // playerFourHand.GetComponent<Text>().text = string.Join(",",playerFourCards);


        for (int i = 0; i < playerOneCards.Count(); i++)
        {
            if (playerOneCards.Count() > 39)
            {
                newCard = Instantiate(cardPrefabHalf, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80/4,112/4);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(75,10);

            } else if (playerOneCards.Count() > 26)
            {
                newCard = Instantiate(cardPrefabTwoThirds, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80/3,112/3);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(68,10);
            } else if (playerOneCards.Count() > 13)
            {
                newCard = Instantiate(cardPrefabTwoThirds, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(40,56);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(54,10);
            }
            else
            {
                newCard = Instantiate(cardPrefab, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80,112);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(10,10);
            }

            newCard.name = cardNames[playerOneCards[i]].ToString();
            newCard.tag = playerOneCards[i].ToString();
        
            newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardSprites[playerOneCards[i]];
        }
    }

    public bool CallBull(bool PlayerOneCalled = false)
    {
        // this just checks if the discard tiles are bull or not
        // we can probably make this return true or false and the use the turn number to find out who to punish
        if (PlayerOneCalled)
        {
            dialogueSpeaker.GetComponent<Text>().text = "You";
            dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
        }

        for (int i = discardedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            newCard = discardedCardsContainer.transform.GetChild(i).gameObject;
            newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardSprites[int.Parse(newCard.tag)];
        }

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
                    Debug.Log("Player 1 right");
                    ClearCards();
                    Punish(turn % NUM_PLAYERS);
                    Invoke(nameof(NextTurn), 1f);
                    return true;
                }
                Debug.Log("BULLLLLLLLLLL");
                return true;

            }
        }

        if (PlayerOneCalled)
        {
            Debug.Log("Player 1 wrong");
            ClearCards();

            Punish(0);   
            Invoke(nameof(NextTurn), 1f);
        }

        Debug.Log("not bull");
        return false;

    }

    public void CallBullButton()
    {
        if (!turnInProgress || turn % NUM_PLAYERS == 0) return;
        resolvingTurn = true;
        CallBull(true);
    }

    public void PassButton()
    {
        if (!turnInProgress || turn % NUM_PLAYERS == 0) return;
        resolvingTurn = true;

        if (turn % NUM_PLAYERS == 1)
        {
            if (MakeDecision(2))
            {
                dialogueSpeaker.GetComponent<Text>().text = "!^)^&!#)# (Friend 2)";
                dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
                if (CallBull())
                {
                    Debug.Log("PLAYER 3 CALLED BS AND WAS RiGHT");
                    Punish(1);
                } else
                {
                    Debug.Log("PLAYER 3 CALLED BS AND WAS WRONG");
                    Punish(2);
                }
            } else if (MakeDecision(3))
            {
                dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 3)";
                dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
                if (CallBull())
                {
                    Debug.Log("PLAYER 4 CALLED BS AND WAS RiGHT");
                    Punish(1);
                } else
                {
                    Debug.Log("PLAYER 4 CALLED BS AND WAS WRONG");
                    Punish(3);
                }
            }
        } else if (turn % NUM_PLAYERS == 2)
        {
            if (MakeDecision(3))
            {
                dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 3)";
                dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
                if (CallBull())
                {
                    Debug.Log("PLAYER 2 CALLED BS AND WAS RiGHT");
                    Punish(2);
                } else
                {
                    Debug.Log("PLAYER 2 CALLED BS AND WAS WRONG");
                    Punish(3);
                }
            } else if (MakeDecision(1))
            {
                dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 1)";
                dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
                if (CallBull())
                {
                    Debug.Log("PLAYER 4 CALLED BS AND WAS RiGHT");
                    Punish(2);
                } else
                {
                    Debug.Log("PLAYER 4 CALLED BS AND WAS WRONG");
                    Punish(1);
                }
            }
        } else if (turn % NUM_PLAYERS == 3)
        {
            if (MakeDecision(1))
            {
                dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 1)";
                dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
                if (CallBull())
                {
                    Debug.Log("PLAYER 2 CALLED BS AND WAS RiGHT");
                    Punish(3);
                } else
                {
                    Debug.Log("PLAYER 2 CALLED BS AND WAS WRONG");
                    Punish(1);
                }
            } else if (MakeDecision(2))
            {
                dialogueSpeaker.GetComponent<Text>().text = "!^)^&!#)# (Friend 2)";
                dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
                if (CallBull())
                {
                    Debug.Log("PLAYER 3 CALLED BS AND WAS RiGHT");
                    Punish(3);
                } else
                {
                    Debug.Log("PLAYER 3 CALLED BS AND WAS WRONG");
                    Punish(2);
                }
            }
        }

        UpdateFriendCounts();

        ClearCards();

        Invoke(nameof(NextTurn), 1f);
    }

    public bool MakeDecision(int player)
    {
        // output of true means they call bull and false means they don't
        // if (MakeDecision(1)) {if CallBull{ Punishment(0) }}
        // add the decisionmaking and probability stuff
        if (player == 1)
        {
            int numOfType = 0;

            for (int i = 0; i < playerTwoCards.Count(); i++) {
                if (cardTypes[playerTwoCards[i]] == cardIDtoPlay)
                {
                    numOfType++;
                }
            }

            if (numOfType + selectedCards.Count() > 4)
            {
                return true;
            } else
            {
                return false;
            }
        } else if (player == 2)
        {
            int numOfType = 0;

            for (int i = 0; i < playerThreeCards.Count(); i++) {
                if (cardTypes[playerThreeCards[i]] == cardIDtoPlay)
                {
                    numOfType++;
                }
            }

            if (numOfType + selectedCards.Count() > 4)
            {
                return true;
            } else if (numOfType + selectedCards.Count() > 3)
            {
                if (UnityEngine.Random.value < 0.8)
                {
                    return true;
                }
            } else if (numOfType + selectedCards.Count() > 2)
            {
                if (UnityEngine.Random.value < 0.6)
                {
                    return true;
                }
            } else if (numOfType + selectedCards.Count() > 1)
            {
                if (UnityEngine.Random.value < 0.3)
                {
                    return true;
                }
            } else
            {
                if (UnityEngine.Random.value < 0.1)
                {
                    return true;
                }
            }
        } else if (player == 3)
        {
            int numOfType = 0;

            for (int i = 0; i < playerFourCards.Count(); i++) {
                if (cardTypes[playerFourCards[i]] == cardIDtoPlay)
                {
                    numOfType++;
                }
            }

            if (numOfType + selectedCards.Count() > 4)
            {
                return true;
            } 

            if (UnityEngine.Random.value < 0.5)
            {
                if (numOfType + selectedCards.Count() > 3)
                {
                    if (UnityEngine.Random.value < 0.8)
                    {
                        return true;
                    }
                } else if (numOfType + selectedCards.Count() > 2)
                {
                    if (UnityEngine.Random.value < 0.6)
                    {
                        return true;
                    }
                } else if (numOfType + selectedCards.Count() > 1)
                {
                    if (UnityEngine.Random.value < 0.3)
                    {
                        return true;
                    }
                } else
                {
                    if (UnityEngine.Random.value < 0.1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void SelectRandomCard(List<int> playerNumberCards)
    {
        int randomIndex = UnityEngine.Random.Range(0, playerNumberCards.Count);
        discardedCards.Add(playerNumberCards[randomIndex]);
        selectedCards.Add(playerNumberCards[randomIndex]);
        playerNumberCards.Remove(playerNumberCards[randomIndex]);
    }

    public void PlayRandomCard(List<int> playerNumberCards)
    {
        SelectRandomCard(playerNumberCards);

        newCard = Instantiate(cardPrefab, discardedCardsContainer.transform);
        newCard.name = cardNames[discardedCards[^1]].ToString();
        newCard.tag = discardedCards[^1].ToString();     

        newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardBack;
    }

    public void Player1PlayCards()
    {
        if (turn % NUM_PLAYERS != 0 || resolvingTurn) return;
        resolvingTurn = true;

        Debug.Log("Player1PlayCards called. Turn = " + turn);
        Debug.Log("Turn modulo = " + turn % NUM_PLAYERS);

        if (turn % NUM_PLAYERS != 0)
        {
            Debug.Log("NOT YOUR TURN");
            return;
        }

        // Start the coroutine to handle the sequence with pauses
        StartCoroutine(Player1PlayCardsRoutine());
    }

    private IEnumerator Player1PlayCardsRoutine()
    {
        dialogueSpeaker.GetComponent<Text>().text = "You";

        if (selectedCards.Count == 1)
        {
            dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}!";   
        } else if (selectedCards.Count > 1 && turn % 13 == 5) 
        {
            dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}es!";   
        } else if (selectedCards.Count > 1)
        {
            dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}s!";   
        }

        for (int i = selectedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            selectedCardsContainer.transform.GetChild(i).gameObject.GetComponent<UnityEngine.UI.Image>().sprite = cardBack;
            selectedCardsContainer.transform.GetChild(i).SetParent(discardedCardsContainer.transform, false);
        }

        cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

        // Pauses execution here for 2 seconds without freezing the game
        yield return new WaitForSeconds(2f);

        discardedCards.AddRange(selectedCards);

        if (MakeDecision(1))
        {
            dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 1)";
            dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
            if (CallBull())
            {
                Debug.Log("PLAYER 2 CALLED BS AND WAS RiGHT");
                Punish(0);
            }
            else
            {
                Debug.Log("PLAYER 2 CALLED BS AND WAS WRONG");
                Punish(1);
            }
        }
        else if (MakeDecision(2))
        {
            dialogueSpeaker.GetComponent<Text>().text = "!^)^&!#)# (Friend 2)";
            dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
            if (CallBull())
            {
                Debug.Log("PLAYER 3 CALLED BS AND WAS RiGHT");
                Punish(0);
            }
            else
            {
                Debug.Log("PLAYER 3 CALLED BS AND WAS WRONG");
                Punish(2);
            }
        }
        else if (MakeDecision(3))
        {
            dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 3)";
            dialogueText.GetComponent<Text>().text = $"BULLSHIT!"; 
            if (CallBull())
            {
                Debug.Log("PLAYER 4 CALLED BS AND WAS RiGHT");
                Punish(0);
            }
            else
            {
                Debug.Log("PLAYER 4 CALLED BS AND WAS WRONG");
                Punish(3);
            }
        }

        ClearCards();

        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    // Timid Character
    public void Player2PlayCards()
    {
        if (!turnInProgress)
        {
            ClearCards();

            discardedCards.AddRange(selectedCards);
            selectedCards = new List<int> {};
            cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

            Debug.Log("Char 1 turn");

            List<int> validDiscards = new List<int> {};

            for (int i = playerTwoCards.Count() - 1; i >= 0; i--)
            {
                if (cardTypes[playerTwoCards[i]] == cardIDtoPlay)
                {
                    validDiscards.Add(playerTwoCards[i]);
                    playerTwoCards.Remove(playerTwoCards[i]);
                }
            }

            if (validDiscards.Count() > 0)
            {
                discardedCards.AddRange(validDiscards);
                selectedCards.AddRange(validDiscards);

                for (int i = 0; i < validDiscards.Count(); i++)
                {
                    newCard = Instantiate(cardPrefab, discardedCardsContainer.transform);
                    newCard.name = cardNames[validDiscards[i]].ToString();
                    newCard.tag = validDiscards[i].ToString();       

                    newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardBack;
                }        
            } else
            {
                PlayRandomCard(playerTwoCards);
            }
            
            UpdateFriendCounts();

            dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 1)";

            if (selectedCards.Count == 1)
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}!";   
            } else if (selectedCards.Count > 1 && turn % 13 == 5) 
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}es!";   
            } else if (selectedCards.Count > 1)
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}s!";   
            }

            turnInProgress = true;
        }
    }

    // Confident Character
    public void Player3PlayCards()
    {
        if (!turnInProgress)
        {
            ClearCards();

            discardedCards.AddRange(selectedCards);
            selectedCards = new List<int> {};
            cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

            Debug.Log("Char 2 turn");   

            List<int> validDiscards = new List<int> {};

            for (int i = playerThreeCards.Count() - 1; i >= 0; i--)
            {
                if (cardTypes[playerThreeCards[i]] == cardIDtoPlay)
                {
                    validDiscards.Add(playerThreeCards[i]);
                }
            }

            if (validDiscards.Count() > 0)
            {
                double willBS = UnityEngine.Random.value;
                // 80% chance of playing valid cards
                if (willBS < 0.8)
                {
                    discardedCards.AddRange(validDiscards);
                    selectedCards.AddRange(validDiscards);
                    foreach (int card in validDiscards)
                    {
                        playerThreeCards.Remove(card);
                    }

                    // 5% of playing 4 cards
                    double willAddAnotherCard = UnityEngine.Random.value;
                    if (willAddAnotherCard < 0.05)
                    {
                        for (int i = validDiscards.Count; i <= 4; i++)
                        {
                            SelectRandomCard(playerThreeCards);
                        }
                    }
                    // 10% chance of playing 3 cards
                    else if (willAddAnotherCard >= 0.05 & willAddAnotherCard < 0.15)
                    {
                        for (int i = validDiscards.Count; i <= 3; i++)
                        {
                            SelectRandomCard(playerThreeCards);
                        }
                    }
                    // 20% chance of playing 2 cards
                    else if (willAddAnotherCard >= 0.15 & willAddAnotherCard < 0.35)
                    {
                        for (int i = validDiscards.Count; i <= 2; i++)
                        {
                            SelectRandomCard(playerThreeCards);
                        }
                    }
                    for (int i = 0; i < selectedCards.Count(); i++)
                    {
                        newCard = Instantiate(cardPrefab, discardedCardsContainer.transform);
                        newCard.name = cardNames[selectedCards[i]].ToString();
                        newCard.tag = selectedCards[i].ToString();       

                        newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardBack;
                    }
                } 
                else
                {
                    PlayRandomCard(playerThreeCards);
                }
            } 
            // 20% chance of BS
            else
            {
                PlayRandomCard(playerThreeCards);
            }
            
            UpdateFriendCounts();

            dialogueSpeaker.GetComponent<Text>().text = "!^)^&!#)# (Friend 2)";

            if (selectedCards.Count == 1)
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}!";   
            } else if (selectedCards.Count > 1 && turn % 13 == 5) 
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}es!";   
            } else if (selectedCards.Count > 1)
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}s!";   
            }

            turnInProgress = true;
        }

    }

    // Mysterious Character
    public void Player4PlayCards()
    {
        if (!turnInProgress)
        {
            ClearCards();

            discardedCards.AddRange(selectedCards);
            selectedCards = new List<int> {};
            cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

            Debug.Log("Char 3 turn");   
            
            List<int> validDiscards = new List<int> {};

            for (int i = playerFourCards.Count() - 1; i >= 0; i--)
            {
                if (cardTypes[playerFourCards[i]] == cardIDtoPlay)
                {
                    validDiscards.Add(playerFourCards[i]);
                    playerFourCards.Remove(playerFourCards[i]);
                }
            }

            if (UnityEngine.Random.value < 0.5)
            {
                if (validDiscards.Count() > 0)
                {
                    discardedCards.AddRange(validDiscards);
                    selectedCards.AddRange(validDiscards);

                    for (int i = 0; i < validDiscards.Count(); i++)
                    {
                        newCard = Instantiate(cardPrefab, discardedCardsContainer.transform);
                        newCard.name = cardNames[validDiscards[i]].ToString();
                        newCard.tag = validDiscards[i].ToString();       

                        newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardBack;
                    }        
                } else
                {
                    PlayRandomCard(playerFourCards);
                }
                } else
                {
                    if (validDiscards.Count() > 0)
                    {
                        double willBS = UnityEngine.Random.value;
                        // 80% chance of playing valid cards
                        if (willBS < 0.8)
                        {
                            discardedCards.AddRange(validDiscards);
                            selectedCards.AddRange(validDiscards);
                            foreach (int card in validDiscards)
                            {
                                playerThreeCards.Remove(card);
                            }

                            // 5% of playing 4 cards
                            double willAddAnotherCard = UnityEngine.Random.value;
                            if (willAddAnotherCard < 0.05)
                            {
                                for (int i = validDiscards.Count; i <= 4; i++)
                                {
                                    SelectRandomCard(playerThreeCards);
                                }
                            }
                            // 10% chance of playing 3 cards
                            else if (willAddAnotherCard >= 0.05 & willAddAnotherCard < 0.15)
                            {
                                for (int i = validDiscards.Count; i <= 3; i++)
                                {
                                    SelectRandomCard(playerThreeCards);
                                }
                            }
                            // 20% chance of playing 2 cards
                            else if (willAddAnotherCard >= 0.15 & willAddAnotherCard < 0.35)
                            {
                                for (int i = validDiscards.Count; i <= 2; i++)
                                {
                                    SelectRandomCard(playerThreeCards);
                                }
                            }
                            for (int i = 0; i < selectedCards.Count(); i++)
                            {
                                newCard = Instantiate(cardPrefab, discardedCardsContainer.transform);
                                newCard.name = cardNames[selectedCards[i]].ToString();
                                newCard.tag = selectedCards[i].ToString();       

                                newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardBack;
                            }
                        } 
                        else
                        {
                            PlayRandomCard(playerThreeCards);
                        }
                    } 
                    // 20% chance of BS
                    else
                    {
                        PlayRandomCard(playerThreeCards);
                    }
            }

            UpdateFriendCounts();

            dialogueSpeaker.GetComponent<Text>().text = "%^&@#* (Friend 3)";

            if (selectedCards.Count == 1)
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}!";   
            } else if (selectedCards.Count > 1 && turn % 13 == 5) 
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}es!";   
            } else if (selectedCards.Count > 1)
            {
                dialogueText.GetComponent<Text>().text = $"{selectedCards.Count} {cardNameTypes[turn % 13]}s!";   
            }

            turnInProgress = true;
        }

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
    }

    public void NextTurn()
    {
        resolvingTurn = true;
        ClearCards();

        selectedCards = new List<int> {};
        cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

        if (turn % NUM_PLAYERS != 3) 
        {
            Debug.Log("A");
            turnInProgress = false;
        } else if (turn % NUM_PLAYERS == 3)
        {
            turnInProgress = true;
            Debug.Log("Player 1 Turn");

            dialogueSpeaker.GetComponent<Text>().text = "You";
            dialogueText.GetComponent<Text>().text = $"What should I play?";  
        }

        if (turn % NUM_PLAYERS == 0 & playerOneCards.Count() == 0)
        {
            Debug.Log("Player 1 WINS");
            sceneChanger.WinningEndScreen();
            return;
        }

        if (turn % NUM_PLAYERS == 1 & playerTwoCards.Count() == 0)
        {
            Debug.Log("Player 2 WINS");
            sceneChanger.LosingEndScreen();
            return;
        }

        if (turn % NUM_PLAYERS == 2 & playerThreeCards.Count() == 0)
        {
            Debug.Log("Player 3 WINS");
            sceneChanger.LosingEndScreen();
            return;
        }

        if (turn % NUM_PLAYERS == 3 & playerFourCards.Count() == 0)
        {
            Debug.Log("Player 4 WINS");
            sceneChanger.LosingEndScreen();
            return;
        }
        
        turn++;
        cardIDtoPlay = turn % 13;
        cardToPlayContainer.GetComponent<Text>().text = $"The Desired Cards: {cardNameTypes[turn % 13]}";

        if (turn % NUM_PLAYERS == 0)
        {
            turnContainer.GetComponent<Text>().text = $"Round {turn + 1} / Your Turn";
        } else if (turn % NUM_PLAYERS == 1)
        {
            turnContainer.GetComponent<Text>().text = $"Round {turn + 1} / %^&@#* (Friend 1)'s Turn";
        } else if (turn % NUM_PLAYERS == 2)
        {
            turnContainer.GetComponent<Text>().text = $"Round {turn + 1} / !^)^&!#)# (Friend 2)'s Turn";
        } else if (turn % NUM_PLAYERS == 3)
        {
            turnContainer.GetComponent<Text>().text = $"Round {turn + 1} / %^&@#* (Friend 3)'s Turn";
        }

        UpdateFriendCounts();
    }

    public void Punish(int player)
    {
        if (player == 0)
        {
            Debug.Log("Punish 0");
            playerOneCards.AddRange(discardedCards);
            discardedCards = new List<int> {};
            playerOneCards.Sort();

            UpdatePlayerHand();
        } else if (player == 1)
        {
            playerTwoCards.AddRange(discardedCards);
            discardedCards = new List<int> {};
            playerTwoCards.Sort();

            // playerTwoHand.GetComponent<Text>().text = string.Join(",", playerTwoCards);
        } else if (player == 2)
        {
            playerThreeCards.AddRange(discardedCards);
            discardedCards = new List<int> {};
            playerThreeCards.Sort();

            // playerThreeHand.GetComponent<Text>().text = string.Join(",", playerThreeCards);
        } else if (player == 3)
        {
            playerFourCards.AddRange(discardedCards);
            discardedCards = new List<int> {};
            playerFourCards.Sort();

            // playerFourHand.GetComponent<Text>().text = string.Join(",", playerFourCards);
        }
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
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80/4,112/4);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(75,10);

            } else if (playerOneCards.Count() > 26)
            {
                newCard = Instantiate(cardPrefabTwoThirds, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80/3,112/3);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(68,10);
            } else if (playerOneCards.Count() > 13)
            {
                newCard = Instantiate(cardPrefabTwoThirds, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(40,56);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(54,10);
            }
            else
            {
                newCard = Instantiate(cardPrefab, playerOneHand.transform);
                playerOneHand.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80,112);
                playerOneHand.GetComponent<GridLayoutGroup>().spacing = new Vector2(10,10);
            }

            newCard.name = cardNames[playerOneCards[i]].ToString();
            newCard.tag = playerOneCards[i].ToString();
        
            newCard.GetComponent<UnityEngine.UI.Image>().sprite = cardSprites[playerOneCards[i]];
        }   

        // playerTwoHand.GetComponent<Text>().text = string.Join(",", playerTwoCards);
        // playerThreeHand.GetComponent<Text>().text = string.Join(",", playerThreeCards);
        // playerFourHand.GetComponent<Text>().text = string.Join(",", playerFourCards);
    }

    public void ClearCards()
    {
        for (int i = selectedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(selectedCardsContainer.transform.GetChild(i).gameObject);
        }

        selectedCards = new List<int> {};
        cardCount.GetComponent<Text>().text = $"{selectedCards.Count} CARDS SELECTED";

        for (int i = discardedCardsContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(discardedCardsContainer.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateFriendCounts()
    {
        friendCounts.GetComponent<Text>().text = $"• <b>Friend 1</b> has <b>{playerTwoCards.Count}</b> card(s) remaining.\n\n• <b>Friend 2</b> has <b>{playerThreeCards.Count}</b> card(s) remaining.\n\n• <b>Friend 3</b> has <b>{playerFourCards.Count}</b> card(s) remaining.";
    }
}
