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
            IsMatch(template, "Name = TheName, Id = idshnick|Name = TheName, Id = idshnick|");
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
            IsMatch(template, "Name = TheName, Id = idshnick|Name = TheName, Id = idshnick|Name = , Id = |Name = , Id = idshnick|");
        }

        [TestMethod]
        public void QuerySelectorSelectUnfullAttributes12to2()
        {
            var template = new TemplateElement()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    }
                }
            };

            var element = new Element()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    }
                }
            };

            Assert.IsTrue(template.IsMatch(element));
        }

        [TestMethod]
        public void QuerySelectorSelectUnfullAttributes2to2()
        {
            var template = new TemplateElement()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    }
                }
            };

            var element = new Element()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    }
                }
            };

            Assert.IsTrue(template.IsMatch(element));
        }

        [TestMethod]
        public void QuerySelectorSelectUnfullAttributes1to2()
        {
            var template = new TemplateElement()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    }
                }
            };

            var element = new Element()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    }
                }
            };

            Assert.IsTrue(template.IsMatch(element));
        }

        [TestMethod]
        public void QuerySelectorSelectEmptyAttributes0to2()
        {
            var template = new TemplateElement()
            {

            };

            var element = new Element()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    }
                }
            };

            Assert.IsTrue(template.IsMatch(element));
        }

        [TestMethod]
        public void QuerySelectorSelectManyAttributes0toMany()
        {
            var template = new TemplateElement()
            {
                
            };

            var element = new Element()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                 new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr1",
                        Value = "val1"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "val2"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr3",
                        Value = "val3"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr4",
                        Value = "val4"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr5",
                        Value = "val5"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr6",
                        Value = "val6"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr7",
                        Value = "val7"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr8",
                        Value = "val8"
                    },
                        new NewSuperModifyedSelector.Attribute()

                    {
                        Name = "attr9",
                        Value = "val9"
                    },
                        new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr10",
                        Value = "val10"
                    }
                
                }
            };

            Assert.IsTrue(template.IsMatch(element));
        }

        [TestMethod]
        public void QuerySelectorMustReturnFalseIfConditionIsWrong()
        {
            var template = new TemplateElement()
            {
                Attributes = new List<NewSuperModifyedSelector.Attribute>()
                {
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "attr2",
                        Value = "value2"
                    },
                    new NewSuperModifyedSelector.Attribute()
                    {
                        Name = "text",
                        Value = "hello"
                    }
                }
            };

            var element = new Element()
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

            Assert.IsFalse(template.IsMatch(element));
        }

        private void IsMatch(TemplateElement template, string token)
        {
            var k = Selector.QuerySelector(template, root);
            string sum = "";
            foreach (var item in k)
            {
                sum += item.ToString() + "|";
            }
            Assert.IsTrue(token == sum);
        }
    }
}
