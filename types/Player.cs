using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using AlgoGame;
using Container = AlgoGame.Container;

public partial class Player : CharacterBody2D, Linkable<Grabable>
{
	[Export] public PackedScene LinkScene;
	
	private const float WalkSpeed = 200.0f;
	private Vector2 _targetVelocity = Vector2.Zero;
	private float _targetVelChange = 1f;
	private Area2D grabArea;
	private Node2D pivot;
	private Grabable ItemHold = null;
	private int justGrabbed;
	private Link currentLink = null;
	private Linkable<Grabable> _linkableImplementation;

	public override void _Ready()
	{
		base._Ready();
		grabArea = GetNode<Area2D>("Pivot/GrabArea");
		pivot = GetNode<Node2D>("Pivot");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		GrabAction();
		LinkAction();
	}

	private void LinkAction()
	{
		if (Input.IsActionJustPressed("create_link"))
		{
			var bodies = grabArea.GetOverlappingBodies();
			Linkable<Grabable> componentToLink = null;
			foreach (var i in bodies)
			{
				if (i is Linkable<Grabable> c) componentToLink = c;
			}

			if (componentToLink != null)
			{
				var	newLink = LinkScene.Instantiate<Link>();
				newLink.Item1 = componentToLink;
				newLink.Item2 = this;
				currentLink = newLink;
				GetTree().Root.AddChild(newLink);
			}
		}else if (Input.IsActionJustReleased("create_link") && currentLink != null)
		{
			var bodies = grabArea.GetOverlappingBodies();
			Linkable<Grabable> componentToLink = null;
			foreach (var i in bodies)
			{
				if (i is Linkable<Grabable> c) componentToLink = c;
			}

			if (componentToLink == null)
			{
				currentLink.QueueFree();
			}
			else
			{
				if (!ReferenceEquals(currentLink.Item1, componentToLink) && !ReferenceEquals(this, componentToLink))
				{
					currentLink.Item2 = componentToLink;
					currentLink.MakeLink();
				}
				else
				{
					currentLink.QueueFree();
				}
			}

			currentLink = null;
		}
	}

	private void GrabAction()
	{
		if (!Input.IsActionJustPressed("grab")) return;
		var bodies = grabArea.GetOverlappingBodies();
		Container container = null;
		foreach (var i in bodies)
		{
			if (i is Container c) container = c;
		}
		
		if (ItemHold!= null)
		{
			ThrowItem(container);
			return;
		};

		if (container != null)
		{
			GrabItem(container);
			return;
		}

		if (bodies.Count <= 0) return;
		GrabItem(bodies.First() as Grabable);
	}

	private void GrabItem(Grabable item)
	{
		ItemHold = item;
		item.PutAt(Vector2.Zero, GetNode("GrabPos"));
	}

	private void ThrowItem(Container container = null)
	{
		GD.Print("pre "+ ItemHold);
		if (ItemHold is Container || container == null)// to ground
		{
			ItemHold.PutAt(GetParent<Node2D>().ToLocal(GetNode<Area2D>("Pivot/GrabArea").GlobalPosition+_targetVelocity/5), GetParent());
			ItemHold = null;
		}
		else // to container
		{
			GD.Print("to container");
			container.SetItem(ItemHold);
			ItemHold = null;
		}
		GD.Print("pos "+ ItemHold);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetVector("mv_l", "mv_r", "mv_u", "mv_d");
		direction = direction.Normalized();
		if (direction != Vector2.Zero)
		{
			pivot.Rotation = direction.Angle();
			
			_targetVelocity = direction * WalkSpeed;
			_targetVelChange = 4f;
		}
		else
		{
			_targetVelocity = direction * WalkSpeed;
			_targetVelChange = 8f;
		}

		Velocity = Velocity.Lerp(_targetVelocity, (float)delta*_targetVelChange);
		MoveAndSlide();
	}


	public void SetRefValue(Grabable value)
	{
		throw new NotImplementedException();
	}

	public Grabable GetRefValue()
	{
		return ItemHold;
	}

	public List<Link> GetLinks()
	{
		var list = new List<Link>();
		list.Add(currentLink);
		return list;
	}

	public void AddLink(Link link)
	{
		throw new NotImplementedException();
	}

	public void RemoveLink(Link link)
	{
		throw new NotImplementedException();
	}
}
