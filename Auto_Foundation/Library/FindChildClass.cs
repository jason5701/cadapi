using System.Windows;
using System.Windows.Media;

public class FindChildClass
{
    public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
    {
        // Confirm parent adn childName are vaild
        if (parent == null) return null;
        T foundChild = null;
        int childrentCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrentCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            // If the child is not of the request child type child
            T childType = child as T;
            if (childType == null)
            {
                // recuresively drill down the tree
                foundChild = FindChild<T>(child, childName);
                // If the child is found, break so we do not overwrite the found child
                if (foundChild != null) break;
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                var frameworkElement = child as FrameworkElement;
                // If the child's name is set for search
                if (frameworkElement != null && frameworkElement.Name == childName)
                {
                    // if the child's name is of the request name
                    foundChild = (T)child; break;
                }
            }
            else
            {
                // child element found
                foundChild = (T)child;
                break;
            }
        }
        return foundChild;
    }
}
