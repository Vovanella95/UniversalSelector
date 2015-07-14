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
        private Dictionary<string, int> dictionary;
        public bool CompareAttributes(IEnumerable<Attribute> tokenAttribs)
        {
            if (Attributes == null) return true;
            if (tokenAttribs == null) return false;

            if (dictionary == null)
            {
                dictionary = Attributes
                    .Select((w, ii) => new Tuple<string, int>(w.Name, ii))
                    .ToDictionary(w => w.Item1, w => w.Item2 + 1);
            }

            foreach (var item in tokenAttribs.Where(w => dictionary.Keys.Contains(w.Name)))
            {
                dictionary.Remove(item.Name);
            }
            return dictionary.Count == 0;
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
        public bool IsMatch(Element element)
        {
            if (!string.IsNullOrEmpty(Name) && Name != element.Name) return false;
            if (!string.IsNullOrEmpty(Id) && Id != element.Id) return false;
            if (Attributes != null)
            {
                return CompareAttributes(element.Attributes);
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