﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Overcooked_Socket
{
    internal static class PlayerUtil
    {
        public static bool IsCarrying(PlayerControls playerControls)
        {
            return GetCarrying(playerControls) != null;
        }
        public static GameObject GetCarrying(PlayerControls playerControls)
        {
            ClientPlayerAttachmentCarrier clientCarrier =
                (ClientPlayerAttachmentCarrier)ReflectionUtil.GetValue(playerControls, "m_clientCarrier");

            IClientAttachment[] carriedObjects =
                (IClientAttachment[])ReflectionUtil.GetValue(clientCarrier, "m_carriedObjects");

            // Logger.Log($"Number of carried objects: {carriedObjects.Length}");
            for (int i = 0; i < carriedObjects.Length; i++)
            {

                // Logger.Log($"    Carried object is null: {carriedObjects[i] == null}");
                // Logger.Log($"    CarriedObject Type: {carriedObjects[i].GetType()}");
                // Logger.Log($"    Carried game object is null: {carriedObjects[i].AccessGameObject() == null}");

                if (carriedObjects[i] != null)
                {
                    if (carriedObjects[i].AccessGameObject() != null)
                    {
                        // Logger.Log($"    Carried game object type: {carriedObjects[i].AccessGameObject()}");
                        return carriedObjects[i].AccessGameObject();
                    }
                }
            }
            return null;
        }

        public static Vector3 GetChefPosition(PlayerControls playerControls)
        {
            return playerControls.transform.position;
        }

        public static Vector3 GetChefPosition(int chef)
        {
            PlayerControls[] playerControls = GameObject.FindObjectsOfType<PlayerControls>();
            return playerControls[chef].transform.position;
        }
        public static float GetChefAngles(PlayerControls playerControls)
        {
            return playerControls.transform.rotation.eulerAngles.y;
        }

        public static float GetChefAngles(int chef)
        {
            PlayerControls[] playerControls = GameObject.FindObjectsOfType<PlayerControls>();
            return playerControls[chef].transform.rotation.eulerAngles.y;
        }

        public static float GetAngleFacingDiff(PlayerControls player, Component componentToFace)
        {
            Vector3 playerPos = player.transform.position;
            Vector3 compPos = componentToFace.transform.position;

            float rot = player.transform.rotation.eulerAngles.y;

            float xDif = Math.Abs(playerPos.x - compPos.x);
            float zDif = Math.Abs(playerPos.z - compPos.z);

            // Logger.Log($"AngleFacingDif method: rot={rot}, xDif={xDif}, zDif={zDif}");

            if (xDif > zDif)
            {
                if (playerPos.x > compPos.x)
                {
                    // Should be 270
                    if (rot < 90)
                    {
                        rot += 360;
                    }
                    return Math.Abs(270 - rot);
                }

                // Should be 90
                if (rot > 270)
                {
                    rot -= 360;
                }

                return Math.Abs(90 - rot);
            }
            if (playerPos.z < compPos.z)
            {
                // Should be 0
                if (rot > 180)
                {
                    rot -= 360;
                }

                return Math.Abs(rot);
            }

            // Should be 180 
            return Math.Abs(180 - rot);
        }
    }
}
