using UnityEngine;

namespace HxHGame
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerExplorer : MonoBehaviour
    {
        public GameDirector game;
        CharacterController body;
        Vector3 velocity;
        void Awake() => body = GetComponent<CharacterController>();
        void Update()
        {
            if (game.Mode != GameMode.Exploration) return;
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
            body.Move(input * (Input.GetKey(KeyCode.LeftShift) ? 7f : 4.5f) * Time.deltaTime);
            if (input.sqrMagnitude > .1f) transform.forward = Vector3.Lerp(transform.forward,input,12*Time.deltaTime);
            velocity.y += Physics.gravity.y * Time.deltaTime; if (body.isGrounded) velocity.y = -1; body.Move(velocity*Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.E)) Interact();
            if (Input.GetKeyDown(KeyCode.F5)) game.Save();
        }
        void Interact()
        {
            if (Physics.Raycast(transform.position + Vector3.up*.5f, transform.forward, out var hit, 2.4f))
                hit.collider.GetComponent<WorldInteraction>()?.Use();
        }
    }

    public sealed class WorldInteraction : MonoBehaviour
    {
        string prompt; System.Action action;
        public void Configure(string text, System.Action callback) { prompt=text; action=callback; }
        public void Use() => action?.Invoke();
    }
}

