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

	private CellType _cellType = CellType.Sand;
	private Color[] colors = new Color[262_144];

	// TODO add color randomization

	private readonly Dictionary<CellType, Color> cellColors = new()
	{
		{CellType.Sand, Color.yellow},
		{CellType.Water, Color.blue},
		{CellType.Gas, Color.red},
		{CellType.Stone, Color.gray},
		{CellType.Acid, Color.magenta},
		{CellType.Oil, Color.green},
		{CellType.Wood, (Color.red + Color.yellow) * 0.3f},
		{CellType.Fire, (Color.red + Color.yellow) * 0.5f},
		{CellType.FiringMaterial, (Color.red + Color.yellow) * 0.5f },
		{CellType.Explosion, Color.red },
	};

	private IEnumerator LengthCoroutine()
	{
		var delay = new WaitForSeconds(1f);
		for (; ; )
		{
			Debug.Log(simulation.cellArray.Length);
			yield return delay;
		}

	}

	private void Start()
	{
		StartCoroutine(LengthCoroutine());
	}


	private void Update()
	{
		if (Input.GetMouseButton(0))
			HandleMouse();

		if (simulation.cellArray.Length == 0) return;

		Parallel.ForEach(Partitioner.Create(0, colors.Length),
			(range, loopState) =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
					colors[i] = Color.black;
			}
		);

		Parallel.ForEach(
			Partitioner.Create(0, simulation.cellArray.Length),
			(range, loopState) =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
				{
					Cell* cell = simulation.cellArray[i];
					var index = ((cell->y << 9) + cell->x);
					var color = cellColors[cell->cellType];
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

			Cell* cellPtr = simulation.cellGrid[deltaY, deltaX];
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
	public void SetStone() => _cellType = CellType.Stone;
	public void SetErase() => _cellType = CellType.Empty;
	public void SetAcid() => _cellType = CellType.Acid;
	public void SetWood() => _cellType = CellType.Wood;
	public void SetOil() => _cellType = CellType.Oil;
	public void SetFire() => _cellType = CellType.Fire;
}