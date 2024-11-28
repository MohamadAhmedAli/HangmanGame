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
        Console.ForegroundColor = ConsoleColor.Cyan; // Set text color for the UI
        Console.WriteLine("#############################################");
        Console.WriteLine("#         WELCOME TO THE HANGMAN GAME       #");
        Console.WriteLine("#############################################");
        Console.ResetColor();

        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();

        do
        {
            AddWords();
            int difficulty = GetDifficulty();
            maxAttempts = difficulty == 1 ? 10 : difficulty == 2 ? 6 : 4;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=== GAME START ===");
            Console.ResetColor();

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
                DrawHeader("Hangman Game");
                DrawHangman(maxAttempts - attemptsLeft,difficulty);
                DrawWord(guessedWord);
                DrawAttempts(attemptsLeft);
                DrawWrongGuesses(wrongGuesses);

                // Display the timer
                TimeSpan timeElapsed = stopwatch.Elapsed;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nTime elapsed: {timeElapsed.Minutes:D2}:{timeElapsed.Seconds:D2}");
                Console.ResetColor();

                Console.Write("\nEnter a letter: ");
                string input = Console.ReadLine().ToLower();
                if (string.IsNullOrEmpty(input) || !char.IsLetter(input[0]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input! Please enter a valid letter.");
                    Console.ResetColor();
                    continue;
                }

                char guess = input[0];
                if (wrongGuesses.Contains(guess) || Array.Exists(guessedWord, c => c == guess))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You already guessed that letter!");
                    Console.ResetColor();
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Good guess!");
                    Console.ResetColor();
                }
                else
                {
                    wrongGuesses.Add(guess);
                    attemptsLeft--;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong guess!");
                    Console.ResetColor();
                }
            }

            // Stop the timer after the game ends
            stopwatch.Stop();

            Console.Clear();
            if (new string(guessedWord) == wordToGuess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Congratulations! You guessed the word: {wordToGuess}");
                Console.ResetColor();
            }
            else
            {
                DrawHangman(maxAttempts,difficulty);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Game over! The word was: {wordToGuess}");
                Console.ResetColor();
            }

            int score = maxAttempts - attemptsLeft;
            UpdateLeaderboard(playerName, new string(guessedWord) == wordToGuess ? score + 10 : -5);
            DisplayLeaderboard();

            // Add the time from this round to the total time
            totalTime += stopwatch.Elapsed;

            // Show elapsed time for this round
            TimeSpan finalTime = stopwatch.Elapsed;
            Console.WriteLine($"\nTime for this game: {finalTime.Minutes:D2}:{finalTime.Seconds:D2}");

            // Show total accumulated time
            Console.WriteLine($"Total time accumulated: {totalTime.Minutes:D2}:{totalTime.Seconds:D2}");

            Console.WriteLine("\nDo you want to play again? (yes/no): ");

        } while (Console.ReadLine().ToLower() == "yes");
    }

    static string GetRandomWord()
    {
        Random random = new Random();
        return wordList[random.Next(wordList.Count)];
    }

    static void DrawHangman(int incorrectGuesses, int difficulty)
    {
        string[] hangmanStages = {
        @"
  _______
  |     |
        |
        |
        |
        |
=========", 
        @"
  _______
  |     |
  O     |
        |
        |
        |
=========", 
        @"
  _______
  |     |
  O     |
  |     |
        |
        |
=========",
        @"
  _______
  |     |
  O     |
 /|     |
        |
        |
=========", 
        @"
  _______
  |     |
  O     |
 /|\    |
        |
        |
=========", 
        @"
  _______
  |     |
  O     |
 /|\    |
 /      |
        |
=========", 
        @"
  _______
  |     |
  O     |
 /|\    |
 / \    |
        |
=========", 
    };

        int totalStages = difficulty == 1 ? 10 : difficulty == 2 ? 6 : 4;
        int stagesToUse = hangmanStages.Length - 1; 

        int stageIndex = (int)Math.Ceiling((double)incorrectGuesses * stagesToUse / totalStages);

        stageIndex = Math.Min(stageIndex, stagesToUse);

        Console.WriteLine(hangmanStages[stageIndex]);
    }


    static int GetDifficulty()
    {
        DrawHeader("Choose Difficulty Level");
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
        DrawHeader("Add Custom Words");
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
        DrawHeader("Leaderboard");
        foreach (var player in leaderboard)
        {
            Console.WriteLine($"{player.Key}: {player.Value} points");
        }
    }

    static void DrawHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=================================");
        Console.WriteLine($"        {title.ToUpper()}");
        Console.WriteLine("=================================");
        Console.ResetColor();
    }

    static void DrawWord(char[] guessedWord)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nWord: {string.Join(" ", guessedWord)}");
        Console.ResetColor();
    }

    static void DrawAttempts(int attemptsLeft)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Attempts left: {attemptsLeft}");
        Console.ResetColor();
    }

    static void DrawWrongGuesses(List<char> wrongGuesses)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Wrong guesses: {string.Join(", ", wrongGuesses)}");
        Console.ResetColor();
    }
}
