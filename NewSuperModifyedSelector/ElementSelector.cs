using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSuperModifyedSelector
{
    public class Attribute
    {
        public string Name;
        public string Value;
    }

    public class Element
    {
        public int Name;
        public int Id;
        public IEnumerable<string> Classes;
        public IEnumerable<Attribute> Attributes;
        public IEnumerable<Element> Children;
        private IEnumerable<int[]> Combinations;

        public bool CompareAttributes(IEnumerable<Attribute> neededAttribs, IEnumerable<Attribute> tokenAttribs)
        {
            int dim = neededAttribs.Count();
            if (Combinations == null)
                Combinations = GenerateAllPermutations(dim);
            var dictionary = neededAttribs
                .Select((w, ii) => new Tuple<string, int>(w.Name, ii))
                .ToDictionary(w => w.Item1, w => w.Item2 + 1);

            int currentState = 0;
            var currentValues = new List<int>();
            int count = Combinations.Count();
            foreach (var item in tokenAttribs)
            {
                if (!dictionary.Keys.Contains(item.Name)) continue;
                int index = dictionary[item.Name];
                if (!currentValues.Contains(index))
                {
                    currentValues.Add(index);
                }
                currentState = GetStateNumber(dim, currentValues);
                //if (currentState == count) return true;
            }
            return currentState == count;
        }
        private static IEnumerable<int[]> GenerateAllPermutations(int number)
        {
            var source = new int[number];
            for (int i = 0; i < number; i++)
            {
                source[i] = i + 1;
            }
            for (int ctr = 1; ctr <= source.Length; ctr++)
            {
                for (int i = 0; i <= source.Length - ctr; i++)
                {
                    for (int j = i + ctr - 2; j >= i; j--)
                        for (int k = 0; k < i; k++)
                        {
                            Swap(source, j, k);
                            yield return source.Skip(i).Take(ctr).OrderBy(x => x).ToArray();
                            Swap(source, j, k);
                        }

                    yield return source.Skip(i).Take(ctr).ToArray();
                }
            }
        }
        private static void Swap<T>(T[] array, int first, int second)
        {
            T tmp;
            tmp = array[first];
            array[first] = array[second];
            array[second] = tmp;
        }
        private int GetStateNumber(int dim, IEnumerable<int> values)
        {
            if (Combinations == null)
            {
                Combinations = GenerateAllPermutations(dim);
            }
            int i = 1;
            foreach (var item in Combinations)
            {
                if (item.All(w => values.Contains(w)) && values.All(w => item.Contains(w)))
                {
                    break;
                }
                i++;
            }
            return i;
        }
    }
}
