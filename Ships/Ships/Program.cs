using System;
using System.Collections.Generic;

class Program
{
    static char[,] CreateEmptyBoard()
    {
        char[,] board = new char[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                board[i, j] = '.';
            }
        }
        return board;
    }

    static void PrintBoard(char[,] board)
    {
        Console.WriteLine("  A B C D E F G H I J");
        for (int i = 0; i < 10; i++)
        {
            Console.Write((i + 1).ToString().PadLeft(2) + " ");
            for (int j = 0; j < 10; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static bool PlaceShip(char[,] board, int x, int y, int length, bool horizontal)
    {
        if (horizontal)
        {
            if (y + length > 10) return false;
            for (int i = 0; i < length; i++)
            {
                if (board[x, y + i] != '.') return false;
            }
            for (int i = 0; i < length; i++)
            {
                board[x, y + i] = '#';
            }
        }
        else
        {
            if (x + length > 10) return false;
            for (int i = 0; i < length; i++)
            {
                if (board[x + i, y] != '.') return false;
            }
            for (int i = 0; i < length; i++)
            {
                board[x + i, y] = '#';
            }
        }
        return true;
    }

    static void PlaceAllShips(char[,] board)
    {
        var shipSizes = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        foreach (var size in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                Console.WriteLine($"Ustawienie statku o długości {size}:");
                PrintBoard(board);
                Console.Write("Podaj pozycję startową (np. A5): ");
                string pos = Console.ReadLine();
                int x = int.Parse(pos.Substring(1)) - 1;
                int y = pos[0] - 'A';
                Console.Write("Podaj orientację (h dla poziomej, v dla pionowej): ");
                string orientation = Console.ReadLine();
                placed = PlaceShip(board, x, y, size, orientation == "h");
                if (!placed) Console.WriteLine("Błąd ustawienia statku, spróbuj ponownie.");
            }
        }
    }

    static bool Attack(char[,] board, int x, int y)
    {
        if (board[x, y] == '#')
        {
            board[x, y] = 'X';
            return true;
        }
        else if (board[x, y] == '.')
        {
            board[x, y] = 'O';
            return false;
        }
        return false;
    }

    static bool CheckWin(char[,] board)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (board[i, j] == '#') return false;
            }
        }
        return true;
    }

    static void NextPlayer()
    {
        Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć turę.");
        Console.ReadKey();
        Console.Clear();
        Console.WriteLine("Zamiana miejsc! Naciśnij dowolny klawisz, aby kontynuować.");
        Console.ReadKey();
        Console.Clear();
    }

    static void Main(string[] args)
    {
        char[,] board1 = CreateEmptyBoard();
        char[,] board2 = CreateEmptyBoard();
        char[,] targetBoard1 = CreateEmptyBoard();
        char[,] targetBoard2 = CreateEmptyBoard();

        Console.WriteLine("Gracz 1 ustaw swoje statki.");
        PlaceAllShips(board1);
        Console.Clear();
        Console.WriteLine("Gracz 2 ustaw swoje statki.");
        PlaceAllShips(board2);
        Console.Clear();

        bool player1Turn = true;
        while (true)
        {
            char[,] targetBoard = player1Turn ? targetBoard1 : targetBoard2;
            char[,] enemyBoard = player1Turn ? board2 : board1;

            Console.WriteLine($"Tura gracza {(player1Turn ? 1 : 2)}");
            PrintBoard(targetBoard);
            Console.Write("Podaj pozycję do ataku (np. A5): ");
            string pos = Console.ReadLine();
            int x = int.Parse(pos.Substring(1)) - 1;
            int y = pos[0] - 'A';

            bool hit = Attack(enemyBoard, x, y);
            targetBoard[x, y] = hit ? 'X' : 'O';
            Console.WriteLine(hit ? "Trafiony!" : "Pudło!");

            if (CheckWin(enemyBoard))
            {
                Console.WriteLine($"Gracz {(player1Turn ? 1 : 2)} wygrywa!");
                Console.WriteLine("Gratulacje!");
                break;
            }

            NextPlayer();
            player1Turn = !player1Turn;
        }
    }
}
