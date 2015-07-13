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
        private int dim;
        private Dictionary<string, int> dictionary;

        public bool CompareAttributes(IEnumerable<Attribute> neededAttribs, IEnumerable<Attribute> tokenAttribs)
        {
            if (neededAttribs == null) return true;
            if (tokenAttribs == null) return false;

            if (dictionary == null)
            {
                dim = neededAttribs.Count();
                dictionary = neededAttribs
                    .Select((w, ii) => new Tuple<string, int>(w.Name, ii))
                    .ToDictionary(w => w.Item1, w => w.Item2 + 1);
            }

            var currentValues = "";
            int index;
            var testedValues = string.Join(" ", dictionary.Select(w => w.Value)) + ' ';
            foreach (var item in tokenAttribs)
            {
                if (!dictionary.Keys.Contains(item.Name)) continue;
                index = dictionary[item.Name];
                currentValues = AddValueToString(currentValues, index);
                if (currentValues == testedValues) return true;
            }
            return false;
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
                 Where(w => !string.IsNullOrEmpty(w))
                .Distinct()
                .Select(w => Convert.ToInt32(w)), value));
        }
        private static IEnumerable<int> AddToEnumerable(IEnumerable<int> collection, int value)
        {
            int prev = 0;
            bool checker = false;
            foreach (var item in collection)
            {
                if (prev < value && item > value)
                {
                    yield return value;
                    checker = true;
                }
                yield return item;
                prev = item;
            }
            if (!checker)
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