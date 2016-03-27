using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts
{
	public class GameController : MonoBehaviour
	{
		// The base entities to be copied from
		public GameObject TileBase;

		// List of all sprites
		public List<Sprite> SpriteList;

		// The players
		public GameObject _player1;
		public GameObject _player2;

		// A list of all collidables on the screen
		private IDictionary<Vector2, Collidable> _collidableList;

		// The title of the level
		private string _levelTitle;

		// Generate the level based on the given file
		public void GenerateLevel(string levelDataPath)
		{
			// Initialize variables
			this._collidableList = new Dictionary<Vector2, Collidable>();

			// The position of the first sprite of the given entity
			var spritePosition = new List<int>() {0,0,5,8,10,13,17};

			// Get the stream reader for the file
			var fileReader = new StreamReader(File.OpenRead(levelDataPath));
			
			// Read the title
			this._levelTitle = fileReader.ReadLine();

			// Read the players
			var line = fileReader.ReadLine();
			line = fileReader.ReadLine();

			// Read the weight locations
			while (line[0] != '-')
			{
				line = fileReader.ReadLine();
			}

			// Read through the file
			while (!fileReader.EndOfStream)
			{
				// read in the next line
				line = fileReader.ReadLine();

				// If empty line, skip
				if (line == null || line.Length == 0)
				{
					continue;
				}

				// Check for comment line
				if (line[0] == '#')
				{
					continue;
				}

				// Split the line and get the position
				var splitLine = line.Split(new char[] { ',',':','{','}'});
				var xPos = int.Parse(splitLine[0]);
				var yPos = int.Parse(splitLine[1]);
				var tileVector = new Vector2(xPos, yPos);

				// Get the entity and sprite ID
				var entityID = int.Parse(splitLine[2]);
				var spriteID = spritePosition[entityID] + int.Parse(splitLine[3]);

				// The copied new game object
				var newGameObject = Instantiate(TileBase);
				Collidable newGameObjectClass = null;

				switch (entityID)
				{
					case 1:
						{
							newGameObject.AddComponent<Block>();
							newGameObjectClass = newGameObject.GetComponent<Block>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							
                            break;
						}
					case 2:
						{
							newGameObject.AddComponent<Glass>();
							newGameObjectClass = newGameObject.GetComponent<Glass>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							break;
						}
					case 3:
						{
							newGameObject.AddComponent<Spike>();
							newGameObjectClass = newGameObject.GetComponent<Spike>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							break;
						}
					case 6:
						{
							newGameObject.AddComponent<EndZone>();
							newGameObjectClass = newGameObject.GetComponent<EndZone>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							break;
						}
				}
			}

			fileReader.Close();
		}

		// Use this for initialization
		void Start()
		{
			this.GenerateLevel("Assets\\Levels\\Level1.txt");
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}