using System;
using System.Linq;
using MyBox;
using ProjectAwakening.DungeonGeneration.Rooms;
using StowyTools.Logger;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectAwakening.DungeonGeneration.UI
{
	public class UIDungeonDrawer : MonoBehaviour
	{
		[SerializeField] private GameObject roomImagePrefab;
		[SerializeField] private RectTransform parent;
		[SerializeField] private float roomSize = 10f;

		#region Sprites

		[Foldout("Sprites", true)]
		[SerializeField] private Sprite t;
		[SerializeField] private Sprite b;
		[SerializeField] private Sprite l;
		[SerializeField] private Sprite r;
		[SerializeField] private Sprite tb;
		[SerializeField] private Sprite tl;
		[SerializeField] private Sprite tr;
		[SerializeField] private Sprite lr;
		[FormerlySerializedAs("lb")] [SerializeField]
		private Sprite bl;
		[SerializeField] private Sprite br;
		[SerializeField] private Sprite tlr;
		[SerializeField] private Sprite blr;
		[FormerlySerializedAs("lbt")] [SerializeField]
		private Sprite tbl;
		[FormerlySerializedAs("rtb")] [SerializeField]
		private Sprite tbr;
		[SerializeField] private Sprite tblr;
		[SerializeField] private Sprite start;
		[SerializeField] private Sprite end;
		[SerializeField] private Sprite error;

		#endregion

		public void DrawDungeon(RoomMap[,] rooms)
		{
			EmptyParent();

			int roomCount = rooms.Cast<RoomMap>().Count(room => !DungeonGenerator.IsRoomEmpty(room));
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

		private void DrawRooms(RoomMap[,] rooms, Vector2Int middlePosInArray, Vector2 middlePosInParent)
		{
			foreach (RoomMap room in rooms)
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

		private void SetRoomImage(RoomMap room, Image roomImage)
		{
			switch (room.Type)
			{
				case RoomType.Basic:
					roomImage.sprite = room.Neighborhood.Type switch
					{
						NeighborhoodType.Bottom => b,
						NeighborhoodType.BottomLeft => bl,
						NeighborhoodType.BottomLeftRight => blr,
						NeighborhoodType.BottomRight => br,
						NeighborhoodType.Left => l,
						NeighborhoodType.LeftRight => lr,
						NeighborhoodType.Right => r,
						NeighborhoodType.Top => t,
						NeighborhoodType.TopBottom => tb,
						NeighborhoodType.TopBottomLeft => tbl,
						NeighborhoodType.TopBottomLeftRight => tblr,
						NeighborhoodType.TopBottomRight => tbr,
						NeighborhoodType.TopLeft => tl,
						NeighborhoodType.TopLeftRight => tlr,
						NeighborhoodType.TopRight => tr,
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