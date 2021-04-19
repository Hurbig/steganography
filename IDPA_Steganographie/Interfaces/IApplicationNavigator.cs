using System.Windows.Controls;

namespace IDPA_Steganographie.Interfaces
{
    /// <summary>
    /// Helps to navigate through the Application
    /// </summary>
    public interface IApplicationNavigator
    {
        /// <summary>
        /// Navigates to the content object.
        /// </summary>
        /// <param name="content">UiElement that navigates</param>
        /// <param name="type"></param>
        void NavigateTo(UserControl content);
        /// <summary>
        /// Navigate to the home screen of the application
        /// </summary>
        void NavigateHome();
    }
}
