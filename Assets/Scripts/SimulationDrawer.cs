using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public unsafe class SimulationDrawer : MonoBehaviour
{
	[SerializeField] private Texture2D texture;
	[SerializeField] private int brushRadius;
	[SerializeField] private Simulation simulation;
	[SerializeField] private SimulationUpdater simulationUpdater;

	private CellType _cellType = CellType.Sand;
	private Color[] colors = new Color[262_144];



	private void Update()
	{
		if (Input.GetMouseButton(0))
			HandleMouse();

		if (simulation.Length == 0) return;

		Parallel.ForEach(Partitioner.Create(0, colors.Length),
			(range, loopState) =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
					colors[i] = Color.black;
			}
		);

		Parallel.ForEach(
			Partitioner.Create(0, simulation.Length),
			(range, loopState) =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
				{
					Cell* cell = simulation[i];
					var index = ((cell->y << 9) + cell->x);
					var color = simulationUpdater.cellUpdaters[cell->cellType].GetColor(cell);
					colors[index] = color;
				}
			}
		);

		texture.SetPixels(colors);
		texture.Apply();
	}

	private void HandleMouse()
	{
		var mousePos = Input.mousePosition;
		var x = mousePos.x / Screen.width * 512;
		var y = mousePos.y / Screen.height * 512;
		DrawInCircle((int)x, (int)y);
	}

	private void DrawInCircle(int x, int y)
	{

		for (var _ = 0; _ < 40; _++)
		{
			int deltaX = x + Random.Range(-brushRadius, brushRadius);
			int deltaY = y + Random.Range(-brushRadius, brushRadius);

			if (deltaX < 0 || deltaX > 511) continue;
			if (deltaY < 0 || deltaY > 511) continue;

			Cell* cellPtr = simulation[deltaY, deltaX];
			bool cellIsEmpty = cellPtr->cellType == CellType.Empty;

			if (_cellType == CellType.Empty && (!cellIsEmpty))
				simulation.Remove(deltaX, deltaY);
			else if (_cellType != CellType.Empty && cellIsEmpty)
				simulation.Add(deltaX, deltaY, _cellType);
		}
	}

	public void SetSand() => _cellType = CellType.Sand;
	public void SetGas() => _cellType = CellType.Gas;
	public void SetWater() => _cellType = CellType.Water;
	public void SetWall() => _cellType = CellType.Wall;
	public void SetErase() => _cellType = CellType.Empty;
	public void SetAcid() => _cellType = CellType.Acid;
	public void SetWood() => _cellType = CellType.Wood;
	public void SetOil() => _cellType = CellType.Oil;
	public void SetFire() => _cellType = CellType.Fire;
	public void SetStone() => _cellType = CellType.Stone;
	public void SetIce() => _cellType = CellType.Ice;
	public void SetPlant() => _cellType = CellType.Plant;
	public void SetSeed() => _cellType = CellType.Seed;
}