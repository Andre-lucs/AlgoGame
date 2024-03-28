using Godot;
namespace AlgoGame;

public interface  Grabable 
{
	public void PutAt(Vector2 position, Node node = null)
	{
		if (this is not Node th) return;
		th.Reparent(node ?? th.GetTree().Root);
		th.CreateTween().TweenProperty(th, "position", position, .2);
	}
}