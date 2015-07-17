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
        public Attribute() { }
        public Attribute(string attribs)
        {
            Name = attribs.Substring(1, attribs.IndexOf('=') - 1);
            Value = attribs.Substring(attribs.IndexOf('=') + 1, attribs.Length - attribs.IndexOf('=') - 2);
        }
    }

    public class Element
    {
        public IEnumerable<Attribute> Attributes;
        public IEnumerable<Element> Children;
        public string GetAttributeValue(string attributeName)
        {
            if (Attributes == null || Attributes.All(w => w.Name != attributeName))
            {
                throw new ArgumentException("Attributes doesn't contain inputed attribute");
            }
            return Attributes.First(w => attributeName == w.Name).Value;
        }
        public override string ToString()
        {
            if (Attributes == null) return "Empty";
            return string.Join("", Attributes.Select(w => "[" + w.Name + " = " + w.Value + "]"));
        }
    }

    public class Selector
    {
        public IEnumerable<State> States;
        Element Root;
        public Selector(Element root)
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
                if (node.Attributes == null)
                {
                    yield return node;
                    continue;
                }
                if (States.Where(w => !string.IsNullOrEmpty(w.ChangeState(node.Attributes))).Count() != 0)
                {
                    yield return node;
                }
            }
        }
    }

    public class State
    {
        #region Private Fields
        IEnumerable<Attribute> Attributes;
        Action<string> Trigger;
        int Dim;
        Dictionary<string, int> Dict;
        string CurrentValues;
        string TestedValues;
        string NeededName;
        string INeedThis;
        #endregion

        #region Public Methods
        public State(string selector, Action<string> action)
        {
            if (selector == "*") return;
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
        }
        public string ChangeState(IEnumerable<Attribute> attributes)
        {
            if (Attributes == null)
            {
                return "$none";
            }
            foreach (var item in attributes)
            {
                if (Attributes.Any(w => w.Name == item.Name) && item.Value == Attributes.First(w => w.Name == item.Name).Value)
                    CurrentValues = AddValueToString(CurrentValues, Dict[item.Name]);

                if (item.Name == NeededName)
                {
                    INeedThis = item.Value;
                    CurrentValues = AddValueToString(CurrentValues, Dict[item.Name]);
                }
                if (CurrentValues == TestedValues)
                {
                    if (string.IsNullOrEmpty(INeedThis))
                    {
                        return "$none";
                    }
                    else
                    {
                        Trigger(INeedThis);
                        var temp = INeedThis;
                        CurrentValues = String.Empty;
                        return temp;
                    }
                }
            }
            CurrentValues = String.Empty;
            return null;
        }
        #endregion

        #region Private Methods
        private IEnumerable<Attribute> GetAttributes(string representation)
        {
            return SplitProperties(representation).Where(w => !string.IsNullOrEmpty(w)).Select(w => new Attribute(w));
        }
        private IEnumerable<string> SplitProperties(string selector)
        {
            int i1 = 0;
            for (int i = 0; i < selector.Length; i++)
            {
                if (selector[i]=='[' || i == selector.Length - 1)
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
            return TransformToString(AddToEnumerable(str.Split(' ')
                .Where(w => !string.IsNullOrEmpty(w))
                .Select(w => Convert.ToInt32(w)), value));
        }
        private IEnumerable<int> AddToEnumerable(IEnumerable<int> collection, params int[] values)
        {
            return Enumerable.Concat(collection, values).OrderBy(w => w);
        }
        #endregion
    }
}