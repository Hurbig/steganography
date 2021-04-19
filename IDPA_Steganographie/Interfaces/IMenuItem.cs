using System;
using System.Windows.Input;

namespace IDPA_Steganographie.Interfaces
{
    public interface IMenuItem
    {
        /// <summary>
        /// The element is selected as long as IsSelected == true
        /// </summary>
        bool IsSelected { get; set; }
        /// <summary>
        /// Item got clicked 
        /// </summary>
        event EventHandler ItemClicked;
        /// <summary>
        /// Action that gets executed when the mouse hovers the item
        /// </summary>
        void MouseEnterAction(object sender, MouseEventArgs mouseEventArgs);
        /// <summary>
        /// Action that gets executed when the mouse leaves the item
        /// </summary>
        void MouseLeaveAction(object sender, MouseEventArgs mouseEventArgs);
        /// <summary>
        /// Action that gets executed when the item gets clicked
        /// </summary>
        void LeftMouseButtonUpAction(object sender, MouseEventArgs mouseEventArgs);
    }
}
