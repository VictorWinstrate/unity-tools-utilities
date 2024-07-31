using UnityEngine;

public static class MathHelpers
{
	public const int RightAngle = 90;
	public const int StraightAngle = 180;
	public const int RevolutionAngle = 360;

	public enum Axis
	{
		XAxis = 0,
		YAxis = 1,
		ZAxis = 2,
	}

	public static bool IsBetweenTwoValues(double min, double value, double max) =>
		min <= value && value <= max;

	public static bool IsBetweenTwoValues(float min, float value, float max) =>
		min <= value && value <= max;

	public static bool IsBetweenTwoValues(int min, int value, int max) =>
		min <= value && value <= max;

	public static float GetLeastCongruentAngle(float angle)
	{
		if (angle > RightAngle)
			return GetLeastCongruentAngle(angle - StraightAngle);

		if (angle < -RightAngle)
			return GetLeastCongruentAngle(angle + StraightAngle);

		return angle;
	}

	public static float GetVector2Distance(float x1, float y1, float x2, float y2) =>
		Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2));

	public static Vector3 QuadraticBezier(Vector3 origin, Vector3 control, Vector3 destination, float t)
	{
		float oneMinusT = 1f - t;

		return
			(oneMinusT * oneMinusT * origin) +
			(2f * t * oneMinusT * control) +
			(t * t * destination);
	}

	public static Vector3 CubicBezier(Vector3 origin, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 destination, float t)
	{
		float u = 1f - t;
		float u2 = u * u;
		float u3 = u2 * u;

		float t2 = t * t;
		float t3 = t2 * t;

		var position = u3 * origin;
		position += 3 * u2 * t * controlPoint1;
		position += 3 * u * t2 * controlPoint2;
		position += t3 * destination;

		return position;
	}
}
