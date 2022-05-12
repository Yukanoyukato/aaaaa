using System;
using SFML.Learning;
using SFML.System;
using SFML.Window;

namespace Sravni_Kartochki
{
     class Sravni_Kartochki : Game
    {

        static string opencardSound = LoadSound("Poppo_0_3.wav"); 
        static string misstake = LoadSound("Poppo_3_0.wav");
        static string cardisopen = LoadSound("Poppo_3_1.wav");
        static string losegame = LoadSound("Poppo_19_3.wav");
        static string wingame = LoadSound("Poppo_28_0.wav");
        static string[] iconsNames;
        static int[,] cards;
        static int cardCount = 18;
        static int cardWidth = 140;
        static int CardHeight = 190;
        static int countPerLine = 6;
        static int space = 50;
        static int leftOffset = 50;
        static int topOffset = 50;

        static void LoadIcons()
        {
            iconsNames = new string[7];
            iconsNames[0] = LoadTexture("Icon_back.png");
            for (int i = 1; i < iconsNames.Length; i++)
            {
                iconsNames[i] = LoadTexture("Icon_"+ (i).ToString() +".png");
            }

        }



        static void Shuffle(int[] arr)
        {
            Random random = new Random();
            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = random.Next(1, i + 1);

                int tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }

        static void InitCard ()
        {
            Random rnd = new Random();
            cards = new int[cardCount, 6];
            int[] iconId = new int [cards.GetLength(0)];
            int id = 0;

            for (int i = 0; i < iconId.Length; i++)
            {
                if (i % 2 == 0)
                {
                    id = rnd.Next(1, 7);
                }
                iconId[i] = id;
            }

            Shuffle(iconId);
            Shuffle(iconId);
            Shuffle(iconId);
            Shuffle(iconId);

            for (int i = 0; i < cards.GetLength(0); i++)
            {
                cards[i, 0] = 0; 
                cards[i, 1] = (i % countPerLine) * (cardWidth + space) + leftOffset; // posX
                cards[i, 2] = (i / countPerLine) * (CardHeight + space) + topOffset; // posY
                cards[i, 3] = cardWidth;  // width
                cards[i, 4] = CardHeight; // height
                cards[i, 5] = iconId[i]; // id

            }
        }

        static void SetStateToAllCards(int state)
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                cards[i, 0] = state;
            }
        }

            static void DrawCards()
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                if (cards[i, 0] == 1) // open
                {
                    DrawSprite(iconsNames[cards[i, 5]], cards[i, 1], cards[i, 2]);

       

                }
                if (cards[i, 0] == 0) // close
                {
                    DrawSprite(iconsNames[0], cards[i, 1], cards[i, 2]);
                }

                
            }
        }

        static int GetMousePositonIndex()
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                if (MouseX >= cards[i, 1] && MouseX <= cards[i,1] + cards[i,3] && MouseY >= cards[1,2] && MouseY <= cards[i,2] + cards[i,4])    
                {
                    return i;
                    
                }
            }
            return -1;
        }

        
        static void Main(string[] args)
        {
            
    

            int openCardAmount = 0;
            int firstOpenCardIndex = -1; 
            int secondOpenCardIndex = -1;
            int remainingCard = cardCount;
            int HP = 4;
            LoadIcons();
            SetFont("arial.ttf");
            InitWindow(1280, 1024, "Find Couple");
            while (true)
            {
                
                ClearWindow(26, 46, 92);
                DrawText(400, 200, "Hi! chose difficult", 44);
                DrawText(400, 300, "Easy mode: press '1'", 44);
                DrawText(400, 400, "Normal mode: press '2'", 44);
                DrawText(400, 500, "Hard mode: press '3'", 44);
                DrawText(400, 600, "???? '4'", 44);
                DisplayWindow();
                DispatchEvents();
                if (GetKeyDown(Keyboard.Key.Num1))
                {
                    HP = 10;
                    break;
                }
                if (GetKeyDown(Keyboard.Key.Num2))
                {
                    HP = 5;
                    break;
                }
                if (GetKeyDown(Keyboard.Key.Num3))
                {
                    HP = 3;
                    break;
                }
                if (GetKeyDown(Keyboard.Key.Num4))
                {
                    HP = 1;
                    break;
                }

            }

            InitCard();



            SetStateToAllCards(1);
            ClearWindow(26, 46, 92);
            DrawCards();
            DisplayWindow();

            Delay(2500);

            SetStateToAllCards(0);

            while (true)
            {
                DispatchEvents();
             
                
                if (HP == 0) // Lose Game
                {
                    
                    ClearWindow();
                    PlaySound(losegame);
                    SetFillColor(255, 255, 255);
                    DrawText(550, 550, "You lose!", 64);

                    DisplayWindow();
                    break;
                }

                if (remainingCard == 0) // Win Game
                {
                   
                    ClearWindow();
                    PlaySound(wingame);
                    SetFillColor(255, 255, 255);
                    DrawText(550, 550, "You won!", 64);

                    DisplayWindow();
                    break;
                }

                if (openCardAmount == 2)
                {
                    if (cards[firstOpenCardIndex, 5] == cards[secondOpenCardIndex, 5])
                    {
                        cards[firstOpenCardIndex, 0] = -1;
                        cards[secondOpenCardIndex, 0] = -1;
                        PlaySound(cardisopen,40);
                        remainingCard -= 2;
                    }
                    else
                    {
                        cards[firstOpenCardIndex, 0] = 0;
                        cards[secondOpenCardIndex, 0] = 0;
                        PlaySound(misstake,40);
                        HP--;
                    }
                    firstOpenCardIndex = -1;
                    secondOpenCardIndex = -1;
                    openCardAmount = 0;
                    Delay(1000);
                }

                if (GetMouseButtonDown(0) == true)
                {
                    int index = GetMousePositonIndex();
                   
                    if (index != -1 && index != firstOpenCardIndex)
                    {
                        cards[index, 0] = 1;
                        PlaySound(opencardSound,40);
                        openCardAmount++;

                        if (openCardAmount == 1) firstOpenCardIndex = index;
                        if (openCardAmount == 2) secondOpenCardIndex = index;

                    }
                }
                ClearWindow(26,46,92);
                DrawCards();
                SetFillColor(255, 255, 255);
                DrawText(45, 0, "HP - " + HP, 44);
                DisplayWindow();


                Delay(1);
            }



            Delay(4000);
        }

       
    }
}
