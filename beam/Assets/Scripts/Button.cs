﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
	public class Button:Switch
	{
		// Constructor
		public void Initialize()
		{
			this._childToggleObjectList = new List<ScreenEntity>();
		}
	}
}
