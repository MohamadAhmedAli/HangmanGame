using System;
using System.Collections.Generic;
using System.Diagnostics;

class HangmanGame
{
    static List<string> wordList = new List<string> { "developer", "programming", "hangman", "console", "challenge" };
    static int maxAttempts;
    static Dictionary<string, int> leaderboard = new Dictionary<string, int>();
    static TimeSpan totalTime = TimeSpan.Zero; // Variable to track total time across rounds

    static void Main()
    {
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();
        do
        {
            AddWords();
            int difficulty = GetDifficulty();
            maxAttempts = difficulty == 1 ? 10 : difficulty == 2 ? 6 : 4;
            Console.WriteLine("=== Welcome to Hangman ===");

            // Start the timer
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

                // Display the timer
                TimeSpan timeElapsed = stopwatch.Elapsed;
                Console.WriteLine($"Time elapsed: {timeElapsed.Minutes:D2}:{timeElapsed.Seconds:D2}");

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

            // Stop the timer after the game ends
            stopwatch.Stop();

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

            int score = maxAttempts - attemptsLeft;
            UpdateLeaderboard(playerName, new string(guessedWord) == wordToGuess ? score + 10 : -5);
            DisplayLeaderboard();

            // Add the time from this round to the total time
            totalTime += stopwatch.Elapsed;

            // Show elapsed time for this round
            TimeSpan finalTime = stopwatch.Elapsed;
            Console.WriteLine($"Time for this game: {finalTime.Minutes:D2}:{finalTime.Seconds:D2}");

            // Show total accumulated time
            Console.WriteLine($"Total time accumulated: {totalTime.Minutes:D2}:{totalTime.Seconds:D2}");

            Console.WriteLine("Do you want to play again? (yes/no): ");

        } while (Console.ReadLine().ToLower() == "yes");
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

    static void AddWords()
    {
        Console.WriteLine("Do you want to add custom words? (yes/no): ");
        if (Console.ReadLine().ToLower() == "yes")
        {
            Console.WriteLine("Enter words (type 'done' to finish):");
            while (true)
            {
                string word = Console.ReadLine();
                if (word.ToLower() == "done") break;
                if (!string.IsNullOrWhiteSpace(word) && word.Length > 0)
                {
                    wordList.Add(word.ToLower());
                    Console.WriteLine($"Word '{word}' added!");
                }
                else
                {
                    Console.WriteLine("Invalid word. Try again.");
                }
            }
        }
    }

    static void UpdateLeaderboard(string playerName, int score)
    {
        if (leaderboard.ContainsKey(playerName))
        {
            leaderboard[playerName] += score;
        }
        else
        {
            leaderboard[playerName] = score;
        }
    }

    static void DisplayLeaderboard()
    {
        Console.WriteLine("=== Leaderboard ===");
        foreach (var player in leaderboard)
        {
            Console.WriteLine($"{player.Key}: {player.Value} points");
        }
    }
}
