using System;
using System.Linq;
using MyBox;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAwakening.DungeonGeneration.UI
{
	public class UIDungeonDrawer : MonoBehaviour
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
			int height = rooms.GetLength(1);
			Vector2Int middleRooms = new(width / 2, height / 2);
			var drawnRooms = new bool[width, height];
			DrawRooms(rooms, middleRooms, middleParent);
			// DrawRoomsRecursive(rooms, middleRooms, middleParent, drawnRooms);
			// int drawnCount = drawnRooms.Cast<bool>().Count(isDrawn => isDrawn);
			// this.Log($"Drawn count: {drawnCount}");
		}

		private void EmptyParent()
		{
			foreach (Transform child in parent)
			{
				Destroy(child.gameObject);
			}
		}

		private void DrawRooms(Room[,] rooms, Vector2Int middlePosInArray, Vector2 middlePosInParent)
		{
			foreach (Room room in rooms)
			{
				if (room == null || room.Type == RoomType.Empty) continue;

				Vector2 posFromMiddle = new Vector2(room.Pos.x, room.Pos.y) - middlePosInArray;
				Vector2 offset = posFromMiddle * roomSize;
				offset = new Vector2(offset.x, -offset.y);
				Vector2 posInParent = middlePosInParent + offset;
				GameObject roomImageObject = Instantiate(
					roomImagePrefab,
					posInParent,
					Quaternion.identity,
					parent
				);

				var roomImage = roomImageObject.GetComponent<Image>();
				SetRoomImage(room, roomImage);

				var roomBehaviour = roomImageObject.GetOrAddComponent<RoomBehaviour>();
				roomBehaviour.room = room;
			}
		}

		private void SetRoomImage(Room room, Image roomImage)
		{
			int neighborCount = room.Neighborhood.Count;

			switch (room.Type)
			{
				case RoomType.Basic:
					roomImage.sprite = neighborCount switch
					{
						// ReSharper disable ConvertIfStatementToSwitchStatement
						1 when room.Neighborhood.Top => t,
						1 when room.Neighborhood.Bottom => b,
						1 when room.Neighborhood.Right => r,
						1 when room.Neighborhood.Left => l,
						1 => error,
						2 when room.Neighborhood.Top && room.Neighborhood.Left => tl,
						2 when room.Neighborhood.Top && room.Neighborhood.Bottom => tb,
						2 when room.Neighborhood.Top && room.Neighborhood.Right => tr,
						2 when room.Neighborhood.Left && room.Neighborhood.Bottom => lb,
						2 when room.Neighborhood.Left && room.Neighborhood.Right => lr,
						2 when room.Neighborhood.Bottom && room.Neighborhood.Right => br,
						2 => error,
						3 when room.Neighborhood.Top && room.Neighborhood.Left && room.Neighborhood.Right => tlr,
						3 when room.Neighborhood.Top && room.Neighborhood.Left && room.Neighborhood.Bottom => lbt,
						3 when room.Neighborhood.Top && room.Neighborhood.Right && room.Neighborhood.Bottom => rtb,
						3 when room.Neighborhood.Left && room.Neighborhood.Bottom && room.Neighborhood.Right => blr,
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
	}
}