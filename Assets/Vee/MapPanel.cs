using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : MonoBehaviour {

	public bool ShowGridLine = true;
	public float TileWidth = 70;
	public float TileHeight = 38;
	float TileWidthHalf;
	float TileHeightHalf;
	public int MapSizeX = 10;
	public int MapSizeY = 20;

	public Transform tileTest;

	public static MapPanel instance;

	Rect m_MapRect;
	// Use this for initialization
	void Start () {
		MapPanel.instance = this;
		TileWidthHalf = TileWidth / 2f;
		TileHeightHalf = TileHeight / 2f;
		m_MapRect = new Rect (transform.position.x, transform.position.y + TileHeight*MapSizeY, TileWidth*MapSizeX, TileHeight*MapSizeY);

		Instantiate(tileTest, MapPanel.DiamondGridToMapPos(new Vector2(1,-1)) ,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// 检测 diamond坐标是否合法
//	public static bool IsAvailableDiamondGrid (Vector2 dGrid) {
//		if (dGrid.x < 0 || dGrid.x > (MapPanel.instance.MapSizeX + MapPanel.instance.MapSizeY - 2))
//			return false;
//
//	}

	//将diamond地图坐标转换为地图坐标(世界坐标)
	public static Vector3 DiamondGridToMapPos (Vector2 dGrid) {
		int x = Mathf.FloorToInt(dGrid.x);
		int y = Mathf.FloorToInt(dGrid.y);
		return new Vector3 ((x-y+1)*MapPanel.instance.TileWidthHalf, (x+y+1)*MapPanel.instance.TileHeightHalf, 0);
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		// rect
		float rootX = transform.position.x;
		float rootY = transform.position.y;
		float rootZ = transform.position.z;
		float mapWidth = TileWidth * MapSizeX;
		float mapHeight = TileHeight * MapSizeY;
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, new Vector3(rootX, rootY + mapHeight, rootZ));
		Gizmos.DrawLine(transform.position, new Vector3(rootX + mapWidth, rootY, rootZ));
		Gizmos.DrawLine(new Vector3(rootX, rootY + mapHeight, rootZ), new Vector3(rootX + mapWidth, rootY + mapHeight, rootZ));
		Gizmos.DrawLine(new Vector3(rootX + mapWidth, rootY, rootZ), new Vector3(rootX + mapWidth, rootY + mapHeight, rootZ));

		// 斜格线
		if (ShowGridLine) {
			float halfWidth = TileWidth / 2;
			float halfHeight = TileHeight / 2;

			Gizmos.color = Color.blue;
			int gridNum = Mathf.Max(MapSizeX, MapSizeY);
			for (int i = 0; i < gridNum; ++i) {
				Gizmos.DrawLine(new Vector3(rootX + i*TileWidth + halfWidth, rootY, rootZ), new Vector3(rootX, rootY + i*TileHeight + halfHeight, rootZ));
				Gizmos.DrawLine(new Vector3(rootX + i*TileWidth + halfWidth, rootY, rootZ), new Vector3(rootX + gridNum*TileWidth, rootY + (gridNum - 1 -i)*TileHeight + halfHeight, rootZ));
				Gizmos.DrawLine(new Vector3(rootX, rootY + i*TileHeight + halfHeight, rootZ), new Vector3(rootX + (gridNum - 1 - i)*TileWidth + halfWidth, rootY + gridNum*TileHeight, rootZ));
				Gizmos.DrawLine(new Vector3(rootX + gridNum*TileWidth, rootY + (gridNum - 1 -i)*TileHeight + halfHeight, rootZ), new Vector3(rootX + (gridNum - 1 - i)*TileWidth + halfWidth, rootY + gridNum*TileHeight, rootZ));
			}
		}
	}
	#endif
}
