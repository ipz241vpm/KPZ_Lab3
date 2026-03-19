using System;

// БАЗА 
abstract class Hero
{
    public abstract string GetDescription();
    public abstract int GetPower();
}

// ГЕРОЇ 
class Warrior : Hero
{
    public override string GetDescription() => "Warrior";
    public override int GetPower() => 10;
}

class Mage : Hero
{
    public override string GetDescription() => "Mage";
    public override int GetPower() => 8;
}

class Paladin : Hero
{
    public override string GetDescription() => "Paladin";
    public override int GetPower() => 12;
}

// ===== ДЕКОРАТОР =====
abstract class HeroDecorator : Hero
{
    protected Hero hero;

    public HeroDecorator(Hero hero)
    {
        this.hero = hero;
    }

    public override string GetDescription() => hero.GetDescription();
    public override int GetPower() => hero.GetPower();
}

// ===== ІНВЕНТАР =====
class Sword : HeroDecorator
{
    public Sword(Hero hero) : base(hero) { }

    public override string GetDescription() => hero.GetDescription() + " + Sword";
    public override int GetPower() => hero.GetPower() + 5;
}

class Armor : HeroDecorator
{
    public Armor(Hero hero) : base(hero) { }

    public override string GetDescription() => hero.GetDescription() + " + Armor";
    public override int GetPower() => hero.GetPower() + 3;
}

class Ring : HeroDecorator
{
    public Ring(Hero hero) : base(hero) { }

    public override string GetDescription() => hero.GetDescription() + " + Ring";
    public override int GetPower() => hero.GetPower() + 2;
}

class Program
{
    static void Main()
    {
        Hero hero = new Warrior();

        hero = new Sword(hero);
        hero = new Armor(hero);
        hero = new Ring(hero);
        hero = new Sword(hero);

        Console.WriteLine("Hero: " + hero.GetDescription());
        Console.WriteLine("Power: " + hero.GetPower());
    }
}