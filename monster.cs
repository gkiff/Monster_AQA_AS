using System;
using System.IO;
public class Monster
{
    const int NO_OF_TRAPS = 2;
    const int N_S_DISTANCE = 5;
    const int W_E_DISTANCE = 7;

    Random random = new Random();
    /*
     * The Logical class is used to allow the value of a bool variable to be returned from a subroutine.
     */
    public class Logical
    {
        public bool isit;
    }

    public class CellReference
    {
        public int noOfCellsSouth;
        public int noOfCellsEast;
    }

    public class GameData
    {
        public CellReference[] trapPositions = new CellReference[NO_OF_TRAPS + 1];
        public CellReference monsterPosition = new CellReference();
        public CellReference playerPosition = new CellReference();
        public CellReference flaskPosition = new CellReference();
        public Logical monsterAwake = new Logical();
    }

    void setUpTrapPositions(CellReference[] trapPositions)
    {
        for (int count = 1; count <= NO_OF_TRAPS; count++)
        {
            trapPositions[count] = new CellReference();
        }
    }
    void displayMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine();
        Console.WriteLine("1.  Start new game");
        Console.WriteLine("2.  Load game");
        Console.WriteLine("3.  Save game");
        Console.WriteLine("4.  Play training game");
        Console.WriteLine("9.  Quit");
        Console.WriteLine();
        Console.Write("Please enter your choice: ");
    }

    int getMainMenuChoice()
    {
        int choice;
        choice = Convert.ToInt16(Console.ReadLine());
        Console.WriteLine();
        return choice;
    }

    void resetCavern(char[,] cavern)
    {
        int count1;
        int count2;
        for (count1 = 1; count1 <= N_S_DISTANCE; count1++)
        {
            for (count2 = 1; count2 <= W_E_DISTANCE; count2++)
            {
                cavern[count1, count2] = ' ';
            }
        }
    }

    CellReference getNewRandomPosition()
    {
        CellReference position;
        position = new CellReference();
        do
        {
            position.noOfCellsSouth = random.Next(N_S_DISTANCE) + 1;
            position.noOfCellsEast = random.Next(W_E_DISTANCE) + 1;
        } while ((position.noOfCellsSouth == 1) && (position.noOfCellsEast == 1));
        /*
         * a random coordinate of (1,1) needs to be rejected as this is the starting position of the player
         */
        return position;
    }

    void setPositionOfItem(char[,] cavern, CellReference objectPosition, char item, bool newGame)
    {
        CellReference position;
        position = new CellReference();
        if (newGame && (item != '*'))
        {
            do
            {
                position = getNewRandomPosition();
            } while (cavern[position.noOfCellsSouth, position.noOfCellsEast] != ' ');
            objectPosition.noOfCellsSouth = position.noOfCellsSouth;
            objectPosition.noOfCellsEast = position.noOfCellsEast;
        }
        cavern[objectPosition.noOfCellsSouth, objectPosition.noOfCellsEast] = item;
    }

    void setUpGame(char[,] cavern, CellReference[] trapPositions, CellReference monsterPosition, CellReference playerPosition, CellReference flaskPosition, Logical monsterAwake, bool newGame)
    {
        int count;
        resetCavern(cavern);
        if (newGame)
        {
            playerPosition.noOfCellsSouth = 1;
            playerPosition.noOfCellsEast = 1;
            monsterAwake.isit = false;
        }
        for (count = 1; count <= NO_OF_TRAPS; count++)
        {
            setPositionOfItem(cavern, trapPositions[count], 'T', newGame);
        }
        setPositionOfItem(cavern, monsterPosition, 'M', newGame);
        setPositionOfItem(cavern, flaskPosition, 'F', newGame);
        setPositionOfItem(cavern, playerPosition, '*', newGame);
    }

    void setUpTrainingGame(char[,] cavern, CellReference[] trapPositions, CellReference monsterPosition, CellReference playerPosition, CellReference flaskPosition, Logical monsterAwake)
    {
        resetCavern(cavern);
        playerPosition.noOfCellsSouth = 3;
        playerPosition.noOfCellsEast = 5;
        monsterAwake.isit = false;
        trapPositions[1].noOfCellsSouth = 2;
        trapPositions[1].noOfCellsEast = 7;
        trapPositions[2].noOfCellsSouth = 4;
        trapPositions[2].noOfCellsEast = 5;
        monsterPosition.noOfCellsSouth = 1;
        monsterPosition.noOfCellsEast = 4;
        flaskPosition.noOfCellsSouth = 5;
        flaskPosition.noOfCellsEast = 6;
        setUpGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake, false);
    }

    void loadGame(CellReference[] trapPositions, CellReference
  monsterPosition, CellReference playerPosition, CellReference
  flaskPosition, Logical monsterAwake)
    {
        GameData loadedGameData;
        loadedGameData = new GameData();
        String fileName;
        Console.Write("Enter the name of the file to load: ");
        fileName = Console.ReadLine();
        Console.WriteLine();
        readGame(fileName, loadedGameData);
        trapPositions[1] = loadedGameData.trapPositions[1];
        trapPositions[2] = loadedGameData.trapPositions[2];
        monsterPosition.noOfCellsSouth = loadedGameData.monsterPosition.noOfCellsSouth;
        monsterPosition.noOfCellsEast = loadedGameData.monsterPosition.noOfCellsEast;
        playerPosition.noOfCellsSouth = loadedGameData.playerPosition.noOfCellsSouth;
        playerPosition.noOfCellsEast = loadedGameData.playerPosition.noOfCellsEast;
        flaskPosition.noOfCellsSouth = loadedGameData.flaskPosition.noOfCellsSouth;
        flaskPosition.noOfCellsEast = loadedGameData.flaskPosition.noOfCellsEast;
        monsterAwake.isit = loadedGameData.monsterAwake.isit;
    }

    void readGame(String filename, GameData loadedGameData)
    {
        try
        {
            StreamReader reader = new StreamReader(filename);
            for (int count = 1; count <= NO_OF_TRAPS; count++)
            {
                loadedGameData.trapPositions[count] = new CellReference();
                loadedGameData.trapPositions[count].noOfCellsSouth = Convert.ToInt16(reader.ReadLine());
                loadedGameData.trapPositions[count].noOfCellsEast = Convert.ToInt16(reader.ReadLine()); ;
            }
            loadedGameData.monsterPosition.noOfCellsSouth = Convert.ToInt16(reader.ReadLine());
            loadedGameData.monsterPosition.noOfCellsEast = Convert.ToInt16(reader.ReadLine());
            loadedGameData.playerPosition.noOfCellsSouth = Convert.ToInt16(reader.ReadLine());
            loadedGameData.playerPosition.noOfCellsEast = Convert.ToInt16(reader.ReadLine());
            loadedGameData.flaskPosition.noOfCellsSouth = Convert.ToInt16(reader.ReadLine());
            loadedGameData.flaskPosition.noOfCellsEast = Convert.ToInt16(reader.ReadLine());
            loadedGameData.monsterAwake.isit = (bool)Convert.ToBoolean(reader.ReadLine());
            reader.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    void saveGame(CellReference[] trapPositions, CellReference monsterPosition, CellReference playerPosition, CellReference flaskPosition, Logical monsterAwake)
    {
        GameData currentGameData;
        currentGameData = new GameData();
        String filename;
        currentGameData.trapPositions = trapPositions;
        currentGameData.monsterPosition = monsterPosition;
        currentGameData.playerPosition = playerPosition;
        currentGameData.flaskPosition = flaskPosition;
        currentGameData.monsterAwake = monsterAwake;
        Console.Write("Enter new file name: ");
        filename = Console.ReadLine();
        Console.WriteLine();
        writeGame(filename, currentGameData);
    }

    void writeGame(String filename, GameData currentGameData)
    {
        try
        {
            StreamWriter write = new StreamWriter(filename);
            for (int count = 1; count <= NO_OF_TRAPS; count++)
            {
                write.WriteLine(currentGameData.trapPositions[count].noOfCellsSouth);
                write.WriteLine(currentGameData.trapPositions[count].noOfCellsEast);
            }
            write.WriteLine(currentGameData.monsterPosition.noOfCellsSouth);
            write.WriteLine(currentGameData.monsterPosition.noOfCellsEast);
            write.WriteLine(currentGameData.playerPosition.noOfCellsSouth);
            write.WriteLine(currentGameData.playerPosition.noOfCellsEast);
            write.WriteLine(currentGameData.flaskPosition.noOfCellsSouth);
            write.WriteLine(currentGameData.flaskPosition.noOfCellsEast);
            write.WriteLine(currentGameData.monsterAwake.isit);
            write.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    void displayCavern(char[,] cavern, Logical monsterAwake)
    {
        int count1;
        int count2;
        for (count1 = 1; count1 <= N_S_DISTANCE; count1++)
        {
            Console.WriteLine(" ------------- ");
            for (count2 = 1; count2 <= W_E_DISTANCE; count2++)
            {
                if (cavern[count1, count2] == ' ' || cavern[count1, count2] == '*' || (cavern[count1, count2] == 'M' && monsterAwake.isit))
                {
                    Console.Write("|" + cavern[count1, count2]);
                }
                else
                {
                    Console.Write("| ");
                }
            }
            Console.WriteLine("|");
        }
        Console.WriteLine(" ------------- ");
        Console.WriteLine();
    }

    void displayMoveOptions()
    {
        Console.WriteLine();
        Console.WriteLine("Enter N to move NORTH");
        Console.WriteLine("Enter E to move EAST");
        Console.WriteLine("Enter S to move SOUTH");
        Console.WriteLine("Enter W to move WEST");
        Console.WriteLine("Enter M to return to the Main Menu");
        Console.WriteLine();
    }

    char getMove()
    {
        char move;
        move = Convert.ToChar(Console.ReadLine());
        Console.WriteLine();
        return move;
    }

    void makeMove(char[,] cavern, char direction, CellReference playerPosition)
    {
        cavern[playerPosition.noOfCellsSouth, playerPosition.noOfCellsEast] = ' ';
        switch (direction)
        {
            case 'N':
                playerPosition.noOfCellsSouth--;
                break;
            case 'S':
                playerPosition.noOfCellsSouth++;
                break;
            case 'W':
                playerPosition.noOfCellsEast--;
                break;
            case 'E':
                playerPosition.noOfCellsEast++;
                break;
        }
        cavern[playerPosition.noOfCellsSouth, playerPosition.noOfCellsEast] = '*';
    }

    bool checkValidMove(CellReference playerPosition, char direction)
    {
        bool validMove;
        validMove = true;
        if (!(direction == 'N' || direction == 'S' || direction == 'W'
                || direction == 'E' || direction == 'M'))
        {
            validMove = false;
        }
        return validMove;
    }

    bool checkIfSameCell(CellReference firstCellPosition, CellReference secondCellPosition)
    {
        bool inSameCell;
        inSameCell = false;
        if ((firstCellPosition.noOfCellsSouth == secondCellPosition.noOfCellsSouth)
                && (firstCellPosition.noOfCellsEast == secondCellPosition.noOfCellsEast))
        {
            {
                inSameCell = true;
            }
        }
        return inSameCell;
    }

    void displayWonGameMessage()
    {
        Console.WriteLine("Well done!  You have found the flask containing the Styxian potion.");
        Console.WriteLine("You have won the game of MONSTER!");
        Console.WriteLine();
    }

    void displayTrapMessage()
    {
        Console.WriteLine("Oh no!  You have set off a trap.  Watch out, the monster is now awake!");
        Console.WriteLine();
    }

    void moveFlask(char[,] cavern, CellReference newCellForFlask, CellReference flaskPosition)
    {
        cavern[newCellForFlask.noOfCellsSouth, newCellForFlask.noOfCellsEast] = 'F';
        cavern[flaskPosition.noOfCellsSouth, flaskPosition.noOfCellsEast] = ' ';
        flaskPosition = newCellForFlask;
    }

    void makeMonsterMove(char[,] cavern, CellReference monsterPosition, CellReference flaskPosition, CellReference playerPosition)
    {
        CellReference originalMonsterPosition;
        bool monsterMovedToSameCellAsFlask;
        originalMonsterPosition = new CellReference();
        originalMonsterPosition.noOfCellsEast = monsterPosition.noOfCellsEast;
        originalMonsterPosition.noOfCellsSouth = monsterPosition.noOfCellsSouth;
        cavern[monsterPosition.noOfCellsSouth, monsterPosition.noOfCellsEast] = ' ';
        if (monsterPosition.noOfCellsSouth < playerPosition.noOfCellsSouth)
        {
            monsterPosition.noOfCellsSouth++;
        }
        else if (monsterPosition.noOfCellsSouth > playerPosition.noOfCellsSouth)
        {
            monsterPosition.noOfCellsSouth--;
        }
        else if (monsterPosition.noOfCellsEast < playerPosition.noOfCellsEast)
        {
            monsterPosition.noOfCellsEast++;
        }
        else
        {
            monsterPosition.noOfCellsEast--;
        }
        monsterMovedToSameCellAsFlask = checkIfSameCell(monsterPosition, flaskPosition);
        if (monsterMovedToSameCellAsFlask)
        {
            moveFlask(cavern, originalMonsterPosition, flaskPosition);
        }
        cavern[monsterPosition.noOfCellsSouth, monsterPosition.noOfCellsEast] = 'M';
    }

    void displayLostGameMessage()
    {
        Console.WriteLine("ARGHHHHHH!  The monster has eaten you.  GAME OVER.");
        Console.WriteLine("Maybe you will have better luck next time you play MONSTER!");
        Console.WriteLine();
    }

    void playGame(char[,] cavern, CellReference[] trapPositions, CellReference monsterPosition, CellReference playerPosition, CellReference flaskPosition, Logical monsterAwake)
    {
        int count;
        bool eaten;
        bool flaskFound;
        char moveDirection;
        bool validMove;
        eaten = false;
        flaskFound = false;
        displayCavern(cavern, monsterAwake);
        do
        {
            do
            {
                displayMoveOptions();
                moveDirection = getMove();
                validMove = checkValidMove(playerPosition, moveDirection);
            } while (!validMove);
            if (moveDirection != 'M')
            {
                makeMove(cavern, moveDirection, playerPosition);
                displayCavern(cavern, monsterAwake);
                flaskFound = checkIfSameCell(playerPosition, flaskPosition);
                if (flaskFound)
                {
                    displayWonGameMessage();
                }
                eaten = checkIfSameCell(monsterPosition, playerPosition);
                if (!monsterAwake.isit && !flaskFound && !eaten)
                {
                    monsterAwake.isit = checkIfSameCell(playerPosition, trapPositions[1]);
                    if (!monsterAwake.isit)
                    {
                        monsterAwake.isit = checkIfSameCell(playerPosition, trapPositions[2]);
                    }
                    if (monsterAwake.isit)
                    {
                        displayTrapMessage();
                        displayCavern(cavern, monsterAwake);
                    }
                }
                if (monsterAwake.isit && !eaten && !flaskFound)
                {
                    count = 0;
                    do
                    {
                        makeMonsterMove(cavern, monsterPosition, flaskPosition, playerPosition);
                        eaten = checkIfSameCell(monsterPosition, playerPosition);
                        Console.WriteLine();
                        Console.WriteLine("Press Enter key to continue");
                        Console.ReadLine();
                        displayCavern(cavern, monsterAwake);
                        count++;
                    } while (!((count == 2) || eaten));
                }
                if (eaten)
                {
                    displayLostGameMessage();
                }
            }
        } while (!(eaten || flaskFound || moveDirection == 'M'));
    }

    public Monster()
    {
        char[,] cavern = new char[N_S_DISTANCE + 1, W_E_DISTANCE + 1];
        int choice;
        CellReference flaskPosition = new CellReference();
        Logical monsterAwake = new Logical();
        CellReference monsterPosition = new CellReference();
        CellReference playerPosition = new CellReference();
        CellReference[] trapPositions = new CellReference[NO_OF_TRAPS + 1];
        setUpTrapPositions(trapPositions);
        do
        {
            displayMenu();
            choice = getMainMenuChoice();
            switch (choice)
            {
                case 1:
                    setUpGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake, true);
                    playGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake);
                    break;
                case 2:
                    loadGame(trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake);
                    setUpGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake, false);
                    playGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake);
                    break;
                case 3:
                    saveGame(trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake);
                    break;
                case 4:
                    setUpTrainingGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake);
                    playGame(cavern, trapPositions, monsterPosition, playerPosition, flaskPosition, monsterAwake);
                    break;
            }
        } while (!(choice == 9));
    }
}