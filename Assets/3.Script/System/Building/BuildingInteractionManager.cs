using UnityEngine;

public class BuildingInteractionManager : MonoBehaviour {
    public InteractionUIMapping InteractionUI { get; private set; }
    public Menu_Controll menuControl { get; private set; }

    private void Awake() {
        InteractionUI = FindObjectOfType<InteractionUIMapping>();
        menuControl = FindObjectOfType<Menu_Controll>();
    }
}
