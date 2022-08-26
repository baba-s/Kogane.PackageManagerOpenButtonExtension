using Unity.CodeEditor;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Kogane.Internal
{
    [InitializeOnLoad]
    internal sealed class PackageManagerOpenButtonExtension
        : VisualElement,
          IPackageManagerExtension
    {
        private bool        m_isInitialized;
        private PackageInfo m_selectedPackageInfo;

        static PackageManagerOpenButtonExtension()
        {
            var extension = new PackageManagerOpenButtonExtension();
            PackageManagerExtensions.RegisterExtension( extension );
        }

        VisualElement IPackageManagerExtension.CreateExtensionUI()
        {
            m_isInitialized = false;
            return this;
        }

        void IPackageManagerExtension.OnPackageSelectionChange( PackageInfo packageInfo )
        {
            Initialize();

            m_selectedPackageInfo = packageInfo;
        }

        private void Initialize()
        {
            if ( m_isInitialized ) return;

            VisualElement root = this;

            while ( root is { parent: { } } )
            {
                root = root.parent;
            }

            var openButton = new Button
            (
                () =>
                {
                    var assetPath       = m_selectedPackageInfo.assetPath;
                    var packageJsonPath = $"{assetPath}/package.json";

                    CodeEditor.CurrentEditor.OpenProject( packageJsonPath );
                }
            )
            {
                text = "Open",
            };

            var removeButton = root.FindElement( x => x.name == "PackageRemoveCustomButton" );
            removeButton.parent.Insert( 0, openButton );

            m_isInitialized = true;
        }

        void IPackageManagerExtension.OnPackageAddedOrUpdated( PackageInfo packageInfo )
        {
        }

        void IPackageManagerExtension.OnPackageRemoved( PackageInfo packageInfo )
        {
        }
    }
}