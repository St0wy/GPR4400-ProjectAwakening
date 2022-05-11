﻿using System;
using System.Linq;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAwakening.DungeonGeneration
{
	public class DungeonDrawer : MonoBehaviour
	{
		[SerializeField] private GameObject roomImagePrefab;
		[SerializeField] private RectTransform parent;
		[SerializeField] private float roomSize = 10f;

		#region Sprites

		[SerializeField] private Sprite t;
		[SerializeField] private Sprite b;
		[SerializeField] private Sprite l;
		[SerializeField] private Sprite r;
		[SerializeField] private Sprite tb;
		[SerializeField] private Sprite tl;
		[SerializeField] private Sprite tr;
		[SerializeField] private Sprite lr;
		[SerializeField] private Sprite lb;
		[SerializeField] private Sprite br;
		[SerializeField] private Sprite tlr;
		[SerializeField] private Sprite blr;
		[SerializeField] private Sprite lbt;
		[SerializeField] private Sprite rtb;
		[SerializeField] private Sprite tblr;
		[SerializeField] private Sprite start;
		[SerializeField] private Sprite end;
		[SerializeField] private Sprite error;

		#endregion

		public void DrawDungeon(Room[,] rooms)
		{
			EmptyParent();

			int roomCount = rooms.Cast<Room>().Count(room => !DungeonGenerator.IsRoomEmpty(room));
			this.Log($"Room count: {roomCount}");

			Rect rect = parent.rect;
			Vector2 middleParent = new(rect.width / 2, rect.height / 2);

			int width = rooms.GetLength(0);
			int length = rooms.GetLength(1);
			Vector2Int middleRooms = new(width / 2, length / 2);
			var drawnRooms = new bool[width, length];
			DrawRoomsRecursive(rooms, middleRooms, middleParent, drawnRooms);
			int drawnCount = drawnRooms.Cast<bool>().Count(isDrawn => isDrawn);
			this.Log($"Drawn count: {drawnCount}");
		}

		private void EmptyParent()
		{
			foreach (Transform child in parent)
			{
				Destroy(child.gameObject);
			}
		}

		private void DrawRoomsRecursive(Room[,] rooms, Vector2Int posInArray, Vector2 posInParent, bool[,] drawnRooms)
		{
			Room room = rooms[posInArray.x, posInArray.y];

			drawnRooms[posInArray.x, posInArray.y] = true;

			GameObject roomImageObject = Instantiate(
				roomImagePrefab,
				posInParent,
				Quaternion.identity,
				parent
			);

			var roomImage = roomImageObject.GetComponent<Image>();

			var topPos = new Vector2Int(posInArray.x, posInArray.y - 1);
			var bottomPos = new Vector2Int(posInArray.x, posInArray.y + 1);
			var leftPos = new Vector2Int(posInArray.x - 1, posInArray.y);
			var rightPos = new Vector2Int(posInArray.x + 1, posInArray.y);

			bool topIsValid = IsValidRoom(rooms, topPos);
			bool bottomIsValid = IsValidRoom(rooms, bottomPos);
			bool leftIsValid = IsValidRoom(rooms, leftPos);
			bool rightIsValid = IsValidRoom(rooms, rightPos);

			SetRoomImage(topIsValid, bottomIsValid, leftIsValid, rightIsValid, room, roomImage);

			if (topIsValid && !drawnRooms[topPos.x, topPos.y])
			{
				DrawRoomsRecursive(rooms, topPos, new Vector2(posInParent.x, posInParent.y + roomSize), drawnRooms);
			}
			else
			{
				this.Log("Testo");
			}

			if (bottomIsValid && !drawnRooms[bottomPos.x, bottomPos.y])
				DrawRoomsRecursive(rooms, bottomPos, new Vector2(posInParent.x, posInParent.y - roomSize), drawnRooms);
			else
				this.Log("Testo");
			if (leftIsValid && !drawnRooms[leftPos.x, leftPos.y])
				DrawRoomsRecursive(rooms, leftPos, new Vector2(posInParent.x - roomSize, posInParent.y), drawnRooms);
			else
				this.Log("Testo");
			if (rightIsValid && !drawnRooms[rightPos.x, rightPos.y])
				DrawRoomsRecursive(rooms, rightPos, new Vector2(posInParent.x + roomSize, posInParent.y), drawnRooms);
			else
				this.Log("Testo");
		}

		private void SetRoomImage(
			bool topIsValid,
			bool bottomIsValid,
			bool leftIsValid,
			bool rightIsValid,
			Room room,
			Image roomImage)
		{
			int neighborCount =
				(topIsValid ? 1 : 0) + (bottomIsValid ? 1 : 0) +
				(leftIsValid ? 1 : 0) + (rightIsValid ? 1 : 0);

			switch (room.Type)
			{
				case RoomType.Basic:
					roomImage.sprite = neighborCount switch
					{
						// ReSharper disable ConvertIfStatementToSwitchStatement
						1 when topIsValid => t,
						1 when bottomIsValid => b,
						1 when rightIsValid => r,
						1 when leftIsValid => l,
						1 => error,
						2 when topIsValid && leftIsValid => tl,
						2 when topIsValid && bottomIsValid => tb,
						2 when topIsValid && rightIsValid => tr,
						2 when leftIsValid && bottomIsValid => lb,
						2 when leftIsValid && rightIsValid => lr,
						2 when bottomIsValid && rightIsValid => br,
						2 => error,
						3 when topIsValid && leftIsValid && rightIsValid => tlr,
						3 when topIsValid && leftIsValid && bottomIsValid => lbt,
						3 when topIsValid && rightIsValid && bottomIsValid => rtb,
						3 when leftIsValid && bottomIsValid && rightIsValid => blr,
						3 => error,
						4 => tblr,
						_ => error,
					};

					break;
				case RoomType.Start:
					roomImage.sprite = start;
					break;
				case RoomType.Final:
					roomImage.sprite = end;
					break;
				case RoomType.Empty:
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static bool IsValidRoom(Room[,] rooms, Vector2Int pos)
		{
			int width = rooms.GetLength(0);
			int length = rooms.GetLength(1);

			bool isOutOfBound = pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= length;

			if (isOutOfBound) return false;

			Room room = rooms[pos.x, pos.y];

			if (room == null) return false;

			return room.Type != RoomType.Empty;
		}
	}
}