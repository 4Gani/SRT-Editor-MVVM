using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SRTEditor_MVVM.Infrastructure
{
    /// <summary>
    /// Base class for ViewModels. Implements INotifyPropertyChanged to notify
    /// the View when a property value changes.
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// All class's Fields and Properties define here
        /// </summary>
        #region Fields and Properties
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        public static event PropertyChangedEventHandler? StaticPropertyChanged = delegate { };
        #endregion


        /// <summary>
        /// All class's Methods define here
        /// </summary>
        #region Methods
        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string? propertyName = null)
        {
            if (Equals(member, val)) return;

            member = val;

            OnPropertyChanged(propertyName);
            OnStaticPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static void OnStaticPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
