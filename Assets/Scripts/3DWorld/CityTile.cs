using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTile : MonoBehaviour
{
    [System.Serializable]
    public class Connection
    {
        public enum ConnectionTypes
        {
            None,
            Street,
            Building,
        }
        public enum Side
        {
            Top,
            Right,
            Bottom,
            Left,
        }

        public Vector2 position;
        public ConnectionTypes type;
        public Side side;

        public void DrawGizmo(Transform transform)
        {
            switch (type)
            {
                case ConnectionTypes.None:
                    Gizmos.color = Color.black;
                    break;
                case ConnectionTypes.Street:
                    Gizmos.color = Color.green;
                    break;
                case ConnectionTypes.Building:
                    Gizmos.color = Color.red;
                    break;
            }
            Gizmos.DrawWireCube(transform.TransformPoint(position.ToVector3XZ()), Vector3.one);
        }
    }

    public Connection[] connections = new Connection[4];

    void Awake()
    {
        int yRot = Mathf.RoundToInt(transform.rotation.eulerAngles.y) % 360;
        for (int angle = yRot; angle > 0; angle -= 90)
        {
            Rotate90();
        }
    }

    [ContextMenu("Rotate 90°")]
    public void Rotate90()
    {
        foreach (var connection in connections)
        {
            switch (connection.side)
            {
                case Connection.Side.Top:
                    connection.side = Connection.Side.Left;
                    connection.position = new Vector2(-connection.position.y, connection.position.x);
                    break;
                case Connection.Side.Right:
                    connection.side = Connection.Side.Top;
                    connection.position = new Vector2(connection.position.y, -connection.position.x);
                    break;
                case Connection.Side.Bottom:
                    connection.side = Connection.Side.Right;
                    connection.position = new Vector2(-connection.position.y, connection.position.x);
                    break;
                case Connection.Side.Left:
                    connection.side = Connection.Side.Bottom;
                    connection.position = new Vector2(connection.position.y, -connection.position.x);
                    break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        foreach (Connection connection in connections)
        {
            connection.DrawGizmo(transform);
        }
    }
}
