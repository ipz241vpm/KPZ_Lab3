using System;
using System.Collections.Generic;
using System.IO;

abstract class LightNode
{
    public abstract string OuterHTML();
}

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
}

class LightElementTemplate
{
    public string TagName;
    public bool IsBlock;
    public bool IsSelfClosing;

    public LightElementTemplate(string tag, bool block, bool selfClosing)
    {
        TagName = tag;
        IsBlock = block;
        IsSelfClosing = selfClosing;
    }
}

class ElementFactory
{
    private static Dictionary<string, LightElementTemplate> templates = new();

    public static LightElementTemplate GetTemplate(string tag)
    {
        if (!templates.ContainsKey(tag))
        {
            templates[tag] = new LightElementTemplate(tag, true, false);
        }

        return templates[tag];
    }

    public static int TemplateCount()
    {
        return templates.Count;
    }
}

class LightElementNode : LightNode
{
    private LightElementTemplate template;
    private List<LightNode> children = new();

    public LightElementNode(string tag)
    {
        template = ElementFactory.GetTemplate(tag);
    }

    public void AddChild(LightNode node)
    {
        children.Add(node);
    }

    public override string OuterHTML()
    {
        string inner = "";

        foreach (var child in children)
        {
            inner += child.OuterHTML();
        }

        return $"<{template.TagName}>{inner}</{template.TagName}>";
    }
}

class Program
{
    static void Main()
    {
        string path = "book.txt";

        string[] lines = File.ReadAllLines(path);

        List<LightNode> nodes = new();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string tag;

            if (i == 0)
                tag = "h1";
            else if (line.StartsWith(" "))
                tag = "blockquote";
            else if (line.Length < 20)
                tag = "h2";
            else
                tag = "p";

            LightElementNode element = new LightElementNode(tag);
            element.AddChild(new LightTextNode(line));

            nodes.Add(element);
        }

        Console.WriteLine("=== HTML ===");
        foreach (var node in nodes)
        {
            Console.WriteLine(node.OuterHTML());
        }

        int totalNodes = nodes.Count;
        int templates = ElementFactory.TemplateCount();

        Console.WriteLine("\n=== Memory Info ===");
        Console.WriteLine("Total nodes: " + totalNodes);
        Console.WriteLine("Unique templates (Flyweight): " + templates);
    }
}