using MiNET.Worlds;
using System;
using System.Collections.Generic;

class HangmanGame
{
    static List<string> wordList = new List<string> { "developer", "programming", "hangman", "console", "challenge" };
    static int maxAttempts;
   

    static void Main()
    {
        int difficulty = GetDifficulty();
        maxAttempts = difficulty == 1 ? 10 : difficulty == 2 ? 6 : 4;
        Console.WriteLine("=== Welcome to Hangman ===");
        string wordToGuess = GetRandomWord();
        char[] guessedWord = new string('_', wordToGuess.Length).ToCharArray();
        List<char> wrongGuesses = new List<char>();

        int attemptsLeft = maxAttempts;

        while (attemptsLeft > 0 && new string(guessedWord) != wordToGuess)
        {
            Console.Clear();
            Console.WriteLine("=== Hangman ===");
            DrawHangman(maxAttempts - attemptsLeft);
            Console.WriteLine($"Word: {string.Join(" ", guessedWord)}");
            Console.WriteLine($"Wrong guesses: {string.Join(", ", wrongGuesses)}");
            Console.WriteLine($"Attempts left: {attemptsLeft}");
            Console.Write("Enter a letter: ");
            char guess = Console.ReadLine().ToLower()[0];

            if (!char.IsLetter(guess))
            {
                Console.WriteLine("Please enter a valid letter.");
                continue;
            }

            if (wrongGuesses.Contains(guess) || Array.Exists(guessedWord, c => c == guess))
            {
                Console.WriteLine("You already guessed that letter.");
                continue;
            }

            if (wordToGuess.Contains(guess))
            {
                for (int i = 0; i < wordToGuess.Length; i++)
                {
                    if (wordToGuess[i] == guess)
                    {
                        guessedWord[i] = guess;
                    }
                }
                Console.WriteLine("Good guess!");
            }
            else
            {
                wrongGuesses.Add(guess);
                attemptsLeft--;
                Console.WriteLine("Wrong guess!");
            }
        }

        Console.Clear();
        if (new string(guessedWord) == wordToGuess)
        {
            Console.WriteLine($"Congratulations! You guessed the word: {wordToGuess}");
        }
        else
        {
            DrawHangman(maxAttempts);
            Console.WriteLine($"Game over! The word was: {wordToGuess}");
        }
    }

    static string GetRandomWord()
    {
        Random random = new Random();
        return wordList[random.Next(wordList.Count)];
    }

    static void DrawHangman(int incorrectGuesses)
    {
        string[] hangmanStages = {
            "  +---+\n      |\n      |\n      |\n     ===",
            "  +---+\n  O   |\n      |\n      |\n     ===",
            "  +---+\n  O   |\n  |   |\n      |\n     ===",
            "  +---+\n  O   |\n /|   |\n      |\n     ===",
            "  +---+\n  O   |\n /|\\  |\n      |\n     ===",
            "  +---+\n  O   |\n /|\\  |\n /    |\n     ===",
            "  +---+\n  O   |\n /|\\  |\n / \\  |\n     ==="
        };

        Console.WriteLine(hangmanStages[incorrectGuesses]);
    }

    static int GetDifficulty()
    {
        Console.WriteLine("Choose Difficulty Level:");
        Console.WriteLine("1. Easy (10 attempts)");
        Console.WriteLine("2. Medium (6 attempts)");
        Console.WriteLine("3. Hard (4 attempts)");

        while (true)
        {
            Console.Write("Enter 1, 2, or 3: ");
            string choice = Console.ReadLine();
            if (choice == "1" || choice == "2" || choice == "3")
            {
                return int.Parse(choice); // Convert the choice to an integer
            }
            Console.WriteLine("Invalid input. Please try again.");
        }
    }

}
