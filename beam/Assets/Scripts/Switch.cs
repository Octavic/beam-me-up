using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class Switch : ScreenEntity
	{
		public Sprite overlaySprite;

		// A list of child objects to be toggled
		private IList<ScreenEntity> childToggleObjectList;

		// Constructor
		public Switch()
		{
			this.childToggleObjectList = new List<ScreenEntity>();
		}

		// Add to the child objects
		public void AddToChildToggleObjectList(ScreenEntity entity)
		{
			this.childToggleObjectList.Add(entity);
		}
	}
}
