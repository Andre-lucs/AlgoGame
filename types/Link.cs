using Godot;
using System;
using AlgoGame;

public partial class Link : Node2D
{
	public Linkable<Grabable> Item1 { get; set; }
	public Linkable<Grabable> Item2 { get; set; }

	private Vector2[] positions = {Vector2.Zero, Vector2.Zero,Vector2.Zero, Vector2.Zero,Vector2.Zero,Vector2.Zero};
	public override void _Draw()
	{
		base._Draw();
		if (Item1 != null && Item2 != null)
		{
			positions[0] = ToLocal(Item1.GetGlobalPosition());
			positions[1] = ToLocal(Item2.GetGlobalPosition());
			positions[2] = positions[1];
			positions[3] = positions[1]+ (new Vector2(-10, 5).Rotated((positions[1] - positions[0]).Angle()));
			positions[4] = positions[1]+ (new Vector2(-10, -5).Rotated((positions[1] - positions[0]).Angle()));
			positions[5] = positions[1];
			DrawLine(positions[0], positions[1], Colors.White, 4);
			DrawLine(positions[2], positions[3], Colors.White, 4);
			DrawLine(positions[3], positions[4], Colors.White, 4);
			DrawLine(positions[4], positions[5], Colors.White, 4);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (!positions[0].Equals(Item1.GetGlobalPosition()) || !positions[1].Equals(Item2.GetGlobalPosition()))
			QueueRedraw();
	}

	public void MakeLink()
	{
		if (Item2 is null or Player) return;
		
		Item1.AddLink(this);
		if (Item1.GetRefValue() == null) return;
		Item2.SetRefValue(Item1.GetRefValue());
	}

	public void PropagateLinkRef()
	{
		GD.Print("PropagateLinkRef"+ this.Name);
		if (Item2.GetRefValue() == Item1.GetRefValue()) return;
		Item2.SetRefValue(Item1.GetRefValue());
	}

	public void Unlink()
	{
		if(Item2 is Player || Item2 == null) return;		
		Item2.SetRefValue(null);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		Unlink();
	}
}
