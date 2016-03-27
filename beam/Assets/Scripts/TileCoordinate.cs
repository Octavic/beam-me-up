using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public static class TileCoordinate
	{
		public static Vector2 TranslateToUnity(Vector2 tileCoordinate)
		{
			return new Vector2(tileCoordinate.x * 0.32f, -tileCoordinate.y * 0.32f);
		}
	}
}
