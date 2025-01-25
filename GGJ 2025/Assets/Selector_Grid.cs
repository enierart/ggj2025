using UnityEngine;

public class Selector3x3 : MonoBehaviour
{
    private int rows = 3; // Número de filas
    private int cols = 3; // Número de columnas
    private int selectedRow = 0; // Fila actual seleccionada
    private int selectedCol = 0; // Columna actual seleccionada

    private Cell[,] grid; // Matriz de celdas personalizadas

    public GameObject cellPrefab; // Prefab para cada celda
    public Transform gridParent; // Padre para contener la matriz
    public float spacing = 1.5f; // Espaciado entre cubos

    public float cooldownDuration = 0.5f; // Duración del cooldown en segundos
    private float cooldownTimer = 0f; // Temporizador del cooldown

    private void Start()
    {
        GenerateGrid();
        HighlightSelected();
    }

    private void Update()
    {
        int prevRow = selectedRow;
        int prevCol = selectedCol;

        // Actualizar temporizador del cooldown
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Navegación con teclas
        if (Input.GetKeyDown(KeyCode.UpArrow))
            selectedRow = Mathf.Clamp(selectedRow - 1, 0, rows - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            selectedRow = Mathf.Clamp(selectedRow + 1, 0, rows - 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            selectedCol = Mathf.Clamp(selectedCol - 1, 0, cols - 1);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            selectedCol = Mathf.Clamp(selectedCol + 1, 0, cols - 1);

        // Alternar estado al presionar espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleOccupied();
        }

        // Actualiza el resaltado solo si cambió la selección
        if (prevRow != selectedRow || prevCol != selectedCol)
        {
            HighlightSelected();
        }
    }

    private void GenerateGrid()
    {
        grid = new Cell[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Crear celda
                GameObject cellObject = Instantiate(cellPrefab, gridParent);
                cellObject.transform.position = new Vector3(j * spacing, -i * spacing, 0);
                cellObject.name = $"Cell {i},{j}";

                // Configurar celda
                Cell cell = new Cell
                {
                    gameObject = cellObject,
                    ocupado = false
                };

                grid[i, j] = cell;
            }
        }
    }

    private void HighlightSelected()
    {
        // Restablece el estado visual de todas las celdas
        foreach (var cell in grid)
        {
            cell.gameObject.GetComponent<Renderer>().material.color = cell.ocupado ? Color.blue : Color.white;
        }

        // Resalta la celda seleccionada
        grid[selectedRow, selectedCol].gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private void ToggleOccupied()
    {
        if (cooldownTimer > 0)
        {
            // No se puede alternar el estado durante el cooldown
            return;
        }

        Cell selectedCell = grid[selectedRow, selectedCol];

        if (!selectedCell.ocupado)
        {
            selectedCell.ocupado = true;
            cooldownTimer = cooldownDuration; // Inicia el cooldown

            // Actualiza visualmente el estado
            HighlightSelected();
        }
    }
}

public class Cell
{
    public GameObject gameObject; // Referencia al objeto del grid
    public bool ocupado; // Estado de la celda
}
