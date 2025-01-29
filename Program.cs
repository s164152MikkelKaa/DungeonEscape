namespace DungeonEscape
{
    internal class Program
    {
        // Assignment: "Dungeon Escape".
        // Author: Mikkel Kaa.

        // Declaring the chars used to display the players soroundings, using constants as specified in the assignment text.
        const char WallSquare = 'M'; // DK: "Mur".
        const char ExitSquare = 'U'; // DK: "Udgang".
        const char EmptySquare = ' '; // Universal: |empty|.
        const char KeySquare = 'N'; // DK: "Nøgle".
        const char TrapSquare = 'F'; // DK: "Fælde".
        const char ThePlayer = 'S'; // DK: "Spiller".
        // An array for ease of refering to the chars (arrays can't be constants as they are objects).
        private static readonly char[] DisplayChar = [WallSquare, ExitSquare, EmptySquare, KeySquare, TrapSquare];

        // The main dungeon array, layout of the dungeon, the value at [y][x] is the index for 'DisplayChar'.
        // Play area is 10 tall and 15 wide not including the border wall, the exit is in the border wall.
        // [Array index y = 0-11] [Play area y = 1-10]   [Array index x = 0-16] [Play area x = 1-15], exit at [2][16] ends the game.
        /* The assignment of '[][]' instead of '[,]' makes it a jagged 2D array,
         * by this the shape of the dungeon don't need to be square or rectangle (future proofing and/or point of expansion). */
        private static readonly int[][] DungeonLayout = [
            [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,0,2,2,2,0],
            [0,0,0,0,0,0,0,0,0,0,0,2,2,2,4,2,1],
            [0,2,4,2,2,2,2,2,2,2,0,2,0,2,2,2,0],
            [0,2,3,4,2,0,2,0,2,4,0,2,0,0,0,0,0],
            [0,2,4,4,2,0,2,0,2,2,2,2,2,4,4,2,0],
            [0,2,2,2,2,0,2,0,0,0,0,0,2,2,2,2,0],
            [0,0,0,2,0,0,2,2,4,2,2,0,0,0,0,2,0],
            [0,2,2,2,4,0,2,2,2,2,2,0,2,2,2,2,0],
            [0,2,4,4,2,2,2,4,2,4,2,2,2,2,2,2,0],
            [0,2,2,2,2,0,2,2,2,4,2,0,2,2,2,2,0],
            [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
        ];

        /* The players starting square is tied to the labyrinth layout, so defining them together,
         * this is the start position, not to be confused with the current position. */
        private static readonly int StartY = 9;
        private static readonly int StartX = 14;

        static void Main(string[] args)
        {
            // Dungeon Escape game, main.

            // The indicator for if the program is to continue to run, used to end the entire program instead of going back to the pre game menue.
            bool theProgramRunState = true;
            // The indicator for if a game is in progress (true) or if the pre game menue is to be displayed (false).
            bool aGameIsRunning = false;
            // Player position, used during the game.
            int pCurY = StartY, pCurX = StartX;
            // Player have the key or not.
            bool pHavKey = false;
            // Flag for continuing to read inputs from the player until a valid input is given.
            bool watingForInput = true;
            // Temporary container for storing the given user input.
            string tempInputRead;

            // The main program loop.
            while (theProgramRunState)
            {
                if (aGameIsRunning) {
                    // A game is currently in progress.
                    DisplayPlayersVision(pCurY, pCurX, pHavKey);
                    Console.WriteLine();
                    // Displays a ledger of the meanings in the players view.
                    WriteThePlayerVisionLedger();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    WriteWithIndentation("For at udføre en handling indtast handlingen og afslut med enter.", true);
                    WriteWithIndentation("(Nøglen samles op automatisk, fælder udløses straks når du går hen på dem)", true);
                    WriteWithIndentation("Ryk din karakter: Op Ned Venstre Højre", true);
                    WriteWithIndentation("Til menuen (giv op): Menu", true);
                    WriteWithIndentation("Afslut programmet: Afslut", true);
                    // Sets the flag for read inputs.
                    watingForInput = true;
                    while (watingForInput)
                    {
                        WriteWithIndentation("", false);
                        // Input from the user.
                        tempInputRead = (Console.ReadLine() ?? "").ToLower();
                        switch (tempInputRead)
                        {
                            case "afslut":
                                // End the program.
                                theProgramRunState = false;
                                watingForInput = false;
                                break;
                            case "menu":
                                // End the current game and switch to menue mode.
                                // Switches to game mode.
                                aGameIsRunning = false;
                                watingForInput = false;
                                break;
                            case "op":
                                if (CanMoveToSquare(pCurY - 1, pCurX, pHavKey)) {
                                    // Perform move.
                                    pCurY--;
                                    watingForInput = false;
                                    // Removes the "Unrecogniced input handeling" after a valid input is given.
                                    RemoveUserInput(tempInputRead, "                        "); // The message part is 24 characters long.
                                } else {
                                    // Can't move to that square.
                                    RemoveUserInput(tempInputRead, "Kan ikke gå den vej.    "); // The message is 24 characters long.
                                }
                                break;
                            case "ned":
                                if (CanMoveToSquare(pCurY + 1, pCurX, pHavKey)) {
                                    // Perform move.
                                    pCurY++;
                                    watingForInput = false;
                                    // Removes the "Unrecogniced input handeling" after a valid input is given.
                                    RemoveUserInput(tempInputRead, "                        "); // The message part is 24 characters long.
                                } else {
                                    // Can't move to that square.
                                    RemoveUserInput(tempInputRead, "Kan ikke gå den vej.    "); // The message is 24 characters long.
                                }
                                break;
                            case "venstre":
                                if (CanMoveToSquare(pCurY, pCurX - 1, pHavKey)) {
                                    // Perform move.
                                    pCurX--;
                                    watingForInput = false;
                                    // Removes the "Unrecogniced input handeling" after a valid input is given.
                                    RemoveUserInput(tempInputRead, "                        "); // The message part is 24 characters long.
                                } else {
                                    // Can't move to that square.
                                    RemoveUserInput(tempInputRead, "Kan ikke gå den vej.    "); // The message is 24 characters long.
                                }
                                break;
                            case "højre":
                                if (CanMoveToSquare(pCurY, pCurX + 1, pHavKey)) {
                                    // Perform move.
                                    pCurX++;
                                    watingForInput = false;
                                    // Removes the "Unrecogniced input handeling" after a valid input is given.
                                    RemoveUserInput(tempInputRead, "                        "); // The message part is 24 characters long.
                                } else {
                                    // Can't move to that square.
                                    RemoveUserInput(tempInputRead, "Kan ikke gå den vej.    "); // The message is 24 characters long.
                                }
                                break;
                            default:
                                // Unrecogniced input handeling.
                                RemoveUserInput(tempInputRead, "Ukendt input, prøv igen."); // The message is 24 characters long.
                                break;
                        }
                    }
                    // A valid input was read, perform action based on the square at hand.
                    switch (PerformActionOnNewSquare(pCurY, pCurX))
                    {
                        case -1:
                            // End run due to trap.
                            aGameIsRunning = false;
                            Console.WriteLine();
                            WriteWithIndentation("Du er død i en fælde. (tryk på en tast for at gå til menuen)", false);
                            Console.ReadKey(true);
                            break;
                        case 1:
                            // End run due to winning.
                            aGameIsRunning = false;
                            Console.WriteLine();
                            WriteWithIndentation("Du har vundet! Du undslap labyrinten! (tryk på en tast for at gå til menuen)", false);
                            Console.ReadKey(true);
                            break;
                        case 2:
                            // Picks up the key.
                            pHavKey = true;
                            break;
                        default:
                            // Nothing happens.
                            break;
                    }
                } else {
                    // There isn't a game running, displaying the pre game menue.
                    ClearAndWriteTitle();
                    WriteWithIndentation("Velkommen til spillet.", true);
                    WriteWithIndentation("Du er fanget i en labyrint og dit mål er at nå udgangen.", true);
                    WriteWithIndentation("Men for dette skal du først bruge en nøgle.", true);
                    WriteWithIndentation("Labyrinten er også fyldt med fælder som du skal undgå.", true);
                    WriteWithIndentation("Du kan kun se ét felt omkring dig og du ved fra rygter at labyrinten er 10x15 felter.", true);
                    Console.WriteLine();
                    WriteWithIndentation("For at udføre en handling indtast handlingen og afslut med enter.", true);
                    WriteWithIndentation("Start spillet: Start", true);
                    WriteWithIndentation("Afslut programmet: Afslut", true);
                    // Sets the flag for read inputs.
                    watingForInput = true;
                    while (watingForInput)
                    {
                        WriteWithIndentation("", false);
                        // Input from the user.
                        tempInputRead = (Console.ReadLine() ?? "").ToLower();
                        switch (tempInputRead)
                        {
                            case "afslut":
                                // End the program.
                                theProgramRunState = false;
                                watingForInput = false;
                                break;
                            case "start":
                                // Starting the game, setting up the displayed area.
                                ClearAndWriteTitle();
                                // Resets player position and have key stat.
                                pCurY = StartY;
                                pCurX = StartX;
                                pHavKey = false;
                                // Switches to game mode.
                                aGameIsRunning = true;
                                watingForInput = false;
                                break;
                            default:
                                // Unrecogniced input handeling.
                                RemoveUserInput(tempInputRead, "Ukendt input, prøv igen.");
                                break;
                        }
                    }
                }
            }
        }

        static void WriteWithIndentation(string theText, bool lineBreakAtEnd)
        {
            // Used to write all text at some distance from the border of the console window.

            // Change the left side distance from consoles border to text for all the text here.
            Console.SetCursorPosition(4, Console.CursorTop);
            Console.Write(theText);
            if (lineBreakAtEnd)
                Console.WriteLine();
        }

        static void ClearAndWriteTitle()
        {
            // Clears the console and writes the titel, shared in the game mode and pre game menue.
            Console.Clear();
            Console.WriteLine();
            WriteWithIndentation("DUNGEON ESCAPE", true);
            Console.WriteLine();
        }

        static void WriteThePlayerVisionLedger()
        {
            // Writes the information for the player to untherstand what their character can see.
            // The different information pices for the ledger.
            string ledgerThePlayer = "\"" + ThePlayer.ToString() + "\" = Spilleren"; // Length 15.
            string ledgerEmpty = "\"" + EmptySquare.ToString() + "\" = Tomt"; // Length 10.
            string ledgerWall = "\"" + WallSquare.ToString() + "\" = En mur"; // Length 12.
            string ledgerTrap = "\"" + TrapSquare.ToString() + "\" = Fælde"; // Length 11.
            string ledgerKey = "\"" + KeySquare.ToString() + "\" = Nøglen"; // Length 12.
            string ledgerExit = "\"" + ExitSquare.ToString() + "\" = Udgang"; // Length 12.
            // Combining the different ledger units to be displayed in a grid layout, with 2 ledger pices pr. line.
            // (Can't just use 'SetCursorPosition' due to 'WriteWithIndentation')
            WriteWithIndentation($"{ledgerThePlayer,-18}{ledgerEmpty}", true);
            WriteWithIndentation($"{ledgerWall,-18}{ledgerTrap}", true);
            WriteWithIndentation($"{ledgerKey,-18}{ledgerExit}", true);
        }

        static void RemoveUserInput(string theInput, string messageUnderInputField)
        {
            // Writes a invalid input message after the users input, then goes back up and owerrides the users input.
            WriteWithIndentation(messageUnderInputField, false);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            // Handels for the indentation, wriding a new string consisting of ' ' characters in the same length as the users input.
            WriteWithIndentation(new string(' ', theInput.Length), false);
        }

        static void DisplayPlayersVision(int pCurY, int pCurX, bool haveKey)
        {
            // Displays the squares around the player, at distance 1.
            Console.SetCursorPosition(0, 3);
            /* Concatenates the 3 one char long strings, for eatch of the 3 lines to display,
             * a 3x3 grid with the player at the center.*/
            WriteWithIndentation(DisplayAsStr(pCurY - 1, pCurX - 1, haveKey) + DisplayAsStr(pCurY - 1, pCurX, haveKey) + DisplayAsStr(pCurY - 1, pCurX + 1, haveKey), !haveKey);
            if (haveKey) {
                // If the player have the key: write the following to the right of the top line of the players view.
                Console.WriteLine("     " + "Du har nøglen.");
                // In the above 'WriteWithIndentation("...", !haveKey)' meaning 'bool lineBreakAtEnd = !haveKey'.
            }
            WriteWithIndentation(DisplayAsStr(pCurY, pCurX - 1, haveKey) + ThePlayer.ToString() + DisplayAsStr(pCurY, pCurX + 1, haveKey), true);
            WriteWithIndentation(DisplayAsStr(pCurY + 1, pCurX - 1, haveKey) + DisplayAsStr(pCurY + 1, pCurX, haveKey) + DisplayAsStr(pCurY + 1, pCurX + 1, haveKey), true);
        }

        static string DisplayAsStr(int pCurY, int pCurX, bool haveKey)
        {
            // Gets a string for the char supposed to be displayed in a given square.
            int squaresValue = DungeonLayout[pCurY][pCurX];
            // If the player already have the key, display key square as empty.
            if (haveKey && squaresValue == 3)
                squaresValue = 2;
            return DisplayChar[squaresValue].ToString();
        }

        static bool CanMoveToSquare(int y, int x, bool haveKey)
        {
            // Method determines if the player can move to the square or not.

            // The '?' operator: "[expresion] ? [exp is true] : [exp is false]".
            /* If the player have the key they can move to the new square if it isn't a wall,
             * if they don't have the key, they can't move to the exit or a wall. */
            return (haveKey ? DungeonLayout[y][x] > 0 : DungeonLayout[y][x] > 1);
        }

        static int PerformActionOnNewSquare(int pCurY, int pCurX)
        {
            // Determins what happens due to entering a square.
            int currentSquareType = DungeonLayout[pCurY][pCurX];
            if (currentSquareType == 1) {
                // Current square is the exit, player have won.
                return 1;
            } else if (currentSquareType == 4) {
                // Current square is a trap, player have lost.
                return -1;
            } else if (currentSquareType == 3) {
                // Current square is the key, player pick up the key.
                return 2;
            }
            // No special action is performed.
            return 0;
        }
    }
}
