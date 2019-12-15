using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Reader3 : MonoBehaviour
{

    public GameObject Tabletop;

    //public GameObject gameController;
    public Level3_GameController gameController;

    public GameObject[] cards = null;
    public System.Collections.Generic.List<GameObject> cardList;  // equal to cards, contains all cards
    public List<GameObject> cardsOnTableTop;  // cards that are on tabletop

    public List<string> answer;  // answer is set upon Play based on gameController

    public bool answerCorrect;  //answer correctness
    public List<int> wrongStep;  // the steps that are wrong
    
    /* When the Play button is clicked, check the order of the cards in the table top */

    // Start is called before the first frame update
    void Start()
    {

        //gameController = gameController2;
        
        Tabletop = GameObject.Find("Tabletop");

        //cards = GameObject.FindGameObjectsWithTag("Card");
        //cardList = new System.Collections.Generic.List<GameObject>(cards);  // same as cards
        //cardsOnTableTop = new List<GameObject>();

        wrongStep = new List<int>();

    }

    /* Find all of the cards game object using the tag, then check if their order matches their names */
    public void Play()
    {        
        cards = GameObject.FindGameObjectsWithTag("Card");
        cardList = new System.Collections.Generic.List<GameObject>(cards);  // same as cards

        if (gameController.task_Counter == 1)
        {
            answer = gameController.task1_answer;
        }
        else if (gameController.task_Counter == 2)
        {
            answer = gameController.task2_answer;
        }
        else if (gameController.task_Counter == 3)
        {
            answer = gameController.task3_answer;
        }
        else if (gameController.task_Counter == 4)
        {
            answer = gameController.task4_answer;
        }
        // Add cards to a new empty cardsOnTableTop each time, so there are no duplicates due to the cards added before
        // So when a card added before but removed later will disappear
        // --------> Previously added card that later removed will not appear
        cardsOnTableTop = new List<GameObject>();

        // reset the answer correctness and wrong steps so that every time only the correctness
        // and wrong steps of this time will be saved
        answerCorrect = false;  //set to false so that the character won't move with no correct answers
        wrongStep = new List<int>();

        // 1. Make sure the tabletop isn't empty
        if (Tabletop.transform.childCount > 0)
        {
            Debug.Log("Not Empty!");
        }


        // 2. Find all objects with the tag "Card" AND make sure they are children of tabletop
        for(int i = 0; i < cardList.Count ; i++)
        {
            // if the card isn't on tabletop
            if (cardList[i].transform.parent.gameObject.name != "Tabletop")
            {
                Debug.Log(cardList[i].name + " NOT ON TABLETOP");
            }
            // if the card is on tabletop, add to cardsOnTableTop
            else
            {
                //cardsOnTableTop.Insert(cardsOnTableTop.Count, cardList[i]);
                cardsOnTableTop.Add(cardList[i]);
            }
        }

        // Update cards every time
        if(cardsOnTableTop != null)
        {
            cards = cardsOnTableTop.ToArray();           
        }



        // 3. Order the cards in the list based on x position from the left to the right
        cardsOnTableTop = cardsOnTableTop.OrderBy(card=>card.transform.position.x).ToList();
    


        // 4. Check if the sequence of the cards names match the answer --> This way

            // True
                // Step Bar Moves ( Simulator.cs )

                // Meanwhile Game Plays By Steps ( Game Controller.cs )

            // False
                // Indicator: Show Nothing / Mark the Wrong Steps (ADVANCED FEATURE)
        
        for (int i = 0; i < answer.Count; i++)
        {
            // cardsOnTableTop size is 0 at the beginning, no comparing answer in that case
            if(cardsOnTableTop.Count != 0)
            {
                if (answer[i] == cardsOnTableTop[i].name)
                {
                    Debug.Log("Step: " + (i + 1) + " is Correct");
                    answerCorrect = true;
                }
                // at least one step in the answer is wrong
                else
                {
                    Debug.Log("Step: " + (i + 1) + " is not correct!");
                    answerCorrect = false;
                    wrongStep.Add(i+1);
                }
            }            
        }

        // If there are more cards on table top than the answer, even if the first several cards
        // are in the correct sequence, if more cards are put on table, it will be wrong
        //Debug.Log("Answer: " + answer.Count + " CardsOnTableTop: " + cardsOnTableTop.Count);
        if (answer.Count != cardsOnTableTop.Count)
        {
            answerCorrect = false;
        }

        // Indicator of incorrect steps
        // Need to reset the indicator every time, add reset code above at the beginning of play

        // notify gameController that answer is correct or not
        gameController.taskComplete(answerCorrect);

    }
}
