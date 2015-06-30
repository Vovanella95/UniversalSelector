using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace UniParser
{
    public class ElementSelector
    {
        public static IEnumerable<IElement> QuerySelectorAll(IDocument document, ISelector selector)
        {
            var temp = document.Children;
            if (selector.Parts.Count() == 1 && selector.Parts.First().Item1 == "*")
            {
                return SelectChildren(new List<IElement>() { new Element() { Children = document.Children } }, new Tuple<string, ConnectionType>("", ConnectionType.Children));
            }
            foreach (var selectorPart in selector.Parts)
            {
                if (selectorPart.Item1 != "*")
                {
                    temp = SelectPart(temp, selectorPart);
                }
            }
            return temp;
        }
        public static IEnumerable<string> QuerySelectorAll(IDocument document, ISelector selector, string property)
        {
            foreach (var item in QuerySelectorAll(document, selector))
            {
                if (item.Attributes.Any(w => w.Name == property))
                {
                    yield return item.Attributes.First(w => w.Name == property).Value;
                }
            }
        }

        #region Private methods
        public static bool IsMatch(string selectorPart, IElement element)
        {
            if (selectorPart == "$") return true;
            Regex comparer = new Regex(@"(?'classes'[.])|(?'id'[#])|(?'attribs'\W\w+\W?\W\w+\W)|(?'tag'[.#=]*)");
            var selects = SplitProperties(selectorPart);
            var attributes = selects.Where(w => comparer.Match(w).Groups["attribs"].Success).Select(w => ParseAttributes(w));
            var classes = selects.Where(w => comparer.Match(w).Groups["classes"].Success).Select(w => w.Replace(".", ""));
            var id = selects.Where(w => comparer.Match(w).Groups["id"].Success).Count() != 0 ?
                            selects.First(w => comparer.Match(w).Groups["id"].Success).Replace("#", "") : null;
            var name = selects.Where(w => comparer.Match(w).Groups["tag"].Success).Count() != 0 ? selects.First(w => comparer.Match(w).Groups["tag"].Success) : null;
            if (!string.IsNullOrEmpty(id) && element.Id != id) return false;
            if (!string.IsNullOrEmpty(name) && element.Name != name) return false;
            if (classes != null && classes.Count() != 0)
            {
                if (element.Classes == null) return false;
                if (classes.Any(w => !element.Classes.Contains(w))) return false;
            }
            if (attributes != null && attributes.Count() != 0)
            {
                if (element.Attributes == null) return false;
                foreach (var attr in attributes)
                {
                    bool k = false;
                    foreach (var item in element.Attributes)
                    {
                        if (attr.CompareTo(item)) k = true;
                    }
                    if (k == false) return false;
                }
            }
            return true;
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
        static Attribute ParseAttributes(string attribs)
        {
            string val1 = attribs.Substring(1, attribs.IndexOf('=') - 1);
            string val2 = attribs.Substring(attribs.IndexOf('=') + 1, attribs.Length - attribs.IndexOf('=') - 2);
            return new Attribute()
            {
                Name = val1,
                Value = val2
            };
        }
        static IEnumerable<IElement> SelectDirectChildren(IEnumerable<IElement> elements, Tuple<string, ConnectionType> selectorPart)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                foreach (var item in Elements)
                {
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                    }
                    if (IsMatch(selectorPart.Item1, item))
                    {
                        if (item.Children != null)
                        {
                            foreach (var element in item.Children)
                            {
                                yield return element;
                            }
                        }
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
        static IEnumerable<IElement> SelectChildren(IEnumerable<IElement> elements, Tuple<string, ConnectionType> selectorPart)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                foreach (var item in Elements)
                {
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                    }
                    if (IsMatch(selectorPart.Item1, item))
                    {
                        List<IElement> parrent;
                        if (item.Children != null)
                        {
                            parrent = new List<IElement>(item.Children);
                        }
                        else
                        {
                            parrent = new List<IElement>();
                        }
                        var children = new List<IElement>();
                        while (parrent.Count() != 0)
                        {
                            foreach (var element in parrent)
                            {
                                if (element.Children != null && !IsMatch(selectorPart.Item1, element))
                                {
                                    children.AddRange(element.Children);
                                }
                                yield return element;
                            }
                            parrent.Clear();
                            parrent.AddRange(children);
                            children.Clear();
                        }
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
        static IEnumerable<IElement> SelectAfter(IEnumerable<IElement> elements, Tuple<string, ConnectionType> selectorPart)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                bool IsTrue = false;
                foreach (var item in Elements)
                {
                    if (item != null && IsTrue)
                    {
                        yield return item;
                    }
                    if (item == null)
                    {
                        IsTrue = false;
                        continue;
                    }
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                        Children.Add(null);
                    }
                    if (IsMatch(selectorPart.Item1, item))
                    {
                        IsTrue = true;
                        continue;
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
        static IEnumerable<IElement> SelectImmediatlyAfter(IEnumerable<IElement> elements, Tuple<string, ConnectionType> selectorPart)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                bool IsTrue = false;
                foreach (var item in Elements)
                {
                    if (item != null && IsTrue)
                    {
                        yield return item;
                        if (!IsMatch(selectorPart.Item1, item))
                        {
                            IsTrue = false;
                        }
                    }
                    if (item == null)
                    {
                        IsTrue = false;
                        continue;
                    }
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                        Children.Add(null);
                    }
                    if (IsMatch(selectorPart.Item1, item))
                    {
                        IsTrue = true;
                        continue;
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
        static IEnumerable<IElement> SelectAll(IEnumerable<IElement> elements, Tuple<string, ConnectionType> selectorPart)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                foreach (var item in Elements)
                {
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                    }
                    if (IsMatch(selectorPart.Item1, item))
                    {
                        yield return item;
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
        static IEnumerable<IElement> SelectPart(IEnumerable<IElement> elements, Tuple<string, ConnectionType> selectorPart)
        {
            if (selectorPart.Item2 == ConnectionType.DirectChildren)
            {
                return SelectDirectChildren(elements, selectorPart);
            }
            if (selectorPart.Item2 == ConnectionType.Children)
            {
                return SelectChildren(elements, selectorPart);
            }
            if (selectorPart.Item2 == ConnectionType.After)
            {
                return SelectAfter(elements, selectorPart);
            }
            if (selectorPart.Item2 == ConnectionType.ImmideatlyAfter)
            {
                return SelectImmediatlyAfter(elements, selectorPart);
            }
            if (selectorPart.Item2 == ConnectionType.None)
            {
                return SelectAll(elements, selectorPart);
            }
            throw new ArgumentException("Invalid selectorPart");
        }
        #endregion
    }



    public class Selector2
    {
        public string SelectorText;
        public virtual IEnumerable<IElement> Select(IEnumerable<IElement> elements) { throw new Exception(); }
        public IEnumerable<IElement> QuerySelector(IEnumerable<IElement> elements)
        {
            return NextSelector == null ? Select(elements) : NextSelector.QuerySelector(Select(elements));
        }
        public Selector2 NextSelector;
    }

    public class DirectChildSelector : Selector2
    {
        public override IEnumerable<IElement> Select(IEnumerable<IElement> elements)
        {
            List<IElement> Token = new List<IElement>();
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                foreach (var item in Elements)
                {
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                    }
                    if (ElementSelector.IsMatch(SelectorText, item))
                    {
                        if (item.Children != null)
                        {
                            foreach (var element in item.Children)
                            {
                                yield return element;
                            }
                        }
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
    }

    public class ChildSelector : Selector2
    {
        public override IEnumerable<IElement> Select(IEnumerable<IElement> elements)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                foreach (var item in Elements)
                {
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                    }
                    if (ElementSelector.IsMatch(SelectorText, item))
                    {
                        List<IElement> parrent;
                        if (item.Children != null)
                        {
                            parrent = new List<IElement>(item.Children);
                        }
                        else
                        {
                            parrent = new List<IElement>();
                        }
                        var children = new List<IElement>();
                        while (parrent.Count() != 0)
                        {
                            foreach (var element in parrent)
                            {
                                if (element.Children != null && !ElementSelector.IsMatch(SelectorText, element))
                                {
                                    children.AddRange(element.Children);
                                }
                                yield return element;
                            }
                            parrent.Clear();
                            parrent.AddRange(children);
                            children.Clear();
                        }
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
    }

    public class AfterSelector : Selector2
    {
        public override IEnumerable<IElement> Select(IEnumerable<IElement> elements)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                bool IsTrue = false;
                foreach (var item in Elements)
                {
                    if (item != null && IsTrue)
                    {
                        yield return item;
                    }
                    if (item == null)
                    {
                        IsTrue = false;
                        continue;
                    }
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                        Children.Add(null);
                    }
                    if (ElementSelector.IsMatch(SelectorText, item))
                    {
                        IsTrue = true;
                        continue;
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
    }

    public class ImmediatlyAfterSelector : Selector2
    {
        public override IEnumerable<IElement> Select(IEnumerable<IElement> elements)
        {
            List<IElement> Children = new List<IElement>();
            List<IElement> Elements = new List<IElement>(elements);
            while (Elements.Count() != 0)
            {
                bool IsTrue = false;
                foreach (var item in Elements)
                {
                    if (item != null && IsTrue)
                    {
                        yield return item;
                        if (!ElementSelector.IsMatch(SelectorText, item))
                        {
                            IsTrue = false;
                        }
                    }
                    if (item == null)
                    {
                        IsTrue = false;
                        continue;
                    }
                    if (item.Children != null)
                    {
                        Children.AddRange(item.Children);
                        Children.Add(null);
                    }
                    if (ElementSelector.IsMatch(SelectorText, item))
                    {
                        IsTrue = true;
                        continue;
                    }
                }
                Elements.Clear();
                Elements.AddRange(Children);
                Children.Clear();
            }
        }
    }
}