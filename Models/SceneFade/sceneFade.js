/*
* By @findemor
* www.devergence.com
* blog.findemor.es
*
* Free Scene FadeIn/Out prefab for unity
*/


var fadeOutTexture : Texture;
var playOnAwake = false;
var startFadingIn = true;

var fadeInDuration = 3;
var fadeOutDuration = 3;

private var isFading = false;
private var isFadeIn = true;

private var alpha = 0.0f;
private var allowFading = true;

function Start() {
	fadeOutTexture.wrapMode = TextureWrapMode.Repeat;
	isFadeIn = startFadingIn;
	
	if (isFadeIn)
	{
		alpha = 1.0f;
	}
	
	isFading = playOnAwake;
}

function OnGUI() {
	//new alpha value
	if (isFading) {
		if (isFadeIn)
		{
			//Debug.Log("fadingin");
			alpha -= Mathf.Clamp01(Time.deltaTime)/(fadeInDuration*2); 
		}else
		{
			//Debug.Log("fadingout");
			alpha += Mathf.Clamp01(Time.deltaTime)/(fadeOutDuration*2); 
		}
			
		if (alpha < 0 || alpha > 1)
		{
			isFading = false; //stop fading
		}
	}
	//draw texture if necessary
	if (allowFading && alpha > 0)
	{
		//Debug.Log(Time.time + " " + alpha + " " + isFading);

		var oldAlpha = GUI.color.a;
		GUI.color.a = alpha;
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture, new Rect(0, 0, 1, 1));
		GUI.color.a = oldAlpha;
	}
}


/**
 * Start new Fade In
 * @param {Number} duration 
 */
function FadeIn(duration) {
	fadeInDuration = duration;
	alpha = 1.0f;
	FadeInWithoutReset();
}


/**
 * Start new Fade Out
 * @param {Number} duration 
 */
function FadeOut(duration) {
	fadeOutDuration = duration;
	alpha = 0.0f;
	FadeOutWithoutReset();
}

/**
 * Start new Fade In
 */
function FadeIn() {
	alpha = 1.0f;
	FadeInWithoutReset();
}

/**
 * Start new Fade Out
 */
function FadeOut() {
	alpha = 0.0f;
	FadeOutWithoutReset();
}

/**
 * Continue Fade In
 * @param {Number} duration 
 */
function FadeInWithoutReset() {
	allowFading = true;
	isFadeIn = true;
	isFading = true;
}

/**
 * Continue Fade Out
 * @param {Number} duration 
 */
function FadeOutWithoutReset(duration) {
	fadeOutDuration = duration;
	allowFading = true;
	isFadeIn = false;
	isFading = true;
}

/**
 * Continue Fade In
 */
function FadeInWithoutReset(duration) {
	fadeInDuration = duration;
	allowFading = true;
	isFadeIn = true;
	isFading = true;
}

/**
 * Continue Fade Out
 */
function FadeOutWithoutReset() {
	allowFading = true;
	isFadeIn = false;
	isFading = true;
}


/**
 * Stop the current fading (pause)
 */
function StopFading() {
	isFading = false;
}

/**
 * Stop and hide the current fading
 */
function HideFade() {
	allowFading = false;
	isFading = false;
}

