using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Assertions;
using Xml2CSharp;
using System.IO;

public class Central : MonoBehaviour
{
	private Scorepartwise me;
	public Camera mainCamera;
	private Vector2? priorPos;
	private bool inDrag = false;
	public int score;
    // Start is called before the first frame update
    void Start()
    {
		Assert.IsNotNull(mainCamera,"Blew it getting camera");
		mainCamera.GetComponent<Rigidbody>().velocity=new Vector3(0,0,2f);
		LoadStaff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnSlash(CallbackContext context) {
		if(!inDrag) {
			priorPos = null;
			return;
		}
		Vector2 pos = (context.control as Vector2Control).ReadValue();
		if(priorPos == null)
		{
			priorPos = pos;
			return;
		}
		//Debug.Log(pos.ToString());
		//Vector2 delta = pos - priorPos;
		RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(pos);
        
        if (Physics.Raycast(ray, out hit, 10f)) {
            Transform objectHit = hit.transform;
            
            // Do something with the object that was hit by the raycast.
            ToneTarget hittee = objectHit.GetComponent<ToneTarget>();
            if(hittee != null) {
				score+=Whack(hittee);
			}
        }
		priorPos = pos;
	}
	
	public void OnPress(CallbackContext context) {
		ButtonControl b = context.control as ButtonControl;
		inDrag = b.isPressed;
	}
	
	int Whack(ToneTarget hitTarget) {
		int augend = hitTarget.points;
		hitTarget.Play();
		return augend;
	}
	
	//C4 is middle C, A4 is 440 Hz, Treble Staff goes from C4 (one leger line below bottommost line) to B5, Bass Staff goes from G2 (bottommost line) to B3 pokes above top line
	
	void LoadStaff() {
		//List<Note> netnotes=
		me = Staff.Load(Path.Combine(Application.dataPath, "Songs/scaly.musicxml"));
		foreach(Measure m in me.Part.Measure) {
			if(m.Note.Count == 0) {
				continue;
			}
			foreach(Note la in m.Note) {
				if(la.Rest == null) {
					Debug.Log(la.Type+" note:"+la.Pitch.Step+la.Pitch.Octave);
				}
				else {
					Debug.Log(la.Type+" rest:");
				}
			}
			if(m.Number == "3") {
				break;
			}
		}
		Debug.Log(me.Part.Measure.Count);
	}
}
