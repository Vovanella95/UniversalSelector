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
        public string Name;
        public string Id;
        public IEnumerable<Attribute> Attributes;
        public IEnumerable<Element> Children;
        public override string ToString()
        {
            return "Name = " + Name + ", Id = " + Id;
        }
    }

    public class TemplateElement : Element
    {
        private IEnumerable<int[]> Combinations;
        private int dim;
        private Dictionary<string, int> dictionary;
        private Dictionary<string, int> dictionary2;
        private int count;



        public bool CompareAttributes(IEnumerable<Attribute> neededAttribs, IEnumerable<Attribute> tokenAttribs)
        {
            if (neededAttribs == null) return true;
            if (tokenAttribs == null) return false;

            if (dictionary2 == null)
            {
                dim = neededAttribs.Count();
                dictionary = neededAttribs
                    .Select((w, ii) => new Tuple<string, int>(w.Name, ii))
                    .ToDictionary(w => w.Item1, w => w.Item2 + 1);

                dictionary2 = GenerateAllPermutationsDict(dim);
                count = dictionary2.Count();
            }

            int currentState = 0;
            var currentValues = "";
            int index;
            foreach (var item in tokenAttribs)
            {
                if (!dictionary.Keys.Contains(item.Name)) continue;
                index = dictionary[item.Name];
                currentValues = AddValueToString(currentValues, index);
                currentState = dictionary2[currentValues];
                if (currentState == count) return true;
            }
            return false;
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
        private static Dictionary<string, int> GenerateAllPermutationsDict(int number)
        {
            var dic = new Dictionary<string, int>();
            int counter = 1;

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
                            dic.Add(TransformToString(source.Skip(i).Take(ctr).OrderBy(x => x).ToArray()), counter);
                            counter++;
                            Swap(source, j, k);
                        }
                    dic.Add(TransformToString(source.Skip(i).Take(ctr).ToArray()), counter);
                    counter++;
                }
            }
            return dic;
        }
        private static string TransformToString(IEnumerable<int> array)
        {
            string val = "";
            foreach (var item in array)
            {
                val += item + " ";
            }
            return val;
        }

        private static string AddValueToString(string str, int value)
        {
            return TransformToString(AddToEnumerable(str.Split(' ').
                Where(w=>!string.IsNullOrEmpty(w))
                .Distinct()
                .Select(w=>Convert.ToInt32(w)), value));
        }

        private static IEnumerable<int> AddToEnumerable(IEnumerable<int> collection, int value)
        {
            int prev = 0;
            bool checker = false;
            foreach (var item in collection)
            {
                if(prev<value && item > value)
                {
                    yield return value;
                    checker = true;
                }
                yield return item;
                prev = item;
            }
            if(!checker)
            {
                yield return value;
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

        public bool IsMatch(Element element)
        {
            if (!string.IsNullOrEmpty(Name) && Name != element.Name) return false;
            if (!string.IsNullOrEmpty(Id) && Id != element.Id) return false;
            if (Attributes != null)
            {
                return CompareAttributes(Attributes, element.Attributes);
            }
            return true;
        }
    }

    public class Selector
    {
        public static IEnumerable<Element> QuerySelector(TemplateElement template, Element root)
        {
            if (root == null) throw new ArgumentNullException();
            var nodeStack = new Stack<Element>();
            nodeStack.Push(root);
            while (nodeStack.Count != 0)
            {
                var node = nodeStack.Pop();
                if (node.Children != null)
                {
                    foreach (var subNode in node.Children.Reverse())
                    {
                        nodeStack.Push(subNode);
                    }
                }
                if (template.IsMatch(node))
                {
                    yield return node;
                }
            }
        }
    }
}