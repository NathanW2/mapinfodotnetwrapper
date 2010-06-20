namespace Mapinfo.Wrapper.Geometries.Lines
{
	public interface ILine : IGeometry
	{
		decimal GetLength(string unit);
	}
}
