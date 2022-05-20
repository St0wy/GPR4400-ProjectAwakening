using System;

namespace ProjectAwakening.Overworld.WaveFunctionCollapse
{
    [Serializable]
    public class TileWfc : IEquatable<TileWfc>
    {
        public int Rotation { get; }
        public int Id { get; }


        public TileWfc(int id, int rotation)
        {
            Id = id;
            Rotation = rotation;
        }

        public override bool Equals(object obj)
        {
	        return obj != null && Equals((TileWfc)obj);
        }

        public bool Equals(TileWfc other)
        {
            if (other == null)
                return false;

            return other.Id == Id && other.Rotation == Rotation;
        }

        public override int GetHashCode()
        {
            int hashCode = -623507625;
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }
    }
}