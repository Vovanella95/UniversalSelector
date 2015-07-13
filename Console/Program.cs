using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewSuperModifyedSelector;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
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


        static void Main(string[] args)
        {
            #region Attributes
            var list1 = new List<NewSuperModifyedSelector.Attribute>()
            {
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "href",
                    Value = "nothing"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "type",
                    Value = "tag"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "text",
                    Value = "hello"
                }
            };
            var list2 = new List<NewSuperModifyedSelector.Attribute>()
            {
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "ne4w",
                    Value = "nerws"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "santa",
                    Value = "claus"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "href",
                    Value = "nothing"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "new",
                    Value = "news"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "type",
                    Value = "tag"
                },
                new NewSuperModifyedSelector.Attribute()
                {
                    Name = "text",
                    Value = "hello"
                }
            };
            #endregion

            #region Elements
            var root = new Element()
            {
                Children = new List<Element>()
                {
                    new Element()
                    {
                        Id = "idshnick",
                        Name = "TheName",
                        Attributes = list1
                    },
                    new Element()
                    {
                        Id = "idshnick",
                        Name = "TheName",
                        Attributes = list2
                    },
                    new Element()
                    {
                        Attributes = list2,
                        Children = new List<Element>()
                    },
                    new Element()
                    {
                        Id = "idshnick",
                        Attributes = list1
                    }
                }

            };
            #endregion

            #region Template
            TemplateElement template = new TemplateElement()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "href",
                        Value = "nothing"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "type",
                        Value = "tag"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    }
                }
            };
            #endregion

            var kh = AddToEnumerable(new int[] { 2, 3, 4, 5 }, 1);

            var k = Selector.QuerySelector(template, root).ToList();


            

        }
    }
}
