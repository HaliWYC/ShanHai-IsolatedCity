using ShanHai_IsolatedCity.Astar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace ShanHai_IsolatedCity.Astar
{
    public class AStarTest : MonoBehaviour
    {
        private AStar aStar;
        [Header("For Test")]
        public Vector2Int startPos;
        public Vector2Int finishPos;
        public Tilemap displayMap;
        public TileBase displayTile;
        public bool displayStartAndFinish;
        public bool displayPath;

        private Stack<MovementStep> npcMovmentStepStack;

        private void Awake()
        {
            aStar = GetComponent<AStar>();
            npcMovmentStepStack = new Stack<MovementStep>();
        }

        private void Update()
        {
            showPathOnGridMap();
        }

        private void showPathOnGridMap()
        {
            if (displayMap != null && displayTile != null)
            {
                if (displayStartAndFinish)
                {
                    displayMap.SetTile((Vector3Int)startPos, displayTile);
                    displayMap.SetTile((Vector3Int)finishPos, displayTile);
                }
                else
                {
                    displayMap.SetTile((Vector3Int)startPos, null);
                    displayMap.SetTile((Vector3Int)finishPos, null);
                }

                if (displayPath)
                {
                    var sceneName = SceneManager.GetActiveScene().name;

                    aStar.buildPath(sceneName, startPos, finishPos, npcMovmentStepStack);

                    foreach (var step in npcMovmentStepStack)
                    {
                        displayMap.SetTile((Vector3Int)step.gridCoordinate, displayTile);
                    }
                }
                else
                {
                    if (npcMovmentStepStack.Count > 0)
                    {
                        foreach (var step in npcMovmentStepStack)
                        {
                            displayMap.SetTile((Vector3Int)step.gridCoordinate, null);
                        }
                        npcMovmentStepStack.Clear();
                    }
                }
            }
        }
    }
}