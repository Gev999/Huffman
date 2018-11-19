using System;
using System.Linq;
using System.Collections.Generic;
using static System.Console;

namespace Huffman_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            string data, encryptedData, decryptedData;
            int[] count;
            List<Symbols> tree = new List<Symbols>();
            List<Symbols> newTree = new List<Symbols>();
            FirstInitilization(out data, tree, newTree);
            CreateTree(tree, newTree);
            ShowTree(tree);
            EncryptData(data, out encryptedData, out count, tree);
            WriteLine($"\n\nEncryptedData : {encryptedData}");
            DecryptData(encryptedData, out decryptedData, tree, count);
            WriteLine($"\nDecryptedData : {decryptedData}");
            ReadKey();
        }

        private static void DecryptData(string encryptedData, out string decryptedData, List<Symbols> tree, int[] count)
        {
            decryptedData = ""; int startIdx = 0;
            for (int i = 0; i < count.Length; i++)
            {
                string path = "";
                for (int j = 0; j < count[i]; j++)
                {
                    path += encryptedData[startIdx++];
                }
                decryptedData += tree.Where(val => val.Path == path).First().Value;
            }
        }

        private static void EncryptData(string data, out string encryptData, out int[] count, List<Symbols> tree)
        {
            encryptData = ""; int idx = 0;
            count = new int[data.Length];
            foreach (var sym in data)
            {
                var value = tree.Where(i => i.Value == sym.ToString()).First().Path; 
                encryptData += value;
                count[idx++] = value.Length;
            }
        }

        private static void CreateTree(List<Symbols> tree, List<Symbols> newTree)
        {
            while (newTree.Count != 1)
            {
                ChangeTree(tree, newTree);
            }
            foreach (var value in tree)
            {
                value.Path = new string(value.Path.Reverse().ToArray());
            }
            WriteLine();
        }

        private static void ShowTree(List<Symbols> tree)
        {
            foreach (var num in tree)
            {
                WriteLine($"Symbol : {num.Value}\tSize : {num.Size}\tPath : {num.Path}");
            }
        }

        private static void ChangeTree(List<Symbols> tree, List<Symbols> newTree)
        {
            string value = newTree[0].Value + newTree[1].Value;
            int size = newTree[0].Size + newTree[1].Size;

            if (newTree[0].Size >= newTree[1].Size)
            {
                newTree[0].Path = "1";
                newTree[1].Path = "0";
            }
            else
            {
                newTree[0].Path = "0";
                newTree[1].Path = "1";
            }

            for (int i = 0; i < tree.Count; i++)
            {
                if (newTree[0].Value.Contains(tree[i].Value))
                {
                    tree[i].Path += newTree[0].Path;
                }
                if (newTree[1].Value.Contains(tree[i].Value))
                {
                    tree[i].Path += newTree[1].Path;
                }
            }

            newTree.RemoveRange(0, 2);
            newTree.Add(new Symbols { Value = value, Size = size, Path = "" });
            newTree.Sort((a, b) => { return a.Size - b.Size; });
        }

        private static void FirstInitilization(out string data, List<Symbols> tree, List<Symbols> newTree)
        {
            data = ReadLine();
            for (char symbol = (char)0x00; symbol <= 0xFF; symbol++)
            {
                int count = data.Where(sym => sym == symbol).Count();
                if (count > 0)
                {
                    tree.Add(new Symbols { Value = symbol.ToString(), Size = count, Path = "" });
                }
            }
            tree.Sort((a, b) => { return a.Size - b.Size; });
            foreach (var value in tree)
            {
                newTree.Add(new Symbols { Value = value.Value, Size=value.Size, Path=value.Path});
            }
        }
    }
}
