using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace RSA_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Log("==========================================");
            char[] alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ".ToCharArray();
            string word = String.Empty;
            string decryptedWord = String.Empty;
            

            int[] publicKey = new int[2];
            int[] privateKey = new int[2];

            Console.WriteLine("Enter word to encrypt");
            word = Console.ReadLine().ToUpper();

            List<int> hash = new List<int>();
            List<int> encryptedHash = new List<int>();
            List<long> decryptedHash = new List<long>();
            foreach (var sym in word.ToCharArray())
            {
                
                for (int i = 0; i < alphabet.Length; i++)
                {
                    if (sym == alphabet[i])
                    {
                        hash.Add(i+1);
                        break;
                    }
                }
            }

            //Console.WriteLine($"Hash for word {word} is {hash.ToHashString()}" );
            Log($"Hash for word {word} is {hash.ToHashString()}");

            Log("=================Расчет открытого ключа=================");
            //Выбираю два простых числа. Пусть это будет p = 5 и q = 7.
            int p = 5;
            int q = 7;
            Log($"Выбираем два простых числа: p={p}, q={q}");
            
            //Вычисляем модуль — произведение наших p и q: n = p×q = 3×7 = 21.
            int n = p * q;
            Log($"Вычисляем модуль n = p*q: n={n}");

            //Вычисляем функцию Эйлера: φ = (p - 1)×(q - 1) = 2×6 = 12.
            int phi = (p - 1) * (q - 1);
            Log($"Вычисляем функцию Эйлера: phi = (p - 1)*(q - 1): phi={phi}");

            //Выбираем число e, отвечающее следующим критериям: (i)оно должно быть простое, (ii)оно должно быть меньше φ — остаются варианты: 3, 5, 7, 11, (iii)оно должно быть взаимно простое с φ; остаются варианты 5, 7, 11.Выберем e = 5.Это, так называемая, открытая экспонента.
            int e = 7;
            Log($"Открытая экспонента e={e}");

            //Формируем открытый ключ {e,n}
            publicKey[0] = e; publicKey[1] = n;
            Log($"Формируем открытый ключ {{e,n}}: {{{e},{n}}}");

            //вычислить число d, обратное е по модулю φ, что бы выполнялось условие (d×е)%φ=1
            List<int> validNumbers = new List<int>();
            int num = phi + 1;
            if (num % e == 0)
                validNumbers.Add(num/e);

            for (int i = 0; i < 100; i++)
            {
                num += phi;
                if (num % e == 0)
                    validNumbers.Add(num/e);

            }           

            Log($"Доступные числа для выбора d:{validNumbers.ToHashString()}");
            int d = validNumbers[0];

            Log($"Выбираем d={d}");
            //Формируем секретный ключ {d,n}
            privateKey[0] = d; privateKey[1] = n;
            Log($"Формируем секретный ключ {{d,n}}: {{{d},{n}}}");

            Log($"=================Шифрование=====================");
            Log($"Вычисляем значения E(P) по формуле  E=(P^e) mod n");
            foreach (var item in hash)
            {
                
                var encrypted = Convert.ToInt32(Math.Pow(item, publicKey[0]) % publicKey[1]);
                encryptedHash.Add(encrypted);
            }

            Log($"Шифрованый хэш:{encryptedHash.ToHashString()}");

            Log($"=================Расшифровка=====================");
            Log($"Вычисляем значения P(E) по формуле  P=(E^d) mod n");
            foreach (var item in encryptedHash)
            {
                //P=(E^d) mod n
                //var dd0 = Convert.ToInt64(Math.Pow(item, privateKey[0]));
                //int dd;
                //Math.DivRem(dd0, privateKey[1], out dd);
                ////var dd = dd0 % privateKey[1];
                //var decrypted = Convert.ToInt64(dd);
                var decrypted = Convert.ToInt64(Math.Pow(item, privateKey[0]) % privateKey[1]);
                decryptedHash.Add(decrypted);
            }

            Log($"Расшифрованый хэш:{decryptedHash.ToHashString()}");

            foreach (var item in decryptedHash)
            {
                decryptedWord += alphabet[item - 1];
            }

            Log($"Расшифрованое слово:{decryptedWord}");



            Console.ReadKey();         


        }

        public static void Log(string message)
        {
            string fName = @"Log.txt";

            Console.WriteLine(message);
            using (StreamWriter sw = new StreamWriter(fName, File.Exists(fName)))
            {
                sw.WriteLine(message);
            }
            
        }

       
    }

    public static class Extension
    {
        
        public static string ToHashString(this List<int> hash)
        {
            string result = "";
            foreach (var item in hash)
            {
                result += (item+" ");
            }
            return result;
        }

        public static string ToHashString(this List<long> hash)
        {
            string result = "";
            foreach (var item in hash)
            {
                result += (item + " ");
            }
            return result;
        }
    }
}
