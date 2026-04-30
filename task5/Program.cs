using System;
using System.Collections.Generic;

// TEMPLATE METHOD (життєвий цикл елементів)

abstract class LightNode
{
    public LightNode()
    {
        OnCreated(); // викликається при створенні
    }

    // lifecycle hooks
    protected virtual void OnCreated() { }
    public virtual void OnInserted() { }
    protected virtual void OnRendered() { }

    public abstract string OuterHTML();
    public abstract string InnerHTML();
}

// TEXT NODE

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

// ELEMENT NODE

class LightElementNode : LightNode
{
    private string tagName;
    private bool isSelfClosing;

    private List<string> classes = new List<string>();
    private List<LightNode> children = new List<LightNode>();

    public LightElementNode(string tagName, bool isSelfClosing)
    {
        this.tagName = tagName;
        this.isSelfClosing = isSelfClosing;
    }

    // TEMPLATE METHOD hooks (реалізація)

    protected override void OnCreated()
    {
        Console.WriteLine($"[LIFECYCLE] <{tagName}> created");
    }

    public override void OnInserted()
    {
        Console.WriteLine($"[LIFECYCLE] <{tagName}> inserted");
    }

    protected override void OnRendered()
    {
        Console.WriteLine($"[LIFECYCLE] <{tagName}> rendered");
    }

    // BASIC METHODS

    public void AddClass(string className)
    {
        classes.Add(className);
    }

    public void AddChild(LightNode node)
    {
        children.Add(node);
        node.OnInserted(); // виклик lifecycle
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
        OnRendered(); // lifecycle

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

// MAIN

class Program
{
    static void Main()
    {
        var ul = new LightElementNode("ul", false);
        ul.AddClass("menu");

        var li1 = new LightElementNode("li", false);
        li1.AddChild(new LightTextNode("Item 1"));

        var li2 = new LightElementNode("li", false);
        li2.AddChild(new LightTextNode("Item 2"));

        var li3 = new LightElementNode("li", false);
        li3.AddChild(new LightTextNode("Item 3"));

        ul.AddChild(li1);
        ul.AddChild(li2);
        ul.AddChild(li3);

        Console.WriteLine("\n=== HTML ===");
        Console.WriteLine(ul.OuterHTML());
    }
}