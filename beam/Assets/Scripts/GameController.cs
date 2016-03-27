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
		public GameObject Player1;
		public GameObject Player2;

		// The spikes
		public GameObject HorizontalSpikes;
		public GameObject VerticalSpikes;

		// Button base
		public GameObject ButtonBase;

		// Weight base
		public GameObject WeightBase;

		// A list of all collidables on the screen
		private IDictionary<Vector2, Collidable> _collidableList;

		// The title of the level
		private string _levelTitle;

		// Wait
		IEnumerator WaitForS(int i)
		{
			yield return new WaitForSeconds(i);
		}


		// Reset the level
		public void ResetLevel()
		{
			StartCoroutine( WaitForS(100));
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}

		// Generate the level based on the given file
		public void GenerateLevel(string levelDataPath)
		{
			// Initialize variables
			this._collidableList = new Dictionary<Vector2, Collidable>();

			// The position of the first sprite of the given entity
			var spritePosition = new List<int>() {-1,0,12,9,5,7,17};

			// Get the stream reader for the file
			var fileReader = new StreamReader(File.OpenRead(levelDataPath));
			
			// Read the title
			this._levelTitle = fileReader.ReadLine();

			// Read the players spawn location
			var line = fileReader.ReadLine();
			var playerX = int.Parse(line.Split(',')[0]);
			var playerY = int.Parse(line.Split(',')[1]);
			Player1.transform.position = TileCoordinate.TranslateToUnity(new Vector2(playerX, playerY));

			line = fileReader.ReadLine();
			playerX = int.Parse(line.Split(',')[0]);
			playerY = int.Parse(line.Split(',')[1]);
			Player2.transform.position = TileCoordinate.TranslateToUnity(new Vector2(playerX, playerY));

			line = fileReader.ReadLine();
			// Read the weight locations
			while (line[0] != '-')
			{
				var weightX= int.Parse(line.Split(',')[0]);
				var weightY= int.Parse(line.Split(',')[1]);
				var weightGameObject = Instantiate(WeightBase);
				weightGameObject.AddComponent<Weight>();
				var weightClass = weightGameObject.GetComponent<Weight>();
				weightClass.Initialize(new Vector2(weightX, weightY), this.SpriteList[16]);
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
				var splitLine = line.Split(new char[] { ',',':','{','}'},System.StringSplitOptions.RemoveEmptyEntries);
				var xPos = int.Parse(splitLine[0]);
				var yPos = int.Parse(splitLine[1]);
				var tileVector = new Vector2(xPos, yPos);

				// Get the entity and sprite ID
				var entityID = int.Parse(splitLine[2]);
				var spriteID = spritePosition[entityID] + int.Parse(splitLine[3]);

				// The copied new game object
				GameObject newGameObject = null;
				Collidable newGameObjectClass = null;

				switch (entityID)
				{
					case 1:
						{
							newGameObject = Instantiate(TileBase);
							newGameObject.AddComponent<Block>();
							newGameObjectClass = newGameObject.GetComponent<Block>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							newGameObject.tag = "Solid";
                            break;
						}
					case 2:
						{
							newGameObject = Instantiate(TileBase);
							newGameObject.AddComponent<Glass>();
							newGameObjectClass = newGameObject.GetComponent<Glass>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							newGameObject.tag = "Transparent";
							break;
						}
					case 3:
						{
							if (spriteID == 9)
							{
								newGameObject = Instantiate(HorizontalSpikes);
							}
							else
							{
								newGameObject = Instantiate(VerticalSpikes);
							}
							newGameObject.AddComponent<Spike>();
							newGameObjectClass = newGameObject.GetComponent<Spike>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							break;
						}
					case 4:
						{
							newGameObject = Instantiate(ButtonBase);
							newGameObjectClass = newGameObject.GetComponent<Button>();
							for (int i = 5; i < splitLine.Length; i += 2)
							{
								var childX = int.Parse(splitLine[i]);
								var childY = int.Parse(splitLine[i+1]);
								var searchVector = new Vector2(childX, childY);
								Collidable result;
								this._collidableList.TryGetValue(searchVector, out result);
								var switchClass = (Switch)newGameObjectClass;
								switchClass.AddToChildToggleObjectList((ScreenEntity)result);
                            }
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							break;
						}
					case 6:
						{
							newGameObject = Instantiate(TileBase);
							newGameObject.AddComponent<EndZone>();
							newGameObjectClass = newGameObject.GetComponent<EndZone>();
							newGameObjectClass.Initialize(tileVector, this.SpriteList[spriteID]);
							break;
						}
				}
				this._collidableList.Add(new KeyValuePair<Vector2, Collidable>(tileVector, newGameObjectClass));
			}

			fileReader.Close();
		}

		// Use this for initialization
		void Start()
		{
			this.GenerateLevel("Assets\\Levels\\testlevel.txt");
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}