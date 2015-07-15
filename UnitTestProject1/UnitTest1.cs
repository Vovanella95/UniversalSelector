using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NewSuperModifyedSelector;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        #region Attributes
        static List<NewSuperModifyedSelector.Attribute> list1 = new List<NewSuperModifyedSelector.Attribute>()
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
        static List<NewSuperModifyedSelector.Attribute> list2 = new List<NewSuperModifyedSelector.Attribute>()
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
        Element root = new Element()
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
                        Id = "NewElement",
                        Name = "NewName",
                        Attributes = new List<NewSuperModifyedSelector.Attribute>()
                        {
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "NewAttr1",
                                Value = "NewValue1"
                            },
                            new NewSuperModifyedSelector.Attribute()
                            {
                                Name = "NewAttr2",
                                Value = "NewValue2"
                            }
                        }
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
        

        [TestMethod]
        public void SelectorMustDoSimpleSelects()
        {
            var list = new List<string>()
            {
                "hello", "I Am Vasya", "hello"
            };
            Match("#idshnick[href=nothing][type=tag][text=$result]", list);
        }

        [TestMethod]
        public void SelectorMustDoSimpleSelects2()
        {
            var list = new List<string>()
            {
                "hello", "I Am Vasya", "hello"
            };
            Match("TheName[href=nothing][text=$result]", list);
        }

        [TestMethod]
        public void SelectorMustDoSeveralSelects() // Three Selects For One Element
        {
            var k = (new Selector(root)).QuerySelector("[NewAttr1=NewValue1]", w => { }).First();
            var name = k.Name;
            Assert.IsTrue(name == "NewName");

            k = (new Selector(root)).QuerySelector("[NewAttr2=NewValue2]", w => { }).First();
            name = k.Name;
            Assert.IsTrue(name == "NewName");

            k = (new Selector(root)).QuerySelector("[NewAttr1=NewValue1][NewAttr2=NewValue2]", w => { }).First();
            name = k.Name;
            Assert.IsTrue(name == "NewName");
        }

        public void Match(string selector, IEnumerable<string> results)
        {
            var list = new List<string>();
            var k = (new Selector(root)).QuerySelector(selector, w => list.Add(w)).ToList();
            Assert.IsTrue(results.All(w => list.Contains(w)) && list.All(w => results.Contains(w)));
        }
    }
}
