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
        public IEnumerable<Attribute> Attributes;
        public IEnumerable<Element> Children;
        public override string ToString()
        {
            return "oops!";
        }
    }

    public class TemplateElement : Element
    {
        #region Fields
        private int dim;
        private Dictionary<string, int> dictionary;
        #endregion

        #region PublicMethods
        public TemplateElement() { }
        public TemplateElement(string representation)
        {
            Regex comparer = new Regex(@"(?'id'[#])|(?'attribs'\W\w+\W?\W\w+\W)|(?'tag'[.#=]*)");
            var selects = SplitProperties(representation);
            Attributes = selects.Where(w => comparer.Match(w).Groups["attribs"].Success).Select(w => ParseAttributes(w));
        }
        public string CompareAttributes(IEnumerable<Attribute> tokenAttribs)
        {
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
            string neededName = null;
            if (Attributes.Any(w => w.Value == "$result"))
                neededName = Attributes.First(w => w.Value == "$result").Name;

            foreach (var item in tokenAttribs.Where(w => dictionary.Keys.Contains(w.Name)))
            {
                if (item.Value == Attributes.First(w => w.Name == item.Name).Value)
                    currentValues = AddValueToString(currentValues, dictionary[item.Name]);

                if (item.Name == neededName)
                {
                    INeedThis = item.Value;
                    currentValues = AddValueToString(currentValues, dictionary[item.Name]);
                }
                if (currentValues == testedValues)
                {
                    if (string.IsNullOrEmpty(INeedThis))
                        return "$none";
                    break;
                }
            }
            return INeedThis;
        }
        public string IsMatch(Element element)
        {
            if (Attributes != null && Attributes.Count() != 0)
            {
                return CompareAttributes(element.Attributes);
            }
            return "$none";
        }
        #endregion

        #region PrivateMethods
        private string TransformToString(IEnumerable<int> array)
        {
            return string.Join(" ", array.Select(w => w.ToString())) + ' ';
        }
        private string AddValueToString(string str, int value)
        {
            return TransformToString(AddToEnumerable(str.Split(' ').
                 Where(w => !string.IsNullOrEmpty(w))
                .Distinct()
                .Select(w => Convert.ToInt32(w)), value));
        }
        private IEnumerable<int> AddToEnumerable(IEnumerable<int> collection, params int[] values)
        {
            return Enumerable.Concat(collection, values).OrderBy(w => w);
        }
        private void Swap(int[] array, int first, int second)
        {
            int tmp = array[first];
            array[first] = array[second];
            array[second] = tmp;
        }
        private Attribute ParseAttributes(string attribs)
        {
            string val1 = attribs.Substring(1, attribs.IndexOf('=') - 1);
            string val2 = attribs.Substring(attribs.IndexOf('=') + 1, attribs.Length - attribs.IndexOf('=') - 2);
            return new Attribute()
            {
                Name = val1,
                Value = val2
            };
        }
        private IEnumerable<string> SplitProperties(string selector)
        {
            var delimeners = new char[] { '[' };
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
        #endregion
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
                if (!string.IsNullOrEmpty(isMatch))
                {
                    if (isMatch != "$none")
                    {
                        action(isMatch);
                    }
                    yield return node;
                }
            }
        }
        public IEnumerable<Element> QuerySelector(IEnumerable<Tuple<string, Action<string>>> selectors)
        {
            var templates = selectors.Select(w => new Tuple<TemplateElement, Action<string>>(new TemplateElement(w.Item1), w.Item2)).ToList();
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
                bool wasReturned = false;
                foreach (var template in templates)
                {
                    var isMatch = template.Item1.IsMatch(node);
                    if (!string.IsNullOrEmpty(isMatch))
                    {
                        if (isMatch != "$none")
                        {
                            template.Item2(isMatch);
                        }
                        if (!wasReturned)
                        {
                            wasReturned = true;
                            yield return node;
                        }
                    }
                }
            }
        }
    }

    public class Selector2
    {
        public IEnumerable<State> States;
        Element Root;
        public Selector2(Element root)
        {
            Root = root;
        }
        public IEnumerable<Element> QuerySelector(IEnumerable<Tuple<string, Action<string>>> selectors)
        {
            States = selectors.Select(w => new State(w.Item1, w.Item2));
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
                if (node.Attributes == null) continue;

                

                if(States.Any(w=>!string.IsNullOrEmpty(w.ChangeState(node.Attributes))))
                {
                    yield return node;
                }
            }
        }
    }

    public class State
    {
        public IEnumerable<Attribute> Attributes;
        public Action<string> Trigger;
        public int Dim;
        public Dictionary<string, int> Dict;
        public string CurrentValues;
        public string TestedValues;
        public string NeededName;
        public string INeedThis;
        public bool WasReturned;

        public State(string selector, Action<string> action)
        {
            Attributes = GetAttributes(selector);
            Dim = Attributes.Count();
            Dict = Attributes
                    .Select((w, ii) => new Tuple<string, int>(w.Name, ii))
                    .ToDictionary(w => w.Item1, w => w.Item2 + 1);
            Trigger = action;

            CurrentValues = String.Empty;
            TestedValues = string.Join(" ", Dict.Select(w => w.Value)) + ' ';
            INeedThis = null;
            NeededName = null;
            if (Attributes.Any(w => w.Value == "$result"))
                NeededName = Attributes.First(w => w.Value == "$result").Name;
            WasReturned = false;
        }
        public string ChangeState(IEnumerable<Attribute> attributes)
        {
            foreach (var item in attributes)
            {
                if (Attributes.Any(w => w.Name == item.Name) && item.Value == Attributes.First(w => w.Name == item.Name).Value)
                {
                    CurrentValues = AddValueToString(CurrentValues, Dict[item.Name]);
                }

                if (item.Name == NeededName)
                {
                    INeedThis = item.Value;
                    CurrentValues = AddValueToString(CurrentValues, Dict[item.Name]);
                }
                if (CurrentValues == TestedValues)
                {
                    WasReturned = true;
                    if (string.IsNullOrEmpty(INeedThis))
                    {
                        return "$none";
                    }
                    else
                    {
                        Trigger(INeedThis);
                        var temp = INeedThis;
                        Reset();
                        return temp;
                    }
                }
            }
            Reset();
            return null;
        }
        public void Reset()
        {
            CurrentValues = String.Empty;
            INeedThis = null;
            NeededName = null;
            WasReturned = false;
        }

        private IEnumerable<Attribute> GetAttributes(string representation)
        {
            var selects = SplitProperties(representation);
            return selects.Where(w => !string.IsNullOrEmpty(w)).Select(w => ParseAttributes(w));
        }
        private Attribute ParseAttributes(string attribs)
        {
            string val1 = attribs.Substring(1, attribs.IndexOf('=') - 1);
            string val2 = attribs.Substring(attribs.IndexOf('=') + 1, attribs.Length - attribs.IndexOf('=') - 2);
            return new Attribute()
            {
                Name = val1,
                Value = val2
            };
        }
        private IEnumerable<string> SplitProperties(string selector)
        {
            var delimeners = new char[] { '[' };
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
        private string TransformToString(IEnumerable<int> array)
        {
            return string.Join(" ", array.Select(w => w.ToString())) + ' ';
        }
        private string AddValueToString(string str, int value)
        {
            return TransformToString(AddToEnumerable(str.Split(' ').
                 Where(w => !string.IsNullOrEmpty(w))
                .Distinct()
                .Select(w => Convert.ToInt32(w)), value));
        }
        private IEnumerable<int> AddToEnumerable(IEnumerable<int> collection, params int[] values)
        {
            return Enumerable.Concat(collection, values).OrderBy(w => w);
        }
    }
}