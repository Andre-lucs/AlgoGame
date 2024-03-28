using System;
using Godot;
namespace AlgoGame;
[Tool]
public partial class GameNumber : CharacterBody2D, Grabable
{
	[Export]
	public Double Number
	{
		get => int.Parse(GetNode<Label>("Label").Text);
		set => GetNode<Label>("Label").Text = value.ToString();
	}

	public Double GetValue()
	{
		return Number;
	}

}	