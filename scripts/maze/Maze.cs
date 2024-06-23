using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using global::mazetank.scripts.global;

namespace mazetank.scripts.maze;
public partial class Maze : Node2D
{
	[Export] public PackedScene CellScene { get; set; }
	
	private Vector2I _size;
	private int _randomWallsCount;
	private RandomNumberGenerator _rng;
	public List<List<Cell>> _maze;
	
	public override void _Ready()
	{
		_maze = new List<List<Cell>>();
		_size = Global.GameSettings.GetSize();
		_randomWallsCount = Global.GameSettings.GetRandomWallsDestroy();
		
		_rng = new RandomNumberGenerator();
		_rng.Seed = (ulong)Global.GameSettings.GetSeed();
	}

	private List<List<Cell>> CreateArrayMaze()
	{
		var maze = new List<List<Cell>>();
		// Create
		for (int i = 0; i <= _size.X; i++)
		{
			maze.Add(new List<Cell>());
			for (int j = 0; j <= _size.Y; j++)
			{
				var cell = (Node2D)CellScene.Instantiate();
				((Cell)cell).Init(i, j);
				cell.Name = $"{i} {j}";
				maze[i].Add((Cell)cell);
			}
		}

		// Destroy extra
		for (int i = 0; i <= _size.X; i++)
		{
			maze[i][0].DestroyLeftWall();
			maze[i][0].DestroyFloor();
		}
		for (int j = 0; j <= _size.Y; j++)
		{
			maze[_size.X][j].DestroyBottomWall();
			maze[_size.X][j].DestroyFloor();
		}

		return maze;
	}

	private void BacktrackingPath()
	{
		Cell currentCell = _maze[0][1];
		currentCell.Visited = true;
		Stack<Cell> stack = new Stack<Cell>();

		while (true)
		{
			var unvisitedCells = new List<Cell>();
			int x = currentCell.Pos.X;
			int y = currentCell.Pos.Y;

			if (x > 0 && !_maze[x - 1][y].Visited)
				unvisitedCells.Add(_maze[x - 1][y]);
			if (y > 1 && !_maze[x][y - 1].Visited)
				unvisitedCells.Add(_maze[x][y - 1]);
			if (x < _size.X - 1 && !_maze[x + 1][y].Visited)
				unvisitedCells.Add(_maze[x + 1][y]);
			if (y < _size.Y && !_maze[x][y + 1].Visited)
				unvisitedCells.Add(_maze[x][y + 1]);

			if (unvisitedCells.Count > 0)
			{
				var chosenCell = unvisitedCells[_rng.RandiRange(0, unvisitedCells.Count - 1)];
				RemoveWall(currentCell, chosenCell);
				chosenCell.Visited = true;
				stack.Push(currentCell);
				currentCell = chosenCell;
			}
			else
			{
				if (stack.Count > 0)
				{
					currentCell = stack.Pop();
				}
				else
				{
					break;
				}
			}
		}
	}

	private void RemoveWall(Cell current, Cell chosen)
	{
		if (current.Pos.X == chosen.Pos.X)
		{
			if (chosen.Pos.Y > current.Pos.Y)
			{
				current.DestroyBottomWall();
			}
			else
			{
				chosen.DestroyBottomWall();
			}
		}
		else
		{
			if (current.Pos.X > chosen.Pos.X)
			{
				current.DestroyLeftWall();
			}
			else
			{
				chosen.DestroyLeftWall();
			}
		}
	}

	private void RemoveRandomWall()
	{
		int q = 0;
		while (q < _randomWallsCount)
		{
			int x = _rng.RandiRange(1, _maze.Count - 2);
			int y = _rng.RandiRange(1, _maze[0].Count - 2);
			Cell cell = _maze[x][y];

			if (cell.LeftWall)
			{
				cell.DestroyLeftWall();
			}
			else if (cell.BottomWall)
			{
				cell.DestroyBottomWall();
			}
			else
			{
				q--;
			}
			q++;
		}
	}

	private void RemoveExtraColumn()
	{
		for (int x = 0; x <= _size.X; x++)
		{
			for (int y = 0; y <= _size.Y; y++)
			{
				Cell cell = _maze[x][y];
				if (!cell.LeftWall && !cell.BottomWall)
				{
					if (!_maze[x - 1][y].BottomWall && !_maze[x][y + 1].LeftWall)
					{
						cell.DestroyColumnWall();
					}
				}
			}
		}
	}

	public void FinishMaze()
	{
		_maze = CreateArrayMaze();
		BacktrackingPath();
		RemoveRandomWall();
		RemoveExtraColumn();
	}
	
	public Cell GetRandomCell()
	{
		var x = _rng.RandiRange(0, _maze.Count - 2);
		var y = _rng.RandiRange(1, _maze[0].Count - 1);
		return _maze[x][y];
	}
}
