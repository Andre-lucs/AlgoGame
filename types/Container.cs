using System.CodeDom.Compiler;
using System.Collections.Generic;
using Godot;

namespace AlgoGame;

public partial class Container : CharacterBody2D, Grabable, Linkable<Grabable>
{
	[Export] public bool Sticky = false;
	private Grabable Item { get; set; } = null;
	private List<Link> links { get; set; } = new List<Link>();


	public Grabable TakeOff(Node newParent)
	{
		if (Item is Node nod)
		{
			nod.Reparent(newParent);
		}
		Item = null;
		return Item;
	}

	public void SetItem(Grabable itemHold)
	{
		GD.Print("setitem "+itemHold);
		if (ReferenceEquals(this, itemHold) || itemHold == null)
		{
			return;
		}
		SetRefValue(itemHold);//manda para a logica
		itemHold.PutAt(Vector2.Zero, GetNode("."));
	}

	public void SetRefValue(Grabable value)
	{
		if (value == null)
		{
			GD.Print(value);
			RemoveChild(Item as Node2D);
			Item = null;
			return;
		}
		Item = value;
		if (Item is Node2D nodeItem)
		{
			AddChild(nodeItem.Duplicate());
		}
		foreach (var link in links)
		{
			GD.Print("setreflink " + link);
			link.PropagateLinkRef();
		}
		
	}

	public Grabable GetRefValue()
	{
		return Item;
	}

	public List<Link> GetLinks()
	{
		return links;
	}

	public void AddLink(Link link)
	{
		if(!links.Contains(link)) links.Add(link);
	}

	public void RemoveLink(Link link)
	{
		links.Remove(link);
	}
}