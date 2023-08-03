using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private BoardGenerator boardGenerator;

    private void Awake()
    {
        transform.position = new Vector3((boardGenerator.N - 1) * boardGenerator.TilePrefabs[0].Y / 2,
            (boardGenerator.M - 1) * boardGenerator.TilePrefabs[0].X / 2, transform.position.z);
    }
}