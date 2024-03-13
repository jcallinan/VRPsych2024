using UnityEngine;
using System.Collections;

/// <summary>
/// Example script. Controls a point light and a material to sets the light bulb's colour and intensity.
/// </summary>
[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour {

	// Required components
	Light mLight;
	MeshRenderer mMeshRenderer;
	Animation mAnimation;
	public CharacterController characterController;
    public float maxHeight = 10f; // Maximum height the player can move to

    void OnEnable()
	{
		
		mLight = GetComponent<Light> ();
		if (mLight == null)
			Debug.LogError ("Light is missing from " + name);

		mMeshRenderer = GetComponent<MeshRenderer> ();
		if (mMeshRenderer == null)
			Debug.LogError ("MeshRenderer is missing from " + name);

		mAnimation = GetComponent<Animation> ();
		if (mAnimation == null)
			Debug.LogError ("Animation is missing from " + name);
		else
			mAnimation["Bob"].time = Random.Range(0.0f, mAnimation["Bob"].length);

		// Initialize the light with it's first colour and intensity
		//SetLightColour (Colours [CurrentColour]);
	 //	SetModelColour (Materials [CurrentColour]);		
	}

	/// <summary>
	/// List of colours the light cycles through
	/// </summary>
	public Color[] Colours;

	/// <summary>
	/// List of materials the light cycles through.
	/// </summary>
	public Material[] Materials;

	/// <summary>
	/// Cache to remember position in the colour list
	/// </summary>
	public int CurrentColour = 0;

	/// <summary>
	/// Sets the point light intensity.
	/// </summary>
	/// <param name="_value">Value. 0 - 1</param>
	void SetIntensity(float _value)
	{
		mLight.intensity = _value * 8.0f;
	}

	/// <summary>
	/// Sets the point light colour.
	/// </summary>
	/// <param name="_colour">Colour.</param>
	void SetLightColour (Color _colour)
	{
		mLight.color = _colour;
	}

	/// <summary>
	/// Sets the model material colour.
	/// </summary>
	/// <param name="_mat">Mat.</param>
	void SetModelColour (Material _mat)
	{
		mMeshRenderer.material = _mat;
	}

	/// <summary>
	/// Function called by the button to change the lights
	/// </summary>
	public void ColourChanged()
	{
		CurrentColour = CurrentColour >= ( Colours.Length - 1 ) ?  0 : CurrentColour + 1;

		SetLightColour (Colours [CurrentColour]);
		SetModelColour (Materials [CurrentColour]);
			
	}

	/// <summary>
	/// Changes the light intensity, called by the lever
	/// </summary>
	/// <param name="_lever">Lever.</param>

	public void IntensityChanged(VRLever _lever, float _currentValue, float _lastValue)
	{
		//SetIntensity(_currentValue);
	}

	/// <summary>
	/// Changes the light intensity, called by the lever
	/// </summary>
	/// <param name="_lever">Lever.</param>
	public void IntensityChanged(VRLever _lever)
	{
		if (_lever == null) {
			Debug.LogError ("_lever is null");
			return;
		}
			
		//SetIntensity(_lever.Value);
	}
    /// <summary>
    /// Moves the CharacterController up and down, ensuring it does not go above maxHeight.
    /// </summary>
    public void MoveUpDown(VRLever _lever, float _movement, float _lastValue)
    {
        if (_lever != null && _lever.Value != _lastValue && characterController != null)
        {
            Vector3 movement = transform.up * _movement;
            movement.x = 0f; // Exclude horizontal movement
            movement.z = 0f; // Exclude depth movement

            // Predict the next position of the player
            Vector3 nextPosition = characterController.transform.position + movement;

            // Limit the y position to maxHeight
            if (nextPosition.y > maxHeight)
            {
                // Adjust movement so that it stops exactly at maxHeight
                float difference = maxHeight - characterController.transform.position.y;
                movement = transform.up * difference;
            }

            characterController.Move(movement);
        }
    }

    /// <summary>
    /// Moves the CharacterController left and right.
    /// </summary>
    public void MoveLeftRight(VRLever _lever, float _movement, float _lastValue)
    {
        if (_lever != null && _lever.Value != _lastValue && characterController != null)
        {
            Vector3 movement = transform.right * _movement;
            movement.y = 0f; // Exclude vertical movement
            movement.z = 0f; // Exclude depth movement

            characterController.Move(movement);
        }
    }

}
