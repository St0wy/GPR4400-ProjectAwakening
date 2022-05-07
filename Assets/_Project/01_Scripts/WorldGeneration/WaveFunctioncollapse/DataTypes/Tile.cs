using System;

[Serializable]
public class Tile : IEquatable<Tile>
{   
    public int Rotation { get; set; }
    public int Id { get; set; }


    public Tile(int id, int rotation)
    {
        Id = id;
        Rotation = rotation;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        return Equals((Tile)obj);
    }

    public bool Equals(Tile other)
    {
        if (other == null)
            return false;

        if (other.Id == Id && other.Rotation == Rotation)
            return true;
        return false;
    }

    public override int GetHashCode()
    {
        int hashCode = -623507625;
        hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
        hashCode = hashCode * -1521134295 + Id.GetHashCode();
        return hashCode;
    }
}
