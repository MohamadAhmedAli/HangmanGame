using System;
using System.Collections.Generic;

class HangmanGame
{
    static List<string> wordList = new List<string> { "developer", "programming", "hangman", "console", "challenge" };
    static int maxAttempts = 6;

    static void Main()
    {
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
}
