using UnityEngine;

namespace ShopWindow
{
    public class DropTarget : MonoBehaviour
    {
        private bool _occupied = false; // Tracks if the placeholder is occupied

        public bool IsOccupied()
        {
            return _occupied;
        }

        public void SetOccupied(bool state)
        {
            _occupied = state;
        }
    }
}