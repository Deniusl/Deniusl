using UnityEngine;
using System.Collections;

public class MorphTree: MonoBehaviour
{
	
	Animation anim;
	public  int treeISsmall = 1;
	
	void Start ()
	{
		
		transform.GetComponent<Animation> ().Play ("init");
		
	}

	void Awake ()
	{
		anim = GetComponent<Animation> ();
		
	
	}


	
	void Update ()
	{
		
		if (Input.GetKey (KeyCode.Space) && treeISsmall == 1 && !anim.isPlaying) {
			
			transform.GetComponent<Animation> ().Play ("grow");
			{
				treeISsmall = treeISsmall + 1;
				
			}	
		}	
		
		if (Input.GetKey (KeyCode.Space) && treeISsmall == 2 && !anim.isPlaying) {
			
			transform.GetComponent<Animation> ().Play ("die");
			{
				treeISsmall = 1;
			}	
		}	
		
		
		
	}
}