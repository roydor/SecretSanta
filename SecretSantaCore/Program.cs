using System;
using System.IO;
using System.Threading;

namespace SecretSanta
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var picker = new SecretSantaPicker("participants.json");
            var mailer = new SecretSantaMailer("mailSettings.json");

            var results = picker.DrawNames();

            // Ensure the results directory is available.
            if (!Directory.Exists("results"))
                Directory.CreateDirectory("results");

            bool allEmailsValid = true;
            if (results != null)
            {
                Console.WriteLine("Found a valid match for {0} participants after {1} attempt(s).", picker.Participants.Count, picker.Attempts);
                Console.WriteLine("");
                foreach (var kvp in results)
                {
                    Console.WriteLine("Recording result for {0}....", kvp.Key.Name);
                    mailer.WriteFile(kvp.Key, kvp.Value);
                    if (string.IsNullOrEmpty(kvp.Key.Email))
                    {
                        allEmailsValid = false;
                        ConsoleColor consoleColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0} has no email address.", kvp.Key.Name);
                        Console.ForegroundColor = consoleColor;
                    }
                }
            }

            if (allEmailsValid)
            {
                Console.WriteLine("Press Enter to send mails.");
                Console.ReadLine();
                foreach (var kvp in results)
                {
                    int sendMailAttempts = 0;
                    while (sendMailAttempts < 3)
                    {
                        try
                        {
                            Console.WriteLine("Sending Mail to {0} ({1}).", kvp.Key.Name, kvp.Key.Email);
                            mailer.SendMail(kvp.Key, kvp.Value);
                            break;
                        }
                        catch
                        {
                            ConsoleColor consoleColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Failed to send mail to {0} ({1})", kvp.Key.Name, kvp.Key.Email);
                            Console.ForegroundColor = consoleColor;
                            sendMailAttempts++;
                            Thread.Sleep(1000);
                        }
                    }
                }
            }

            Console.WriteLine("");
            Console.WriteLine("[Press any key to exit...]");
            Console.ReadLine();
        }
    }
}
