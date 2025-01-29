namespace DungeonEscape
{
    internal class Program
    {
        // Assignment: "Dungeon Escape".
        // Author: Mikkel Kaa.

        // Declaring the chars used to display the players soroundings, using constants as specified in the assignment text.
        const char WallSquare  = 'M'; // DK: "Mur".
        const char ExitSquare  = 'U'; // DK: "Udgang".
        const char EmptySquare = ' '; // Universal: |empty|.
        const char KeySquare   = 'N'; // DK: "Nøgle".
        const char TrapSquare  = 'F'; // DK: "Fælde".
        const char ThePlayer   = 'S'; // DK: "Spiller".
        // An array for ease of refering to the chars (arrays can't be constants as they are objects).
        private static readonly char[] DisplayChar = [WallSquare, ExitSquare, EmptySquare, KeySquare, TrapSquare, ThePlayer];

        // The main dungeon array, layout of the dungeon, the value at [y][x] is the index for 'DisplayChar'.
        // Play area is 10 tall and 15 wide not including the border wall, the exit is in the border wall.
        /* The assignment of '[][]' instead of '[,]' makes it a jagged 2D array,
         * by this the shape of the dungeon don't need to be square or rectangle (future proofing and/or point of expansion). */
        private static readonly int[][] DungeonLayout = [
            [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0],
            [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
        ];
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        static bool CanMoveToSquare(int y, int x, bool haveKey)
        {
            // Method determines if the player can move to the square or not.

            // The '?' operator: "[expresion] ? [exp is true] : [exp is false]".
            /* If the player have the key they can move to the new square if it isn't a wall,
             * if they don't have the key, they can't move to the exit or a wall. */
            return (haveKey ? DungeonLayout[y][x] > 0 : DungeonLayout[y][x] > 1);
        }
    }
}
