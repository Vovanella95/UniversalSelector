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
        static void PrintItem(string w, int num)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(w);
            Console.ForegroundColor = (ConsoleColor)((num) % 16 + 1);
            Console.WriteLine(" - Selector" + num);
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
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "name",
                        Value = "root"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "description",
                        Value = "everything_is_connected"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "health",
                        Value = "good"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "person",
                        Value = "Aiden_Pears"
                    }
                },
                Children = new List<Element>()
                {
                    new Element()
                    {
                        Attributes = list1
                    },
                    new Element()
                    {
                        Attributes = list1
                    },
                    new Element()
                    {

                    },
                    new Element()
                    {
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "mounth",
                                Value = "january"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "link",
                                Value = "stylesheet"
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

            var ss = new Selector(root).QuerySelector(new List<Tuple<string, Action<string>>>()
                {
                    new Tuple<string,Action<string>>("[type=tag][href=$result]",w=>PrintItem(w,1)),
                    new Tuple<string,Action<string>>("[type=$result][href=nothing]",w=>PrintItem(w,2)),
                    new Tuple<string,Action<string>>("[Height=$result]",w=>PrintItem(w,3)),
                    new Tuple<string,Action<string>>("[name=$result]",w=>PrintItem(w,4)),
                    new Tuple<string,Action<string>>("*",w=>PrintItem(w,5))
                }).ToList();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            foreach (var item in ss)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }
}
