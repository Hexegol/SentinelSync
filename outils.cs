using System;
using System.Collections.Generic;
using System.Text;

namespace SentinelSyncV1
{
    static class outils
    {
        public static int AskNPositiveNumberNotNull(string question)
        {
            return AskNumberBetween(question, 1, int.MaxValue, "ERROR : the number must be positive and not null");
        }

        public static int AskNumberBetween(string question, int min, int max, string messageErreurPersonnalise = null)
        {

            int nombre = AskNumber(question);

            if (nombre >= min && nombre <= max)
            {
                return nombre;
            }
            if (messageErreurPersonnalise == null)
            {
                Console.WriteLine($"the number must be between {min} and {max}");
            }
            else
            {
                Console.WriteLine(messageErreurPersonnalise);
            }


            Console.WriteLine();
            return AskNumberBetween(question, min, max, messageErreurPersonnalise);



        }
        public static int AskNumber(string question)
        {
            Console.Write(question);
            int reponse = 0;
            while (true)
            {
                try
                {
                    reponse = int.Parse(Console.ReadLine());
                    return reponse;
                }
                catch
                {
                    Console.WriteLine("ERROR : you must enter a number");
                    Console.WriteLine();
                    Console.Write(question);
                }
            }
        }


    }
}