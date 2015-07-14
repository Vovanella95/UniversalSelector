using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
        public string CompareAttributes(IEnumerable<Attribute> tokenAttribs)
        {
            if (Attributes == null) return null;
            if (tokenAttribs == null) return null;

            if (dictionary == null)
            {
                dim = Attributes.Count();
                dictionary = Attributes
                    .Select((w, ii) => new Tuple<string, int>(w.Name, ii))
                    .ToDictionary(w => w.Item1, w => w.Item2 + 1);
            }
            var currentValues = String.Empty;
            var testedValues = string.Join(" ", dictionary.Select(w => w.Value)) + ' ';


            string INeedThis = null;
            string neededName = Attributes.First(w => w.Value == "$result").Name;

            foreach (var item in tokenAttribs.Where(w => dictionary.Keys.Contains(w.Name)))
            {
                if (item.Value == Attributes.First(w => w.Name == item.Name).Value)
                {
                    currentValues = AddValueToString(currentValues, dictionary[item.Name]);
                }
                if(item.Name == neededName)
                {
                    INeedThis = item.Value;
                    currentValues = AddValueToString(currentValues, dictionary[item.Name]);
                }
                if (currentValues == testedValues)
                {
                    break;
                }
            }
            return INeedThis;
        }
        private static string TransformToString(IEnumerable<int> array)
        {
            return string.Join(" ", array.Select(w => w.ToString())) + ' ';
        }
        private static string AddValueToString(string str, int value)
        {
            return TransformToString(AddToEnumerable(str.Split(' ').
                 Where(w => !string.IsNullOrEmpty(w))
                .Distinct()
                .Select(w => Convert.ToInt32(w)), value));
        }
        private static IEnumerable<int> AddToEnumerable(IEnumerable<int> collection, params int[] values)
        {
            return Enumerable.Concat(collection, values).OrderBy(w => w);
        }
        private static void Swap(int[] array, int first, int second)
        {
            int tmp = array[first];
            array[first] = array[second];
            array[second] = tmp;
        }
        public string IsMatch(Element element)
        {
            if (!string.IsNullOrEmpty(Name) && Name != element.Name) return null;
            if (!string.IsNullOrEmpty(Id) && Id != element.Id) return null;
            if (Attributes != null)
            {
                return CompareAttributes(element.Attributes);
            }
            return null;
        }

        public TemplateElement() { }
        public TemplateElement (string representation)
        {
            Regex comparer = new Regex(@"(?'id'[#])|(?'attribs'\W\w+\W?\W\w+\W)|(?'tag'[.#=]*)");
            var selects = SplitProperties(representation);
            Attributes = selects.Where(w => comparer.Match(w).Groups["attribs"].Success).Select(w => ParseAttributes(w));
            Id = selects.Where(w => comparer.Match(w).Groups["id"].Success).Count() != 0 ?
                            selects.First(w => comparer.Match(w).Groups["id"].Success).Replace("#", "") : null;
            Name = selects.Where(w => comparer.Match(w).Groups["tag"].Success).Count() != 0 ? selects.First(w => comparer.Match(w).Groups["tag"].Success) : null;
        }

        Attribute ParseAttributes(string attribs)
        {
            string val1 = attribs.Substring(1, attribs.IndexOf('=') - 1);
            string val2 = attribs.Substring(attribs.IndexOf('=') + 1, attribs.Length - attribs.IndexOf('=') - 2);
            return new Attribute()
            {
                Name = val1,
                Value = val2
            };
        }
        static IEnumerable<string> SplitProperties(string selector)
        {
            var delimeners = new char[] { '#', '.', '[' };
            int i1 = 0;
            for (int i = 0; i < selector.Length; i++)
            {
                if (delimeners.Contains(selector[i]) || i == selector.Length - 1)
                {
                    var temp = i == selector.Length - 1 ? i + 1 : i;
                    yield return selector.Substring(i1, temp - i1);
                    i1 = i;
                }
            }
        }
    }

    public class Selector
    {
        Element Root;
        public Selector(Element root)
        {
            Root = root;
        }

        public IEnumerable<Element> QuerySelector(string selector, Action<string> action)
        {
            var template = new TemplateElement(selector);
            if (Root == null) throw new ArgumentNullException();
            var nodeStack = new Stack<Element>();
            nodeStack.Push(Root);
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
                var isMatch = template.IsMatch(node);
                if (isMatch!=null)
                {
                    action(isMatch);
                    yield return node;
                }
            }
        }
    }
}