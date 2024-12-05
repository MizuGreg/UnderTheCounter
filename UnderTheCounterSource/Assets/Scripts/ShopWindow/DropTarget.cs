using UnityEngine;

namespace ShopWindow
{
    public class DropTarget : MonoBehaviour
    {
        [SerializeField] private bool _occupied = false; // Tracks if the placeholder is occupied
        [SerializeField] private bool windowSlot = false;

        public bool IsOccupied()
        {
            return _occupied;
        }

        public void SetOccupied(bool state)
        {
            _occupied = state;
        }

        public bool isWindowSlot()
        {
            return windowSlot;
        }
    }
}