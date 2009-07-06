namespace MapinfoWrapper.Geometries.Lines
{
	public interface IMILine : IGeometry
	{
		decimal GetLength(string unit);
	}
}
