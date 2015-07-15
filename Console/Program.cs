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
                Name = "John_Wick",
                Id = "Killing_Strangers",
                Children = new List<Element>()
                {
                    new Element()
                    {
                        Id = "idshnick",
                        Name = "Arno_Dorian",
                        Attributes = list1
                    },
                    new Element()
                    {
                        Id = "Crown",
                        Name = "John_Snow",
                        Attributes = list1
                    },
                    new Element()
                    {
                        Id = "idshnick",
                        Name = "Jacob_Fry",
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
                                Value = "I Am Vasya"
                            }
                        }
                    },
                    new Element()
                    {
                        Name="Vova",
                        Id = "Zdarova",
                        Children = new List<Element>(),
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "Mass",
                                Value = "58kg"
                            }
                        }
                    },
                    new Element()
                    {
                        Id = "Kotember",
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "Height",
                                Value = "230cm"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "Height2",
                                Value = "2302cm"
                            }
                        }
                    }
                }
            };
            #endregion

            var k = new List<Tuple<string, Action<string>>>()
            {
                new Tuple<string,Action<string>>("#idshnick[href=nothing][type=$result]",w => Console.WriteLine(w + " - Selector1")),
                new Tuple<string,Action<string>>("Vova[Mass=$result]",w => Console.WriteLine(w + " - Selector2")),
                new Tuple<string,Action<string>>("#Kotember[Height=$result]",w => Console.WriteLine(w + " - Selector3")),
                new Tuple<string,Action<string>>("#idshnick[href=$result]",w => Console.WriteLine(w + " - Selector4")),
                new Tuple<string,Action<string>>("John_Snow",w => {}),
            };
            var cells = (new Selector(root)).QuerySelector(k).ToList();
        }
    }
}
