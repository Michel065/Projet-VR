/// <summary>
/// Interface commune à tous les objets / PNJ interactables.
/// Le système d'interaction VR appelle Interact() sur l'objet ciblé.
/// </summary>
public interface IInteractable
{
    void Interact();
}