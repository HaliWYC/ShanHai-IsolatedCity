﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShanHai_IsolatedCity.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("地图信息")]
        public List<MapData_SO> mapDataList;

        //SceneName+postion and matched Tilemap information
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

        private Grid currentGrid;


        private void Start()
        {
            foreach (var mapData in mapDataList)
            {
                initTileDetailsDict(mapData);
            }
        }

        /// <summary>
        /// Generate Dictionary accroding to the Map information
        /// </summary>
        /// <param name="mapData">Map information</param>
        private void initTileDetailsDict(MapData_SO mapData)
        {
            foreach (TileProperty tileProperty in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperty.tileCoordinate.x,
                    gridY = tileProperty.tileCoordinate.y
                };

                //Key of dictionary
                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;

                if (getTileDetails(key) != null)
                {
                    tileDetails = getTileDetails(key);
                }

                switch (tileProperty.gridType)
                {
                    case GridType.可投掷区:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.近战区:
                        tileDetails.meleeOnly = tileProperty.boolTypeValue;
                        break;
                    case GridType.远程区:
                        tileDetails.rangedOnly = tileProperty.boolTypeValue;
                        break;
                    case GridType.NPC障碍:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }

                if (getTileDetails(key) != null)
                    tileDetailsDict[key] = tileDetails;
                else
                    tileDetailsDict.Add(key, tileDetails);
            }
        }

        private void OnEnable()
        {
            EventHandler.executeActionAfterAnimation += onExecuteActionAfterAnimation;
            EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;
        }

        private void OnDisable()
        {
            EventHandler.executeActionAfterAnimation -= onExecuteActionAfterAnimation;
            EventHandler.afterSceneLoadedEvent += onAfterSceneLoadedEvent;
        }

        private void onAfterSceneLoadedEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
        }

        private void onExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = getTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                switch (itemDetails.itemType)
                {
                    case ItemType.商品:
                        EventHandler.callDropItemEvent(itemDetails.itemID, mouseWorldPos);
                        break;
                }
            }

        }

        /// <summary>
        /// Return Tilemap information accroding to the key
        /// </summary>
        /// <param name="key">x+y+Name of Map</param>
        /// <returns></returns>
        private TileDetails getTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
            {
                return tileDetailsDict[key];
            }
            return null;
        }

        /// <summary>
        /// Return tilemap information accroding to the mouse position
        /// </summary>
        /// <param name="mouseGridPos">mouse postion</param>
        /// <returns></returns>
        public TileDetails getTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return getTileDetails(key);
        }
    }
}
