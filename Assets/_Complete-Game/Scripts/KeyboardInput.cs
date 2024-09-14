using UnityEngine;

namespace Completed
{
    public class KeyboardInput : IGetInput
    {
        public Vector2Int GetInput()
        {
            var horizontal = (int)Input.GetAxisRaw("Horizontal");
            var vertical = (int)Input.GetAxisRaw("Vertical");
            return new Vector2Int(horizontal, vertical);
        }
    }
}