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
        static Element root = new Element()
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

        [TestMethod]
        public void QuerySelectorMustDoSimpleSelects()
        {
            TemplateElement template = new TemplateElement()
            {
                Name = "TheName",
                Attributes = list1
            };
            for (int i = 0; i < 10000; i++)
            {
                IsMatch(template, "Name = TheName, Id = idshnick|Name = TheName, Id = idshnick|");
            }
        }

        [TestMethod]
        public void QuerySelectorMustDoSimpleSelects2()
        {
            TemplateElement template = new TemplateElement()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    }
                }
            };

            for (int i = 0; i < 10000; i++)
            {
                IsMatch(template, "Name = TheName, Id = idshnick|Name = TheName, Id = idshnick|Name = , Id = |Name = , Id = idshnick|");
            }
            
        }





        private void IsMatch(TemplateElement template, string token)
        {
            var k = Selector.QuerySelector(template, root);
            string sum = "";
            foreach (var item in k)
            {
                sum += item.ToString()+"|";
            }
            Assert.IsTrue(token == sum);
        }
    }
}
