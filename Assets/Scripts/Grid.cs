using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Grid :MonoBehaviour
{
	public static Grid Instance { get; set; }
	public Vector3 targetPosition = new Vector3(3.5f, 0, 3.5f);
	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;
	public GameObject whitePiecePrefab;
	public GameObject blackPiecePrefab;
	public Piece[,] pieces = new Piece[7, 7];
	int t = 0;
	Piece king;
	Piece blackking;
	private Client client;
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	int pieceCount;
	int cubeCount;
	int kingX;
	int kingY;
	int blackKingX;
	int blackKingY;
	int players = 0;

	public void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		pieceCount = PlayerPrefs.GetInt("pieceCount");
		cubeCount = PlayerPrefs.GetInt("cubeCount");
		players = PlayerPrefs.GetInt("playerCount");
		CreateGrid();
	}

	

    public int MaxSize
	{
		get
		{
			return gridSizeX * gridSizeY;
		}
	}

	public void CreateGrid()
	{
		Instance = this;
		client = FindObjectOfType<Client>();

		if (client)
		{
			Debug.Log("clientcame");
		}
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

				grid[x, y] = new Node(walkable, worldPoint, x, y);

			}
		}
		// Cube oluşturma
		t = 0;
		while (true)
		{
			int x = Random.Range(0, 7);
			int y = Random.Range(0, 7);
			if (grid[x, y].walkable == true)
			{
				if (pieces[x, y] == null)
				{
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
					GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cubeObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
					cubeObj.transform.position = worldPoint;
					cubeObj.layer = LayerMask.NameToLayer("Unwalkable");
					grid[x, y].walkable = false;
					t++;
					if (t == cubeCount)
						break;
				}
			}
		}
		
		t = 0;
		GenerateKingPiece();
		while (true)
		{
			int x = Random.Range(0, 7);
			int y = Random.Range(0, 7);
			if (grid[x, y].walkable == true)
			{
				if (pieces[x, y] == null)
				{				
						Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
						GameObject go = Instantiate(whitePiecePrefab);
						go.transform.SetParent(transform);
						Piece p = go.GetComponent<Piece>();
						pieces[x, y] = p;
						p.name = "piece";
						p.tag = "pieces";
						p.target = king.transform;
						MovePiece(p, worldPoint);
						t++;
						if (t == pieceCount)
							break;					
			}
			}
		}
		if (Client.isMulti)
		{
			GenerateBlackKingPiece();
			t = 0;
			while (true)
			{
				int x = Random.Range(0, 7);
				int y = Random.Range(0, 7);
				if (grid[x, y].walkable == true)
				{
					if (pieces[x, y] == null)
					{

						Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
						GameObject go = Instantiate(blackPiecePrefab);
						go.transform.SetParent(transform);
						Piece p = go.GetComponent<Piece>();
						pieces[x, y] = p;
						p.name = "piece";
						p.tag = "pieces";
						p.target = blackking.transform;
						MovePiece(p, worldPoint);
						t++;
						if (t == pieceCount)
							break;
					}
				}
			}
		}		
	}



	public void GenerateKingPiece()
    {
		while (true)
		{
		
			int x = Random.Range(0, 7);
			int y = Random.Range(0, 7);
			if (grid[x, y].walkable == true)
			{
				if (pieces[x, y] == null)
				{
					Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
					
					GameObject go = Instantiate(whitePiecePrefab);
					go.transform.SetParent(transform);
					Piece p = go.GetComponent<Piece>();
					pieces[x, y] = p;
					kingX = x;
					kingY = y;
					p.target = null;
					p.isKing = true;
					p.transform.Rotate(Vector3.right * 180);
					king = p;
					MovePiece(p, worldPoint);
					break;
				}
			}
		}
	}

	public void GenerateBlackKingPiece()
	{
		while (true)
		{

			int x = Random.Range(0, 7);
			int y = Random.Range(0, 7);
			if (grid[x, y].walkable == true)
			{
				if (pieces[x, y] == null)
				{
					Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

					GameObject go = Instantiate(blackPiecePrefab);
					go.transform.SetParent(transform);
					Piece p = go.GetComponent<Piece>();
					pieces[x, y] = p;
					p.target = null;
					p.isKing = true;
					p.transform.Rotate(Vector3.right * 180);
					blackking = p;
					MovePiece(p, worldPoint);
					break;
				}
			}
		}
	}

	


	public List<Node> GetNeigbours(Node node)
	{
		List<Node> neighbours = new List<Node>();
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX - x;
				int checkY = node.gridY - y;
				if (Mathf.Abs(node.gridX - checkX) == 1 && Mathf.Abs(node.gridY - checkY) == 1)
				{

				}
				else
				{
					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
					{
						neighbours.Add(grid[checkX, checkY]);
					}
				}

			}
		} 

		return neighbours;
	}


	public void MovePiece(Piece p, Vector3 position)
    {
		p.transform.position = position;
	}



    public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	public List<Node> path;
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		if (grid != null)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				if (path != null)
					if (path.Contains(n))
						Gizmos.color = Color.black;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}

	}
}