using UnityEngine;
using System.Collections;

public class Lines {
	
	/// <summary>
	/// Make the specified Line.
	/// example usage:
	/// <code>
	/// Lines.Make (ref forwardLine, Color.blue, transform.position,
	///             transform.position + transform.forward, 0.1f, 0);
	/// //This makes a long thin triangle, pointing forward.
	/// </code>
	/// </summary>
	/// <param name="lineObject">which-existing-GameObject /
	///             where-to-create-resulting-GameObject</param>
	/// <param name="color">Color.</param>
	/// <param name="start">Start, as an absolute coordinate</param>
	/// <param name="end">End, as an absolute coordinate</param>
	/// <param name="startSize">Start size.</param>
	/// <param name="endSize">End size.</param>
	public static LineRenderer Make(ref GameObject lineObject, Color color,
	                                Vector3 start, Vector3 end,
	                                float startSize, float endSize)
	{
		if(lineObject == null)
		{
			lineObject = new GameObject ();
		}
		LineRenderer lr = lineObject.GetComponent<LineRenderer> ();
		if(lr == null)
		{
			lr = lineObject.AddComponent<LineRenderer>();
		}
		lr.SetWidth(startSize, endSize);
		lr.SetVertexCount (2);
		lr.SetPosition(0,start); lr.SetPosition(1,end);
		SetColor (lr, color);
		return lr;
	}
	
	/// <summary>Make the specified Line from a list of points</summary>
	/// <param name="lineObject">which-existing-GameObject /
	///             where-to-create-resulting-GameObject</param>
	/// <param name="color">Color.</param>
	/// <param name="points">Points.</param>
	/// <param name="pointCount">Point count.</param>
	/// <param name="startSize">Start size.</param>
	/// <param name="endSize">End size.</param>
	public static LineRenderer Make(ref GameObject lineObject, Color color,
	                                Vector3[] points, int pointCount,
	                                float startSize, float endSize)
	{
		if(lineObject == null)
		{
			lineObject = new GameObject ();
		}
		LineRenderer lr = lineObject.GetComponent<LineRenderer> ();
		if(lr == null)
		{
			lr = lineObject.AddComponent<LineRenderer>();
		}
		lr.SetWidth(startSize, endSize);
		lr.SetVertexCount (pointCount);
		for(int i = 0; i < pointCount; ++i)
			lr.SetPosition(i,points[i]);
		SetColor (lr, color);
		return lr;
	}
	
	public static void SetColor(LineRenderer lr, Color color)
	{
		const string colorShaderName = "Self-Illumin/Diffuse";
		if(lr.material == null || lr.material.name != colorShaderName)
		{
			// Shader.Find won't export well! For most platforms, consider a
			// reference passed through a global variable
			lr.material = new Material(Shader.Find(colorShaderName));
		}
		lr.material.color = color;
	}
	
	/// <summary>Calculate a 2D arc in 3D space, into the given Vector3 array
	/// </summary>
	/// <param name="points">Points.</param>
	/// <param name="pointCount">How many vertices to use. Anything less than 1
	///             will cause problems</param>
	/// <param name="normal">The surface-normal of the plane that this arc is
	//              drawn on</param>
	/// <param name="startPositionRelativeToCenter">Where to start the arc
	//              (relative to the center)</param>
	/// <param name="angle">A two dimensional angle. Tip: Vector3.Angle(v1, v2)
	//              </param>
	/// <param name="offset">Where to make the center of this arc</param>
	public static void MakeArc(ref Vector3[] points, int pointCount,
	                           Vector3 normal, Vector3 startPositionRelativeToCenter,
	                           float angle, Vector3 offset)
	{
		if(points == null)
		{
			points = new Vector3[pointCount];
		}
		points [0] = startPositionRelativeToCenter;
		Quaternion q = Quaternion.AngleAxis (angle / (pointCount - 1), normal);
		for(int i = 1; i < pointCount; ++i)
		{
			points[i] = q * points[i-1];
		}
		for(int i = 0; i < pointCount; ++i)
		{
			points[i] += offset;
		}
	}
	
	/// <summary>
	/// Make the specified arc line in 3D space. Example usage: <code>
	/// Lines.MakeArc(ref turnArc, Color.green, transform.position,
	///     Vector3.Cross(transform.forward, direction), transform.forward,
	/// Vector3.Angle(transform.forward, direction), 10, 0.1f, 0);
	/// // makes a curve showing theturn from transform.forward to direction
	/// </code>
	/// </summary>
	/// <returns>The arc line renderer</returns>
	/// <param name="lineObject">which-existing-GameObject /
	///             where-to-create-resulting-GameObject</param>
	/// <param name="color">Color.</param>
	/// <param name="centerPosition">Where the center of this arc is (absolute
	//              coordinate)</param>
	/// <param name="normal">The surface-normal of the plane that this arc is
	///             drawn on</param>
	/// <param name="startPositionAsUnitVector">Where to start the arc
	///             (relative to the center)</param>
	/// <param name="angle">A two dimensional angle. Tip: Vector3.Angle(v1, v2)
	///             </param>
	/// <param name="pointCount">How many vertices to use. Anything less than 1
	///             will cause problems</param>
	/// <param name="startSize">Start size.</param>
	/// <param name="endSize">End size.</param>
	public static LineRenderer MakeArc(ref GameObject lineObject, Color color,
	                                   Vector3 centerPosition, Vector3 normal,
	                                   Vector3 startPositionRelativeToCenter,
	                                   float angle, int pointCount,
	                                   float startSize, float endSize)
	{
		Vector3[] points = null;
		MakeArc (ref points, pointCount, normal, startPositionRelativeToCenter,
		         angle, centerPosition);
		return Make (ref lineObject, color, points, pointCount, startSize,
		             endSize);
	}
}