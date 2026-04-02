using System;
using System.Collections.Generic;


abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();
}

// ТЕКСТ 
class LightTextNode : LightNode
{
    private string text;

    public LightTextNode(string text)
    {
        this.text = text;
    }

    public override string OuterHTML()
    {
        return text;
    }

    public override string InnerHTML()
    {
        return text;
    }
}

// ЕЛЕМЕНТ 
class LightElementNode : LightNode
{
    private string tagName;
    private bool isBlock;
    private bool isSelfClosing;

    private List<string> classes = new List<string>();
    private List<LightNode> children = new List<LightNode>();
    private Dictionary<string, List<Action>> eventListeners = new Dictionary<string, List<Action>>();
    public void AddEventListener(string eventType, Action listener)
    {
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Action>();
        }

        eventListeners[eventType].Add(listener);
    }
    public void TriggerEvent(string eventType)
    {
        Console.WriteLine($"[EVENT] {tagName} -> {eventType}");

        if (eventListeners.ContainsKey(eventType))
        {
            foreach (var listener in eventListeners[eventType])
            {
                listener();
            }
        }
    }
    public LightElementNode(string tagName, bool isBlock, bool isSelfClosing)
    {
        this.tagName = tagName;
        this.isBlock = isBlock;
        this.isSelfClosing = isSelfClosing;
    }

    public void AddClass(string className)
    {
        classes.Add(className);
    }

    public void AddChild(LightNode node)
    {
        children.Add(node);
    }

    public int ChildrenCount()
    {
        return children.Count;
    }

    public override string InnerHTML()
    {
        string result = "";

        foreach (var child in children)
        {
            result += child.OuterHTML();
        }

        return result;
    }

    public override string OuterHTML()
    {
        string classAttr = classes.Count > 0
            ? $" class=\"{string.Join(" ", classes)}\""
            : "";

        if (isSelfClosing)
        {
            return $"<{tagName}{classAttr}/>";
        }

        return $"<{tagName}{classAttr}>{InnerHTML()}</{tagName}>";
    }
}

class Program
{
    static void Main()
    {
        LightElementNode ul = new LightElementNode("ul", true, false);
        ul.AddClass("menu");

        LightElementNode li1 = new LightElementNode("li", true, false);
        li1.AddChild(new LightTextNode("Item 1"));

        LightElementNode li2 = new LightElementNode("li", true, false);
        li2.AddChild(new LightTextNode("Item 2"));

        LightElementNode li3 = new LightElementNode("li", true, false);
        li3.AddChild(new LightTextNode("Item 3"));

        ul.AddChild(li1);
        ul.AddChild(li2);
        ul.AddChild(li3);

        Console.WriteLine("OuterHTML:");
        Console.WriteLine(ul.OuterHTML());

        Console.WriteLine("\nInnerHTML:");
        Console.WriteLine(ul.InnerHTML());

        Console.WriteLine("\nChildren count: " + ul.ChildrenCount());
        Console.WriteLine("\n=== EVENTS ===");
        // підписка
        li1.AddEventListener("click", () =>
        {
            Console.WriteLine("Item 1 clicked!");
        });

        li2.AddEventListener("mouseover", () =>
        {
            Console.WriteLine("Item 2 hovered!");
        });
        // виклик
        li1.TriggerEvent("click");
        li2.TriggerEvent("mouseover");
    }
}