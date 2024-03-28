using System.Collections.Generic;
using Godot;
namespace AlgoGame;

public interface Linkable<T>
{
	public void SetRefValue(T? value);
	public T GetRefValue();
	public List<Link> GetLinks();
	public void AddLink(Link link);
	public void RemoveLink(Link link);

	public Vector2 GetGlobalPosition()
	{
		if (this is Node2D th)
		{
			return th.GlobalPosition;
		}

		return Vector2.Zero;
	}
}