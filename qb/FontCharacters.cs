using System;
using System.Collections;
using System.Collections.Generic;

namespace qb
{
    public static class FontCharacters
    {
        static UpperDictionary<Character> Characters;

        static FontCharacters()
        {
            Characters = new UpperDictionary<Character>("111 101 101 101 111");

            #region symbols

            Characters.Add(' ', "000 000 000 000 000");
            Characters.Add(':', "1 0 0 0 1");
            Characters.Add('+', "000 010 111 010 000");
            Characters.Add('-', "000 000 111 000 000");
            Characters.Add('(', "01 10 10 10 01");
            Characters.Add(')', "10 01 01 01 10");
            Characters.Add('[', "11 10 10 10 11");
            Characters.Add(']', "11 01 01 01 11");
            Characters.Add('/', "01 01 10 10 10");
            Characters.Add('\\', "10 10 01 01 01");
            Characters.Add('▲', "00100 01110 01110 11111 11111");
            Characters.Add('▼', "11111 11111 01110 01110 00100");
            Characters.Add('~', "1110000 0001000 1111101 1000111 1111101"); // advertisment sign

            #endregion

            #region Numbers

            Characters.Add('0', "111 101 101 101 111");
            Characters.Add('1', "110 010 010 010 111");
            Characters.Add('2', "111 001 111 100 111");
            Characters.Add('3', "111 001 010 001 111");
            Characters.Add('4', "101 101 111 001 001");
            Characters.Add('5', "111 100 111 001 111");
            Characters.Add('6', "111 100 111 101 111");
            Characters.Add('7', "111 001 001 001 001");
            Characters.Add('8', "111 101 111 101 111");
            Characters.Add('9', "111 101 111 001 111");

            #endregion

            #region english

            Characters.Add('A', "111 101 111 101 101");
            Characters.Add('B', "111 101 110 101 111");
            Characters.Add('C', "111 100 100 100 111");
            Characters.Add('D', "110 101 101 101 110");
            Characters.Add('E', "111 100 110 100 111");
            Characters.Add('F', "111 100 110 100 100");
            Characters.Add('G', "111 100 101 101 111");
            Characters.Add('H', "101 101 111 101 101");
            Characters.Add('I', "1 1 1 1 1");
            Characters.Add('J', "001 001 001 101 111");
            Characters.Add('K', "101 101 110 101 101");
            Characters.Add('L', "100 100 100 100 111");
            Characters.Add('M', "10001 11011 10101 10001 10001");
            Characters.Add('N', "10001 11001 10101 10011 10001");
            Characters.Add('O', "111 101 101 101 111");
            Characters.Add('P', "111 101 111 100 100");
            Characters.Add('Q', "1111 1001 1001 1011 1111");
            Characters.Add('R', "111 101 111 110 101");
            Characters.Add('S', "111 100 111 001 111");
            Characters.Add('T', "111 010 010 010 010");
            Characters.Add('U', "101 101 101 101 111");
            Characters.Add('V', "101 101 101 101 010");
            Characters.Add('W', "10001 10001 10001 10101 01110");
            Characters.Add('X', "101 101 010 101 101");
            Characters.Add('Y', "101 101 101 010 010");
            Characters.Add('Z', "111 001 010 100 111");

            #endregion

            #region russian

            Characters.Add('А', Characters['A']);
            Characters.Add('Б', "111 100 111 101 111");
            Characters.Add('В', Characters['B']);
            Characters.Add('Г', "111 100 100 100 100");
            Characters.Add('Д', "01110 01010 01010 11111 10001");
            Characters.Add('Е', Characters['E']);
            Characters.Add('Ё', Characters['Е']);
            Characters.Add('Ж', "10101 10101 01110 10101 10101");
            Characters.Add('З', "111 001 010 001 111");
            Characters.Add('И', "1001 1001 1011 1101 1001");
            Characters.Add('Й', "0110 0000 1001 1011 1101");
            Characters.Add('К', Characters['K']);
            Characters.Add('Л', "011 101 101 101 101");
            Characters.Add('М', Characters['M']);
            Characters.Add('Н', Characters['H']);
            Characters.Add('О', Characters['O']);
            Characters.Add('П', "111 101 101 101 101");
            Characters.Add('Р', Characters['P']);
            Characters.Add('С', Characters['C']);
            Characters.Add('Т', Characters['T']);
            Characters.Add('У', "101 101 111 001 111");
            Characters.Add('Ф', "01110 10101 10101 01110 00100");
            Characters.Add('Х', Characters['X']);
            Characters.Add('Ц', "1010 1010 1010 1111 0001");
            Characters.Add('Ч', "101 101 111 001 001");
            Characters.Add('Ш', "10101 10101 10101 10101 11111");
            Characters.Add('Щ', "101010 101010 101010 111111 000001");
            Characters.Add('Ъ', "1100 0100 0111 0101 0111");
            Characters.Add('Ы', "10001 10001 11101 10101 11101");
            Characters.Add('Ь', "100 100 111 101 111");
            Characters.Add('Э', "110 001 111 001 110");
            Characters.Add('Ю', "10111 10101 11101 10101 10111");
            Characters.Add('Я', "111 101 111 011 101");

            #endregion
        }

