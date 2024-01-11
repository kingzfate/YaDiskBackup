using Prism.Mvvm;
using Prism.Navigation;

namespace YaDiskBackup.Shared.ViewModels;

/// <inheritdoc cref="Prism.Mvvm.BindableBase" />
public abstract class ViewModelBase : BindableBase, IDestructible
{
    /// <inheritdoc />
    public virtual void Destroy()
    {
    }
}