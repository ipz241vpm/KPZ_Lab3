using System;
using System.Collections.Generic;

// TEMPLATE METHOD (життєвий цикл елементів)

abstract class LightNode
{
    public abstract void Accept(IVisitor visitor);
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
    public override void Accept(IVisitor visitor)
    {
        visitor.VisitText(this);
    }
}

// ELEMENT NODE

class LightElementNode : LightNode
{
    //Команд

    // Словник для зберігання команд: назва події -> список команд
    private Dictionary<string, List<ICommand>> eventListeners = new Dictionary<string, List<ICommand>>();
    // Метод для реєстрації команди
    public void AddEventListener(string eventType, ICommand command)
    {
        if (!eventListeners.ContainsKey(eventType))
            eventListeners[eventType] = new List<ICommand>();

        eventListeners[eventType].Add(command);
    }
    // Метод для імітації виклику події
    public void TriggerEvent(string eventType)
    {
        Console.WriteLine($"[EVENT] Подія '{eventType}' на тегу <{tagName}>");
        if (eventListeners.ContainsKey(eventType))
        {
            foreach (var command in eventListeners[eventType])
            {
                command.Execute(); 
            }
        }
    }

    //Для візітор
    public override void Accept(IVisitor visitor)
    {
        visitor.VisitElement(this);

        foreach (var child in children)
        {
            child.Accept(visitor);
        }
    }

    private string tagName;
    private bool isSelfClosing;

    private List<string> classes = new List<string>();
    private List<LightNode> children = new List<LightNode>();
    public List<LightNode> GetChildren()
    {
        return children;
    }
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


//Iterator

interface ILightIterator
{
    bool HasNext();
    LightNode Next();
}
class DepthFirstIterator : ILightIterator
{
    private Stack<LightNode> stack = new Stack<LightNode>();

    public DepthFirstIterator(LightNode root)
    {
        stack.Push(root);
    }

    public bool HasNext()
    {
        return stack.Count > 0;
    }

    public LightNode Next()
    {
        var current = stack.Pop();

        if (current is LightElementNode element)
        {
            var children = element.GetChildren();

            for (int i = children.Count - 1; i >= 0; i--)
            {
                stack.Push(children[i]);
            }
        }

        return current;
    }
}

//Команд
public interface ICommand
{
    void Execute(); 
}
class ClickCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Дія: Елемент було натиснуто! (Логіка команди)");
    }
}

class LogHoverCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("Дія: Мишка наведена на елемент.");
    }
}
//visitor
interface IVisitor
{
    void VisitText(LightTextNode textNode);
    void VisitElement(LightElementNode elementNode);
}

class CountVisitor : IVisitor
{
    public int Count = 0;

    public void VisitText(LightTextNode textNode)
    {
        Count++;
    }

    public void VisitElement(LightElementNode elementNode)
    {
        Count++;
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


        Console.WriteLine("\n=== ITERATOR DFS ===");

        var iterator = new DepthFirstIterator(ul);

        while (iterator.HasNext())
        {
            var node = iterator.Next();
            Console.WriteLine(node.OuterHTML());
        }


        // 1. Створюємо об'єкти команд
        var clickAction = new ClickCommand();
        var hoverAction = new LogHoverCommand();

        // 2. Підписуємо елементи на події
        li1.AddEventListener("click", clickAction);
        li2.AddEventListener("mouseover", hoverAction);

        // 3. Імітуємо події (ніби користувач клацнув у браузері)
        Console.WriteLine("\n=== TESTING COMMANDS ===");
        li1.TriggerEvent("click");
        li2.TriggerEvent("mouseover");

        Console.WriteLine("\n=== VISITOR ===");

        var visitor = new CountVisitor();
        ul.Accept(visitor);

        Console.WriteLine("Total nodes: " + visitor.Count);
    }
}