        public static Character Get(char value) => Characters[value];
    }

    public class Character
    {
        public readonly int Width, Height;
        bool[, ] Matrix;

        public Character(bool[, ] matrix)
        {
            Matrix = matrix;

            Width = matrix.GetLength(0);
            Height = matrix.GetLength(1);
        }

        public bool this [int x, int y] => Matrix[x, y];

        public static implicit operator Character(bool[, ] matrix) => new Character(matrix);
        public static implicit operator Character(string text)
        {
            var spt = text.Split(' ');
            var length = spt[0].Length;
            var matrix = new bool[length, spt.Length];

            for (int x = 0; x < length; x++)
                for (int y = 0; y < spt.Length; y++)
                    matrix[x, y] = spt[y][x] == '1';

            return matrix;
        }
    }
    public class UpperDictionary<TValue> : IDictionary<char, TValue>
    {
        Dictionary<char, TValue> InnerDictionary = new Dictionary<char, TValue>();

        readonly TValue DefaultValue;
        public ICollection<char> Keys => InnerDictionary.Keys;
        public ICollection<TValue> Values => InnerDictionary.Values;
        public int Count => InnerDictionary.Count;
        public bool IsReadOnly => false;

        public UpperDictionary(TValue defaultValue = default) => DefaultValue = defaultValue;

        public TValue this [char key]
        {
            get => InnerDictionary.ContainsKey(char.ToUpper(key)) ? InnerDictionary[char.ToUpper(key)] : DefaultValue;
            set => InnerDictionary[char.ToUpper(key)] = value;
        }

        public bool TryGetValue(char key, out TValue value) => InnerDictionary.TryGetValue(char.ToUpper(key), out value);

        public void Add(char key, TValue value) => InnerDictionary.Add(char.ToUpper(key), value);
        public void Add(KeyValuePair<char, TValue> item) => Add(item.Key, item.Value);

        public bool Remove(char key) => InnerDictionary.Remove(char.ToUpper(key));
        public bool Remove(KeyValuePair<char, TValue> item) => Remove(item.Key);

        public bool ContainsKey(char key) => InnerDictionary.ContainsKey(char.ToUpper(key));
        public bool Contains(KeyValuePair<char, TValue> item) => ContainsKey(item.Key);

        public void CopyTo(KeyValuePair<char, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public void Clear() => InnerDictionary.Clear();

        public IEnumerator<KeyValuePair<char, TValue>> GetEnumerator() => InnerDictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => InnerDictionary.GetEnumerator();
    }
}