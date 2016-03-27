using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
	public abstract class ScreenEntity : Collidable
	{
		// Constructor
		public ScreenEntity()
		{

		}

		// The toggle state
		private bool _isToggled;

		// Toggle the entity
		public void Toggle()
		{
			this._isToggled = !this._isToggled;
		}

		// Return the toggle state
		public bool IsToggled()
		{
			return this._isToggled;
		}
	}
}
