using UnityEngine;
using System.Collections;
using UnityEngine.UI; //Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
    //Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
    public class Player : MovingObject
    {
        [SerializeField] private float restartLevelDelay = 1f; //Delay time in seconds to restart level.
        [SerializeField] private int pointsPerFood = 10; //Number of points to add to player food points when picking up a food object.
        [SerializeField] private int pointsPerSoda = 20; //Number of points to add to player food points when picking up a soda object.
        [SerializeField] private int wallDamage = 1; //How much damage a player does to a wall when chopping it.
        [SerializeField] private Text foodText; //UI Text to display current player food total.
        [SerializeField] private AudioClip moveSound1; //1 of 2 Audio clips to play when player moves.
        [SerializeField] private AudioClip moveSound2; //2 of 2 Audio clips to play when player moves.
        [SerializeField] private AudioClip eatSound1; //1 of 2 Audio clips to play when player collects a food object.
        [SerializeField] private AudioClip eatSound2; //2 of 2 Audio clips to play when player collects a food object.
        [SerializeField] private AudioClip drinkSound1; //1 of 2 Audio clips to play when player collects a soda object.
        [SerializeField] private AudioClip drinkSound2; //2 of 2 Audio clips to play when player collects a soda object.
        [SerializeField] private AudioClip gameOverSound; //Audio clip to play when player dies.

        private Animator _animator; //Used to store a reference to the Player's animator component.
        private int _food; //Used to store player food points total during level.
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        private Vector2 touchOrigin = -Vector2.one;	//Used to store location of screen touch origin for mobile controls.
#endif


        //Start overrides the Start function of MovingObject
        protected override void Start()
        {
            //Get a component reference to the Player's animator component
            _animator = GetComponent<Animator>();

            //Get the current food point total stored in GameManager.instance between levels.
            _food = GameManager.Instance.playerFoodPoints;

            //Set the foodText to reflect the current player food total.
            foodText.text = "Food: " + _food;

            //Call the Start function of the MovingObject base class.
            base.Start();
        }


        //This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
            GameManager.Instance.playerFoodPoints = _food;
        }


        private void Update()
        {
            //If it's not the player's turn, exit the function.
            if (!GameManager.Instance.playersTurn) return;

            var horizontal = 0; //Used to store the horizontal move direction.
            var vertical = 0; //Used to store the vertical move direction.

            //Check if we are running either in the Unity editor or in a standalone build.
 #if UNITY_STANDALONE || UNITY_WEBPLAYER

            var input = GetInputs();
            horizontal = input.x;
            vertical = input.y;

            //Check if moving horizontally, if so set vertical to zero.
            if (horizontal != 0) vertical = 0;
            //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			//Check if Input has registered more than zero touches
			var input = GetTouchInput();
			horizontal = input.x;
			vertical = input.y;
			
#endif
 //End of mobile platform dependendent compilation section started above with #elif
            //Check if we have a non-zero value for horizontal or vertical
            if (horizontal != 0 || vertical != 0)
                //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
                //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                AttemptMove<Wall>(horizontal, vertical);
        }
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        
#endif
        private static Vector2Int GetInputs()
        {
	        
	        var horizontal = (int)Input.GetAxisRaw("Horizontal");
	        var vertical = (int)Input.GetAxisRaw("Vertical");
	        return new Vector2Int(horizontal,vertical);
        }

        //AttemptMove overrides the AttemptMove function in the base class MovingObject
        //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            //Every time player moves, subtract from food points total.
            _food--;

            //Update food text display to reflect current score.
            foodText.text = "Food: " + _food;

            //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
            base.AttemptMove<T>(xDir, yDir);

            //Hit allows us to reference the result of the Linecast done in Move.
            RaycastHit2D hit;

            //If Move returns true, meaning Player was able to move into an empty space.
            if (Move(xDir, yDir, out hit))
                //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
                SoundManager.Instance.RandomizeSfx(moveSound1, moveSound2);

            //Since the player has moved and lost food points, check if the game has ended.
            CheckIfGameOver();

            //Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.Instance.playersTurn = false;
        }


        //OnCantMove overrides the abstract function OnCantMove in MovingObject.
        //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
        protected override void OnCantMove<T>(T component)
        {
            //Set hitWall to equal the component passed in as a parameter.
            var hitWall = component as Wall;

            //Call the DamageWall function of the Wall we are hitting.
            hitWall.DamageWall(wallDamage);

            //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
            _animator.SetTrigger("playerChop");
        }


        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit")
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);

                //Disable the player object since level is over.
                enabled = false;
            }

            //Check if the tag of the trigger collided with is Food.
            else if (other.tag == "Food")
            {
                //Add pointsPerFood to the players current food total.
                _food += pointsPerFood;

                //Update foodText to represent current total and notify player that they gained points
                foodText.text = "+" + pointsPerFood + " Food: " + _food;

                //Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
                SoundManager.Instance.RandomizeSfx(eatSound1, eatSound2);

                //Disable the food object the player collided with.
                other.gameObject.SetActive(false);
            }

            //Check if the tag of the trigger collided with is Soda.
            else if (other.tag == "Soda")
            {
                //Add pointsPerSoda to players food points total
                _food += pointsPerSoda;

                //Update foodText to represent current total and notify player that they gained points
                foodText.text = "+" + pointsPerSoda + " Food: " + _food;

                //Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
                SoundManager.Instance.RandomizeSfx(drinkSound1, drinkSound2);

                //Disable the soda object the player collided with.
                other.gameObject.SetActive(false);
            }
        }


        //Restart reloads the scene when called.
        private void Restart()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }


        //LoseFood is called when an enemy attacks the player.
        //It takes a parameter loss which specifies how many points to lose.
        public void LoseFood(int loss)
        {
            //Set the trigger for the player animator to transition to the playerHit animation.
            _animator.SetTrigger("playerHit");

            //Subtract lost food points from the players total.
            _food -= loss;

            //Update the food display with the new total.
            foodText.text = "-" + loss + " Food: " + _food;

            //Check to see if game has ended.
            CheckIfGameOver();
        }


        //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
        private void CheckIfGameOver()
        {
            //Check if food point total is less than or equal to zero.
            if (_food <= 0)
            {
                //Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
                SoundManager.Instance.PlaySingle(gameOverSound);

                //Stop the background music.
                SoundManager.Instance.musicSource.Stop();

                //Call the GameOver function of GameManager.
                GameManager.Instance.GameOver();
            }
        }
    }
}